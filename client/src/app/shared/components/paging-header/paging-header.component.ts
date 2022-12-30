import { Component, Input, OnInit } from '@angular/core';
import { Pagination } from 'src/app/models/Pagination';

@Component({
  selector: 'app-paging-header',
  templateUrl: './paging-header.component.html',
  styleUrls: ['./paging-header.component.scss']
})
export class PagingHeaderComponent implements OnInit {
  @Input() pagination: Pagination;
  
  constructor() { }

  ngOnInit(): void {
  }

}
