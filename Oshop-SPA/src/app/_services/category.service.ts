import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Category } from '../_models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  baseUrl = environment.apiUrl + 'category/';

  constructor(private http: HttpClient) { }

  getAllCategories() {
    return this.http.get<Category[]>(this.baseUrl);
  }

  getCategoryById(id: string) {
    return this.http.get<Category>(this.baseUrl + id);
  }

  addCategory(category: Category) {
    return this.http.post<Category>(this.baseUrl, category);
  }

  updateCategory(category: Category) {
    return this.http.put<Category>(this.baseUrl, category);
  }

  deleteCategory(id: string) {
    return this.http.delete<any>(this.baseUrl + id);
  }

}
