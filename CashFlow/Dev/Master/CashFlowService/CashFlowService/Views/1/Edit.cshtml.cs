using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CashFlowRepositoryService.CashFlowDBModels;

namespace CashFlowService
{
    public class EditModel : PageModel
    {
        private readonly CashFlowRepositoryService.CashFlowDBModels.CashFlowContext _context;

        public EditModel(CashFlowRepositoryService.CashFlowDBModels.CashFlowContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(WorkMatters).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkMattersExists(WorkMatters.WorkMatter))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool WorkMattersExists(string id)
        {
            return _context.WorkMatters.Any(e => e.WorkMatter == id);
        }
    }
}
