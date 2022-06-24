using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.Extensions;

namespace API.Data
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly DataContext _context;
        public ExpenseRepository(DataContext context)
        {
            _context = context;
            
        }
        public void AddExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }

        public async void DeleteExpense(int? id)
        {
            Expense expense = await _context.Expenses.FindAsync(id);
            _context.Expenses.Remove(expense);
            _context.SaveChanges();
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }

        public async Task<IEnumerable<Expense>> GetExpenseByPastMonthAsync(DateTime previousMonth)
        {
            // previousMonth = DateTime.Now.AddMonths(-1);
            return await _context.Expenses.Where(x => x.Date.Month == previousMonth.Month - 1).ToListAsync();
        }

        public async Task<IEnumerable<Expense>> GetExpensesAsync()
        {
             return await _context.Expenses.ToListAsync();
        }

        public void UpdateExpense(Expense expense)
        {
            _context.Expenses.Update(expense);
            _context.Entry(expense).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}          