using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class ExpenseParams : PaginationParams
    {
        public string Username { get; set; }
        public int UserId {get; set;}
        public string OrderBy { get; set; } = "lastAdded";
    }
}