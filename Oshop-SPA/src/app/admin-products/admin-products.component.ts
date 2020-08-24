import { Component, OnInit } from '@angular/core';
import { Product } from '../_models/product';
import { Category } from '../_models/category';
import { Pagination } from '../_models/pagination';
import { ProductService } from '../_services/product.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { CategoryService } from '../_services/category.service';

@Component({
  selector: 'app-admin-products',
  templateUrl: './admin-products.component.html',
  styleUrls: ['./admin-products.component.css'],
})
export class AdminProductsComponent implements OnInit {
  products: Product[];
  categoryList: Category[];

  productParams: any = {};
  pagination: Pagination;

  constructor(
    private categoryService: CategoryService,
    private productService: ProductService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.productParams.categoryId = '';
    this.productParams.soldOut = '';
    this.categoryService.getAllCategories().subscribe(next => {
      this.categoryList = next;
    });
    this.route.data.subscribe(data => {
      this.products = data['products'].result;
      this.pagination = data['products'].pagination;
    });
  }

  loadProducts() {
    if (!this.productParams.productId) {
      this.productParams.productId = '';
    }
    if (!this.productParams.categoryId) {
      this.productParams.categoryId = '';
    }
    if (!this.productParams.soldOut) {
      this.productParams.soldOut = '';
    }
    this.productService
      .getAllProductsWithPaging(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.productParams
      )
      .subscribe(
        (next) => {
          this.products = next.result;
          this.pagination = next.pagination;
        },
        (error) => this.alertify.error(error)
      );
  }

  resetFilters() {
    this.productParams.orderId = '';
    this.productParams.categoryId = '';
    this.productParams.SoldOut = '';
    this.loadProducts();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadProducts();
  }
}
