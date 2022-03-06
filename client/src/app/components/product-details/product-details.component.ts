import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Product } from 'src/app/models/Product';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  product: Product;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor(private productService: ProductsService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadProduct();

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent:  100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ]
  }

  getImages(): NgxGalleryImage[] {
    const imageUrls = [];
    for (const photo of this.product.photos) {
      imageUrls.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      })
    }
    return imageUrls;
  }

  loadProduct() { 
    this.productService.getProductById(+this.route.snapshot.paramMap.get('id')).subscribe(product => {
      this.product = product;
      this.galleryImages = this.getImages();
    }, error => {console.log(error); })
  }

  deleteProduct() {
    return this.productService.deleteProduct(this.product.id).subscribe(response => {
      console.log(response);
    }, error => {console.log(error);} );
  }
}