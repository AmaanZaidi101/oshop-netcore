import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';
import { Product } from '../_models/product';
import { ProductService } from '../_services/product.service';

@Injectable()

export class AdminProductsResolver implements Resolve<Product[]> {
    pageNumber = 1;
    pageSize = 5;

    constructor(private authService: AuthService, private productService: ProductService,
                private alertify: AlertifyService, private router: Router) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Product[]> {
        return this.productService.getAllProductsWithPaging(this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error('Problem retreiving data');
                this.router.navigate(['/products']);
                return of(null);
            })
        );
    }
}
