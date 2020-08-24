import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';
import { Cart } from '../_models/cart';
import { CartService } from '../_services/cart.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  cart$: Observable<Cart>;

  constructor(public authService: AuthService, private alertify: AlertifyService,
              private router: Router, public cartService: CartService) {}

  ngOnInit() {
    this.cart$ = this.cartService.currrentCart;
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.cartService.getCart(this.authService.decodedToken.nameid);
      this.alertify.success('Logged in Successfully');
    }, error => {
      this.alertify.error('Could not log in');
    }, () => {
      this.router.navigate(['products']);
    });
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alertify.message('Logged out');
    this.router.navigate(['/home']);
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

}
