import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Order } from '../_models/order';
import { AuthService } from '../_services/auth.service';
import { OrderService } from '../_services/order.service';

@Injectable()

export class AdminOrdersResolver implements Resolve<Order[]> {
    pageNumber = 1;
    pageSize = 5;

    constructor(private authService: AuthService, private orderService: OrderService,
                private alertify: AlertifyService, private router: Router) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Order[]> {
        return this.orderService.getAllOrders(this.authService.decodedToken.nameid, this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error('Problem retreiving data');
                this.router.navigate(['/products']);
                return of(null);
            })
        );
    }
}
