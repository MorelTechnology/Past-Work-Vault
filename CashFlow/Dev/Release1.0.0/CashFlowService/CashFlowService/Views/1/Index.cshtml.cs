using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CashFlowRepositoryService.CashFlowDBModels;

namespace CashFlowService
{
    public class IndexModel : PageModel
    {
        private readonly CashFlowRepositoryService.CashFlowDBModels.CashFlowContext _context;

        public IndexModel(CashFlowRepositoryService.CashFlowDBModels.CashFlowContext context)
        {
            _context = context;
        }

        public IList<WorkMatters> WorkMatters { get;set; }

        public async Task OnGetAsync()
        {
            WorkMatters = await _context.WorkMatters.ToListAsync();
        }
    }
}
