using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApplication3
{
    //public class Program
    //{
    //    static void Main(string[] args)
    //    {

    //        String username = ""; String password = ""; String MobileNumbers = ""; String voiceCode = ""; String CallID = "";


    //        String postData = "username=" + username + "&password=" + password + "&MobileNo=" + MobileNumbers + "&CallID" + CallID + "&voiceCode=" + voiceCode;//data to post
    //        String url = "https://services.mgov.gov.in/PushCallAPI/MakePushCall";//url 

    //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
    //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

    //        request.KeepAlive = false;
    //        request.ProtocolVersion = HttpVersion.Version10;
    //        request.Method = "POST";
    //        //System.Net.ServicePointManager.CertificatePolicy = new MyPolicy();



    //        byte[] array = Encoding.ASCII.GetBytes(postData);

    //        request.ContentType = "application/x-www-form-urlencoded";
    //        request.ContentLength = array.Length; Stream requestStream = request.GetRequestStream();


    //        requestStream.Write(array, 0, array.Length);
    //        requestStream.Close();

    //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    //        Console.WriteLine(new StreamReader(response.GetResponseStream()).ReadLine());
    //        Console.WriteLine(response.StatusCode);
    //        Console.ReadLine();

    //    }
    //    //class MyPolicy : ICertificatePolicy
    //    //{
    //    //    public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
    //    //    {
    //    //        return true;
    //    //    }
    //    //}
    //}
}