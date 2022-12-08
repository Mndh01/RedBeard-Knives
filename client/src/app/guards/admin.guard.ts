import { Injectable } from '@angular/core';
import { CanActivate, } from '@angular/router';
import { map } from 'rxjs/operators';
import { AccountService } from '../services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private accountService: AccountService) {}
  canActivate() {
    return this.accountService.currentUser$.pipe(
      map(user => {
        // if(user && user.roles.includes('Admin') || user && user.roles.includes('Moderator')) return true;
        // else{
        //   alert("Not authorized!");
        //   return false;
        // }
        return true;
      }))
  }
      
}