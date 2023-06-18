import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {    
  selectedMenu: string = 'Products';
  constructor() { }
  
  ngOnInit(): void {
  }

  changeMenu(menu: string) {
    this.selectedMenu = menu;
  }
}