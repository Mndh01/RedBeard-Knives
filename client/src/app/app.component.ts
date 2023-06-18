import { Component, OnInit } from '@angular/core';
import { BasketService } from './basket/basket.service';
import { AccountService } from './user-profile/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'RedBeared Smithery';
  
  constructor(private accountService: AccountService,
     private basketService: BasketService) {}
  
  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadBasket()
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    if (token) {
      this.accountService.loadCurrentUser(token).subscribe(() => {
        console.log('loaded user'); // TODO: remove
      }, error => {
        console.log(error); // TODO: replace with toastr (notification)
      });
    }
  }

  loadBasket() {
    const basketId = localStorage.getItem('basket_id');
    if(basketId) {
      this.basketService.getBasket(basketId).subscribe(() => {
        console.log('initialised basket'); // TODO: remove
      }, error => {
        console.log(error); // TODO: replace with toastr (notiffication)
      })
    }
  }

}