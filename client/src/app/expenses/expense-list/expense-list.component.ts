import { Component, OnInit } from '@angular/core';
import { Expense } from 'src/app/_models/Expense';
import { Pagination } from 'src/app/_models/pagination';
import { ExpensesService } from 'src/app/_services/expenses.service';

@Component({
  selector: 'app-expense-list',
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.css']
})
export class ExpenseListComponent implements OnInit {
  expenses: Expense[] = [];
  pagination: Pagination;
  pageNumber = 1;
  pageSize = 50;
 
   constructor(private expenseService: ExpensesService) {}
 
   ngOnInit(): void {
    this.loadExpenses();
   }

   loadExpenses(){
    this.expenseService.getExpenses(this.pageNumber,this.pageSize).subscribe(response => {
      this.expenses = response.result;
      this.pagination = response.pagination;
    })
   }
 
}
