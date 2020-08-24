import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Order } from '../_models/order';
import { environment } from 'src/environments/environment';
import { CartService } from './cart.service';
import { flatMap, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { DatePipe } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = environment.apiUrl + 'order/';
  pipe: DatePipe = new DatePipe('en-US');
constructor(private http: HttpClient, private cartService: CartService) { }

  getOrder(userId: string, orderId: string) {
    return this.http.get<Order>(this.baseUrl + userId + '/' + orderId);
  }

  getOrders(userId: string, page?, itemsPerPage?, orderParams?): Observable<PaginatedResult<Order[]>> {
    const paginatedResult: PaginatedResult<Order[]> = new PaginatedResult<Order[]>();

    let params = new HttpParams();

    if (page && itemsPerPage) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (orderParams) {
      params = params.append('status', orderParams.status);
      params = params.append('isDescending', orderParams.isDescending);
      params = params.append('orderId', orderParams.orderId);
      params = params.append('minDate', this.pipe.transform(orderParams.minDate, 'yyyy/MM/dd'));
      params = params.append('maxDate', this.pipe.transform(orderParams.minDate, 'yyyy/MM/dd'));
    }

    params = params.append('orderBy', 'DatePlaced');

    return this.http.get<Order[]>(this.baseUrl + userId + '/paged', {observe: 'response', params})
      .pipe(
        map(response => {
          paginatedResult.result = response.body;

          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  getAllOrders(userId: string, page?, itemsPerPage?, orderParams?) {
    const paginatedResult: PaginatedResult<Order[]> = new PaginatedResult<Order[]>();

    let params = new HttpParams();

    if (page && itemsPerPage) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (orderParams) {
      params = params.append('status', orderParams.status);
      params = params.append('isDescending', orderParams.isDescending);
      params = params.append('orderId', orderParams.orderId);
      params = params.append('minDate', this.pipe.transform(orderParams.minDate, 'yyyy/MM/dd'));
      params = params.append('maxDate', this.pipe.transform(orderParams.minDate, 'yyyy/MM/dd'));
    }
    params = params.append('orderBy', 'DatePlaced');

    return this.http.get<Order[]>(this.baseUrl + userId + '/getAll', {observe: 'response', params})
      .pipe(
        map(response => {
          paginatedResult.result = response.body;

          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        })
      );
  }

  createOrder(userId: string, order: Order): Observable<Order> {
    return this.http.post<Order>(this.baseUrl + userId, order).pipe(
      map((res1: Order) => {
        this.cartService.emptyCart(); return res1;
      })
    );
  }

  cancelOrder(userId: string, orderId: string) {
    return this.http.delete(this.baseUrl + userId + '/' + orderId);
  }

  updateOrder(userId: string, orderId: string, status: string) {
    let params = new HttpParams();
    params = params.append('status', status);
    return this.http.put<Order>(this.baseUrl + userId + '/' + orderId + '/' + status, {});
  }

}
