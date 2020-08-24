import { Component, OnInit } from '@angular/core';
import { Product } from '../_models/product';
import { AuthService } from '../_services/auth.service';
import { CategoryService } from '../_services/category.service';
import { ProductService } from '../_services/product.service';
import { Category } from '../_models/category';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css'],
})
export class ProductFormComponent implements OnInit {
  product: Product  = new Product();
  categories: Category[];
  id: string;

  constructor(
    private authService: AuthService,
    private categoryService: CategoryService,
    private productService: ProductService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.id = params.get('id');
    });

    if (this.id !== 'add') {
    this.route.data.subscribe(data => {
      this.product = data['product'];
    });
    } else {
      this.product = new Product();
      this.product.categoryId = '';
    }

    this.categoryService.getAllCategories().subscribe(
      (next) => {
        this.categories = next;
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  save(product) {
    product.id = this.product.id;
    if (this.product.id) {
      this.productService.updateProduct(product).subscribe(next => {
        this.alertify.success('Product updated succesfully');
        this.router.navigate(['admin/products']);
      });
    } else {
      this.productService.addProduct(product).subscribe(next => {
        this.alertify.success('Product added successfully');
        this.router.navigate(['admin/products']);
      }, error => this.alertify.error('Could not add product'));
    }
  }

  openDialog() {
    const msg = this.product.soldOut ? 'available' : 'sold out';
    this.alertify.confirm('Are you sure you want to mark this item as ' + msg + ' ?', () => {
      this.product.soldOut = !this.product.soldOut;
      this.productService.updateProduct(this.product).subscribe(next => {
        this.alertify.success('Product marked as ' + msg);
      }, error => this.alertify.error('Could not mark product as ' + msg));
    });
  }
}
