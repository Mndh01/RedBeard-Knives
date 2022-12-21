import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {
  products: Product[];
  categoryList: Array<string> = ['axe', 'web developer', 'Software Engineer','IT', 'web developer', 'Software Engineer','IT'];
  category:string = ''; 
  inputPrice: number; price:number;
  inStock:number = -1; soldItems:number = -1;
  
  constructor(private productService: ProductsService) { }

  ngOnInit() {
    this.getProducts();
  }

  getProducts() {
    this.checkPrice();
    this.productService.setParams(this.category, this.price, this.inStock, this.soldItems);
    this.productService.getProducts().subscribe(data => {
      this.products = data.result;
    }, error => {
      console.log(error);
    });
  }

  getCategory(category:string){
    this.category = category;      
  }
  checkPrice(){
    if (this.inputPrice > 0 && this.inputPrice < Number.POSITIVE_INFINITY){
      this.price = this.inputPrice;
    }else{
      this.price = -1;
    }
  }
}