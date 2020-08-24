import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, ActivatedRoute, Router } from '@angular/router';
import { Product } from '../_models/product';
import { Observable, of } from 'rxjs';
import { AlertifyService } from '../_services/alertify.service';
import { ProductService } from '../_services/product.service';
import { CategoryService } from '../_services/category.service';
import { AuthService } from '../_services/auth.service';
import { catchError } from 'rxjs/operators';
import { Cart } from '../_models/cart';
import { CartService } from '../_services/cart.service';

@Injectable()

export class CartResolver implements Resolve<Cart> {

    constructor(
        private authService: AuthService,
        private categoryService: CategoryService,
        private productService: ProductService,
        private alertify: AlertifyService,
        private router: Router,
        private cartService: CartService
      ) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Cart> {
        return this.cartService.currrentCart;
    }
}
