import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Product } from 'src/app/models/Product';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  baseUrl = environment.apiUrl;
  @Input() product: Product;
  uploader: FileUploader;
  hasBaseDropzoneOver = false;

  constructor() { }

  ngOnInit(): void {
    this.initializeUploader();
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
