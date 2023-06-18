import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { User } from 'src/app/shared/models/User';
import { AccountService } from 'src/app/user-profile/account.service';
import { LoginComponent } from 'src/app/shared/components/login/login.component';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { ModalComponent } from 'src/app/shared/components/modal/modal.component';
import { IBasket } from 'src/app/basket/Basket';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  @ViewChild('loginModal') loginModal: ModalComponent;
  @ViewChild(LoginComponent) loginComponent: LoginComponent;
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
    this.loginModal?.show();
  }

  onLogin(event: boolean) {
    if (event) {
      this.loginModal?.hide();
    }
    else return;
  }

  onModalHide() {
    this.loginComponent?.resetForm();   
  }
  //#endregion
}
