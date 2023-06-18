import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { Product } from 'src/app/shop/models/Product';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent implements OnInit {
  @Input() product: Product;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    if(this.product.photoUrl === null && this.product.photos.length > 0) {
      this.product.photoUrl = this.product.photos[0].url;
    }
  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.product)
  }

}
