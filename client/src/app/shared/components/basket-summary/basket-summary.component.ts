import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IBasketItem } from 'src/app/basket/Basket';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-basket-summary',
  templateUrl: './basket-summary.component.html',
  styleUrls: ['./basket-summary.component.scss']
})
export class BasketSummaryComponent {
  @Output() incrementItem = new EventEmitter<IBasketItem>();
  @Output() decrementItem = new EventEmitter<IBasketItem>();
  @Output() removeItem = new EventEmitter<IBasketItem>();
  @Input() isBasket = true;
  constructor(public basketService: BasketService) { }

  incrementItemQuantity(item: IBasketItem) {
    this.incrementItem.emit(item);
  }

  decrementItemQuantity(item: IBasketItem) {
    this.decrementItem.emit(item);
  }

  removeBasketItem(item: IBasketItem) {
    this.removeItem.emit(item);
  }

}
