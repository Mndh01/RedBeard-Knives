import { Component, Input, OnInit } from '@angular/core';
import { Photo } from 'src/app/shared/models/Photo';

@Component({
  selector: 'app-image-slide',
  templateUrl: './image-slide.component.html',
  styleUrls: ['./image-slide.component.scss']
})
export class ImageSlideComponent implements OnInit {
  photoIndex: number = 0;
  @Input() photos: Photo[] = [];
  constructor() { }

  ngOnInit(): void {
  }

  nextPhoto(): void {
    if (this.photoIndex < this.photos.length - 1) {
      this.photoIndex++;
    }
    else {
      this.photoIndex = 0;
    }
  }

  previousPhoto(): void {
    if (this.photoIndex > 0) {
      this.photoIndex--;
    }
    else {
      this.photoIndex = this.photos.length - 1;
    }
  }
}
