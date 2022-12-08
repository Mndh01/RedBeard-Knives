import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Product } from 'src/app/shared/models/Product';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  @ViewChild('editForm') editform: NgForm;
  product: Product;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editform.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private productService: ProductsService) { }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    this.product = JSON.parse(localStorage.getItem("product"));
    localStorage.setItem("product", JSON.stringify(this.product));
  }

  updateProduct() {
    this.productService.updateProduct(this.product).subscribe(() => {
      this.editform.reset(this.product);
    })
  }
}
