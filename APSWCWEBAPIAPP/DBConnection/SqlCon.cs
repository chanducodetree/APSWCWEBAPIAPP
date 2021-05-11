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

        public async Task<dynamic> GetSpaceReservationDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "GEN_BOOKING";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
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
    }
}