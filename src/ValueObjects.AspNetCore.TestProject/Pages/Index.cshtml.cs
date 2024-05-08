using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ValueObjects.Common;

namespace ValueObjects.AspNetCore.TestProject.Pages;

public class IndexModel : PageModel
{
	private readonly ILogger<IndexModel> _logger;

	[BindProperty]
	public SemVer SemVer { get; set; } = new SemVer(1, 1, 1);

	public IndexModel(ILogger<IndexModel> logger)
	{
		_logger = logger;
	}

	public async Task<IActionResult> OnGet()
	{
		return Page();
	}

	public async Task<IActionResult> OnPost()
	{
		return RedirectToPage();
	}
}
