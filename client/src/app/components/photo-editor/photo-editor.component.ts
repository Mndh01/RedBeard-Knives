import { HttpParams } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/models/Photo';
import { Product } from 'src/app/models/Product';
import { ProductsService } from 'src/app/services/products.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  baseUrl = environment.apiUrl;
  @Input() product: Product;
  photo: Photo;
  params = new HttpParams();
  uploader: FileUploader;
  hasBaseDropzoneOver = false;

  constructor(private productService: ProductsService) { }

  ngOnInit(): void {
    this.initializeUploader();
  }

  setParams() {
    this.params = this.params.set("productId", this.product.id)
    this.params = this.params.set("photoId", this.photo.id);
  }

  deletePhoto(photo:Photo) {
    this.photo = photo;
    this.setParams();
    this.productService.deletePhoto(this.params).subscribe(() => {
      this.product.photos.slice[photo.id];
    })
  }

  setMainPhoto(photo:Photo) {
    this.photo = photo;
    this.setParams();
    this.productService.updateMainPhoto(this.params).subscribe(response => {
      console.log(response);
    })
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + "products/add-photo?id=" + this.product.id,
      // authToken: 'Bearer ' + this.user.token, // after adding users
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }


    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response) {
        const photo = JSON.parse(response);
        this.product.photos.push(photo);
      }
    }
  }
}
