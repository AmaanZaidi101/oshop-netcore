import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, ActivatedRoute, Router } from '@angular/router';
import { Product } from '../_models/product';
import { Observable, of } from 'rxjs';
import { AlertifyService } from '../_services/alertify.service';
import { ProductService } from '../_services/product.service';
import { CategoryService } from '../_services/category.service';
import { AuthService } from '../_services/auth.service';
import { catchError } from 'rxjs/operators';

@Injectable()

export class ProductsResolver implements Resolve<Product[]> {

    constructor(
        private authService: AuthService,
        private categoryService: CategoryService,
        private productService: ProductService,
        private alertify: AlertifyService,
        private router: Router
      ) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Product[]> {
        return this.productService.getAllProducts().pipe(
            catchError(error => {
                this.alertify.error('Problem retreiving data');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
