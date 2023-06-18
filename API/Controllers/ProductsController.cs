using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using API.Data.Specifications;
using API.DTOs;
using API.DTOs.Admin;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Models;
using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResponseCacheService _responseCacheService;
        public ProductsController (IUnitOfWork unitOfWork, IPhotoService photoService, IMapper mapper, IResponseCacheService responseCacheService) 
        {
            _responseCacheService = responseCacheService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoService = photoService;
        }
        
        [Cached(600)]
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {   
            var spec = new ProductsWithTypesSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if (product != null) return Ok(product);

            return BadRequest("Couldn't find product...");
        }

        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams productParams) 
        {
            List<Expression<Func<Product, bool>>> criteria = new List<Expression<Func<Product, bool>>>();

            criteria.Add(p => (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)));
            criteria.Add(p => (!productParams.CategoryId.HasValue || p.CategoryId == productParams.CategoryId));
            criteria.Add(p => (!productParams.Price.HasValue || p.Price <= productParams.Price));
            criteria.Add(p => (!productParams.InStock || p.InStock > 0));
             
            var spec = new ProductsWithTypesSpecification(productParams, criteria);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _unitOfWork.Repository<Product>().CountAsync(countSpec);

            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);

            return Ok(new Pagination<ProductDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));

        }

        [Cached(600)]
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _unitOfWork.Repository<ProductCategory>().ListAllAsync();

            if (categories != null) return Ok(categories);

            return BadRequest("Couldn't find categories...");
        }
        
        [Authorize(Policy="RequireAdminRole")]
        [HttpPost("add-category")]
        public async Task<ActionResult<ProductCategory>> AddCategory(ProductCategory newCategory)
        {
            BaseSpecification<ProductCategory> spec = new BaseSpecification<ProductCategory>(pc => pc.Name == newCategory.Name.ToLower().Trim());
            
            var exists = await _unitOfWork.Repository<ProductCategory>().GetEntityWithSpec(spec);

            if(exists != null) { return BadRequest("Category already exists..."); }
            
            var category = new ProductCategory { Name = newCategory.Name.ToLower().Trim() };

            _unitOfWork.Repository<ProductCategory>().Add(category);

            if (await _unitOfWork.Complete() > 0) 
            {
                await RemoveCache(Request);   
                
                return Ok(category);
            }

            return BadRequest("Couldn't add category...");
        }

        [DisableRequestSizeLimit]
        [Authorize(Policy="RequireAdminRole")]
        [HttpPost("add-product")]
        public async Task<ActionResult<Product>> AddProduct([FromForm] ProductDto product, IFormFile[] files) 
        {
            product.Category = JsonConvert.DeserializeObject<ProductCategory>(product.CategoryJson);

            await SetProductPhotos(product, files);

            Product newProduct = _mapper.Map<Product>(product);
            
            BaseSpecification<Product> spec = new BaseSpecification<Product>(p => p.Name == product.Name.ToLower());

            newProduct.Category = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(product.Category.Id);
            
            if (await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec) != null) return BadRequest("Name already exists for another product..");

            _unitOfWork.Repository<Product>().Add(newProduct);

            if(await _unitOfWork.Complete() > 0) 
            {
                await RemoveCache(Request);

                return Created(new Uri($"{Request.Path}/{newProduct.Id}", UriKind.Relative) ,newProduct);
            }

            foreach (var photo in product.Photos) 
            {
                await _photoService.DeletePhotoAsync(photo.PublicId);
            }

            return BadRequest("Failed to add product");               
        }
        
        [Authorize(Policy="RequireAdminRole")]
        [HttpPost("add-photos")]
        public async Task<ActionResult<List<PhotoDto>>> AddPhotos([FromForm] int productId, IFormFile[] files)
        {
            var spec = new ProductsWithTypesSpecification(productId);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            List<ImageUploadResult> results = await SetProductPhotos(_mapper.Map<ProductDto>(product), files);

            List<Photo> photos = new List<Photo>();

            foreach (var result in results) {

                if (result.Error != null) { continue; };

                var photo = new Photo 
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId
                };

                if (product.Photos.Count == 0) 
                {
                    photo.IsMain = true;
                }

                photos.Add(photo);

                product.Photos.Add(photo);
            }

            if (await _unitOfWork.Complete() > 0) 
            {
                await RemoveCache(Request);

                return CreatedAtRoute("GetProduct", new {id = product.Id}, _mapper.Map<List<PhotoDto>>(photos));
            }

            return BadRequest("Failed to add photos..");
        }

        [Authorize]
        [HttpPost("add-review")]
        public async Task<ActionResult<ReviewDto>> AddReview(ReviewDto reviewDto) 
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(reviewDto.ProductId);
            
            var review = new Review 
            {
                AuthorId = User.GetUserId(),
                AuthorName = User.GetUsername(),
                Body = reviewDto.Body,
                CreatedAt = DateTime.Now,
                ProductId = product.Id,
            };

            product.Reviews.Add(review);

            if (await _unitOfWork.Complete() > 0) 
            {
                await RemoveCache(Request);

                return CreatedAtRoute("GetProduct", new {id = product.Id}, _mapper.Map<ReviewDto>(review));
            }

            return BadRequest("Failed to add comment..");
        }

        [Authorize]
        [HttpDelete("delete-review")]
        public async Task<ActionResult> DeleteReview(int ProductId, int ReviewId)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(ProductId);

            var review = product.Reviews.FirstOrDefault(review => review.Id == ReviewId);

            if (review == null) return NotFound();

            product.Reviews.Remove(review);

            if (await _unitOfWork.Complete() > 0) 
            {
                await RemoveCache(Request);

                return NoContent();
            }

            return BadRequest("Failed to delete the comment..");
        }

        [Authorize(Policy="RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {   
            var spec = new ProductsWithTypesSpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            foreach (Photo photo in product.Photos) {   
               await _photoService.DeletePhotoAsync(photo.PublicId);
            }

            _unitOfWork.Repository<Product>().Delete(new Product {Id = id});

            if (await _unitOfWork.Complete() > 0) 
            {
                await RemoveCache(Request);

                return NoContent();
            }
            
            return BadRequest("Failed to remove product..");
        }

        [Authorize(Policy="RequireAdminRole")]
        [HttpDelete("delete-photo")]
        public async Task<ActionResult<Product>> DeletePhoto(int ProductId, int PhotoId)
        {
            var spec = new ProductsWithTypesSpecification(ProductId);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if(product == null) return NotFound("Product not found");

            var photo = product.Photos.FirstOrDefault(Photo => Photo.Id == PhotoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete the main photo");

            if (photo.PublicId != null) {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }
            
            product.Photos.Remove(photo);

            if (await _unitOfWork.Complete() > 0) 
            {
                await RemoveCache(Request);

                return NoContent();
            }

            return BadRequest("Failed to delete the photo..");
        }
        
        [Authorize(Policy="RequireAdminRole")]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductUpdateDto productUpdateDto) 
        {
            productUpdateDto.Name = productUpdateDto.Name.ToLower();

            productUpdateDto.Category.Name = productUpdateDto.Category.Name.ToLower();
            
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productUpdateDto.Id);

            _mapper.Map(productUpdateDto, product);

            _unitOfWork.Repository<Product>().Update(product);

            if (await _unitOfWork.Complete() > 0) 
            {
                await RemoveCache(Request);

                return NoContent();
            }

            return BadRequest("Failed to update product..");
        }

        [Authorize(Policy="RequireAdminRole")]
        [HttpPut("set-main-photo")]
        public async Task<ActionResult> SetMainPhoto(int ProductId, int PhotoId)
        {
            var spec = new ProductsWithTypesSpecification(ProductId);

            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            var photo = product.Photos.FirstOrDefault(photo => photo.Id == PhotoId);

            if (photo.IsMain) return BadRequest("This is already the main photo");

            var currentMain = product.Photos.FirstOrDefault(photo => photo.IsMain);

            if (currentMain != null) currentMain.IsMain = false;
            
            photo.IsMain = true;

            if (await _unitOfWork.Complete() > 0) 
            {
                await RemoveCache(Request);

                return NoContent();
            }

            return BadRequest("Failed to set main photo..");
        }

        private async Task RemoveCache(HttpRequest request)
        {
            var cacheKey = _responseCacheService.GenerateCacheKeyFromRequest(request);

            await _responseCacheService.DeleteCachedResponseAsync();
        }

        private async Task<List<ImageUploadResult>> SetProductPhotos(ProductDto product, IFormFile[] files) {
            List<ImageUploadResult> results = new List<ImageUploadResult>();
            bool newProduct = product.Id == 0;
            if (files.Any()) 
            {
                
                foreach (var file in files) 
                {
                    var result = await _photoService.AddPhotoAsync(file);

                    results.Add(result);

                    if (result.Error != null) { continue; };

                    var photo = new PhotoDto 
                    {
                        Url = result.SecureUrl.AbsoluteUri,
                        PublicId = result.PublicId
                    };

                    product.Photos.Add(photo);
                }

                if (newProduct) product.Photos.FirstOrDefault().IsMain = true;

                if (product.PhotoUrl == null) product.PhotoUrl = product.Photos.FirstOrDefault().Url;
            }

            return results;
        }
    }
}