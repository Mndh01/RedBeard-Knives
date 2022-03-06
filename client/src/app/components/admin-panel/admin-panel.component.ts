import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AdminService } from 'src/app/services/admin-service.service';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
  addProductForm: FormGroup;
   
  constructor(private adminService: AdminService, private fb: FormBuilder) { }
  
  ngOnInit(): void {
    this.initializeForm();
  }
  
  initializeForm() {
    this.addProductForm = this.fb.group({
      category: ['',Validators.required],
      price: ['',Validators.required],
      inStock: ['',Validators.required],
      soldItems: ['',Validators.required]
    })
  }

  addProduct() {
    this.adminService.addProduct(this.addProductForm.value).subscribe(response =>{
      console.log(response);
    }, error => {
      console.log(error);
    })

    console.log(this.addProductForm.value);
  }

}
