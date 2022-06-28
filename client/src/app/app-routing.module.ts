import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { AuthGuard } from './_guards/auth.guard';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { LandingComponent } from './landing/landing.component';
import { ContactComponent } from './contact/contact.component';
import { DisclaimerComponent } from './disclaimer/disclaimer.component';
import { ExpenseListComponent } from './expenses/expense-list/expense-list.component';
import { ExpenseEditComponent } from './expenses/expense-edit/expense-edit.component';
import { ExpensesResolver } from './_forms/_resolvers/expenses.resolver';

const routes: Routes = [
  {path: '', component: LandingComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'expenses', component: ExpenseListComponent, canActivate:[AuthGuard]},
      
    ]
  },
  {path: 'errors', component: TestErrorsComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'home', component: HomeComponent},
  {path: 'disclaimer', component: DisclaimerComponent},
  {path: 'contact', component: ContactComponent},
  {path: 'expenses/{{id}}', component: ExpenseEditComponent, resolve:{expense: ExpensesResolver}},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'},
      
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
