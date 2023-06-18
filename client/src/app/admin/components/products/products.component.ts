import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ProductCategory } from 'src/app/shop/models/ProductCategory';
import { AdminService } from 'src/app/admin/admin.service';
import { ProductsService } from 'src/app/shop/products.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Product } from 'src/app/shop/models/Product';
import { PaginatedResponse, Pagination } from 'src/app/shared/models/Pagination';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AccountService } from 'src/app/user-profile/account.service';
import { User } from 'src/app/shared/models/User';
import { GenericTableComponent } from '../generic-table/generic-table.component';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit{
  baseUrl = environment.apiUrl;
  @ViewChild('productAddModal', { static: false }) productAddModal?: ModalComponent;
  @ViewChild('categoryAddModal', { static: false }) categoryAddModal?: ModalComponent;
  @ViewChild('categorySelect', { static: false }) categorySelect?: ElementRef<HTMLSelectElement>;
  @ViewChild('photosInput', { static: false }) photosInput?: ElementRef<HTMLInputElement>;
  @ViewChild('table') genericTable?: GenericTableComponent;
  user: User;
  productAddForm: FormGroup;
  categories: ProductCategory[];
  products: Product[];
  pagination: Pagination = {} as Pagination;
  categoryToAdd: string;
  photos: File[] = [];
    
  constructor(private adminService: AdminService, private productService: ProductsService,
    private fb: FormBuilder, private accountService: AccountService, private toastr: ToastrService) 
    {
      this.accountService.currentUser$.subscribe(user => { this.user = user; });
    }
  
  ngOnInit(): void {
    this.initializeForm();
    this.getCategories();
    this.getProducts(); // TODO: Pass pageSize as parameter and correct the error showing in console.
  }
  
  getProducts(pageSize: number = 10) {
    let params = this.productService.getShopParams();
    params.pageSize = pageSize;
    this.productService.setShopParams(params);

    this.productService.getProducts(this.user.roles).subscribe({
      next: (products: PaginatedResponse<Product[]>) => {
        this.products = products.data.sort((a, b) => a.id - b.id);
        this.pagination.pageIndex = products.pageIndex;
        this.pagination.pageSize = products.pageSize;
        this.pagination.count = products.count;
        this.pagination.totalPages = Math.ceil(products.count / products.pageSize);
      }, complete: () => {
        this.genericTable?.setTableValues(this.products);
      }
    });
  }

  onPageChanged(event: any) {
    const params = this.productService.getShopParams();
    if (params.pageIndex !== event){
      params.pageIndex = event;
      this.pagination.pageIndex = params.pageIndex;
      this.productService.setShopParams(params);
      this.getProducts();
    }
  }
  
  initializeForm() {
    this.productAddForm = this.fb.group({
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
    return this.productAddForm.get(name) as FormControl;
  }
  
  getFormGroup() { 
    return this.productAddForm.get("category") as FormGroup;
  }

  getCategories() {
    this.productService.getCategories().subscribe({
      next: (categories: ProductCategory[]) => {
      this.categories = categories;
    },
      error: (error) =>{
        this.toastr.error(error);
      }
    })
  }

  addProduct() {
    this.adminService.addProduct(this.productAddForm.value, this.photos).subscribe({
      next: (product) => {
        this.productService.useCache = false;      
        this.productAddModal?.hide();
      }, error: (error) => {
        this.toastr.error(error.error);
      },
      complete: () => {
        this.getProducts();
      }
    })
  }

  setPhotosInForm(e: any) {
    this.photos = e.target.files;
  }

  showFormValue() {
    console.log(this.productAddForm.value);
    console.log(this.photos);
    console.log(this.photosInput);
  }

  showProductModal(): void {
    this.initializeForm();
    this.productAddModal?.show();
  }
  
  onProductModalHide(): void {
    this.resetForm();
  }
  
  resetForm() {
    this.productAddForm.reset();
    this.categorySelect.nativeElement.selectedIndex = 0;
    this.photosInput.nativeElement.value = '';
    this.photos = [];
  }

  addCategory(){
    let newCategory: ProductCategory = {id: 0, name: ""};

    newCategory.name = this.categoryToAdd;
    this.adminService.addCategory(newCategory).subscribe({
      next: (category) => {
        // this.categories.push(category);
        this.toastr.success("Category \"" + newCategory.name.replace(newCategory.name.charAt(0), newCategory.name.charAt(0).toUpperCase()) + "\" added successfully");
        this.categoryToAdd = '';
        this.categoryAddModal?.hide();
      },
      error: (error) => {
        this.toastr.error(error.error);
      },
      complete: () => {
        this.getCategories();
      }
    })
  }

  setCategoryInForm(e: any) {
    this.categories.forEach(category => {
      if(category.name === e.target.value.toLowerCase()) {
        this.productAddForm.get("category").setValue(category);
      }
    });
  }
  
  showCategoryModal(): void {
    this.categoryAddModal?.show();
  }
 
  onCategoryModalHide(): void {
    this.categoryToAdd = '';
  }

}
