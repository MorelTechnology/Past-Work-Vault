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
    public class DetailsModel : PageModel
    {
        private readonly CashFlowRepositoryService.CashFlowDBModels.CashFlowContext _context;

        public DetailsModel(CashFlowRepositoryService.CashFlowDBModels.CashFlowContext context)
        {
            _context = context;
        }

        public WorkMatters WorkMatters { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WorkMatters = await _context.WorkMatters.FirstOrDefaultAsync(m => m.WorkMatter == id);

            if (WorkMatters == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
