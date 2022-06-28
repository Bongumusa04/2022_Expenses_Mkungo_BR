using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
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
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesAsync([FromQuery] ExpenseParams expenseParams)
        {
             expenseParams.Username = User.GetUsername();

            var expense = await _expenseRepository.GetExpensesAsync(expenseParams);

            Response.AddPaginationHeader(expense.CurrentPage, expense.PageSize,
                expense.TotalCount, expense.TotalPages);

             return Ok(expense);
        }

        [HttpGet("~/api/Expenses/pastmonth")]
        public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByPastMonth(DateTime previousMonth, [FromQuery] ExpenseParams expenseParams)
        {
             expenseParams.Username = User.GetUsername();
             var expense = await _expenseRepository.GetExpenseByPastMonthAsync(previousMonth, expenseParams);
            Response.AddPaginationHeader(expense.CurrentPage, expense.PageSize,
                expense.TotalCount, expense.TotalPages);

            return Ok(expense);
        }

        [HttpGet("~/api/Expenses/{id}",Name = "GetExpense")]
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
             var newExpense = _expenseRepository.AddExpense(new Expense{
                Description = expense.Description,
                Date = expense.Date,
                Amount = expense.Amount,
                AppUserId = User.GetUserId()
             });

            return CreatedAtRoute("GetExpense", new { newExpense.Id }, newExpense);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpense(ExpenseDto expense)
        {
            var ex = await _expenseRepository.GetExpenseByIdAsync(User.GetUserId());
            return Ok( _expenseRepository.UpdateExpense(expense));
        }

        [HttpDelete]
        public IActionResult DeleteExpense(ExpenseDto expense)
        {
            _expenseRepository.DeleteExpense(expense);
            return Ok();
        }
      
    }
}