import { Injectable, Output, ViewChild } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../user-profile/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  private token = localStorage.getItem('token');
  
  constructor(private router: Router, private toastr: ToastrService) {}

  canActivate(): Observable<boolean> {
    if(this.token) {
      return of(true);
    }

    this.toastr.error("You must login first");
    this.router.navigateByUrl('/');
    return of(false);

    // return this.accountService.currentUser$.pipe(
    //   map(user => {
    //     if(user) return true;
    //     this.toastr.error("You must login first");
    //     this.router.navigateByUrl('/');
    //     return false;
    //   })
    // )
  }
}
