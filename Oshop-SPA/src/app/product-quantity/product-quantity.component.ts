import { Component, OnInit, Input } from '@angular/core';
import { Cart } from '../_models/cart';
import { Product } from '../_models/product';
import { AuthService } from '../_services/auth.service';
import { CartService } from '../_services/cart.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-product-quantity',
  templateUrl: './product-quantity.component.html',
  styleUrls: ['./product-quantity.component.css'],
})
export class ProductQuantityComponent implements OnInit {
  @Input() cart: Cart;
  @Input() product: Product;
  constructor(
    private authService: AuthService,
    public cartService: CartService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {}

  removeFromCart() {
    this.cartService.removeFromCart(this.authService.decodedToken.nameid, this.product).subscribe(next => {
    }, error => this.alertify.error('Could not remove from cart'));
  }

  addToCart() {
    this.cartService.addToCart(this.authService.decodedToken.nameid, this.product).subscribe(next => {
    }, error => this.alertify.error('Could not add to cart'));
  }
}
