import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Cart } from '../_models/cart';
import { CartService } from '../_services/cart.service';
import { AuthService } from '../_services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cart$: Observable<Cart>;

  constructor(
    public cartService: CartService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
    this.cart$ = this.cartService.currrentCart;
  }

  clearCart() {
    this.cartService
      .clearCart(this.authService.decodedToken.nameid)
      .subscribe((x) => {
        this.alertify.message('Cart Cleared');
        this.router.navigate(['/products']);
      });
  }
}
