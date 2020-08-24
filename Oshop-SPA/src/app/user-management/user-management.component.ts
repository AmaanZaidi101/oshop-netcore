import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { AdminService } from '../_services/admin.service';
import { User } from '../_models/user';
import { Pagination } from '../_models/pagination';
import { ActivatedRoute } from '@angular/router';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from '../roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css'],
})
export class UserManagementComponent implements OnInit {
  users: User[];
  bsModalRef: BsModalRef;
  pagination: Pagination;
  pagingParams: any = {};
  rolesList = [
    { value: null, display: 'All' },
    { value: 'Customer', display: 'Customer' },
    { value: 'Moderator', display: 'Moderator' },
    { value: 'Admin', display: 'Admin' },
  ];
  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private adminService: AdminService,
    private route: ActivatedRoute,
    private bsModalService: BsModalService
  ) {}

  ngOnInit() {
    this.pagingParams.role = null;
    this.route.data.subscribe(data => {
      this.users = data['users'].result;
      this.pagination = data['users'].pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getUsersWithRoles();
  }

  getUsersWithRoles() {
    this.adminService
      .getUsersWithRoles(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.pagingParams.role,
        this.pagingParams.userName
      )
      .subscribe(
        (next) => {
          this.users = next.result;
          this.pagination = next.pagination;
        },
        (error) => this.alertify.error(error)
      );
  }

  editRolesModal(user: User) {
    const initialState = {
      user,
      roles: this.getRolesArray(user)
    };
    this.bsModalRef = this.bsModalService.show(RolesModalComponent, {initialState});
    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      const rolesToUpdate = {
        roleNames: [...values.filter(el => el.checked === true).map(el => el.name)]
      };
      if (rolesToUpdate) {
        this.adminService.updateUserRoles(user.id, rolesToUpdate).subscribe(() => {
          user.roles = [...rolesToUpdate.roleNames];
        }, error => {
          this.alertify.error(error);
        });
      }
    });
  }

  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
    { name: 'Customer', value: 'Customer', checked: false },
    { name: 'Moderator', value: 'Moderator', checked: false },
    { name: 'Admin', value: 'Admin', checked: false }
    ];

    for (const availableRole of availableRoles) {
      const element = userRoles.find(x => x === availableRole.value);
      if (element) {
        roles.push({name: availableRole.name, value: availableRole.value, checked: true});
      } else {
        roles.push({name: availableRole.name, value: availableRole.value, checked: false});
      }
    }
    return roles;
  }
}
