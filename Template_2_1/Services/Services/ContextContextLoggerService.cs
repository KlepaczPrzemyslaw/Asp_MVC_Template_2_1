using System;
using System.IO;
using Template_2_1.Services.IServices;

namespace Template_2_1.Services.Services
{
	public class ContextContextLoggerService : IContextLoggerService
	{
		public void CreateLog(Exception e)
		{
			using (var sw = File.AppendText("wwwroot/logs/logs.txt"))
			{
				sw.WriteLine($"--- NEW ---> {DateTime.Now} \n{e.StackTrace} \n   {e.GetType()} ---> {e.Message}!!\n");
			}
		}
	}
}
