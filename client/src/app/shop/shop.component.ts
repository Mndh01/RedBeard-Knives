import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { Product } from 'src/app/shop/models/Product';
import { ProductsService } from 'src/app/shop/products.service';
import { PaginatedResponse, Pagination } from '../shared/models/Pagination';
import { ProductCategory } from './models/ProductCategory';
import { ShopParams } from './models/ShopParams';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})

export class ShopComponent implements OnInit {
  @ViewChild('search', {static: true}) searchTerm: ElementRef<HTMLInputElement>;
  @ViewChild('sortSelect', {static: false}) sortSelect?: ElementRef<HTMLSelectElement>;
  @ViewChild('mobileSortSelect', {static: false}) mobileSortSelect?: ElementRef<HTMLSelectElement>;
  @ViewChild('mobileCategorySelect', {static: false}) mobileCategorySelect?: ElementRef<HTMLSelectElement>;
  products: Product[];
  categories: ProductCategory[];
  shopParams: ShopParams;
  pagination: Pagination = {} as Pagination;
  inputPrice: number;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
    { name: 'Top selling', value: 'topSells' },
  ];
  
  constructor(private productService: ProductsService, private toastr: ToastrService) { 
    this.shopParams = this.productService.getShopParams();
  }

  ngOnInit() {
    this.setPageSize();
    this.getProducts();
    this.getCategories();
  }

  getProducts() {
    this.productService.getProducts().subscribe({
      next: (response: PaginatedResponse<Product[]>) => {
          this.products = response.data;
          this.pagination.count = response.count;
          this.pagination.pageIndex = response.pageIndex;
          this.pagination.pageSize = response.pageSize;
          this.pagination.totalPages = Math.ceil(response.count / response.pageSize);
      },
      error: (error) => {
        this.toastr.error(error);
      }
    });
  }
  
  getCategories() {
    this.productService.getCategories().subscribe(Categories => {
      this.categories = [{id: 0, name: "All"}, ...Categories];
    }, error => {
      this.toastr.error(error.message);
    })      
  }

  onPageChanged(event: any) {
    const params = this.productService.getShopParams();
    if (params.pageIndex !== event){
      params.pageIndex = event;
      this.pagination.pageIndex = params.pageIndex;
      this.productService.setShopParams(params);
      this.shopParams = params;
      this.getProducts();
    }
  }

  onCategorySelect(categoryId: number) {
    const params = this.productService.getShopParams();
    if (params.categoryId !== categoryId) {
      params.categoryId = categoryId;
      params.pageIndex = 1;
      this.productService.setShopParams(params);
      this.shopParams = params;
      this.getProducts(); 
    }
  }

  onSortSelect(sort: string) {
    const params = this.productService.getShopParams();
    params.sort = sort;
    this.productService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onSearch() {
    const params = this.productService.getShopParams();
    this.shopParams.search = this.searchTerm?.nativeElement.value;
    params.pageIndex = 1;
    this.productService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.sortSelect.nativeElement.value = 'name';
    this.mobileSortSelect.nativeElement.value = 'name';
    this.mobileCategorySelect.nativeElement.value = '0';
    this.shopParams = new ShopParams();
    this.setPageSize();
    this.getProducts();
  }

  private setPageSize() {
    let width: number = window.innerWidth;
    this.shopParams.pageIndex = 1;
    switch(true) 
    {
      case (width >= 575 && width <= 767):
          this.shopParams.pageSize = 6;
          break;
      case (width >= 768 && width <= 991):
          this.shopParams.pageSize = 9;
          break;
      case (width >= 992 && width <= 1399):
          this.shopParams.pageSize = 12;
          break;
      case (width >= 1400):
          this.shopParams.pageSize = 15;
          break;
      default:
        this.shopParams.pageSize = 5;
        break;
    }
    this.productService.setShopParams(this.shopParams);
  }
}
