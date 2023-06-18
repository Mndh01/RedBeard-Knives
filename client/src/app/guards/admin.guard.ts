import { Injectable } from '@angular/core';
import { CanActivate, Router, } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { map, switchMap, take } from 'rxjs/operators';
import { AccountService } from '../user-profile/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  private token = localStorage.getItem('token');
  constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService) {}
  
  canActivate(): Observable<boolean>{
    return this.accountService.loadCurrentUser(this.token).pipe(
      switchMap(user => {
        if(user?.roles?.includes('Admin') || user?.roles?.includes('Moderator'))
          return of(true);
        this.toastr.error("You are not authorized to enter here!");
        this.router.navigateByUrl('/');
        return of(false);
      }))
  }
}