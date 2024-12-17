using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RPInventories.Data;
using RPInventories.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace RPInventories.Pages.Departments;
public class IndexModel : PageModel
{
    private readonly InventoriesContext _context;
    private readonly IConfiguration _config;

    public IndexModel(InventoriesContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public IPagedList<Department> Departments { get; set; }

    [BindProperty(SupportsGet = true)] public int? CurrentPage { get; set; }
    [BindProperty(SupportsGet = true)] public string SearchText { get; set; }
    public int TotalRecords { get; set; }


    public async Task OnGetAsync()
    {
        var recordsPerPage = _config.GetValue("RecordsPerPage", 3);
        var consult = _context.Departments
            .AsNoTracking()
            .Select(d => d);

        if (!String.IsNullOrEmpty(SearchText))
        {
            consult = consult.Where(d => d.Name.Contains(SearchText));
        }
        
        TotalRecords = await consult.CountAsync();
        var pageNumber = (CurrentPage ?? 1);
        
        Departments = consult.ToPagedList(pageNumber, recordsPerPage);
    }
}
