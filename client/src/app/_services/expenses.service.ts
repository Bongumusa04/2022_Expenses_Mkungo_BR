import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Expense } from '../_models/Expense';
import { PaginatedResult } from '../_models/pagination';
import { map, take } from 'rxjs/operators';
import { AccountService } from './account.service';
import { User } from '../_models/user';


@Injectable({
  providedIn: 'root'
})
export class ExpensesService {
  baseUrl = environment.apiUrl;
  expenses: Expense[] = [];
  paginatedResult: PaginatedResult<Expense[]> = new PaginatedResult<Expense[]>();
  user: User;

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user =>{
      this.user = user;
    })
   } 

  getExpenses(page?: number, itemsPerPage?: number) {
    let params = new HttpParams();

    if(page !== null && itemsPerPage !== null){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http.get<Expense[]>(this.baseUrl + 'expenses/', {observe: 'response', params}).pipe(
        map(response => {
          this.paginatedResult.result = response.body;
          if(response.headers.get('Pagination') !== null){
            this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return this.paginatedResult;
        })
      );
  }
  
  getExpense(id: number) {
    const expense = this.expenses.find(x => x.id == id);
    if(expense !== undefined) return of(expense);
    return this.http.get<Expense>(this.baseUrl + 'expenses/' + id);
  }
}