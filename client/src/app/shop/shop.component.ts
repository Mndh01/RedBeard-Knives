import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { ProductsService } from 'src/app/services/products.service';
import { PaginatedResult, Pagination } from '../models/Pagination';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  products: Product[];
  categoryList: Array<string> = ['axe', 'web developer', 'Software Engineer','IT', 'web developer', 'Software Engineer','IT'];
  category:string = ''; 
  inputPrice: number; price:number;
  inStock:number = -1; soldItems:number = -1;
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 5; // TODO: Change the pageSize depending on screen width
  rotate = false;
  maxSize = 3;
  status = "ON";
  
  constructor(private productService: ProductsService) { }

  ngOnInit() {
    this.getProducts();
  }

  getProducts() {
    this.checkPrice();
    this.productService.setParams(this.category, this.price, this.inStock, this.soldItems);
    this.productService.getProducts(this.pageNumber, this.pageSize).subscribe({
      next: (response: PaginatedResult<Product[]>) => {
        if(response.result && response.pagination) {
          this.products = response.result;
          this.pagination = response.pagination;
        }
      }
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

  pageChanged(event: any) {
    if (this.pageNumber !== event.page){
      this.pageNumber = event.page;
      this.pagination.currentPage = this.pageNumber;
      this.getProducts();
    }
  }
}
