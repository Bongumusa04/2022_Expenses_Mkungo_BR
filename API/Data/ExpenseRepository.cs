using System;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using API.Helpers;
using API.Controllers;
using Microsoft.AspNetCore.Http;

namespace API.Data
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public ExpenseRepository(DataContext context,IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
        }

        public ExpenseDto AddExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
            return (ExpenseDto)expense;
        }

        public async void DeleteExpense(ExpenseDto expense)
        {
             var dbExpense = await _context.Expenses.FirstAsync(e => e.Id == expense.Id);
            _context.Expenses.Remove(dbExpense);
            _context.SaveChanges();
        }

        public async Task<ExpenseDto> GetExpenseByIdAsync(int id)
        {
           return await  _context.Expenses
                .Where(e => e.Id == id)
                .Select(e => (ExpenseDto)e)
                .FirstOrDefaultAsync();
          
        }

        public async Task<PagedList<ExpenseDto>> GetExpenseByPastMonthAsync(DateTime previousMonth,ExpenseParams expenseParams)
        {
            var query =  _context.Expenses
                        .Where(x => x.Date.Month == previousMonth.Month - 1)
                        .ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider)
                        .AsQueryable();
            // previousMonth = DateTime.Now.AddMonths(-1);
            return await PagedList<ExpenseDto>.CreateAsync(query, expenseParams.PageNumber, expenseParams.PageSize);
        }

        public async Task<PagedList<ExpenseDto>> GetExpensesAsync(ExpenseParams expenseParams )
        {
           var query =  _context.Expenses
                        .ProjectTo<ExpenseDto>(_mapper.ConfigurationProvider)
                        .AsQueryable();

            return await PagedList<ExpenseDto>.CreateAsync(query, expenseParams.PageNumber, expenseParams.PageSize);            
        }

        public ExpenseDto UpdateExpense(ExpenseDto expense)
        {
            var dbExpense = _context.Expenses
                 .Where(e => e.Id == expense.Id)
                 .First();
            dbExpense.Description = expense.Description;
            dbExpense.Amount = expense.Amount;
            dbExpense.Date = expense.Date;

            _context.SaveChanges();
            return expense;
        }
    }
}          