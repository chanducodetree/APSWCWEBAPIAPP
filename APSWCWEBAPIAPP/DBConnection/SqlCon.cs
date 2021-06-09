using APSWCWEBAPIAPP.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ModelService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Dynamic;
using AuthService;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel.Client;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace APSWCWEBAPIAPP.DBConnection
{
    public class SqlCon
    {
        private readonly string _connectionString;
        private string exFolder = Path.Combine("ExceptionLogs");
        private string exPathToSave = string.Empty;
        dynamic resultobj = new ExpandoObject();
        private ApplicationAPSWCCDbContext _context;
        private readonly ICaptchaService _authservice;

        public SqlCon(IConfiguration configuration, ApplicationAPSWCCDbContext apcontext, ICaptchaService auth)
        {
            _context = apcontext;
            _authservice = auth;
            exPathToSave = Path.Combine(Directory.GetCurrentDirectory(), exFolder);
            _connectionString = configuration.GetConnectionString("apswcproddb");
        }

        public async Task<dynamic> CheckLogin(MasterSp rootobj)
        {

            try
            {
                var captval = _context.captcha.FirstOrDefault(i => i.Id == rootobj.INPUT_03 && i.Capchid == rootobj.INPUT_04 && i.IsActive == 1);
                if (captval != null)
                {
                    rootobj.DIRECTION_ID = "1";
                    rootobj.TYPEID = "LOGIN";
                    var data = await APSWCMasterSp(rootobj);
                    if (data.Rows.Count > 0)
                    {
                        var user = new User() { UserName = data.Rows[0][0].ToString(), FirstName = data.Rows[0][1].ToString(), UserType = data.Rows[0][2].ToString() };
                        var tokenString = _authservice.GenerateJWT(user);
                        resultobj.StatusCode = 100;
                        resultobj.StatusMessage = "User Login Successfully.";
                        resultobj.Details = data;
                        resultobj.token = tokenString;
                    }
                    else
                    {
                        resultobj.StatusCode = 102;
                        resultobj.StatusMessage = "Invalid User Name OR Password";
                    }

                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Invalid Capcha";
                }

                return resultobj;

            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "Loginlogs", "CheckLogin : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured While Login";
                return resultobj;

            }
        }


        public async Task<dynamic> GetApswcWareHouseMaster()
        {
            try
            {
                MasterSp m = new MasterSp();
                m.DIRECTION_ID = "1";
                m.TYPEID = "101";
                //return await APSWCMasterSp(m);
                DataTable dt = await APSWCMasterSp(m);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {

                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetApswcWareHouseMasterlogs", "GetApswcWareHouseMaster:Method:" + jsondata));
                return ex.Message.ToString();
            }

        }

        public async Task<dynamic> GetBoardofDirectors()
        {
            try
            {
                MasterSp m = new MasterSp();
                m.DIRECTION_ID = "1";
                m.TYPEID = "BOARD_OF_DIRECTORS";
                //return await APSWCMasterSp(m);
                DataTable dt = await APSWCMasterSp(m);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {

                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetBoardofDirectorslogs", "GetBoardofDirectors:Method:" + jsondata));
                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Board of Directors";
                return resultobj;
            }
        }

        public async Task<dynamic> GetWorkLocations()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "WORK_LOCATION";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWorkLocationslogs", "GetWorkLocations : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Work Locations";
                return resultobj;

            }
        }
        public async Task<dynamic> GetDesignations()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "DESIGNATION";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmpListlogs", "GetEmpList : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Employees List";

                return resultobj;
            }
        }
        // resultobj.Details = await 

        public async Task<dynamic> GetEmpList()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "TOTAL_EMP_DETAILS";

                //resultobj.StatusCode = 100;
                //resultobj.StatusMessage = "Data Loaded Successfully";
                //resultobj.Details = await APSWCMasterSp(rootobj);
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmpListlogs", "GetEmpList : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Employees List";

                return resultobj;

            }
        }

        public async Task<dynamic> SaveInspectionPhotos(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "201";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                // resultobj.Details = await 

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveInspectionPhotos : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Inspection Photos";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveHomePageConent(MasterSp rootobj)
        {

            try
            {


                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                // resultobj.Details = await 

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetWorkLocations : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Work Locations";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveNewsScrollMessage(MasterSp rootobj)
        {

            try
            {


                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                // resultobj.Details = await 

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetWorkLocations : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Work Locations";
                return resultobj;

            }
        }

        public async Task<dynamic> GetNewsScrollMessageData(MasterSp rootobj)
        {

            try
            {


                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                    resultobj.Details = dt;
                }
                // resultobj.Details = await 

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetWorkLocations : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Work Locations";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveServiceCharter(MasterSp rootobj)
        {

            try
            {


                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                // resultobj.Details = await 

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveServiceCharter : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Work Locations";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveContactRegisteration(MasterSp rootobj)
        {

            try
            {


                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                // resultobj.Details = await 

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Savecontactregistration : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Work Locations";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveGalleryRegisteration(MasterSp rootobj)
        {

            try
            {


                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                // resultobj.Details = await 

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Savecontactregistration : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Work Locations";
                return resultobj;

            }
        }

        public async Task<dynamic> Saveboardofdirectors(MasterSp rootobj)
        {

            try
            {


                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                // resultobj.Details = await 

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Savecontactregistration : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Work Locations";
                return resultobj;

            }
        }

        public async Task<dynamic> GetEmployeeTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "EMP_TYPE";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeTypeslogs", "GetEmployeeTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Employee Types";
                return resultobj;
            }
        }


        public async Task<dynamic> GetHomepageContent()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "3";
                rootobj.TYPEID = "GET_HOMEPAGE";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeTypeslogs", "GetEmployeeTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Employee Types";
                return resultobj;
            }
        }

        public async Task<dynamic> GetGalleryImages()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "202";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeTypeslogs", "GetEmployeeTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Employee Types";
                return resultobj;
            }
        }

        public async Task<dynamic> GetVisitorsCount()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "3";
                rootobj.TYPEID = "SITE_VISITORS";
                rootobj.INPUT_01 = Logfile.Browsename();
                rootobj.INPUT_02 = Logfile.GetLocalIPAddress();
                rootobj.INPUT_03 = Logfile.MachineName(Logfile.GetLocalIPAddress());

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeTypeslogs", "GetEmployeeTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Not Get Ip Address";
                return resultobj;
            }
        }

        public async Task<dynamic> GetEducations()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "EDU";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEducationslogs", "GetEducations : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Educations";
                return resultobj;

            }
        }

        public async Task<dynamic> GetDistricts()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "DISTRICTS";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetDistrictslogs", "GetDistricts : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Districts";
                return resultobj;

            }
        }

        public async Task<dynamic> GetAreaTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "URBAN_RURAL";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetAreaTypeslogs", "GetAreaTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Area Types";
                return resultobj;

            }
        }

        public async Task<dynamic> GetMandlas(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "MMC";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetMandlaslogs", "GetMandlas : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Mandals";
                return resultobj;

            }
        }

        public async Task<dynamic> GetVillages(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "WARD_VILLAGE";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetVillageslogs", "GetVillages : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Villages";
                return resultobj;

            }
        }

        public async Task<dynamic> GetStorageTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "STORAGE_TYPE";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetStorageTypeslogs", "GetStorageTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Storage Types";
                return resultobj;

            }
        }

        public async Task<dynamic> GetChargeDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "STORAGE_LIST";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetChargeDetailslogs", "GetChargeDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Charge Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetSections()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "SECTION";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetSectionslogs", "GetSections : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Sections";
                return resultobj;

            }
        }

        public async Task<dynamic> GetServiceCharterDetails(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "SERVICE_CHARTER";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetServiceCharterDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Service Charter Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetBloodGroups()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "BLOOD_GROUP";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetBloodGroupslogs", "GetBloodGroups : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Blood Groups";
                return resultobj;

            }
        }

        public async Task<dynamic> GetExperianceYears()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "EXPERIANCE_YEARS";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetExperianceYearslogs", "GetExperianceYears : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Experiance Years";
                return resultobj;

            }
        }

        public async Task<dynamic> GetExperianceMonths()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "EXPERIANCE_MONTH";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetExperianceMonthslogs", "GetExperianceMonths : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Experiance Months";
                return resultobj;

            }
        }

        public async Task<dynamic> GetNationality()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "SECTION";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetNationalitylogs", "GetNationality : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Nationality";
                return resultobj;

            }
        }

        public async Task<dynamic> GetReligions()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "RELIGION";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetReligionslogs", "GetReligions : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Religions";
                return resultobj;

            }
        }

        public async Task<dynamic> GetCommunities()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "COMMUNITY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetCommunitieslogs", "GetCommunities : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Communities";
                return resultobj;

            }
        }

        public async Task<dynamic> GetMaritalStatus()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "MARITAL_STATUS";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetMaritalStatuslogs", "GetMaritalStatus : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Marital Status";
                return resultobj;

            }
        }

        public async Task<dynamic> GetStates()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "STATES";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetStateslogs", "GetStates : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load States";
                return resultobj;

            }
        }

        public async Task<dynamic> GetSpaceDetails()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "SPACE_HOMEPAGE";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetSpaceDetailslogs", "GetSpaceDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Space Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetFiveYearsReport()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "SPACE_ABSTRACT";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetFiveYearsReportlogs", "GetFiveYearsReport : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Five Years Report";
                return resultobj;

            }
        }

        public async Task<dynamic> GetLocations(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "LOCATION_LIST";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetLocationslogs", "GetLocations : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Locations";
                return resultobj;

            }
        }


        public async Task<dynamic> GetContactLists(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "CONTACT";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetLocations : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Locations";
                return resultobj;

            }
        }

        public async Task<dynamic> GetRManagers()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "REPORTING_OFFICER";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetRManagerslogs", "GetRManagers : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Managers";
                return resultobj;

            }
        }



        public async Task<dynamic> GetRelations()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "RELATION";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetRelationslogs", "GetRelations : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Relations";
                return resultobj;

            }
        }

        public async Task<dynamic> GetGenders()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "GENDER";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetGenderslogs", "GetGenders : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Genders";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmpPrimaryDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "101";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmpPrimaryDetailslogs", "SaveEmpPrimaryDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employee General Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmpCommuDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "102";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmpPrimaryDetailslogs", "SaveEmpCommuDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employee Communication Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmpWorkDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "103";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }


                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmpWorkDetailslogs", "SaveEmpWorkDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employee Work Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmpBankDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "104";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmpBankDetailslogs", "SaveEmpBankDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employee Bank Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmpFamilyDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "105";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmpFamilyDetailslogs", "SaveEmpFamilyDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employee Family Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmpPFDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "106";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmpPFDetailslogs", "SaveEmpPFDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employee Providend Fund Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetIFSCCodeDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "BANK_DETAILS";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetIFSCCodeDetailslogs", "GetIFSCCodeDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get IFSC Code Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetEmpFamilyDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "FAMILY_DETAILS";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmpFamilyDetailslogs", "GetEmpFamilyDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee Family Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetEmployeeDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "EMP_DETAILS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeDetailslogs", "GetEmployeeDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdatedEmployeeDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "107";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdatedEmployeeDetailslogs", "UpdatedEmployeeDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Employee Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWHRegionalofficers()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "REGION";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetRegionallogs", "GetRegionaloffice : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Regional Offices";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWHType()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "WH_TYPE";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWarehouseTypeslogs", "GetGetWarehouseTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Warehouse Types";
                return resultobj;

            }
        }

        public async Task<dynamic> GetFacilitiesAvailable()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "FACILITY_AVAILABLE";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetFacilitiesAvailablelogs", "GetFacilitiesAvailable : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Warehouse Available Facilities";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWHList(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "WH_LIST";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWarehouseListlogs", "GetWarehouseList : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Warehouse List";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWH_View(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "WH_VIEW";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWarehouseListlogs", "GetWarehouseList : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Warehouse List";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWH_History(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "3";
                rootobj.TYPEID = "GET_LOG_HISTORY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWarehouseHistorylogs", "GetWarehouseHistory : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Warehouse History";
                return resultobj;

            }
        }

        public async Task<dynamic> GetRegionDistricts(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "REGN_DIST";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetDistrictslogs", "GetDistricts : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Regional Districts";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveWareHouseDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "301";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveWarehouseDetailslogs", "SaveWarehouseDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Warehouse Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetPageDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "602";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetPageDetailslogs", "GetPageDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetPageDetailsByID(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "611";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetPageDetailsByIDlogs", "GetPageDetailsByID : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetPageAccessDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = rootobj.INPUT_01 == "DD" ? "604" : "603";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetPageDetailslogs", "GetPageDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Get_DesigPageAccessDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = rootobj.INPUT_01 == "DD" ? "607" : "606";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetPageDetailslogs", "GetPageDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Page Details";
                return resultobj;

            }
        }
        public async Task<dynamic> SavePageDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "601";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveWarehouseDetailslogs", "SaveWarehouseDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdatePageDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "612";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdatePageDetailslogs", "UpdatePageDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveUseraccessDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = rootobj.INPUT_08 == "Designation_Permission" ? "608" : "605";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveUseraccessDetailslogs", "SaveUseraccessDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Useracces Details";
                return resultobj;

            }
        }
        public async Task<dynamic> UpdateWareHouseDetails(MasterSp rootobj)
        {
            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "302";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdateWarehouseDetailslogs", "UpdateWarehouseDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Warehouse Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdateWareHouseDetails_all(MasterSp rootobj)
        {
            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "305";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Updated Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }




                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdateWarehouseDetails_alllogs", "UpdateWarehouseDetails_all : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Warehouse Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdateEmpPrimaryDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "108";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdateEmpPrimaryDetailslogs", "UpdateEmpPrimaryDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Employee General Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveChangePassword(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "501";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveChangePasswordlogs", "SaveChangePassword : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Change Password";
                return resultobj;

            }
        }

        public async Task<dynamic> ProfileUpdation(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "110";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Profile Updated Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveChangePasswordlogs", "SaveChangePassword : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Change Password";
                return resultobj;

            }
        }

        public async Task<dynamic> Getmasterslist(MasterSp objMa)
        {
            //MasterSp rootobj = new MasterSp();
            try
            {
                //rootobj.DIRECTION_ID = "1";
                //rootobj.TYPEID = "EMP_TYPE";
                DataTable dt = await APSWCMasterlist(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Getmasterslist : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Employee Types";
                return resultobj;
            }
        }

        public async Task<dynamic> SaveEmpmasterreg(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "304";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterRegistrationlogs : Method:" + jsondata + " , Input Data : " + inputdata));
                if (ex.Message.Contains("ORA-00001"))
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "already Data Submitted For This submitted type";
                    return resultobj;
                }


                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save MasterDetails";
                return resultobj;

            }
        }

        public async Task<dynamic> updateEmpmasterreg(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "303";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterUpdatelogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Master Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdateAdminEmpPrimaryDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "109";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdateAdminEmpPrimaryDetailslogs", "UpdateAdminEmpPrimaryDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Employee General Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetEmployeeHistory(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "3";
                rootobj.TYPEID = "GET_LOG_HISTORY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeHistorylogs", "GetEmployeeHistory : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee History";
                return resultobj;

            }
        }

        public async Task<dynamic> GetmastersHistorylist(MasterSp objMa)
        {

            try
            {
                objMa.DIRECTION_ID = "3";
                objMa.TYPEID = "GET_LOG_HISTORY";

                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No History Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetmastersHistorylist : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Master History";
                return resultobj;
            }
        }

        public async Task<dynamic> GetLeaveDetails()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "LEAVES";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetLeaveDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Leave Master Types Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetLeaveTypes_Settings()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "402";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetLeaveTypes_Settings : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while  GetLeaveTypes_Settings Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveLeaveMaster(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "401";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveLeaveMaster : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Leave Master Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdateLeaveMaster(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "404";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Updated Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "UpdateLeaveMaster : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update LeaveMaster Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveHolidayMaster(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "406";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveHolidayMaster : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Holiday Master Details";
                return resultobj;

            }
        }
        public async Task<dynamic> UpdateHolidayMaster(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "407";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Updated Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "UpdateHolidayMaster : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update HolidayMaster Work Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetHolidayMaster(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "HOLIDAYS";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetHolidayMaster : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while GetHolidayMaster Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetHolidayMaster_VIEW(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "HOLIDAYS_VIEW";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetHolidayMaster_VIEW : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while GetHolidayMaster_VIEW Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetLeaveMaster_VIEW(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "LEAVES_VIEW";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetLeaveMaster_VIEW : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while GetLeaveMaster_VIEW Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetRegistrationTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "REGISTRATION";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetRegistrationTypesLogs", "GetRegistrationTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Registration Types";

                return resultobj;

            }
        }
        public async Task<dynamic> GetDocTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "DOCUMENT";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetDocTypesLogs", "GetDocTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Document Types";

                return resultobj;

            }
        }
        public async Task<dynamic> GetReqDocByReg(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "UPD_DOC";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetReqDocByRegLogs", "GetReqDocByReg : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Load Document List";

                return resultobj;

            }
        }
        public async Task<dynamic> SaveRegCommuDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "703";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveRegCommuDetailslogs", "SaveRegCommuDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Communication Details";
                return resultobj;

            }
        }
        public async Task<dynamic> SaveFarCommuDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "714";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveFarCommuDetailslogs", "SaveFarCommuDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Communication Details";
                return resultobj;

            }
        }
        public async Task<dynamic> SaveRegistrationDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "701";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveRegistrationDetailslogs", "SaveRegistrationDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Registration Details";
                return resultobj;

            }
        }
        public async Task<dynamic> SaveRegDocDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "702";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Save Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveRegDocDetailslogs", "SaveRegDocDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Documents Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmpLeave(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "405";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Inserted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveEmpLeave : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save EmpLeave Master Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetEmpLeaves(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "408";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetEmpLeaves : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee Leaves Master Details";
                return resultobj;

            }
        }

        public async Task<dynamic> EmpLeave_Cancel(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "409";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Updated Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "EmpLeave_Cancel : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Employee Leave Cancel  Details";
                return resultobj;

            }
        }

        public async Task<dynamic> EmpLeaveTypes_Get(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "410";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "EmpLeaveTypes_Get : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while  Employee LeaveTypes Get  Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveOfficeTimings(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "411";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveOfficeTimings : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save OfficeTimings Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdateOfficeTimings(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "412";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "UpdateOfficeTimings : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update OfficeTimings Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetOfficeTimings(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "413";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetOfficeTimings : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get OfficeTimings Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveOutsourcingAgency(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "706";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveOutsourcingAgencylogs", "SaveOutsourcingAgency : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Outsourcing Agency Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdateOutsourcingAgency(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "711";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdateOutsourcingAgencylogs", "UpdateOutsourcingAgency : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Outsourcing Agency Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetOutsourcingAgencies()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "707";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetOutsourcingAgenciesLogs", "GetOutsourcingAgencies : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Outsourcing Agencies";

                return resultobj;

            }
        }

        public async Task<dynamic> GetDetailsofOutsourcing(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "708";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetDetailsofOutsourcingLogs", "GetDetailsofOutsourcing : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Details of Outsourcing";

                return resultobj;

            }
        }

        public async Task<dynamic> GetOutsourceContactDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "709";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetOutsourcingAgenciesLogs", "GetOutsourcingAgencies : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Outsourcing Agencies";

                return resultobj;

            }
        }

        public async Task<dynamic> GetEmpLeaveDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "414";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetEmpLeaveDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get EmpLeaveDetails Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetNoOfDays(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "415";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetNoOfDays : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get NoOfDays Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetEmployeeLeavesHistory(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "3";
                rootobj.TYPEID = "GET_LOG_HISTORY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "EmployeeLeavesHistorylogs", "GetEmployeeLeavesHistory : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee Leaves History";
                return resultobj;

            }
        }

        public async Task<dynamic> GetuserAccessmenu(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "609";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetMenuDetailslogs", "GetMenuDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Menu Details";
                return resultobj;

            }
        }


        public async Task<dynamic> GetExistingUserDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "704";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetUserDetailslogs", "GetUserDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load User Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetSubmittedDocs(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "705";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetUserDetailslogs", "GetUserDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load User Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetCommuDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "710";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetCommuDetailslogs", "GetComDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Communication Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetCommodityGroupDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "4";
                //rootobj.TYPEID = rootobj.INPUT_01!=null? "REZ_COMMODITY" : "COMMODITY_GROUP";
                rootobj.TYPEID = rootobj.INPUT_02 == "RATE" ? "REZ_RATE" : rootobj.INPUT_02 == "COMMODITY" ? "REZ_COMMODITY" : "COMMODITY_GROUP";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetCommodityDetailslogs", "GetCommodityDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Commodity Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveSpaceReservation(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "4";
                rootobj.TYPEID = "101";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;


            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SpaceReservationDetailslogs", "SpaceReservationDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save SpaceReservation Details";
                return resultobj;

            }
        }



        public async Task<dynamic> WeighmentTokenList(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = rootobj.INPUT_02 == "WEIGHT_INTKN" ? "WEIGHT_IN" : rootobj.INPUT_02 == "COMDLISTOFTKN" ? "COMMODITY_DEATILS" : "";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Token Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetVarietyGradeList(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = rootobj.INPUT_04 == "VARIETY" ? "VARIETY" : rootobj.INPUT_04 == "GRADE" ? "GRADE" : "";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Variety/Grade Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Variety/Grade Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetQualityPrameters(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "QUALITY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Quality Parameters Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Quality Parameters Details";
                return resultobj;

            }
        }


        public async Task<dynamic> SaveQualityChecking(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "104";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;


            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "QualityCheckingDetailslogs", "QualityCheckingDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save QualityChecking Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveWeighmentOut(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "106";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;


            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "WeighmentOutDetailslogs", "WeighmentOutDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save SaveWeighmentOut Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Weighment_OUTTokenList(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = rootobj.INPUT_02 == "WEIGHT_OUTTKN" ? "WEIGHT_OUT" : rootobj.INPUT_02 == "COMDLISTOFTKN" ? "WEIGHT_OUT_COMMODITY" : "";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Token Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmpAttendance(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "416";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Inserted Successfully";
                    resultobj.Details = dt;
                   }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                  }

                return resultobj;
              }
            catch (Exception ex)
            {
              string jsondata = JsonConvert.SerializeObject(ex.Message);
              string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveEmpAttendance : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save EmpLeave Attendance Details";
                return resultobj;
            }
        }

        public async Task<dynamic> Getcommoditieslist(MasterSp objMa)
        {

            try
            {
                objMa.DIRECTION_ID = "1";

                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                    
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Getmasterslist : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Master Details";
                return resultobj;

            }
        }
        public async Task<dynamic> GetEmpAttendanceDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "417";

                DataTable dt = await APSWCMasterSp(rootobj);
                 if (dt != null && dt.Rows.Count > 0)
                  {
                      resultobj.StatusCode = 100;
                      resultobj.StatusMessage = "Data Loaded Successfully";
                      resultobj.Details = dt;
                  }
                  else
                  {
                      resultobj.StatusCode = 102;
                      resultobj.StatusMessage = "No Data Found";
                  }

                  return resultobj;
            }
              catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetEmpAttendanceDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee Attendance Details";
                return resultobj;
            }
        }

        public async Task<dynamic> GetRegionmasterslist(MasterSp objMa)
        {

            try
            {
                objMa.DIRECTION_ID = "1";


                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Getmasterslist : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Master History";
                return resultobj;
            }
        }

        public async Task<dynamic> GetRegionbydistcode(MasterSp objMa)
        {

            try
            {
                objMa.DIRECTION_ID = "1";
                objMa.TYPEID = "REG_DIST";

                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Getmasterslist : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Master Details";
                return resultobj;
            }
        }

        public async Task<dynamic> insertcommodity(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "306";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterregLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while insert Master Details";
                return resultobj;

            }
        }
      public async Task<dynamic> updateCommodities(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "307";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterUpdatelogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Master Details";
                return resultobj;

            }
        }
      
        public async Task<dynamic> GetCalendarDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "418";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                   resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetCalendarDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Calendar Details";
                return resultobj;
            }
        }

        public async Task<dynamic> Emp_CheckIn_Out(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "416";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Emp_CheckIn_Out : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Emp_CheckIn_Out Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Get_Emp_CheckIn_Out_Details(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "417";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Get_Emp_CheckIn_Out_Details : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get_Emp_CheckIn_Out_Details Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Emp_All_Events(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "418";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Emp_All_Events : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Emp_All_Events Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Emp_Notes_Save(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "419";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Emp_Notes_Save : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Emp_Notes_Save Details";
                return resultobj;

            }
        }

        #region DeadStock
        public async Task<dynamic> GetDeadStockDetails(MasterSp objMa)
        {
            try
            {

                objMa.DIRECTION_ID = "5";
                objMa.TYPEID = objMa.INPUT_01 == "CAT_LIST" ? "105" : objMa.INPUT_01 == "SEC_DEADSTOCK_LIST" ? "112" : objMa.INPUT_01 == "RGN_DEADSTOCK_LIST" ? "111" : objMa.INPUT_01 == "SUBCAT_LIST" ? "106" : objMa.INPUT_01 == "ITEM_LIST" ? "110" : objMa.INPUT_01 == "SECTION_DD" ? "101" : objMa.INPUT_01 == "CATEGORY_DD" ? "103" : objMa.INPUT_01 == "SUBCATEGORY_DD" ? "108" : objMa.INPUT_01 == "ITEM_DD" ? "109" : objMa.INPUT_01 == "DEADSTOCK_LIST" ? "113" : "105";
                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetDeadStockDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Getting Data";
                return resultobj;
            }
        }

        public async Task<dynamic> DeadStockInsertion(MasterSp objMa)
        {
            try
            {

                objMa.DIRECTION_ID = "5";
                objMa.TYPEID = objMa.INPUT_01 == "MAINTENANCE_CATEGORY" ? "102" : objMa.INPUT_01 == "CANCEL_RECORD" ? "114" : objMa.INPUT_01 == "MAINTENANCESUB_CATEGORY" ? "102" : objMa.INPUT_01 == "WAREHOUSE_ITEM_TYPE" ? "102" : objMa.INPUT_01 == "ACTIVE_INACTIVE" ? "107" : objMa.INPUT_01 == "FORM_SUBMISSION" ? "104" : "102";
                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RTN_ID"].ToString() == "1")
                    {
                        resultobj.StatusCode = 100;
                        resultobj.StatusMessage = "Data Inserted Successfully";
                        resultobj.Details = dt;
                    }
                    else
                    {
                        resultobj.StatusCode = 102;
                        resultobj.StatusMessage = "Data Insertion Failed";
                    }
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Data Insertion Failed";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "DeadStockInsertion : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Getting Data";
                return resultobj;
            }
        }

        public async Task<dynamic> GetDeadStockHistory(MasterSp objMa)
        {
            try
            {

                objMa.DIRECTION_ID = "3";
                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetDeadStockDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Getting Data";
                return resultobj;
            }
        }
        #endregion



        #region Help
        public async Task<dynamic> GetHelpDetails(MasterSp objMa)
        {
            try
            {

                objMa.DIRECTION_ID = "9";
                objMa.TYPEID = objMa.INPUT_01 == "PAGE_DROPDOWN" ? "PAGE_DROPDOWN" : objMa.INPUT_01 == "PAGE_ID" ? "PAGE_ID" : objMa.INPUT_01 == "ASSET_MANAGEMENT" ? "ASSET_MANAGEMENT" : objMa.INPUT_01 == "PAGE" ? "PAGE" : objMa.INPUT_01 == "SUBMITTED_PAGES" ? "SUBMITTED_PAGES" : "PAGE_DROPDOWN";
                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetHelpDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Getting Data";
                return resultobj;
            }
        }

        public async Task<dynamic> HelpInsertion(MasterSp objMa)
        {
            try
            {
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log("Mail", "HelpInsertionLog", "Input Data : " + inputdata));

                objMa.DIRECTION_ID = "9";
                objMa.TYPEID = objMa.INPUT_01 == "HELP_SUBMISSION" ? "101" : objMa.INPUT_01 == "ASSET_FORM_SUBMISSION" ? "401" : objMa.INPUT_01 == "301" ? "301" : "101";
                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["RTN_ID"].ToString() == "1")
                    {
                        resultobj.StatusCode = 100;
                        resultobj.StatusMessage = "Data Inserted Successfully";
                        resultobj.Details = dt;
                    }
                    else
                    {
                        resultobj.StatusCode = 102;
                        resultobj.StatusMessage = "Data Insertion Failed";
                    }
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Data Insertion Failed";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "HelpInsertion : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Getting Data";
                return resultobj;
            }
        }

        public async Task<dynamic> GetHelpHistory(MasterSp objMa)
        {
            try
            {

                objMa.DIRECTION_ID = "3";
                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetHelpHistory : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Getting Data";
                return resultobj;
            }
        }
        #endregion

        #region Mail
        public async Task<dynamic> SendMail(MasterSp objMa)
        {
            try
            {
                objMa.DIRECTION_ID = "9";
                objMa.TYPEID = "ESIGN";
                string userid = "";
                string pwd = "";
                int port;
                string domain = "";
                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    userid = EncDecrpt.Decrypt_Data(dt.Rows[0]["USER_NAME"].ToString());
                    pwd = EncDecrpt.Decrypt_Data(dt.Rows[0]["PASSWORD"].ToString());
                    domain = dt.Rows[0]["EMAIL_DOMAIN"].ToString();
                    port = Convert.ToInt32(dt.Rows[0]["PORT_NO"].ToString());
                    if (userid.Contains("\""))
                        userid = userid.Replace("\"", "");
                    if (pwd.Contains("\""))
                        pwd = pwd.Replace("\"", "");

                    string inputdata = JsonConvert.SerializeObject(objMa);
                    Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log("Mail", "SendMailLog", "Input Data : " + inputdata));

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient();
                    if (domain == "@ap.gov.in")
                    {
                        SmtpServer = new SmtpClient("relay.nic.in");
                    }
                    else if (domain == "@gmail.com")
                    {
                        SmtpServer = new SmtpClient("smtp.gmail.com");
                    }

                    else if (domain == "@hotmail.com")
                    {
                        SmtpServer = new SmtpClient("smtp.live.com");
                    }

                    mail.From = new MailAddress(userid);

                    mail.To.Add(objMa.INPUT_01);
                    if (!string.IsNullOrEmpty(objMa.INPUT_02))
                        mail.CC.Add(objMa.INPUT_02);
                    mail.Subject += objMa.INPUT_03;
                    mail.Body += objMa.INPUT_04;

                    SmtpServer.Port = port;
                    SmtpServer.UseDefaultCredentials = true;
                    //SmtpServer.Credentials = new System.Net.NetworkCredential("gmit-apswc@ap.gov.in", "J7#oK3#nD2");
                    SmtpServer.Credentials = new System.Net.NetworkCredential(userid, pwd);
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                    SmtpServer.Send(mail);

                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Mail Sent Successfully";
                    resultobj.Details = "";
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Mail Sending Failed";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SendMail : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Getting Data";
                return resultobj;
            }
        }

        public async Task<dynamic> TestMail(MasterSp objMa)
        {
            try
            {
                string userid = EncDecrpt.Decrypt_Data(objMa.INPUT_02);
                string pwd = EncDecrpt.Decrypt_Data(objMa.INPUT_03);
                string domain = objMa.INPUT_04;
                int port = Convert.ToInt32(objMa.INPUT_05);
                string subject = objMa.INPUT_06;
                string body = objMa.INPUT_07;

                if (userid.Contains("\""))
                    userid = userid.Replace("\"", "");
                if (pwd.Contains("\""))
                    pwd = pwd.Replace("\"", "");
                if (domain == "@ap.gov.in")
                {
                    string inputdata = JsonConvert.SerializeObject(objMa);
                    Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log("Mail", "TestMailLog", "Input Data : " + inputdata));

                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("relay.nic.in");
                    mail.From = new MailAddress(userid);

                    mail.To.Add(userid);
                    mail.Subject += subject;
                    mail.Body += body;

                    SmtpServer.Port = port;
                    SmtpServer.UseDefaultCredentials = true;
                    //SmtpServer.Credentials = new System.Net.NetworkCredential("gmit-apswc@ap.gov.in", "J7#oK3#nD2");
                    SmtpServer.Credentials = new System.Net.NetworkCredential(userid, pwd);
                    SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                    SmtpServer.Send(mail);

                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = "";
                }
                else if (domain == "@gmail.com")
                {
                    var fromAddress = new MailAddress(userid);
                    var toAddress = new MailAddress(userid);
                    string fromPassword = pwd;

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = port,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }

                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = "";
                }
                else if (domain == "@hotmail.com")
                {
                    SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
                    var mail = new MailMessage();
                    mail.From = new MailAddress(userid);
                    mail.To.Add(userid);
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;
                    string htmlBody;
                    htmlBody = body;
                    mail.Body = htmlBody;
                    SmtpServer.Port = port;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(userid, pwd);
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "TestMail : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Getting Data";
                return resultobj;
            }
        }

        #endregion

        public async Task<dynamic> GetLayoutConfiguration(MasterSp rootobj)
        {
            try
            {

                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "713";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 )
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveLayoutConfigurationlogs", "SaveLayoutConfiguration : Method:" + jsondata ));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Layout Configuration Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveLayoutConfiguration(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "712";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveLayoutConfigurationlogs", "SaveLayoutConfiguration : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Layout Configuration Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetQntity_cntrct_Details(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "4";
                rootobj.TYPEID = "CONTRACT_QUANTITY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Quantity/Contarct Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Emp_Leave_Balance(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "420";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Emp_Leave_Balance : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Emp_Leave_Balance Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Emp_Leave_Balance_Save(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "421";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Emp_Leave_Balance_Save : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Emp_Leave_Balance_Save Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Emp_In_Out_Get(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "422";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Emp_In_Out_Get : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Emp_In_Out_Get Details";
                return resultobj;

            }
        }

        #region Depositor Receipt In

        public async Task<dynamic> Savereceiptin(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "108";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;


            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveReceipt_InDetailslogs", "SaveReceipt_InDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Save Receipt-In Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetReceiptInTokens(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "TO_GATEIN";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Token Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetSpaceReservationDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "GEN_BOOKING";
               
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetSpaceReservationDetailsLogs", "GetSpaceReservationDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Space Reservation Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetTransportDetails()
        {
            try
            {
                MasterSp rootobj = new MasterSp();
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "TRANSPORT_TYPE";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTransportDetailsLogs", "GetTransportDetails : Method:" + jsondata ));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Trasport Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveGateIn(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "102";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveGateInlogs", "SaveGateIn : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Gate IN Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetTokens(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "GET_TOKEN";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTokensLogs", "GetTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetTokenInfo(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "GET_BOOKING_DETAILS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTokenInfoLogs", "GetTokenInfo : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveWeighmentIn(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "103";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveWeighmentInlogs", "SaveWeighmentIn : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Weighment IN Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetDumpingTokens(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "DUMP_IN";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetDumpingTokensLogs", "GetDumpingTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveDumping(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "113";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveDumpinglogs", "SaveDumpingIn : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Dumping Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetStackTokens(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "STACK_TOKEN";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetStackTokensLogs", "GetStackTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetGodowns(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "GET_GODOWNS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetGodownsLogs", "GetGodowns : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Godowns Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetStackTokenInfo(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "STACK_COMMODITY";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetStackTokenInfoLogs", "GetStackTokenInfo : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetCompartments(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "GET_COMPARTMENTS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetCompartmentsLogs", "GetCompartments : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Compartments Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetStacks(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "GET_STACKS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetStacksLogs", "GetStacks : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Stacks Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetStackTypes()
        {
            try
            {
                MasterSp rootobj = new MasterSp();
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "STACK_TYPE";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetStackTypesLogs", "GetStackTypes : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Stack Types Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveStacking(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "105";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveStackinglogs", "SaveStacking : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Stacking Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetGateoutTokens(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "GATE_OUT_TOKEN";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetGateoutTokensLogs", "GetGateoutTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveGateOut(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "107";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveGateOutlogs", "SaveGateOut : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Gate Out Details";
                return resultobj;

            }
        }

        #endregion

        public async Task<dynamic> GetInsurancesList()
        {
            try
            {
                MasterSp rootobj = new MasterSp();
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "INSURANCE_LIST";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetInsurancesListLogs", "GetInsurancesList : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Insurences Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetInsuranceByID(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "INSURANCE_GET";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetInsuranceByIDLogs", "GetInsuranceByID : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Insurance Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveInsuranceDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "101";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveInsuranceDetailslogs", "SaveInsuranceDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Insurance Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdateInsuranceDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "102";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdateInsuranceDetailslogs", "UpdateInsuranceDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Insurance Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Getvarietylist(MasterSp objMa)
        {

            try
            {
                objMa.DIRECTION_ID = "6";

                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Getmasterslist : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Master Details";
                return resultobj;
            }
        }

        public async Task<dynamic> Getcommodtilistbycomdid(MasterSp objMa)
        {

            try
            {
                objMa.DIRECTION_ID = "6";

                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Getmasterslist : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Master Details";
                return resultobj;
            }
        }



        public async Task<dynamic> Savecommoditymasterreg(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "201";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterRegistrationlogs : Method:" + jsondata + " , Input Data : " + inputdata));
                if (ex.Message.Contains("ORA-00001"))
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "already Data Submitted For This submitted type";
                    return resultobj;
                }


                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save MasterDetails";
                return resultobj;

            }
        }

        public async Task<dynamic> updateCommodityvarietyreg(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "202";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterUpdatelogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Master Details";
                return resultobj;

            }
        }


        public async Task<dynamic> SaveQualityparams(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "203";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterRegistrationlogs : Method:" + jsondata + " , Input Data : " + inputdata));
                if (ex.Message.Contains("ORA-00001"))
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "already Data Submitted For This submitted type";
                    return resultobj;
                }


                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save MasterDetails";
                return resultobj;

            }
        }

        public async Task<dynamic> updateQualityparams(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "204";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterUpdatelogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Master Details";
                return resultobj;

            }
        }



        public async Task<dynamic> SavecommodityGroup(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "306";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterregLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while insert Master Details";
                return resultobj;

            }
        }

        public async Task<dynamic> updateCommodityGroup(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "307";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "employeemasterUpdatelogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Master Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWH_InspectionData()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "WAREHOUSE";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWH_InspectionDatalogs", "GetWH_InspectionData : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get WareHouse Inspection List";

                return resultobj;

            }
        }
        public async Task<dynamic> WH_Inspection_Save(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "SCHEDULING";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "WH_Inspection_Savelogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save WareHouse Inspection Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Get_Login_Sheduled(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "SCHEDULED_GET";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "Get_Login_SheduledLogs", "Get_Login_Sheduled : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get User Login Sheduled Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Get_ReSheduled_Inspection(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "RE-SCHEDULE";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Get_ReSheduled_Inspectionlogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Re-Sheduled Inspection Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Scheduled_Inspection_Review(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "SCHEDULE_REVIEW";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SheduledInspectionReviewlogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Sheduled Inspection Review Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Review_Inspection(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "SCHEDULE_REVIEWGET";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "Review_InspectionLogs", "Review_Inspection : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get User Inspection Review Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWHRegions()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "REGION";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWHRegionslogs", "GetWHRegions : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get WareHouse Regions List";

                return resultobj;

            }
        }
        public async Task<dynamic> GetWHDistricts(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "DISTRICT";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWHDistrictsLogs", "GetWHDistricts : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get WareHouse District Details";
                return resultobj;

            }
        }
        public async Task<dynamic> GetWHInspectionList(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "WAREHOUSE";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWHInspectionListLogs", "GetWHInspectionList : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get WareHouse Inspection Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetInspectionHistory(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "GET_LOG_HISTORY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetInspectionHistorylogs", "GetInspectionHistory : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Virtual Inspection History";
                return resultobj;

            }
        }

        #region Depositor Receipt Out
        public async Task<dynamic> GetStackinDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "STACK_IN_DETAILS";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetStackinDetailsLogs", "GetStackinDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Stack-in Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveReceiptOutRequest(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "108";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveReceiptOutRequestLogs", "SaveReceiptOutRequest : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Receipt Out Request Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetReceiptOutDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "RECEIPT_OUT_DETAILS";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetReceiptOutDetailsLogs", "GetReceiptOutDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetReceiptDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "RECEIPT_DETAILS";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetReceiptDetailsLogs", "GetReceiptDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetStackInCommodities(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "STACK_IN_COMMODITIES";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetStackInCommoditiesLogs", "GetStackInCommodities : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> QC_OUT_Tokens(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "RO_QUALITYIN";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Token Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Out_Gateouttokens(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "RO_GATE_OUT";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Token Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWeighmentinToken(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "WEIGHT_IN_RECEIPTS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTokensLogs", "GetTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveOutWeighmentIn(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "109";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveOutWeighmentInLogs", "SaveOutWeighmentInLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save OutWeighment IN Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWeighmentoutToken(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "WEIGHT_OUT_RECEIPTS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTokensoutLogs", "GetTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveOutWeighmentout(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "110";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveOutWeighmentoutLogs", "SaveOutWeighmentoutLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save OutWeighment Details";
                return resultobj;

            }
        }

        #endregion

        public async Task<dynamic> DeadStockCategory()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "5";
                rootobj.TYPEID = "103";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DeadStockCategorylogs", "DeadStockCategory : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Dead Stock Category Details";

                return resultobj;

            }
        }

        public async Task<dynamic> DeadStocksubCategory(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "5";
                rootobj.TYPEID = "108";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DeadStocksubCategoryLogs", "DeadStocksubCategory : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Dead Stock Sub Category List";
                return resultobj;

            }
        }

        public async Task<dynamic> GetItemTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "5";
                rootobj.TYPEID = "109";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetItemTypeslogs", "GetItemTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Item Types List";

                return resultobj;

            }
        }

        public async Task<dynamic> DeadstockDetailsSave(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "5";
                rootobj.TYPEID = "104";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "DeadstockDetailsSavelogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Deadstock Insertion Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetPeriodicQCDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "PERIODIC_QUALITY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Periodic Quality Checking Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SavePeriodicQualityChecking(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "111";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;


            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "PQCheckingDetailslogs", "PQualityCheckingDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save PeridoicQualityChecking Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetDeadStockDeatails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "5";
                rootobj.TYPEID = "113";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetDeadStockDeatailsLogs", "GetDeadStockDeatails : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get WareHouse DeadStock Details";
                return resultobj;

            }
        }

        public async Task<dynamic> DeadStockRegionalList(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "5";
                rootobj.TYPEID = "111";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DeadStockRegionalListLogs", "DeadStockRegionalList : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get WareHouse DeadStock Regional List";
                return resultobj;

            }
        }

        public async Task<dynamic> DeadStockSectionList(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "5";
                rootobj.TYPEID = "112";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DeadStockSectionListLogs", "DeadStockSectionList : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get WareHouse DeadStock Section List";
                return resultobj;

            }
        }

        public async Task<dynamic> GetDeadStockStatus(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "5";
                rootobj.TYPEID = "114";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Updated Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetDeadStockStatusLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while WareHouse DeadStock Status Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Savedisinfestation(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "112";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;


            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "Disinfestationlogs", "PQualityCheckingDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Disinfestation  Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdateUseraccessDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "613";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Updated Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdateUseraccessDetailslogs", "SaveUseraccessDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Updateing Useracces Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetTendersDetails()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "3";
                rootobj.TYPEID = "GET_TENDERS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTendersDetailslogs", "GetTendersDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tenders Details";

                return resultobj;

            }
        }

        public async Task<dynamic> GetNewsDetails()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "3";
                rootobj.TYPEID = "GET_NEWS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetNewsDetailslogs", "GetNewsDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get News Details";

                return resultobj;

            }
        }

        public async Task<dynamic> SaveTaxpaymentDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "10";
                rootobj.TYPEID = "101";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveTaxdetailsLogs", "SaveTaxdetailsLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Tax Details";
                return resultobj;

            }
        }



        public async Task<dynamic> GetTaxDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "10";
                rootobj.TYPEID = "TAX_DETAILS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTokensLogs", "GetTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWHDeadStockHistory(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "5";
                rootobj.TYPEID = "116";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetWHDeadStockHistorylogs", "GetWHDeadStockHistory : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Dead Stock History";
                return resultobj;

            }
        }

        public async Task<dynamic> Notifications_Get(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "11";
                rootobj.TYPEID = "GET_NOTIFICATION";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Notifications_Get : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Notifications_Get Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Notifications_Update(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "11";
                rootobj.TYPEID = "101";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "Notifications_Update : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Notifications_Update Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveWeghbridgeDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "401";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveWeighbridgedetailsLogs", "SaveWeighbridgedetailsLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Weighbridge Details";
                return resultobj;

            }
        }


        public async Task<dynamic> GetWeighmentdataDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "WEIGH_BRIDGE";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "Weighbridge", "Weighbridge : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Weighbridge Details";
                return resultobj;

            }
        }


        public async Task<dynamic> updateWeighbridge(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "402";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "WeighbridgeUpdatelogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Master Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWeigh_Bridge_Details(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = rootobj.INPUT_01 == "WB_COMPANY" ? "WB_DROPDOWN" : "WB_PERIOD";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Weigh bridge Details";
                return resultobj;

            }
        }


        public async Task<dynamic> SaveWBDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "308";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveWeighbridgeDetailslogs", "SaveWeighbridgeDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Weigh Bridge Details";
                return resultobj;

            }
        }

        public async Task<dynamic> PastAttendance_Save(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "11";
                rootobj.TYPEID = "102";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "PastAttendance_Save : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while PastAttendance_Save Details";
                return resultobj;

            }
        }

        public async Task<dynamic> PastAttendance_Update(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "11";
                rootobj.TYPEID = "103";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "PastAttendance_Update : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while PastAttendance_Update Details";
                return resultobj;

            }
        }

        public async Task<dynamic> PastAttendance_Get(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "11";
                rootobj.TYPEID = "FORGET_ATTENDANCE";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "PastAttendance_Get : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while PastAttendance_Get Details";
                return resultobj;

            }
        }

        #region Warehouse receipts
        public async Task<dynamic> GetPendingWHReceipts(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "GEN_RECEIPT";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetPendingWHReceiptsLogs", "GetPendingWHReceipts : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Load Warehouse Receipts  Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetInsuranceList()
        {
            try
            {
                MasterSp rootobj = new MasterSp();
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "INSURANCE";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetInsuranceListLogs", "GetInsuranceList : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Load Insurance Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveReceiptDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "114";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveReceiptDetailsLogs", "SaveReceiptDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Generate Receipt";
                return resultobj;

            }
        }

        public async Task<dynamic> GetGeneratedWHReceipts(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "Generated_receipts";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                 
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetGeneratedWHReceiptsLogs", "GetGeneratedWHReceipts : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Load Warehouse Receipts  Details";
                return resultobj;

            }
        }

        public async Task<dynamic> DownloadReceipts(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "7";
                rootobj.TYPEID = "pdf_generation";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DownloadReceiptsLogs", "DownloadReceipts : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Load Download Receipts ";
                return resultobj;

            }
        }

        #endregion

        #region depositor receipt in (Others)
        public async Task<dynamic> GetOtherReceiptInTokens(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = "TO_GATEIN";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Token Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> OtherWeighmentTokenList(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = rootobj.INPUT_02 == "WEIGHT_INTKN" ? "WEIGHT_IN" : rootobj.INPUT_02 == "COMDLISTOFTKN" ? "COMMODITY_DEATILS" : "";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Token Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetOtherTokens(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = "GET_TOKEN";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTokensLogs", "GetTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetOtherStackTokens(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = "STACK_TOKEN";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetStackTokensLogs", "GetStackTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        public async Task<dynamic> OtherWeighment_OUTTokenList(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = rootobj.INPUT_02 == "WEIGHT_OUTTKN" ? "WEIGHT_OUT" : rootobj.INPUT_02 == "COMDLISTOFTKN" ? "WEIGHT_OUT_COMMODITY" : "";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "Token Details Not Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Token Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetOtherGateoutTokens(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = "GATE_OUT_TOKEN";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetGateoutTokensLogs", "GetGateoutTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }

        #endregion

        #region User Access Permission for Mobile

        public async Task<dynamic> GetPageMobDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "602";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetPageMobDetailslogs", "GetPageMobDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetPageMobDetailsByID(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "611";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetPageMobDetailsByIDlogs", "GetPageMobDetailsByID : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetPageMobAccessDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = rootobj.INPUT_01 == "DD" ? "604" : "603";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetPageMobAccessDetailslogs", "GetPageMobAccessDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> Get_DesigPageMobAccessDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = rootobj.INPUT_01 == "DD" ? "607" : "606";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "Get_DesigPageMobAccessDetailslogs", "Get_DesigPageMobAccessDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Page Details";
                return resultobj;

            }
        }
        public async Task<dynamic> SavePageMobDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "601";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SavePageMobDetailslogs", "SavePageMobDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdatePageMobDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "612";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdatePageMobDetailslogs", "UpdatePageMobDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update Page Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveUserMobaccessDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = rootobj.INPUT_08 == "Designation_Permission" ? "608" : "605";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveUserMobaccessDetailslogs", "SaveUserMobaccessDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Useracces Details";
                return resultobj;

            }
        }

        public async Task<dynamic> UpdateUserMobaccessDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "613";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Updated Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdateUserMobaccessDetailslogs", "UpdateUserMobaccessDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Updateing User acces Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetMobuserAccessmenu(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "8";
                rootobj.TYPEID = "609";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetMenuDetailslogs", "GetMenuDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Menu Details";
                return resultobj;

            }
        }

        #endregion

        #region Employee payroll

        public async Task<dynamic> GetEmptypeDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_MASTER";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTokensLogs", "GetTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }



        public async Task<DataTable> EmployeeAPSWCMasterSp(EmployeeMasterSp objMa)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter();
            try
            {
                cmd = new SqlCommand("SP_MASTER_PROC_01", sqlcon);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DIRECTION_ID", objMa.DIRECTION_ID);
                cmd.Parameters.AddWithValue("@TYPEID", objMa.TYPEID);

                cmd.Parameters.AddWithValue("@INPUT_01", objMa.INPUT_01);
                cmd.Parameters.AddWithValue("@INPUT_02", objMa.INPUT_02);
                cmd.Parameters.AddWithValue("@INPUT_03", objMa.INPUT_03);
                cmd.Parameters.AddWithValue("@INPUT_04", objMa.INPUT_04);
                cmd.Parameters.AddWithValue("@INPUT_05", objMa.INPUT_05);
                cmd.Parameters.AddWithValue("@INPUT_06", objMa.INPUT_06);
                cmd.Parameters.AddWithValue("@INPUT_07", objMa.INPUT_07);
                cmd.Parameters.AddWithValue("@INPUT_08", objMa.INPUT_08);
                cmd.Parameters.AddWithValue("@INPUT_09", objMa.INPUT_09);
                cmd.Parameters.AddWithValue("@INPUT_10", objMa.INPUT_10);
                cmd.Parameters.AddWithValue("@INPUT_11", objMa.INPUT_11);
                cmd.Parameters.AddWithValue("@INPUT_12", objMa.INPUT_12);
                cmd.Parameters.AddWithValue("@INPUT_13", objMa.INPUT_13);
                cmd.Parameters.AddWithValue("@INPUT_14", objMa.INPUT_14);
                cmd.Parameters.AddWithValue("@INPUT_15", objMa.INPUT_15);
                cmd.Parameters.AddWithValue("@INPUT_16", objMa.INPUT_16);
                cmd.Parameters.AddWithValue("@INPUT_17", objMa.INPUT_17);
                cmd.Parameters.AddWithValue("@INPUT_18", objMa.INPUT_18);
                cmd.Parameters.AddWithValue("@INPUT_19", objMa.INPUT_19);
                cmd.Parameters.AddWithValue("@INPUT_20", objMa.INPUT_20);
                cmd.Parameters.AddWithValue("@INPUT_21", objMa.INPUT_21);
                cmd.Parameters.AddWithValue("@INPUT_22", objMa.INPUT_22);
                cmd.Parameters.AddWithValue("@INPUT_23", objMa.INPUT_23);
                cmd.Parameters.AddWithValue("@INPUT_24", objMa.INPUT_24);
                cmd.Parameters.AddWithValue("@INPUT_25", objMa.INPUT_25);
                cmd.Parameters.AddWithValue("@INPUT_26", objMa.INPUT_26);
                cmd.Parameters.AddWithValue("@INPUT_27", objMa.INPUT_27);
                cmd.Parameters.AddWithValue("@INPUT_28", objMa.INPUT_28);
                cmd.Parameters.AddWithValue("@INPUT_29", objMa.INPUT_29);
                cmd.Parameters.AddWithValue("@INPUT_30", objMa.INPUT_30);
                cmd.Parameters.AddWithValue("@INPUT_31", objMa.INPUT_31);
                cmd.Parameters.AddWithValue("@INPUT_32", objMa.INPUT_32);
                cmd.Parameters.AddWithValue("@INPUT_33", objMa.INPUT_33);
                cmd.Parameters.AddWithValue("@INPUT_34", objMa.INPUT_34);
                cmd.Parameters.AddWithValue("@INPUT_35", objMa.INPUT_35);

                cmd.Parameters.AddWithValue("@INPUT_36", objMa.INPUT_36);
                cmd.Parameters.AddWithValue("@INPUT_37", objMa.INPUT_37);
                cmd.Parameters.AddWithValue("@INPUT_38", objMa.INPUT_38);
                cmd.Parameters.AddWithValue("@INPUT_39", objMa.INPUT_39);
                cmd.Parameters.AddWithValue("@INPUT_40", objMa.INPUT_40);
                cmd.Parameters.AddWithValue("@INPUT_41", objMa.INPUT_41);

                cmd.Parameters.AddWithValue("@INPUT_42", objMa.INPUT_42);
                cmd.Parameters.AddWithValue("@INPUT_43", objMa.INPUT_43);
                cmd.Parameters.AddWithValue("@INPUT_44", objMa.INPUT_44);
                cmd.Parameters.AddWithValue("@INPUT_45", objMa.INPUT_45);
                cmd.Parameters.AddWithValue("@INPUT_46", objMa.INPUT_46);
                cmd.Parameters.AddWithValue("@INPUT_47", objMa.INPUT_47);
                cmd.Parameters.AddWithValue("@INPUT_48", objMa.INPUT_48);
                cmd.Parameters.AddWithValue("@INPUT_49", objMa.INPUT_49);
                cmd.Parameters.AddWithValue("@INPUT_50", objMa.INPUT_50);
                cmd.Parameters.AddWithValue("@USER_NAME", objMa.USER_NAME);
                cmd.Parameters.AddWithValue("@CALL_SOURCE", objMa.CALL_SOURCE);
                cmd.Parameters.AddWithValue("@CALL_PAGE_ACTIVITY", objMa.CALL_PAGE_ACTIVITY);
                cmd.Parameters.AddWithValue("@CALL_BRO_APP_VER", objMa.CALL_BRO_APP_VER);
                cmd.Parameters.AddWithValue("@CALL_MOBILE_MODEL", objMa.CALL_MOBILE_MODEL);
                cmd.Parameters.AddWithValue("@CALL_LATITUDE", objMa.CALL_LATITUDE);
                cmd.Parameters.AddWithValue("@CALL_LONGITUDE", objMa.CALL_LONGITUDE);
                cmd.Parameters.AddWithValue("@CALL_IP_IMEI", objMa.CALL_IP_IMEI);
                await sqlcon.OpenAsync();
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                await sqlcon.CloseAsync();
                await cmd.DisposeAsync();
                adp.Dispose();
                return dt;
            }
            catch (Exception ex)
            {
                if (sqlcon.State == ConnectionState.Open)
                {
                    await sqlcon.CloseAsync();
                    await cmd.DisposeAsync();
                    adp.Dispose();
                }
                throw ex;
            }

        }
        public async Task<dynamic> GetEmpDetailsBycode(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_SALARY";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTokensLogs", "GetTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Tokens Details";
                return resultobj;

            }
        }
        public async Task<dynamic> SaveEmployeesalDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "101";
                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmpSalDetailsLogs", "SaveEmpSalDetailsLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save EmployeeSalary Details";
                return resultobj;

            }
        }


        public async Task<dynamic> UpdateEmployeesalaryDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "102";
                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "UpdateEmpSalDetailsLogs", "UpdateEmpSalDetailsLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Update EmployeeSalary Details";
                return resultobj;

            }
        }
        public async Task<dynamic> GetmonthsDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_MONTH";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetTokensLogs", "GetTokens : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get months Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetPayroleDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_PAYROLL";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeLogs", "Getpayrole : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee salary Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmployeepayroleDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "103";
                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0]["STATUS_TEXT"].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmployeePayroleLogs", "SaveEmployeePayroleLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employee Payrole Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetEmpltypeDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_TYPE";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeetypeLogs", "GetEmployeetype : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee Type Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetemployeetypeDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_HITORY";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeetypeLogs", "GetEmployeetypeLogs : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee type Details";
                return resultobj;

            }
        }



        public async Task<dynamic> GetemployeeviewDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_PAYROLL_VIEW";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeviewLogs", "GetEmployeeviewLogs : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee  Details";
                return resultobj;

            }
        }

        #endregion

        #region Employee trasfer
        public async Task<dynamic> SaveEmpTransferWorkDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "14";
                rootobj.TYPEID = "101";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }


                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmpTransferWorkDetailsLogs", "SaveEmpTransferWorkDetailsLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employee Transfer Details";
                return resultobj;

            }
        }
        #endregion

        public async Task<dynamic> GetEmployeeLoanDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "LOAN_TYPE";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetLoanLogs", "GetLoanLogs: Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Loan Types";
                return resultobj;

            }
        }


        public async Task<dynamic> GetEmployeeMonthsDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "MONTHS_DROPDOWN";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetMonthsLogs", "GetMonthsLogs : Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Months Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmployeeloanreqDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "201";
                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0]["STATUS_TEXT"].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveEmployeeloanrequestLogs", "SaveEmployeeloanrequestLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employeeloan Request Details";
                return resultobj;

            }
        }


        public async Task<dynamic> GetEmployeeDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_LOAN_DETAILS";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetLoanLogs", "GetLoanLogs: Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Loan Types";
                return resultobj;

            }
        }

        public async Task<dynamic> GetOfficerApprovalDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_LOAN_OFFICER";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeApprovalLogs", "GetEmployeeApprovalLogs: Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get EmployeeDetails";
                return resultobj;

            }
        }

        public async Task<dynamic> Saveemployeeofficerloan(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "202";
                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveemployeeloaninstallmentLogs", "SaveemployeeloaninstallmentLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save EmployeeLoan Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetMDApprovalDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "EMP_LOAN_GM";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetEmployeeApprovalLogs", "GetEmployeeApprovalLogs: Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get EmployeeDetails";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveMDLEVELApprovalloan(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "203";
                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Saved Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveemployeeloaninstallmentLogs", "SaveemployeeloaninstallmentLogs : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save MDLEVEL Approval Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetPaymentapprovalDetails(EmployeeMasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "101";
                rootobj.TYPEID = "GM_APPROVED";

                DataTable dt = await EmployeeAPSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetPaymentApprovalLogs", "GetPaymentApprovalLogs: Method:" + jsondata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Employee Payment Approval Details";
                return resultobj;

            }
        }



        public async Task<dynamic> GetFarmerTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "farmer_type";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetFarmerTypesLogs", "GetFarmerTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Farmer Types";

                return resultobj;

            }
        }

        #region Warehouse Reports

        public async Task<dynamic> GetLorryWBRegister(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "LORRY_WEIGHBRIDGE_REG";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Lorry Weighbridge List";
                return resultobj;

            }
        }

        public async Task<dynamic> GetStockList(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "STOCK_REGISTER";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Stock Register";
                return resultobj;

            }
        }

        public async Task<dynamic> GetDailyValuationStock(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "DAILY_VALUATION";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Daily Valuation Stock";
                return resultobj;

            }
        }

        public async Task<dynamic> GetOpeningBalance(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "OPENING_BALANCE";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load OpenigBalance";
                return resultobj;

            }
        }

        public async Task<dynamic> GetImprestRegister(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "IMPREST_REGISTER";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetFarmersAndCommodites(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = rootobj.INPUT_01 == "Farmers" ? "STACKIN_USERS" : "STACKIN_COMMODITY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetMonthsAndYears(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = rootobj.INPUT_01 == "Months" ? "MONTHS" : "YEARS";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetDepostCmdtys(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "DEPOSITER_COMMODITY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Depositer Commodities  Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Details";
                return resultobj;

            }
        }



        public async Task<dynamic> GetBankloanRegister(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "BANK_REGISTER";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Bank Register Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetDailyTransactionRegister(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "DAILY_TRANSACTION_REG";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Daily Transaction Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetDepositerLedger(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "DEPOSITOR_LEDGER";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Depositer Commodities  Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Depositer Ledger Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWHspillage(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "WAREHOUSE_SPILLING";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Spillage Details  Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Warehose Spillage Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetChemicals(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "CHEMICALS";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Chemicals Details  Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Chemicals Details";
                return resultobj;

            }
        }


        public async Task<dynamic> GetChemicalConsumption(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "CHEMICALS_REPORT";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Chemical Consumption Details  Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Chemical Consumption Details";
                return resultobj;

            }
        }

        #endregion

        #region Periodic Quality Examination

        public async Task<dynamic> GetChemicalTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "CHEMICALS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetChemicalTypesLogs", "GetChemicalTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Chemicals Types";

                return resultobj;

            }
        }

        public async Task<dynamic> GetImprestTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "IMPREST_PAYMENT";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetImprestTypesLogs", "GetImprestTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get Imprest Types";

                return resultobj;

            }
        }

        public async Task<dynamic> SaveDisinfestationDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = "101";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;


            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveDisinfestationDetailslogs", "SaveDisinfestationDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Disinfestation  Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetDisiHistory(MasterSp rootobj)
        {
            //MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = "GET_DISINFECT_DETAILS";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetDisiHistoryLogs", "GetDisiHistory : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Load Disinfestation Details";

                return resultobj;

            }
        }

        public async Task<dynamic> GetQualityiHistory(MasterSp rootobj)
        {
            //MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = "GET_QUALITY";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }
                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetQualityiHistoryLogs", "GetQualityiHistory : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Load Quality Details";

                return resultobj;

            }
        }

        public async Task<dynamic> SaveSpillingDetails(MasterSp rootobj)
        {

            try
            {

                rootobj.DIRECTION_ID = "12";
                rootobj.TYPEID = "102";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Submitted Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = dt.Rows[0][1].ToString();
                }

                return resultobj;


            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveSpillingDetailsLogs", "SaveSpillingDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Spilling  Details";
                return resultobj;

            }
        }

        #endregion

        #region SR
        public async Task<dynamic> GetLeaveTypes()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "LEAVES_DD";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetLeaveTypes : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while  GetLeaveTypes Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SR_GET_INSERT_COMMON(MasterSp rootobj)
        {
            try
            {
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SR_GET_INSERT_COMMON", "SR_GET_INSERT_COMMON : Method:" + jsondata));

                // resultobj.StatusCode = 102;
                // resultobj.StatusMessage = "Error Occured while SR_GET_INSERT_COMMON Details";
                //return resultobj;

                throw ex;

            }
        }

        #endregion

        #region Digilocker
        public async Task<dynamic> DigiLocker(MasterSp objMa)
        {
            try
            {

                objMa.DIRECTION_ID = "16";
                DataTable dt = await APSWCMasterSp(objMa);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Data Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(objMa);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "DigiLocker : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Getting Data";
                return resultobj;
            }
        }

        
        #endregion

        public async Task<dynamic> GetGodown_Stack_cmprtdetails(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = rootobj.INPUT_01 == "Godown" ? "GODOWN" : rootobj.INPUT_01 == "Compart" ? "COMPARTMENT" : rootobj.INPUT_01 == "Stack" ? "STACK" : "COMMODITY";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = rootobj.INPUT_01 == "Godown" ? "No Godown Details Found" : rootobj.INPUT_01 == "Compart" ? "No Compartment Details Found" : rootobj.INPUT_01 == "Stack" ? "No Stack Details Found" : "No Commodity Details Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Chemical Consumption Details";
                return resultobj;

            }
        }

        #region Digilocker

        public async Task<dynamic> DigiLockerAccessToken(DigiLocker root)
        {
            string input = "";
            dynamic obj_data = new ExpandoObject(); MasterSp s = new MasterSp();
            try
            {


                input = "code=" + root.code + "&grant_type=authorization_code&client_id=D6DB15C2&client_secret=064f75643536b04d971a&redirect_uri=" + root.RedirectURL;
                //string logdata = File.ReadAllText(@"C:\Users\DELL\Desktop\12345.txt", Encoding.UTF8);
                var data1 = await PostDataAPSWC("https://api.digitallocker.gov.in/public/oauth2/1/token", input, root.token);
                string logdata = JsonConvert.SerializeObject(data1);
                //string mappath = HttpContext.Current.Server.MapPath("DigiLockerAccessTokenLog");
                //Task WriteTask = Task.Factory.StartNew(() => Log(logdata + "_" + root.RedirectURL, mappath, root.code));
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DigiLockerIssuedFilesLog", "DigiLockerIssueFiles : Method:" + logdata + "_" + root.RedirectURL + " , Input Data : " + root.token));
                AccessToken data = JsonConvert.DeserializeObject<AccessToken>(logdata);
                if (!string.IsNullOrEmpty(data.access_token.ToString()))
                {
                    s.DIRECTION_ID = "16";
                    s.TYPEID = "101";
                    s.INPUT_01 = data.access_token;
                    s.INPUT_02 = data.expires_in;
                    s.INPUT_03 = data.token_type;
                    s.INPUT_04 = data.scope;
                    s.INPUT_05 = data.refresh_token;
                    s.INPUT_06 = data.digilockerid;
                    s.INPUT_07 = data.name;
                    s.INPUT_08 = data.dob;
                    s.INPUT_09 = data.gender;
                    s.INPUT_10 = data.eaadhaar;
                    s.INPUT_11 = data.new_account;
                    s.INPUT_12 = root.code;
                    s.INPUT_13 = "";
                    s.USER_NAME = "";
                    s.CALL_SOURCE = "Web";
                    DataTable d = await APSWCMasterSp(s);
                    if (d.Rows[0][0].ToString() == "0")
                    {
                        string logdata1 = JsonConvert.SerializeObject(data);
                        //string mappath1 = HttpContext.Current.Server.MapPath("DigiLockerAccessTokenInsertionFailedLog");
                        //Task WriteTask1 = Task.Factory.StartNew(() => Log(logdata1, mappath1, root.token));
                        Task WriteTask1 = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DigiLockerAccessTokenInsertionFailedLog", "DigiLockerAccessTokenInsertionFailed : Method:" + logdata + "_" + root.RedirectURL + " , Input Data : " + root.token));
                    }
                    obj_data.Status = "Success";
                    obj_data.DigiLockerToken = data;
                    obj_data.Reason = "Data Loaded Successfully";
                }
                else
                {
                    obj_data.Status = "Failure";
                    obj_data.Reason = "Token Generation Fail";
                    obj_data.DigiLockerToken = data;
                }

            }
            catch (Exception ex)
            {

                obj_data.Status = "Failure";
                obj_data.Reason = ex.Message.ToString();
                //string mappath = HttpContext.Current.Server.MapPath("DigiLockerAccessTokenExceptionLog");
                //Task WriteTask = Task.Factory.StartNew(() => Log(ex.Message.ToString() + "_" + root.RedirectURL, mappath, root.code));
                Task WriteTask1 = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DigiLockerAccessTokenExceptionLog", "DigiLockerAccessTokenException : Method:" + ex.Message.ToString() + "_" + root.RedirectURL + " , Input Data : " + root.token));
            }

            return obj_data;
        }

        public async Task<dynamic> DigiLockerIssueFiles(DigiLocker root)
        {
            string input = "";
            MasterSp s = new MasterSp();
            dynamic obj_data = new ExpandoObject();

            try
            {
               

                input = "";
                var data1 = await PostDataAPSWC("https://api.digitallocker.gov.in/public/oauth2/2/files/issued", input, root.token);

                string logdata = JsonConvert.SerializeObject(data1);

                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DigiLockerIssuedFilesLog", "DigiLockerIssueFiles : Method:" + logdata + "_" + root.RedirectURL + " , Input Data : " + root.token));

                DLDocs data = JsonConvert.DeserializeObject<DLDocs>(logdata);

                if (data.items.Count > 0)
                {

                    dynamic val = data.items;

                    try
                    {
                        foreach (files f in val)
                        {
                            s.DIRECTION_ID = "16";
                            s.TYPEID = "102";
                            s.INPUT_01 = root.token;
                            s.INPUT_02 = f.name;
                            s.INPUT_03 = f.type;
                            s.INPUT_04 = f.size;
                            s.INPUT_05 = f.date;
                            s.INPUT_06 = f.parent;
                            s.INPUT_07 = f.mime[0];
                            s.INPUT_08 = f.uri;
                            s.INPUT_09 = f.doctype;
                            s.INPUT_10 = f.description;
                            s.INPUT_11 = f.issuerid;
                            s.INPUT_12 = f.issuer;
                            s.INPUT_13 = "";
                            s.USER_NAME = "";
                            s.CALL_SOURCE = "Web";
                            DataTable d = await APSWCMasterSp(s);
                            if (d.Rows[0][0].ToString() == "0")
                            {
                                string logdata1 = JsonConvert.SerializeObject(data);
                                

                                Task WriteTask1 = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DigiLockerIssuedFilesFailedLog", "DigiLockerIssueFiles : Method:" + logdata1 + " , Input Data : " + root.token));

                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                       
                       
                    }

                    obj_data.Status = "Success";
                    obj_data.UserFiles = data;
                    obj_data.Reason = "Data Loaded Successfully";
                }
                else
                {
                    obj_data.Status = "Failure";
                    obj_data.Reason = "Documents not found";
                    obj_data.UserFiles = data;
                }

              
            }
            catch (Exception ex)
            {
                obj_data.Status = "Failure";
                obj_data.Reason = ex.Message.ToString();

                Task WriteTask2 = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "DigiLockerIssuedFilesFailedLog", "DigiLockerIssueFiles : Method:" + ex.Message.ToString() + " , Input Data : " + root.token));

            }

            return obj_data; ;
        }

        #endregion

        public async Task<dynamic> GetStackRegisterDetails(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = "STACK_REGISTER";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Stack Register Details  Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Stack Register Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetInvoiceModeDetails()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "INVOICE_MODE";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Invoice Modes  Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Invoice Modes";
                return resultobj;

            }
        }

        public async Task<dynamic> GetInvPriceDetails(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "COMMODITY_AMOUNT";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Price Details  Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Price Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetRebateDetails(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "6";
                rootobj.TYPEID = "COMMODITY_REBATES";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = "No Rebate Details Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Rebate Details";
                return resultobj;

            }
        }

        public async Task<dynamic> GetWHDetails(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "13";
                rootobj.TYPEID = rootobj.INPUT_01 == "WHTYPE" ? "WAREHOUSE_TYPE" : "WAREHOUSES";
                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultobj.StatusCode = 100;
                    resultobj.StatusMessage = "Data Loaded Successfully";
                    resultobj.Details = dt;
                }
                else
                {
                    resultobj.StatusCode = 102;
                    resultobj.StatusMessage = rootobj.TYPEID = rootobj.INPUT_01 == "WHTYPE" ? "No Warehouse Type Details Found" : "No Wareshouses Found";
                }

                return resultobj;
            }
            catch (Exception ex)
            {

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Warehouse Details";
                return resultobj;

            }
        }


        public async Task<DataTable> APSWCMasterSp(MasterSp objMa)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter();
            try
            {
                cmd = new SqlCommand("SP_MASTER_PROC", sqlcon);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DIRECTION_ID", objMa.DIRECTION_ID);
                cmd.Parameters.AddWithValue("@TYPEID", objMa.TYPEID);
                cmd.Parameters.AddWithValue("@INPUT_01", objMa.INPUT_01);
                cmd.Parameters.AddWithValue("@INPUT_02", objMa.INPUT_02);
                cmd.Parameters.AddWithValue("@INPUT_03", objMa.INPUT_03);
                cmd.Parameters.AddWithValue("@INPUT_04", objMa.INPUT_04);
                cmd.Parameters.AddWithValue("@INPUT_05", objMa.INPUT_05);
                cmd.Parameters.AddWithValue("@INPUT_06", objMa.INPUT_06);
                cmd.Parameters.AddWithValue("@INPUT_07", objMa.INPUT_07);
                cmd.Parameters.AddWithValue("@INPUT_08", objMa.INPUT_08);
                cmd.Parameters.AddWithValue("@INPUT_09", objMa.INPUT_09);
                cmd.Parameters.AddWithValue("@INPUT_10", objMa.INPUT_10);
                cmd.Parameters.AddWithValue("@INPUT_11", objMa.INPUT_11);
                cmd.Parameters.AddWithValue("@INPUT_12", objMa.INPUT_12);
                cmd.Parameters.AddWithValue("@INPUT_13", objMa.INPUT_13);
                cmd.Parameters.AddWithValue("@INPUT_14", objMa.INPUT_14);
                cmd.Parameters.AddWithValue("@INPUT_15", objMa.INPUT_15);
                cmd.Parameters.AddWithValue("@INPUT_16", objMa.INPUT_16);
                cmd.Parameters.AddWithValue("@INPUT_17", objMa.INPUT_17);
                cmd.Parameters.AddWithValue("@INPUT_18", objMa.INPUT_18);
                cmd.Parameters.AddWithValue("@INPUT_19", objMa.INPUT_19);
                cmd.Parameters.AddWithValue("@INPUT_20", objMa.INPUT_20);
                cmd.Parameters.AddWithValue("@INPUT_21", objMa.INPUT_21);
                cmd.Parameters.AddWithValue("@INPUT_22", objMa.INPUT_22);
                cmd.Parameters.AddWithValue("@INPUT_23", objMa.INPUT_23);
                cmd.Parameters.AddWithValue("@INPUT_24", objMa.INPUT_24);
                cmd.Parameters.AddWithValue("@INPUT_25", objMa.INPUT_25);
                cmd.Parameters.AddWithValue("@INPUT_26", objMa.INPUT_26);
                cmd.Parameters.AddWithValue("@INPUT_27", objMa.INPUT_27);
                cmd.Parameters.AddWithValue("@INPUT_28", objMa.INPUT_28);
                cmd.Parameters.AddWithValue("@INPUT_29", objMa.INPUT_29);
                cmd.Parameters.AddWithValue("@INPUT_30", objMa.INPUT_30);
                cmd.Parameters.AddWithValue("@INPUT_31", objMa.INPUT_31);
                cmd.Parameters.AddWithValue("@INPUT_32", objMa.INPUT_32);
                cmd.Parameters.AddWithValue("@INPUT_33", objMa.INPUT_33);
                cmd.Parameters.AddWithValue("@INPUT_34", objMa.INPUT_34);
                cmd.Parameters.AddWithValue("@INPUT_35", objMa.INPUT_35);
                cmd.Parameters.AddWithValue("@USER_NAME", objMa.USER_NAME);
                cmd.Parameters.AddWithValue("@CALL_SOURCE", objMa.CALL_SOURCE);
                cmd.Parameters.AddWithValue("@CALL_PAGE_ACTIVITY", objMa.CALL_PAGE_ACTIVITY);
                cmd.Parameters.AddWithValue("@CALL_BRO_APP_VER", objMa.CALL_BRO_APP_VER);
                cmd.Parameters.AddWithValue("@CALL_MOBILE_MODEL", objMa.CALL_MOBILE_MODEL);
                cmd.Parameters.AddWithValue("@CALL_LATITUDE", objMa.CALL_LATITUDE);
                cmd.Parameters.AddWithValue("@CALL_LONGITUDE", objMa.CALL_LONGITUDE);
                cmd.Parameters.AddWithValue("@CALL_IP_IMEI", objMa.CALL_IP_IMEI);
                await sqlcon.OpenAsync();
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                await sqlcon.CloseAsync();
                await cmd.DisposeAsync();
                adp.Dispose();
                return dt;
            }
            catch (Exception ex)
            {
                if (sqlcon.State == ConnectionState.Open)
                {
                    await sqlcon.CloseAsync();
                    await cmd.DisposeAsync();
                    adp.Dispose();
                }
                throw ex;
            }

        }

        public async Task<DataTable> APSWCMasterlist(MasterSp objMa)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter();

            try
            {
                objMa.DIRECTION_ID = "1";
                objMa.TYPEID = "WH_MASTER";


                cmd = new SqlCommand("SP_MASTER_PROC", sqlcon);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DIRECTION_ID", objMa.DIRECTION_ID);
                cmd.Parameters.AddWithValue("@TYPEID", objMa.TYPEID);
                cmd.Parameters.AddWithValue("@INPUT_01", objMa.INPUT_01);
                cmd.Parameters.AddWithValue("@INPUT_02", objMa.INPUT_02);
                cmd.Parameters.AddWithValue("@INPUT_03", objMa.INPUT_03);
                cmd.Parameters.AddWithValue("@INPUT_04", objMa.INPUT_04);
                cmd.Parameters.AddWithValue("@INPUT_05", objMa.INPUT_05);
                cmd.Parameters.AddWithValue("@INPUT_06", objMa.INPUT_06);
                cmd.Parameters.AddWithValue("@INPUT_07", objMa.INPUT_07);
                cmd.Parameters.AddWithValue("@INPUT_08", objMa.INPUT_08);
                cmd.Parameters.AddWithValue("@INPUT_09", objMa.INPUT_09);
                cmd.Parameters.AddWithValue("@INPUT_10", objMa.INPUT_10);
                cmd.Parameters.AddWithValue("@INPUT_11", objMa.INPUT_11);
                cmd.Parameters.AddWithValue("@INPUT_12", objMa.INPUT_12);
                cmd.Parameters.AddWithValue("@INPUT_13", objMa.INPUT_13);
                cmd.Parameters.AddWithValue("@INPUT_14", objMa.INPUT_14);
                cmd.Parameters.AddWithValue("@INPUT_15", objMa.INPUT_15);
                cmd.Parameters.AddWithValue("@INPUT_16", objMa.INPUT_16);
                cmd.Parameters.AddWithValue("@INPUT_17", objMa.INPUT_17);
                cmd.Parameters.AddWithValue("@INPUT_18", objMa.INPUT_18);
                cmd.Parameters.AddWithValue("@INPUT_19", objMa.INPUT_19);
                cmd.Parameters.AddWithValue("@INPUT_20", objMa.INPUT_20);
                cmd.Parameters.AddWithValue("@INPUT_21", objMa.INPUT_21);
                cmd.Parameters.AddWithValue("@INPUT_22", objMa.INPUT_22);
                cmd.Parameters.AddWithValue("@INPUT_23", objMa.INPUT_23);
                cmd.Parameters.AddWithValue("@INPUT_24", objMa.INPUT_24);
                cmd.Parameters.AddWithValue("@INPUT_25", objMa.INPUT_25);
                cmd.Parameters.AddWithValue("@INPUT_26", objMa.INPUT_26);
                cmd.Parameters.AddWithValue("@INPUT_27", objMa.INPUT_27);
                cmd.Parameters.AddWithValue("@INPUT_28", objMa.INPUT_28);
                cmd.Parameters.AddWithValue("@INPUT_29", objMa.INPUT_29);
                cmd.Parameters.AddWithValue("@INPUT_30", objMa.INPUT_30);
                cmd.Parameters.AddWithValue("@INPUT_31", objMa.INPUT_31);
                cmd.Parameters.AddWithValue("@INPUT_32", objMa.INPUT_32);
                cmd.Parameters.AddWithValue("@INPUT_33", objMa.INPUT_33);
                cmd.Parameters.AddWithValue("@INPUT_34", objMa.INPUT_34);
                cmd.Parameters.AddWithValue("@INPUT_35", objMa.INPUT_35);
                cmd.Parameters.AddWithValue("@USER_NAME", objMa.USER_NAME);
                cmd.Parameters.AddWithValue("@CALL_SOURCE", objMa.CALL_SOURCE);
                cmd.Parameters.AddWithValue("@CALL_PAGE_ACTIVITY", objMa.CALL_PAGE_ACTIVITY);
                cmd.Parameters.AddWithValue("@CALL_BRO_APP_VER", objMa.CALL_BRO_APP_VER);
                cmd.Parameters.AddWithValue("@CALL_MOBILE_MODEL", objMa.CALL_MOBILE_MODEL);
                cmd.Parameters.AddWithValue("@CALL_LATITUDE", objMa.CALL_LATITUDE);
                cmd.Parameters.AddWithValue("@CALL_LONGITUDE", objMa.CALL_LONGITUDE);
                cmd.Parameters.AddWithValue("@CALL_IP_IMEI", objMa.CALL_IP_IMEI);
                await sqlcon.OpenAsync();
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);

                await sqlcon.CloseAsync();
                await cmd.DisposeAsync();
                adp.Dispose();

                return dt;

            }
            catch (Exception ex)
            {
                await sqlcon.CloseAsync();
                await cmd.DisposeAsync();
                adp.Dispose();

                throw ex;
            }

        }

        public async Task<dynamic> PostDataAPSWC(string url, string input, string token)
        {
            var response = String.Empty;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                StreamReader reader = null;
                WebRequest request = WebRequest.Create(url);
                request.Method = "POST";
                string postData = input;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Add("Authorization", "Bearer " + token);
                }
                request.ContentLength = byteArray.Length;
                Stream dataStream = await request.GetRequestStreamAsync();

                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response1 = request.GetResponse();
                if ((((HttpWebResponse)response1).StatusCode == HttpStatusCode.Redirect) || (((HttpWebResponse)response1).StatusCode == HttpStatusCode.SeeOther) ||
                    (((HttpWebResponse)response1).StatusCode == HttpStatusCode.RedirectMethod))
                {
                    response = ((HttpWebResponse)response1).StatusDescription;
                }
                else
                {
                    dataStream = response1.GetResponseStream();
                    reader = new StreamReader(dataStream);
                    response = reader.ReadToEnd();
                }
                reader.Close();
                dataStream.Close();
                response1.Close();
            }
            catch (WebException wex)
            {
                throw new Exception(wex.Message);
            }


            return JsonConvert.DeserializeObject<dynamic>(response);
        }

        
    }
}