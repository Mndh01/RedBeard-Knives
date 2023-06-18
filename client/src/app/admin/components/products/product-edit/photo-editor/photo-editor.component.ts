import { HttpParams } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from 'src/app/admin/admin.service';
import { Photo } from 'src/app/shared/models/Photo';
import { Product } from 'src/app/shop/models/Product';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss']
})
export class PhotoEditorComponent implements OnInit {
  baseUrl = environment.apiUrl;
  @Input() product: Product;
  photo: Photo;
  photosToAdd: File[] = [];
  params = new HttpParams();
  uploader: FileUploader;
  hasBaseDropzoneOver = false;

  constructor(private adminService: AdminService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    // this.initializeUploader();
  }

  setParams() {
    this.params = this.params.set("productId", this.product.id)
    this.params = this.params.set("photoId", this.photo.id);
  }
  
  setMainPhoto(photo:Photo) {
    this.photo = photo;
    this.setParams();
    this.adminService.updateMainPhoto(this.params).subscribe({
      next: () => {
        this.product.photos.forEach(p => {
          if(p.isMain) p.isMain = false;
          if(p.id == photo.id) p.isMain = true;
          this.product.photos.sort(a => a.isMain ? -1 : 1);
        })
        this.toastr.success("Main photo updated successfully");
      },
      error: (error) => {
        this.toastr.error(error);
      },
      complete: () => {
      }
    })
  }
  
  setPhotos(e: any) {
    this.photosToAdd = e.target.files;
  }

  deletePhoto(photo:Photo) {
    this.photo = photo;
    this.setParams();
    this.adminService.deletePhoto(this.params).subscribe({
      next: () => {
        this.product.photos = this.product.photos.filter(p => p.id != photo.id);
        this.toastr.success("Photo deleted successfully");
      },
      error: (error) => {
        this.toastr.error(error);
      },
      complete: () => {
      }
    })
  }

  addPhotos() {
    if(this.photosToAdd.length == 0) {
      this.toastr.error("Please select some photos first");
      return; 
    }
    
    this.adminService.addPhotos(this.photosToAdd, this.product.id).subscribe({
      next: (photos: Photo[]) => {
        photos.forEach(p => this.product.photos.push(p));
        this.toastr.success("Photos added successfully");
      },
      error: (error) => {
        this.toastr.error(error);
      }
    })
  }
}
