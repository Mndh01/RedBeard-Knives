import { AfterViewInit, Component, ElementRef, HostListener, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { CarouselConfig } from 'ngx-bootstrap/carousel';
import { Product } from 'src/app/shared/models/Product';
import { ProductsService } from 'src/app/services/products.service';
import { OwlOptions } from 'ngx-owl-carousel-o';
import { Subscription, timer } from 'rxjs';

@Component({
  selector: 'app-new-products',
  templateUrl: './new-products.component.html',
  styleUrls: ['./new-products.component.scss']
})
export class NewProductsComponent implements OnInit, AfterViewInit  {
  @ViewChild("stage") stageRef: ElementRef<HTMLElement>;
  @ViewChildren("slide") slidesQuery: QueryList<ElementRef<HTMLElement>>;
  windowWidth: number = window.innerWidth;
  //TODO: Make the slides observable and display the slides depending on its next value to avoid using setTimeout
  slides: { id:number; image: string; text?: string }[] = [];
  products: Product[]
  activeSlideIndex: number;
  interval: number = 1000;
  btnClicked: boolean = false;
  counter: number = 0;
  timer = timer(0, this.interval);
  
  @HostListener('window:resize', ['$event'])  resizeImage() {
    this.windowWidth = window.innerWidth;
  }

  // "I'm a teapot",
  // "Short and stout",
  // "Here is my handle",
  // "Here is my spout",
  // "When I get all steamed up",
  // "Hear me shout",
  // "Tip me over and pour me out!"
  
  constructor(private productService: ProductsService, private carouselConfig: CarouselConfig) { 
  }
  
  ngOnInit(): void {
    this.getProducts();
    this.addSlides();
  } 
  
  ngAfterViewInit(): void {
    this.activeSlideIndex = Math.floor(this.slides.length / 2) - 1;
    //ToLearn: why is slidesQuery not defined in moveSlides() here, while it is defined if moveSlides is called in the timer.subscribe() below?
    // this.moveSlides();
    // moveSteps() is called every time the timer emits a value with the same parameters everytime
    this.timer.subscribe(() => {
      if(this.counter % 5 == 0){
        this.counter = 0;
        this.moveSteps(1);
      }      
      this.counter++;
    });
  }

  preventMultipleClicks(steps: number): void {
    if(this.btnClicked == false){
      this.btnClicked = true;
      this.moveSteps(steps)
      setTimeout(() => {
        this.btnClicked = false;
      }, 500);  
    }
    else return;
  }
  
  getProducts(): void {
    this.productService.getProducts().subscribe(data => {
      this.products = data;
      //ToLearn: why is the method below not running when we subscribe to the observable?
      // this.addSlides();
    });
  }
  
  addSlides(): void {
    for (let i = 0; i < 10; i++) {
      this.slides.push({
        id: i,
        image: "assets/Toga.png"
      });
    }
    // this.products.forEach((product, i = 0) => {
    //   this.slides.push({ id: i, image: product.photoUrl, text: product.name});
    //   i++;
    // });
    }
    
  moveSlides(): void {
    const arrayHalf = Math.floor(this.slides.length / 2);
    this.stageRef.nativeElement.setAttribute("style", "transform: translateX(-"+ Math.ceil(arrayHalf * 80) +"%);");
    
      for(let i = 0; i < this.slidesQuery.length; i++) {
        const element = this.slidesQuery.toArray()[i].nativeElement as HTMLElement;
        const ElemId = Number(element.getAttribute("id"));
        const baseStyle = "width: 80%; height: 95%; display: flex; transform:\
        translateX("+ (100 * this.calcTransition(arrayHalf + ElemId)) +"%);\
        transition:height .5s ease-in-out, transform .5s ease-in-out, opacity .25s ease-in-out;";
        element.style.cssText = baseStyle;

        if(this.slides.length <= 3) {
          element.style.width = "85%"
          this.stageRef.nativeElement.setAttribute("style", "transform: translateX(-"+ Math.ceil(arrayHalf * 85) +"%);");
        }

        if(ElemId === this.calcTransition(this.activeSlideIndex*2)) {
          element.style.transform = "translateX(" + 100 * this.calcTransition(arrayHalf + ElemId) + "%) scale(1.1)";
        }

        if(this.slides.length % 2 == 0){
          if(ElemId === this.calcTransition(this.activeSlideIndex*2 + arrayHalf - 1) ||
            ElemId === this.calcTransition(this.activeSlideIndex*2 - arrayHalf) ) {
            element.style.opacity = "0";
          }        
        }
        else {
          if(ElemId === this.calcTransition(this.activeSlideIndex*2 + arrayHalf) ||
            ElemId === this.calcTransition(this.activeSlideIndex*2 - arrayHalf) ) {
            element.style.opacity = "0";
          }
        }
        
        
      }
  }
    
  moveSteps(step: number): void {
    this.calcNextActiveSlide(step);
    this.moveSlides();
    this.counter = 1;
  }

  calcNextActiveSlide(step: number): void {
    let nextActive = this.activeSlideIndex + step;
    if (nextActive < 0) {
      this.activeSlideIndex = this.slides.length - 1;
    }
    else if(nextActive >= this.slides.length) {
      this.activeSlideIndex = nextActive % this.slides.length;
    }
    else {
      this.activeSlideIndex = nextActive;
    }
  }
  
  calcTransition(steps: number): number{
    let nextIndex = steps - this.activeSlideIndex;
    if (nextIndex < 0) {
      nextIndex += this.slides.length;
    }
    else if(nextIndex >= this.slides.length) {
      nextIndex %= this.slides.length;
    }
    return nextIndex;
  }

  removeSlide(index?: number): void {
    const toRemove = index ? index : this.activeSlideIndex;
    this.slides.splice(toRemove, 1);
  }

  setCarouselInterval(interval: number): void {
    this.carouselConfig.interval = interval;
  }

  customOptions: OwlOptions = {
    margin: 25,
    loop: true,
    autoWidth: true,
    mouseDrag: true,
    touchDrag: true,
    pullDrag: true,
    dots: false,
    navSpeed: 700,
    navText: ['', ''],
    responsive: 
    {
      0: {
        items: 1
      },
      400: {
        items: 2
      },
      740: {
        items: 3
      },
      940: {
        items: 4
      },
      1280: {
        items: 5
      },
      1920: {
        items: 6
      }
    },
    nav: true
  }

}