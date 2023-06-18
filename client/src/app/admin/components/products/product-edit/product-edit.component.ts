import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/admin/admin.service';
import { Review } from 'src/app/shared/models/Review';
import { Product } from 'src/app/shop/models/Product';
import { ProductCategory } from 'src/app/shop/models/ProductCategory';
import { ProductsService } from 'src/app/shop/products.service';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  product: Product;
  productUpdateForm: FormGroup;
  categories: ProductCategory[];
  newComment: string;

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.productUpdateForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private productService: ProductsService, private adminService: AdminService, 
    private fb: FormBuilder, private route: ActivatedRoute, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.getProductById();
    this.getCategories();
  }
  
  getProductById() {
    let id = +this.route.snapshot.paramMap.get('id');
    this.productService.getProductById(id).subscribe({
      next: (product) => {
        this.product = product;
        this.product.photos.sort(a => a.isMain ? -1 : 1);
      }, 
      error: (error) => {
        this.toastr.error(error)
      },
      complete: () => {
        this.productUpdateForm.patchValue(this.product);
      }
    })
  }
  
  getCategories() {
    this.productService.getCategories().subscribe({
      next: (categories: ProductCategory[]) => {
        this.categories = categories;
      },
      error: (error) => {
        this.toastr.error(error);
      }
    })
  }

  initializeForm() {
    this.productUpdateForm = this.fb.group({
      id: [0,Validators.required],
      name: ['',Validators.required],
      category: this.fb.group({
        id: [0],
        name: ['',Validators.required]
      }),
      price: ['',Validators.required],
      description: ['',Validators.required],
      inStock: ['',Validators.required],
      soldItems: ['',Validators.required],
    })
  }

  getFormControl(name) { 
    return this.productUpdateForm?.get(name) as FormControl;
  }
  
  getCategoryGroup() { 
    return this.productUpdateForm?.get("category") as FormGroup;
  }

  setCategory(category: ProductCategory) {
    this.getCategoryGroup().patchValue(category);
  }

  updateProduct() {
    this.adminService.updateProduct(this.productUpdateForm.value).subscribe({
      next: () => {
        this.toastr.success("Product updated successfully");
        this.product = this.productUpdateForm.value;
      },
      error: (error) => {
        this.toastr.error(error);
      },
      complete: () => {
        this.productUpdateForm.reset(this.product);
      }
    })
  }

  deleteProduct() {
    this.adminService.deleteProduct(this.product.id).subscribe({
      next: () => {
        this.toastr.success("Product deleted successfully");
      },
      error: (error) => {
        this.toastr.error(error);
      },
      complete: () => {
        this.router.navigateByUrl("/admin/products");
      }
    })
  }

  addReview() {
    if(!this.newComment) { 
      this.toastr.error("Please enter a comment first");
      return;
    }

    let review = new Review(this.newComment, this.product.id);

    this.productService.addReviewToProduct(review).subscribe({
      next: (review: Review) => {
        this.toastr.success("Review added successfully");
        this.product.reviews.push(review);
        this.newComment = '';
      },
      error: (error) => {
        this.toastr.error(error.message);
      }
    })
  }
}
