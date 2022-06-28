import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { identity, Observable } from 'rxjs';
import { Expense } from 'src/app/_models/Expense';
import { ExpensesService } from 'src/app/_services/expenses.service';


@Injectable({
  providedIn: 'root'
})
export class ExpensesResolver implements Resolve<Expense> {
id:number;
  constructor(private expenseService: ExpensesService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Expense> {
    return this.expenseService.getExpense(route.params['id']);
  }
}
