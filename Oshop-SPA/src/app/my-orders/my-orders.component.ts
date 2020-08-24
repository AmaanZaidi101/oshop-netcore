import { Component, OnInit } from '@angular/core';
import { Order } from '../_models/order';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { OrderService } from '../_services/order.service';
import { ActivatedRoute } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.css'],
})
export class MyOrdersComponent implements OnInit {
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
      this.orders = data['orders'].result;
      this.pagination = data['orders'].pagination;
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
      .getOrders(
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
