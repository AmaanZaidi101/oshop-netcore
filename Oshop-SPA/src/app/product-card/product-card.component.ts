import { Component, OnInit, Input } from '@angular/core';
import { Product } from '../_models/product';
import { Cart } from '../_models/cart';
import { CartService } from '../_services/cart.service';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.css']
})
export class ProductCardComponent implements OnInit {

  @Input() product: Product;
  @Input() cart: Cart;
  @Input() showActions: boolean;
  constructor(
    private authService: AuthService,
    public cartService: CartService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {
  }

  addToCart() {
    this.cartService.addToCart(this.authService.decodedToken.nameid, this.product).subscribe(next => {
    }, error => this.alertify.error('Could not add to cart'));


  }
}
