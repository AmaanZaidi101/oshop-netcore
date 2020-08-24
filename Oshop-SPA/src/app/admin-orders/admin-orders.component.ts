import { Component, OnInit } from '@angular/core';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { OrderService } from '../_services/order.service';
import { ActivatedRoute } from '@angular/router';
import { Order } from '../_models/order';

@Component({
  selector: 'app-admin-orders',
  templateUrl: './admin-orders.component.html',
  styleUrls: ['./admin-orders.component.css']
})
export class AdminOrdersComponent implements OnInit {

  orders: Order[];
  statusList = [
    { value: 'pending', display: 'Pending' },
    { value: 'confirmed', display: 'Confirmed' },
    { value: 'delivered', display: 'Delivered' },
    { value: 'rejected', display: 'Rejected' },
  ];

  orderParams: any = {};
  pagination: Pagination;
  bsConfig: Partial<BsDatepickerConfig>;
  minOrderDate: Date = new Date();
  maxOrderDate: Date;

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private orderService: OrderService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.minOrderDate.setDate(new Date().getDate() - 100);
    this.maxOrderDate = new Date();
    this.orderParams.minDate = new Date();
    this.orderParams.minDate.setDate(this.orderParams.minDate.getDate() - 60);
    this.orderParams.maxDate = new Date();
    this.orderParams.status = 'pending';
    this.orderParams.isDescending = true;

    this.route.data.subscribe((data) => {
      this.orders = data['adminOrders'].result;
      this.pagination = data['adminOrders'].pagination;
    });
  }

  resetFilters() {
    this.orderParams.status = 'pending';
    this.orderParams.orderId = '';
    this.orderParams.isDescending = true;
    this.orderParams.minDate = new Date();
    this.orderParams.minDate.setDate(this.orderParams.minDate.getDate() - 60);
    this.orderParams.maxDate = new Date();
    this.loadOrders();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadOrders();
  }

  loadOrders() {
    if (!this.orderParams.orderId) {
      this.orderParams.orderId = '';
    }
    if (this.orderParams.minDate > this.orderParams.maxDate) {
      this.orderParams.minDate = this.orderParams.maxDate;
    }
    this.orderService
      .getAllOrders(
        this.authService.decodedToken.nameid,
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.orderParams
      )
      .subscribe(
        (res: PaginatedResult<Order[]>) => {
          this.orders = res.result;
          this.pagination = res.pagination;
        },
        (error) => this.alertify.error(error)
      );
  }

}
