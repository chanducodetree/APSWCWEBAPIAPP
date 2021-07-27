
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace APSWCWEBAPIAPP.Services
{
    public class SMSService
    {
        public string SendSMS(string mobileno, string message)
        {
            try
            {
                SMSHttpPostClient _SMS = new SMSHttpPostClient();
                //string username = "APGOVT"; //"APGOVT";
                //string password = "esd@123";  // "esd@123";
                //string senderid = "GOVTAP";
                //string securekey = "db7bed5a-b622-4e0a-9ab1-5730ff85a0a0"; //"9d70e88e-093d-445d-9d19-5f5f854bb740";//"de8aba8b-27df-49a1-8fa3-72c60b9fe25b";

                string username = "APGOVT-APSWC"; //"APGOVT";
                string password = "Welcome@123";  // "esd@123";
                string senderid = "GOVTAP";
                string securekey = "a1109136-9e1c-459a-a609-64abd51d522b";
                string status = _SMS.sendSingleSMS(username, password, senderid, mobileno, message, securekey, "1007604236977494617");

                //Write_Log(status + mobileno, "SMSLogs");
                return status;
            }
            catch (Exception ex)
            {
                //Write_Log(mobileno + " ,Error Message : " + ex.Message, "SMSLogs");
                return ex.Message;
            }

        }

        public string SendTeluguSMS(string mobileno, string message)
        {
            try
            {
                SMSHttpPostClient _SMS = new SMSHttpPostClient();
                string username = "APGOVT"; //"APGOVT";
                string password = "esd@123";  // "esd@123";
                string senderid = "GOVTAP";
                string secureykey = "db7bed5a-b622-4e0a-9ab1-5730ff85a0a0";
                string status = _SMS.sendUnicodeOTPSMS(username, password, senderid, mobileno, message, secureykey, "10032659996488");
                Write_Log(status + mobileno, "SMSLogs");
                return status;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string SendBulkSMS(string mobilenos, string message)
        {
            try
            {
                SMSHttpPostClient _SMS = new SMSHttpPostClient();
                string username = "APGOVT"; //"APGOVT";
                string password = "esd@123";  // "esd@123";
                string senderid = "GOVTAP";
                string secureykey = "db7bed5a-b622-4e0a-9ab1-5730ff85a0a0";
                string status = _SMS.sendBulkSMStulugu(username, password, senderid, mobilenos, message, secureykey, "10032659996488");
                //  Write_Log(status + mobilenos, "SMSLogs");
                return status;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public object Write_Log(dynamic strMsg, string f_name)
        {

            string strPath = "D:\\website\\SMSGateWayCode\\SMSLog\\" + DateTime.Now.ToString("MMddyyyy");
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            string path2 = strPath + "\\" + "submittedData" + DateTime.Now.ToString("yyyyMMdd");
            StreamWriter swLog = new StreamWriter(path2 + ".txt", true);
            swLog.WriteLine(DateTime.Now.ToString() + ":" + strMsg);
            swLog.Close();
            swLog.Dispose();
            return "";
        }


    }
}
