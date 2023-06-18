import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/shared/models/User';
import { OrderForAdmin } from '../../models/OrderForAdmin';
import { Product } from 'src/app/shop/models/Product';

@Component({
  selector: 'app-generic-table',
  templateUrl: './generic-table.component.html',
  styleUrls: ['./generic-table.component.scss']
})
export class GenericTableComponent implements OnInit {
  @Input() data: User[] | OrderForAdmin[] | Product[];
  photoUrls: string[] = [];
  tableKeys: string[] = [];
  tableValues: string[][] = [];

  constructor() { }

  ngOnInit(): void {
    this.setTableKeys();
    this.setTableValues();
  }
  
  setTableKeys() {
    let allKeys = Object.keys(this.data[0]);
    this.tableKeys = allKeys.filter(key => !(this.data[0][key] instanceof Object) && !(this.data[0][key] instanceof Array) && !key.includes("photo") && !key.includes("description") && !key.includes("Json"));
  }

  setTableValues(data: object[] = this.data) {
    if(this.tableValues.length > 0) {
      this.tableValues = [];
      this.photoUrls = [];
    }
    data.forEach((element, index) => {
      this.tableValues[index] = [];
      if (Object.keys(this.data[0]).some(key => key === "photoUrl")) this.photoUrls.push(element["photoUrl"]);
      for (let i = 0; i < this.tableKeys.length; i++) {
        if(!(element[this.tableKeys[i]] instanceof Object) && !(element[this.tableKeys[i]] instanceof Array)) {
          this.tableValues[index][i] = element[this.tableKeys[i]];
        }
      }
    })
  }

  getType(value: any, index: number) {
    return typeof value;
  }
}
