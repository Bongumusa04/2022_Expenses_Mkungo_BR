import { HttpClient } from '@angular/common/http';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { AccountService } from 'src/app/_services/account.service';
import { ConfirmService } from 'src/app/_services/confirm.service';
import { map, take } from 'rxjs/operators';
import { User } from 'src/app/_models/user';
import { ExpensesService } from 'src/app/_services/expenses.service';
import { Expense } from 'src/app/_models/Expense';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-expense-edit',
  templateUrl: './expense-edit.component.html',
  styleUrls: ['./expense-edit.component.css']
})
export class ExpenseEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm
  user: User;
  expenses: Expense[] = [];
  expense: Expense;
  @HostListener('window:beforeunload', ['$event]']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private expenseService: ExpensesService,private toastr: ToastrService,
     private accountService: AccountService,private confirmService: ConfirmService, ) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user =>{
      this.user = user;
    })
  }

  ngOnInit(): void {
    this.loadExpense();
  }

  loadExpense() {
    this.expenseService.getExpense(this.expense.id).subscribe(() => {
      this.user.expenses = this.user.expenses.filter(i => i.id !== this.expense.id);
    });
  }
  
  editExpense(id: number){
    this.expenseService.getExpense(id).subscribe(() => {
      this.user.expenses = this.user.expenses.filter(i => i.id !==id);
    });
   }
  
  deleteExpense(id: number) {
    this.confirmService.confirm('Confirm delete expense', 'This cannot be undone').subscribe(results => {
      if(results){
        this.expenseService.deleteExpense(id).subscribe(() => {
          this.expenses.splice(this.expenses.findIndex(m => m.id === id), 1);
        });
      }
    })
  }
  updateMember(id: number) {
    this.expenseService.updateMember(id).subscribe(() =>{
      this.toastr.success('Profile updated succesfully');
      this.editForm.reset(id);
    })
  }

}
