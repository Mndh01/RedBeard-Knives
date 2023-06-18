import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/user-profile/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  @Output() success: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.createLoginForm();
  }

  resetForm() {
    debugger
    this.loginForm.reset();
    console.log("Form reset");
  }

  createLoginForm() {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]),
      password: new FormControl('', [Validators.required, Validators.minLength(6)])
    });
  }

  onSubmit() {
    this.accountService.login(this.loginForm.value).subscribe({
      next: () => {
        this.success.emit(true);
        this.router.navigateByUrl('/');
        this.loginForm.reset();
      }, 
      error: (error) => {
        console.log(error);
        this.success.emit(false);
        this.loginForm.get('password').reset();
      }
    });
  }
}