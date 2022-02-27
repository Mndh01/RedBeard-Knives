import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  products: Product[];
  categoryList: Array<string> = ['axe', 'web developer', 'Software Engineer','IT', 'web developer', 'Software Engineer','IT'];
  category:string | undefined = this.categoryList[0]; 
  price:number;
  inStock:number;
  soldItems:number;
  checkedInput:any;
  constructor(private productService: ProductsService) { }

  ngOnInit() {
  }

  getProducts() {
    this.productService.getProducts(this.category, this.price, this.inStock, this.soldItems).subscribe(data => {
      this.products = data;
      localStorage.setItem("product",JSON.stringify(data));
    }, error => {
      console.log(error);
    });
  }

  getType(category:string){
    this.category = category;
    // this.checkedInput = document.querySelectorAll(".selectopt")?.forEach(element => {
    //   if (element.hasAttribute("checked")){
    //     this.checkedInput = element;
    //     console.log(this.checkedInput)
    //   }
    // });
  }
}