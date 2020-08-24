import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { CartService } from '../_services/cart.service';
import { ProductService } from '../_services/product.service';
import { Observable } from 'rxjs';
import { Cart } from '../_models/cart';
import { ShippingService } from '../_services/shipping.service';
import { Shipping } from '../_models/shipping';
import { AlertifyService } from '../_services/alertify.service';
import { OrderService } from '../_services/order.service';
import { Order } from '../_models/order';
import { ProductItem } from '../_models/productItem';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart-summary',
  templateUrl: './cart-summary.component.html',
  styleUrls: ['./cart-summary.component.css'],
})
export class CartSummaryComponent implements OnInit {
  cart$: Observable<Cart>;
  shipping: Shipping;
  order: Order;
  productItems: ProductItem[];

  constructor(
    private authService: AuthService,
    private cartService: CartService,
    private shippingService: ShippingService,
    private alertify: AlertifyService,
    private orderService: OrderService,
    private router: Router
  ) {
  }

  ngOnInit() {
    this.cart$ = this.cartService.currrentCart;
    this.cartService.currrentCart.subscribe(next => {
       this.productItems = next.cartItems ? next.cartItems : [] ;
    });
    this.shippingService.getPreferredShipping(this.authService.decodedToken.nameid).subscribe(next => {
      this.shipping = next;
    }, error => this.alertify.error('Could not get shipping address'));
  }

  placeOrder() {
    this.order =  {
      id: null,
      status: null,
      userId: this.authService.decodedToken.nameid,
      productItems: this.productItems,
      orderShipping: this.shipping,
      orderShippingId: null,
      };

    this.orderService.createOrder(this.authService.decodedToken.nameid, this.order).subscribe(next => {
      this.alertify.success('Order placed successfully');
      this.router.navigate(['/order-success', next.id]);
    }, error => this.alertify.error('Could not place order'));
  }
}
