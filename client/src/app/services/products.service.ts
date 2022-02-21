import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  baseUrl = environment.apiUrl;
  fakeUrl = "../assets/products.json"

  constructor(private http: HttpClient) { }

  getProduct(){
    return this.http.get(this.baseUrl + "products", {});
  }

  getProductById(id: number){
    return this.http.get(this.baseUrl + "products/byId/" + id);
  }

  getProductsByPrice(price: number){
    return this.http.get(this.baseUrl + "products/byPrice/" + price);
  }

  getProductsByType(type: string){
    return this.http.get(this.baseUrl + "products/byType/" + type);
  }
}

