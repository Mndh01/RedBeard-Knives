import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Product } from 'src/app/models/Product';
import { ProductsService } from 'src/app/services/products.service';
import { PaginatedResult, Pagination } from '../models/Pagination';
import { ProductCategory } from '../models/ProductCategory';
import { ShopParams } from '../models/ShopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})

export class ShopComponent implements OnInit {
  @ViewChild('search', {static: true}) searchTerm: ElementRef<HTMLInputElement>;
  @ViewChild('sortSelect', {static: true}) SortSelect: ElementRef<HTMLSelectElement>;
  products: Product[];
  categories: ProductCategory[];
  shopParams: ShopParams = new ShopParams();
  pagination: Pagination;
  inputPrice: number;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' },
    { name: 'Top selling', value: 'topSells' },
  ];
  
  constructor(private productService: ProductsService) { }

  ngOnInit() {
    this.setPageSize();
    this.getProducts();
    this.getCategories();
  }

  getProducts() {
    this.productService.getProducts(this.shopParams).subscribe({
      next: (response: PaginatedResult<Product[]>) => {
        if(response.data && response.pagination) {
          this.products = response.data;
          this.pagination = response.pagination;
        }
      }
    });
  }
  
  getCategories() {
    this.productService.getCategories().subscribe(Categories => {
      this.categories = [{id: 0, name: "All"}, ...Categories];
    }, error => {
      console.log(error); // TODO: Change this log to a toastr popup
    })      
  }

  onPageChanged(event: any) {
    if (this.shopParams.pageIndex !== event){
      this.shopParams.pageIndex = event;
      this.pagination.pageIndex = this.shopParams.pageIndex;
      this.getProducts();
    }
  }

  onCategorySelect(categoryId: number) {
    if (this.shopParams.categoryId !== categoryId) {
      this.shopParams.categoryId = categoryId;
      this.shopParams.pageIndex = 1;
      this.getProducts(); 
    }
  }

  onSortSelect(sort: string) {
    this.shopParams.sort = sort;
    this.getProducts();
  }

  onSearch() {
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.getProducts();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.SortSelect.nativeElement.value = 'name';
    this.shopParams = new ShopParams();
    this.setPageSize();
    this.getProducts();
  }

  private setPageSize() {
    let width: number = window.innerWidth;
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
  }
}
