import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Product } from '../_models/product';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  baseUrl = environment.apiUrl + 'product';

  constructor(private http: HttpClient) { }

  getAllProducts() {
    return this.http.get<Product[]>(this.baseUrl);
  }

  getAllProductsWithPaging(page?, itemsPerPage?, productParams?) {

    let params = new HttpParams();

    if (page && itemsPerPage) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (productParams) {
      params = params.append('categoryId', productParams.categoryId);
      params = params.append('productId', productParams.productId);
      params = params.append('soldOut', productParams.soldOut);
    }

    const paginatedResult: PaginatedResult<Product[]> = new PaginatedResult<Product[]>();

    return this.http.get<Product[]>(this.baseUrl + '/paged', {observe: 'response', params})
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

  getProductById(id: string) {
    return this.http.get<Product>(this.baseUrl + '/' + id);
  }

  getProductsByCategory(categoryId: string) {
    return this.http.get<Product[]>(this.baseUrl + '/category/' + categoryId);
  }

  addProduct(product: Product) {
    return this.http.post<Product>(this.baseUrl, product);
  }

  updateProduct(product: Product) {
    return this.http.put<Product>(this.baseUrl, product);
  }

  deleteProduct(id: string) {
    return this.http.delete<any>(this.baseUrl + id);
  }

}
