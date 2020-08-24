import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  baseUrl = environment.apiUrl + 'admin/';

  constructor(private http: HttpClient) { }

  getUsersWithRoles(page?, itemsPerPage?, role?, userName?) {
    const paginatedResult = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (page && itemsPerPage) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (role) {
      params = params.append('Role', role);
    }

    if (userName) {
      params = params.append('UserName', userName);
    }

    return this.http.get<User[]>(this.baseUrl + 'withRoles', {observe : 'response', params}).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  updateUserRoles(userId: string, roles: {}) {
    return this.http.post<User>(this.baseUrl + 'editRoles/' + userId, roles);
  }

}
