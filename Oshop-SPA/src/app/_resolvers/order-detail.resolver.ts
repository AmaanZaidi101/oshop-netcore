import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/auth.service';
import { catchError } from 'rxjs/operators';
import { OrderService } from '../_services/order.service';
import { Order } from '../_models/order';

@Injectable()

export class OrderDetailResolver implements Resolve<Order> {

    constructor(
        private authService: AuthService,
        private orderService: OrderService,
        private alertify: AlertifyService,
        private router: Router
      ) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Order> {
        return this.orderService.getOrder(this.authService.decodedToken.nameid, route.paramMap.get('id')).pipe(
            catchError(error => {
                this.alertify.error('Problem retreiving data');
                // this.router.navigate(['/products']);
                return of(null);
            })
        );
    }
}
