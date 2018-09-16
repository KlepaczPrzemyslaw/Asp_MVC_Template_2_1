using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template_2_1.Services.IServices
{
	public interface IContextLoggerService
	{
		void CreateLog(Exception e);
	}
}
