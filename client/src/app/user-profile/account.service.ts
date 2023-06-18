import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { User } from '../shared/models/User';

@Injectable({
  providedIn: 'root'
})

export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);
    
    return this.http.get<User>(this.baseUrl + 'account', {headers}).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
          return user;
        }
        return null;
      })
    )
  }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + "account/login", model).pipe(
      map((user: User) => {
        if(user){
          console.log(user);
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + "account/register", model).pipe(
      map((user: User) => {
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  updateUser(user: User) {
    return this.http.put(this.baseUrl + 'users', user).pipe(
      map(() => {
        this.currentUserSource.next(user);
      })
    )
  }

  changePassword(model: any) {
    return this.http.put<boolean>(this.baseUrl + 'account/change-password', model);
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  setCurrentUser(user: User) {
    localStorage.setItem('token', user.token);
    this.currentUserSource.next(user);
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
  }

  getDecodedToken(token) {
    return JSON.parse(atob(token.split('.')[1]));
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/email-exists?email=' + email);
  }
}
