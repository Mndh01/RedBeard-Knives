import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket } from 'src/app/models/Basket';
import { User } from 'src/app/models/User';
import { AccountService } from 'src/app/services/account.service';
import { LoginComponent } from 'src/app/shared/components/login/login.component';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  @ViewChild('childModal', { static: false }) childModal?: ModalDirective;
  @ViewChild('LoginComponent', { static: false }) loginComponent?: LoginComponent;
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
    this.basket$ = this.basketService.basket$;
  }

  logout() {
    return this.accountService.logout();
  }
  
  //#region login modal
  showChildModal(): void {
    this.childModal?.show();
  }
 
  hideChildModal(): void {
    this.childModal?.hide();
    this.onModelHide();
  }

  onLogin(event: boolean) {
    if (event) {
      this.hideChildModal();
    }
    else return;
  }

  onModelHide() {
    this.loginComponent?.loginForm.reset();
  }
  //#endregion
}
