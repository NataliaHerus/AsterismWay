import { Component, OnInit } from '@angular/core';
import { Table } from 'primeng/table';
import { User } from 'src/app/demo/models/user';
import { AuthService } from 'src/app/demo/services/identity services/auth.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  users: User[] = [];
  loading: boolean = true;

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.getAll().subscribe(users => {
        this.users = users;
        this.loading = false;
    });
}

onGlobalFilter(table: Table, event: any) {
  table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
}
}
