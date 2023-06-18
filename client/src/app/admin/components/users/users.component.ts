import { Component, OnInit, ViewChild } from '@angular/core';
import { AdminService } from '../../admin.service';
import { PaginatedResponse, Pagination } from 'src/app/shared/models/Pagination';
import { GenericTableComponent } from '../generic-table/generic-table.component';
import { UserForAdmin } from '../../models/UserForAdmin';


@Component({
  selector: 'app-admin-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit{
  users: UserForAdmin[] = [];
  pagination: Pagination = {} as Pagination;
  @ViewChild('table') genericTable?: GenericTableComponent;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers() {
    this.adminService.getUsers().subscribe({
      next: (users: PaginatedResponse<UserForAdmin[]>) => {
        this.users = users.data.sort((a, b) => a.id - b.id);
        this.pagination.pageIndex = users.pageIndex;
        this.pagination.pageSize = users.pageSize;
        this.pagination.count = users.count;
        this.pagination.totalPages = Math.ceil(users.count / users.pageSize);
        this.adminService.setPageParams(1, 10)
      }, complete: () => {
        this.genericTable?.setTableValues(this.users);
      }
    });
  }

  onPageChanged(event: any) {
    if (this.pagination.pageIndex !== event){
      this.pagination.pageIndex = event;
      this.adminService.setPageParams(this.pagination.pageIndex, this.pagination.pageSize);
      this.getUsers();
    }
  }
}
