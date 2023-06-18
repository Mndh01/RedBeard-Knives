import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductsService } from '../products.service';

import { Product } from 'src/app/shop/models/Product';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { Review } from '../../shared/models/Review';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: Product;
  newComment: string;
  productId: number;
  quantity: number = 1;
  review: Review;

  constructor( private route: ActivatedRoute, private productService: ProductsService,
    private toastr: ToastrService, private basketService: BasketService) 
    { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.productId = params['id'];
      this.getProduct(this.productId);
    });
  }

  getProduct(productId: number): void {
    this.productService.getProductById(productId).subscribe({
      next: (product: Product) => {
        this.product = product;
      },
      error: (error: any) => {
        this.toastr.error(error.message);
      }
    });
  }

  addReview(): void {
    this.review = new Review(this.newComment, this.productId);

    this.productService.addReviewToProduct(this.review).subscribe({
      next: (review: Review) => {
        this.product.reviews.push(review);
        this.toastr.success('Comment added successfully');
      }, 
      error: (error: any) => {
        this.toastr.error(error.message);
      },
      complete: () => {
        this.newComment = '';
      }
    });
  }

  incrementItemQuantity(): void {
    this.quantity++;
  }

  decrementItemQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  addToBasket(): void {
    this.basketService.addItemToBasket(this.product, this.quantity)
    this.toastr.success(this.quantity + ' Item(s) added to basket');
    this.quantity = 1;
  }
}