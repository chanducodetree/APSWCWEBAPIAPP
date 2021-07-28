using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


namespace APSWCWEBAPIAPP.Services
{
	public static class Logfile
	{
		private static IHttpContextAccessor _httpContextAccessor;
		#region"Logs code"

		public static object Write_Log_Exception(string mappath, dynamic strMsg)
		{
			string strPath = mappath + "\\" + DateTime.Now.ToString("MMddyyyy");
			if (!Directory.Exists(strPath))
				Directory.CreateDirectory(strPath);
			string path2 = strPath + "\\" + "submittedData" + DateTime.Now.ToString("yyyyMMddhhmmssmmm");
			StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
			swLog.WriteLine(DateTime.Now.ToString("ddMMyyHHmmssttt") + ":" + strMsg);
			swLog.Close();
			swLog.Dispose();
			return "";



		}
		public static object Write_ReportLog_Exception(string mappath, dynamic strMsg)
		{
			string strPath = mappath + "\\" + DateTime.Now.ToString("MMddyyyy");
			if (!Directory.Exists(strPath))
				Directory.CreateDirectory(strPath);
			string path2 = strPath + "\\" + "submittedData" + DateTime.Now.ToString("yyyyMMddhh");
			StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
			swLog.WriteLine(DateTime.Now.ToString("ddMMyyHHmmssttt") + ":" + strMsg);
			swLog.Close();
			swLog.Dispose();
			return "";



		}
		public static object Write_Log(string f_name, string methodlog, dynamic strMsg)
		{
			//f_name= "//10.96.52.149\\vvolunteers02\\websites\\VVSendOtpLogs";
			string strPath = f_name + "\\" + methodlog + "\\" + DateTime.Now.ToString("MMddyyyy") + "\\" + DateTime.Now.ToString("HH").ToString();
			var random = new Random();
			var number = random.Next(111111, 999999);
			if (!Directory.Exists(strPath))
				Directory.CreateDirectory(strPath);
			string path2 = strPath + "\\" + "submittedData" + DateTime.Now.ToString("yyyyMMddhhmmssmmmffff") + number.ToString();
			StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
			swLog.WriteLine(strMsg);
			swLog.Close();
			swLog.Dispose();
			return "";
		}


		#endregion


		public static string GetLocalIPAddress()
		{


			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}

		public static string MachineName(string ipadrress)
		{
			return Dns.GetHostEntry(ipadrress).HostName;
			//Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName;
		}
		public static string Browsename()
		{
			try
			{
				string userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
				return userAgent;
			}
			catch (Exception ex)
			{
				return "chrome";
			}
		}



	}
}