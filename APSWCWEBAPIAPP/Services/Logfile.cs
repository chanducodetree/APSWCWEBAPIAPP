using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APSWCWEBAPIAPP.Services
{
    public static class Logfile
    {
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
		public static object Write_Log(string f_name, dynamic strMsg)
		{
			//f_name= "//10.96.52.149\\vvolunteers02\\websites\\VVSendOtpLogs";
			string strPath = f_name + "\\" + DateTime.Now.ToString("MMddyyyy") + "\\" + DateTime.Now.ToString("HH").ToString();
			if (!Directory.Exists(strPath))
				Directory.CreateDirectory(strPath);
			string path2 = strPath + "\\" + "submittedData" + DateTime.Now.ToString("yyyyMMddhhmmssmmm");
			StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
			swLog.WriteLine(strMsg);
			swLog.Close();
			swLog.Dispose();
			return "";
		}


		#endregion

	}
}
