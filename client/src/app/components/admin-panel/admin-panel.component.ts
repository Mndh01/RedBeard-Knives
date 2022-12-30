import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ProductCategory } from 'src/app/models/ProductCategory';
import { AdminService } from 'src/app/services/admin-service.service';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent implements OnInit {
  addProductForm: FormGroup;
  categories: ProductCategory[];
  categoryToAdd: string;
  newCategory: ProductCategory = {id: 0, name: ""};
    
  constructor(private adminService: AdminService, private productService: ProductsService, private fb: FormBuilder) { }
  
  ngOnInit(): void {
    this.getCategories();
    this.initializeForm();
  }
  
  initializeForm() {
    this.addProductForm = this.fb.group({
      name: ['',Validators.required],
      category: this.fb.group({
        id: [],
        name: ['',Validators.required]
      }),
      price: ['',Validators.required],
      description: ['',Validators.required],
      inStock: ['',Validators.required],
      soldItems: ['',Validators.required]
    })
  }

  getFormControl(name) { 
    return this.addProductForm.get(name) as FormControl;
  }
  
  getFormGroup() { 
    return this.addProductForm.get("category") as FormGroup;
  }

  getCategories() {
    this.productService.getCategories().subscribe(response => {
      this.categories = response;
    }, error => {
      console.log(error);
    })
  }

  addProduct() {
    this.adminService.addProduct(this.addProductForm.value).subscribe(response =>{
      this.addProductForm.reset();
      this.addProductForm.get("category").get("id").setValue(0);
    }, error => {
      console.log(error);
    })

  }

  addCategory(){
    this.newCategory.name = this.categoryToAdd;
    this.adminService.addCategory(this.newCategory).subscribe(response => {
      this.categoryToAdd = '';
      this.getCategories();
    }, error => {
      console.log(error);
    })
  }

  setCategoryInForm(e: any) {
    this.categories.forEach(category => {
      if(category.name === e) {
        this.addProductForm.get("category").setValue(category);
      }
    });
    
  }

  showFormValue() {
    console.log(this.addProductForm.value);
  }
}
