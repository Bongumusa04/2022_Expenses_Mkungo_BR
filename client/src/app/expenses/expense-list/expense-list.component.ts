import { Component, OnInit } from '@angular/core';
import { Expense } from 'src/app/_models/Expense';
import { Pagination } from 'src/app/_models/pagination';
import { ExpensesService } from 'src/app/_services/expenses.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { HttpClient } from '@angular/common/http';
import { AccountService } from 'src/app/_services/account.service';
import { map, take } from 'rxjs/operators';
import { ExpenseParams } from 'src/app/_models/expenseParams';
import { ConfirmService } from 'src/app/_services/confirm.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-expense-list',
  templateUrl: './expense-list.component.html',
  styleUrls: ['./expense-list.component.css']
})
export class ExpenseListComponent implements OnInit {
  expenses: Expense[] = [];
  pagination: Pagination;
  pageNumber = 1;
  pageSize = 10;
  user: User;
  expense: Expense;
  form: FormGroup;
  total = 0;    
  value; 

  loading = false;
  bsModalRef: BsModalRef;
 
   constructor(private route: ActivatedRoute, private accountService: AccountService,private expenseService: ExpensesService,
     private fb: FormBuilder, private confirmService: ConfirmService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user =>{
      this.user = user;
      this.user.expenses = this.expenses;
    });

   }
 
   ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.expense = data.expense;
    })
    this.initializeForm();
    this.loadExpenses();
   }
   initializeForm() {
    this.form = this.fb.group({
      description: ["",Validators.required],
      amount: ["",Validators.required],
      date: ["",Validators.required],
     
    });}

   editExpense(id: number){
    // this.expenseService.getExpense(id).subscribe(() => {
    //   this.user.expenses = this.user.expenses.filter(i => i.id === id);
    // });
    this.expenseService.getExpense(this.expense.id).subscribe(() => {
      this.expenses = this.expenses.splice(this.expenses.findIndex(m => m.id === id), 1);
    });
   }

  deleteExpense(id:number){
    this.confirmService.confirm('Confirm delete expense', 'This cannot be undone').subscribe(results => {
      if(results){
        this.expenseService.deleteExpense(id).subscribe(() => {
          this.expenses.splice(this.expenses.findIndex(m => m.id === id), 1);
        });
      }
    })
  }

   loadExpenses(){
    this.loading = true;
    this.expenseService.getExpenses(this.pageNumber,this.pageSize).subscribe(response => {
      this.expenses = response.result;
      this.pagination = response.pagination;
      this.loading = false;
    })
  }

  onSubmit( value: Expense) {
    this.expenseService.addExpense(value).subscribe(() => {
        this.form.reset();
      });
    } 

    findsum(){    
      debugger  
      this.value = this.expense.amount    
      console.log(this.value);  
      for(let j=0;j<this.value.length;j++){   
           this.total += this.value[j].Salary  
           console.log(this.total)  
      }  
    }
    pageChanged(event: any){
      this.pageNumber = event.page;
      this.loadExpenses();
    }
  }
  

