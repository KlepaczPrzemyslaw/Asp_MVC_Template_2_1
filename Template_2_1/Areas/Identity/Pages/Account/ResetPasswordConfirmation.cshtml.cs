using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Template_2_1.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class ResetPasswordConfirmationModel : PageModel
	{
		public void OnGet()
		{

		}
	}
}
