import { Component, OnInit } from '@angular/core';
import { CarouselConfig } from 'ngx-bootstrap/carousel';

@Component({
  selector: 'app-carousel',
  templateUrl: './carousel.component.html',
  styleUrls: ['./carousel.component.css']
})

export class CarouselComponent {
  slides: { image: string; text?: string }[] = [];
  activeSlideIndex = 0;
 
  constructor(private carouselConfig: CarouselConfig) {
    for (let i = 0; i < 4; i++) {
      this.addSlide();
    }
  }
  
  addSlide(): void {
    this.slides.push({
      image: `assets/Old-site/images/image${this.slides.length % 3 + 1}.jpg`
    });
  }
 
  removeSlide(index?: number): void {
    const toRemove = index ? index : this.activeSlideIndex;
    this.slides.splice(toRemove, 1);
  }

  setCarouselInterval(interval: number) {
    this.carouselConfig.interval = interval;
  }
}