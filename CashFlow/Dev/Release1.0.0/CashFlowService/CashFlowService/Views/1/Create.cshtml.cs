using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CashFlowRepositoryService.CashFlowDBModels;

namespace CashFlowService
{
    public class CreateModel : PageModel
    {
        private readonly CashFlowRepositoryService.CashFlowDBModels.CashFlowContext _context;

        public CreateModel(CashFlowRepositoryService.CashFlowDBModels.CashFlowContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public WorkMatters WorkMatters { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.WorkMatters.Add(WorkMatters);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}