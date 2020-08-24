import { Component, OnInit } from '@angular/core';
import { Category } from '../_models/category';
import { AuthService } from '../_services/auth.service';
import { CategoryService } from '../_services/category.service';
import { ProductService } from '../_services/product.service';
import { AlertifyService } from '../_services/alertify.service';
import { Product } from '../_models/product';
import { Cart } from '../_models/cart';
import { ActivatedRoute } from '@angular/router';
import { CartService } from '../_services/cart.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  categories: Category[];
  products: Product[];
  cart: Cart;

  constructor(private authService: AuthService, private categoryService: CategoryService,
              private productService: ProductService, private alertify: AlertifyService, private route: ActivatedRoute,
              private cartService: CartService) { }

  ngOnInit() {
    this.cartService.currrentCart.subscribe(x => {
      this.cart = x;
    });
    this.route.data.subscribe(data => {
      this.products = data['products'];
      // this.cart = data['cart'];
      // if (!this.cart) {
      //   this.cart = JSON.parse(localStorage.getItem('user')).cart;
      // }
    });
    this.getCategories();
    this.resetFilter();
  }

  filter(categoryid) {
    this.productService.getProductsByCategory(categoryid).subscribe(next => {
      this.products = next;
    }, error => this.alertify.error('Unable to get products'));
  }

  resetFilter() {
    this.productService.getAllProducts().subscribe(next => {
      this.products = next;
    }, error => this.alertify.error('Unable to get products'));
  }

  getCategories() {
    this.categoryService.getAllCategories().subscribe(next => {
      this.categories = next;
    }, error => this.alertify.error('Unable to get categories'));
  }

}
