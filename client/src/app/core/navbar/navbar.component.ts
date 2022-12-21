import { Component, HostListener, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/models/Basket';
import { User } from 'src/app/models/User';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  currentUser$: Observable<User>;
  basket$: Observable<IBasket>;
  windowWidth: number = window.innerWidth;

  @HostListener('window:resize', ['$event'])  resizeImage() {
    this.windowWidth = window.innerWidth;
  }

  constructor(private accountService: AccountService,
     private basketService: BasketService) { }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
    this.basket$ = this.basketService.basket$
  }

  logout() {
    return this.accountService.logout();
  }

}
