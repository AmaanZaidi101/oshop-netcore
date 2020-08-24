import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Cart } from '../_models/cart';
import { throwError, BehaviorSubject } from 'rxjs';
import { Product } from '../_models/product';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  cart = new BehaviorSubject<Cart>(null);
  currrentCart = this.cart.asObservable();

constructor(private http: HttpClient) { }

  baseUrl = environment.apiUrl + 'cart/';

   updateCurrentCart(newCart: Cart) {
    this.cart.next(newCart);
  }

  updateLocalStorage(newCart: Cart) {
    const user = JSON.parse(localStorage.getItem('user'));
    user.cart = newCart;
    localStorage.setItem('user', JSON.stringify(user));
  }

  emptyCart() {
    const user = JSON.parse(localStorage.getItem('user'));
    const emptyCart: Cart = user.cart;
    emptyCart.totalPrice = 0;
    emptyCart.totalQuantity = 0;
    emptyCart.cartItems = [];
    user.cart = emptyCart;
    localStorage.setItem('user', JSON.stringify(user));
    this.cart.next(emptyCart);
  }

  getCart(userId: string) {
    this.http.get<Cart>(this.baseUrl + userId).subscribe(next => {
      this.updateCurrentCart(next);
      this.updateLocalStorage(next);
    }, error => throwError(error));
  }

  addToCart(userId: string, product: Product) {
    return this.http.post<Cart>(this.baseUrl + userId + '/add', product).pipe(
      map(x => {
        this.updateCurrentCart(x);
        this.updateLocalStorage(x);
      })
    );
  }

  removeFromCart(userId: string, product: Product) {
    return this.http.post<Cart>(this.baseUrl + userId + '/remove', product).pipe(
      map(x => {
        this.updateCurrentCart(x);
        this.updateLocalStorage(x);
      })
    );
  }

  clearCart(userId: string) {
    return this.http.post<Cart>(this.baseUrl + userId + '/clearCart', {}).pipe(
      map(x => {
        this.updateCurrentCart(x);
        this.updateLocalStorage(x);
      })
    );
  }

  getItemQuantity(product) {
    if (this.cart && this.cart.value && this.cart.value.cartItems) {
      const element = this.cart.value.cartItems.find(x => x.productId === product.id );
      return element != null && element.quantity > 0 ? element.quantity : 0 ;
  }
    return 0;
  }
}
