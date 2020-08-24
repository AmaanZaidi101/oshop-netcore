import { Component, OnInit } from '@angular/core';
import { Order } from '../_models/order';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from '../_services/order.service';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.css'],
})
export class OrderDetailComponent implements OnInit {
  orderId: string;
  order: Order = new Order();

  constructor(
    public authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private orderService: OrderService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {

    this.orderId = this.route.snapshot.paramMap.get('id');

    this.route.data.subscribe(data => {
      this.order = data['order'];
    });

    if (!this.order || !this.order.id) {
      this.alertify.error('Could not find order');
      this.router.navigate(['home']);
    }
  }

  openDialog() {
    this.alertify.confirm('Are you sure you want to cancel your order?', () => {
      this.orderService.cancelOrder(this.authService.decodedToken.nameid, this.order.id).subscribe(() => {
        this.alertify.message('Order deleted successfully');
        this.router.navigate(['my/orders']);
      }, error => this.alertify.error('Could not delete order'));
    });
  }

  openUpdateDialog(status: string) {
    this.alertify.confirm('Are you sure you want to mark this order as ' + status + ' ?', () => {
      this.orderService.updateOrder(this.authService.decodedToken.nameid, this.order.id, status).subscribe(() => {
        this.alertify.message('Order marked as ' + status);
        this.router.navigate(['admin/orders']);
      }, error => this.alertify.error('Could not change order status'));
    });
  }

}
