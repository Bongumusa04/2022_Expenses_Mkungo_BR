using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ExpensesController : BaseApiController
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly DataContext _context;

        public ExpensesController(IExpenseRepository expenseRepository, DataContext context)
        {
            _context = context;
            _expenseRepository = expenseRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesAsync()
        {
            var expense = await _expenseRepository.GetExpensesAsync();
             return Ok(expense);
        }

        [HttpGet("~/api/Expenses/pastmonth")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByPastMonth(DateTime previousMonth)
        {
            return Ok(await _expenseRepository.GetExpenseByPastMonthAsync(previousMonth));
        }

        [HttpGet("~/api/Expenses/{id}")]
        public async Task<ActionResult<ExpenseDto>> GetExpensesById(int id)
        {
            return Ok(await _expenseRepository.GetExpenseByIdAsync(id));
        }

        [HttpPost]
        public IActionResult CreateExpense(Expense expense)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid entry.");
            }
             _expenseRepository.AddExpense(new Expense{
                Description = expense.Description,
                Date = expense.Date,
                Amount = expense.Amount,
                Username = User.GetUsername()
            });

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpense(Expense expense)
        {
            var dbExpense = await _context.Expenses.Where(x => x.Id == expense.Id).FirstOrDefaultAsync();
            if (dbExpense == null)
                return NotFound();

            dbExpense.Date = expense.Date;
            dbExpense.Amount = expense.Amount;
            dbExpense.Description = expense.Description;
            // who changed this
            dbExpense.Username = User.GetUsername();

            _expenseRepository.UpdateExpense(dbExpense);

            return Ok(expense);
        }

        [HttpDelete]
        public IActionResult DeleteExpense(int? id)
        {
            if(!id.HasValue)
            {
                return NotFound();
            }
            _expenseRepository.DeleteExpense(id);
            return Ok();
        }
    }
}