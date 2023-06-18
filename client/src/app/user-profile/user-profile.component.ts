import { Component, OnInit } from '@angular/core';
import { AccountService } from './account.service';
import { User } from '../shared/models/User';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  user: User;
  selectedMenu: number = 1;
  isDisabled = 'disabled';

  constructor(private accountService: AccountService) {
      this.accountService.currentUser$.subscribe(user => { this.user = user; });
    }

  ngOnInit(): void {
  }

  changeSelectedMenu(menu: number) {
    this.selectedMenu = menu;
  }
}
