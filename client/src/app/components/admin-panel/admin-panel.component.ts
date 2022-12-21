import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AdminService } from 'src/app/services/admin-service.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent implements OnInit {
  addProductForm: FormGroup;
   
  constructor(private adminService: AdminService, private fb: FormBuilder) { }
  
  ngOnInit(): void {
    this.initializeForm();
  }
  
  initializeForm() {
    this.addProductForm = this.fb.group({
      name: ['',Validators.required],
      category: ['',Validators.required],
      price: ['',Validators.required],
      description: ['',Validators.required],
      inStock: ['',Validators.required],
      soldItems: ['',Validators.required]
    })
  }

  getFormControl(name) { 
    return this.addProductForm.get(name) as FormControl;
  }

  addProduct() {
    this.adminService.addProduct(this.addProductForm.value).subscribe(response =>{
      console.log(response);
      this.addProductForm.reset();
    }, error => {
      console.log(error);
    })

    console.log(this.addProductForm.value);
  }

}
