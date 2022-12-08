import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { User } from '../shared/models/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private curreUserSource = new BehaviorSubject<User>(null);
  currentUser$ = this.curreUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  getCurrentUserValue() {
    return this.curreUserSource.value;
  }

  loadCurrentUser(token: string) {
    let headers = new HttpHeaders();
    let params = new HttpParams();

    headers = headers.set('Authorization', `Bearer ${token}`);
    params = params.set('id', 1);

    return this.http.get<User>(this.baseUrl + 'users' , {headers}).pipe(
      map((user: User) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.curreUserSource.next(user);
        }
      })
    )
  }

  login(values: any) {
    return this.http.post<User>(this.baseUrl + "account/login", values).pipe(
      map((user: User) => {
        if(user){
          localStorage.setItem('token', user.token);
          this.curreUserSource.next(user);
        }
      })
    )
  }

  register(values: any) {
    return this.http.post<User>(this.baseUrl + "account/register", values).pipe(
      map((user: User) => {
        if(user){
          localStorage.setItem('token', user.token);
          this.curreUserSource.next(user);
          console.log(values);
        }
      })
    )
  }

  logout() {
    localStorage.removeItem('token');
    this.curreUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/email-exists?email=' + email);
  }
}
