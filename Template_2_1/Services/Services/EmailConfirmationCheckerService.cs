using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Template_2_1.Context.Context;
using Template_2_1.Services.IServices;

namespace Template_2_1.Services.Services
{
	public class EmailConfirmationCheckerService : IEmailConfirmationCheckerService
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IContextLoggerService _contextLogger;

		public EmailConfirmationCheckerService(ApplicationDbContext context, UserManager<IdentityUser> userManager, IContextLoggerService contextLogger)
		{
			this._context = context;
			this._userManager = userManager;
			this._contextLogger = contextLogger;
		}

		public async Task<bool> IsEmailConfirmed(string email)
		{
			try
			{
				return await _userManager.IsEmailConfirmedAsync(_context.Users.SingleOrDefault(x => x.Email == email));
			}
			catch (Exception e)
			{
				_contextLogger.CreateLog(e);
				return false;
			}
		}
	}
}
