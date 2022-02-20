import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  public products: any;
  constructor(private productService: ProductsService) { }

  ngOnInit() {
    this.productService.getProductsByPrice(300).subscribe(data => {
      localStorage.setItem("product",JSON.stringify(data));
      this.products = data;
    }, error => {
      console.log(error);
    }
    );
  }

}
