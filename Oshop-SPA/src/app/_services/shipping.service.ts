import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Shipping } from '../_models/shipping';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShippingService {

  basUrl = environment.apiUrl + 'shipping/';

constructor(private http: HttpClient) { }

  getShippings(userId: string) {
    return this.http.get<Shipping[]>(this.basUrl + userId);
  }

  getPreferredShipping(userId: string) {
    return this.http.get<Shipping>(this.basUrl + userId + '/getPreferred');
  }

  updateShipping(userId: string, shipping: Shipping) {
    return this.http.put<Shipping>(this.basUrl + userId + '/' + shipping.id, shipping);
  }

  addShipping(userId: string, shipping: Shipping) {
    return this.http.post<Shipping>(this.basUrl + userId + '/add', shipping);
  }

  deleteShipping(userId: string, shipping: Shipping) {
    return this.http.delete(this.basUrl + userId + '/' + shipping.id);
  }


}
