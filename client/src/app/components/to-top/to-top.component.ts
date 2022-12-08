import { Component, HostListener, ElementRef } from '@angular/core';
import { ViewportScroller
 } from '@angular/common';

@Component({
  selector: 'app-to-top',
  templateUrl: './to-top.component.html',
  styleUrls: ['./to-top.component.scss']
})

export class ToTopComponent  {
  pageYoffset = 0;
  @HostListener('window:scroll', ['$event']) onScroll(event){
    this.pageYoffset = window.pageYOffset;
  }

    constructor(private scroll: ViewportScroller) { }

    ngOnInit(): void 
    {
      
    }

  scrollToTop(){
    this.scroll.scrollToPosition([0,0]); 
  }
}