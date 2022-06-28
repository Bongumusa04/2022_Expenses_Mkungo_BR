using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IExpenseRepository
    {
        Task<PagedList<ExpenseDto>> GetExpensesAsync(ExpenseParams expenseParams);
        Task<PagedList<ExpenseDto>> GetExpenseByPastMonthAsync(DateTime previousMonth,ExpenseParams expenseParams);
        Task<ExpenseDto> GetExpenseByIdAsync(int id);
        ExpenseDto AddExpense(Expense expense);
        ExpenseDto UpdateExpense(int id);
        void DeleteExpense(int id);
        
    }
}