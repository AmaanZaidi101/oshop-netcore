import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CartService } from './_services/cart.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Oshop';
  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthService, private cartService: CartService, private titleService: Title) {}

  ngOnInit() {
    this.titleService.setTitle(this.title);
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if (user) {
      this.authService.currentUser = JSON.parse(user);
      if (this.authService.currentUser.cart) {
        this.cartService.updateCurrentCart(this.authService.currentUser.cart);
      }
    }
  }
}
