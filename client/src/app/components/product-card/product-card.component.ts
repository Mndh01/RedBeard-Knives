import { Component, Input, OnInit } from '@angular/core';
import { Product } from 'src/app/shop/models/Product';
import { ProductsService } from 'src/app/shop/products.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent implements OnInit {
  @Input() products: Product[];
  constructor(private productService: ProductsService) { }

  ngOnInit(): void {
  }

}
