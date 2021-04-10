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
                        var user = new User() { UserName = data.Rows[0][0].ToString() , FirstName = data.Rows[0][1].ToString(), UserType = data.Rows[0][2].ToString() };
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
                return await APSWCMasterSp(m);
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
                return await APSWCMasterSp(m);
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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);


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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetWorkLocations : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Work Locations";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveHomePageConent(MasterSp rootobj)
        {

            try
            {
               

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString()=="1")
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

        public async Task<dynamic> GetEducations()
        {
            MasterSp rootobj = new MasterSp();
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "EDU";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

        public async Task<dynamic> SaveInspectionPhotos(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "201";

                DataTable dt = await APSWCMasterSp(rootobj);
                if (dt != null && dt.Rows.Count > 0)
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
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "SaveInspectionPhotos", "GetRManagers : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Managers";
            }

            return resultobj;
        }


        public async Task<dynamic> GetMandlas(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "MMC";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.Details = await APSWCMasterSp(rootobj);
                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

        public async Task<dynamic> GetWHList(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "WH_LIST";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

        public async Task<dynamic> GetRegionDistricts(MasterSp rootobj)
        {

            try
            {
                rootobj.DIRECTION_ID = "1";
                rootobj.TYPEID = "REGN_DIST";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.Details = await APSWCMasterSp(rootobj);
                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";

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

        public async Task<dynamic> UpdateWareHouseDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "302";

                resultobj.Details = await APSWCMasterSp(rootobj);
                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Updated Successfully";

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

        public async Task<dynamic> UpdateEmpPrimaryDetails(MasterSp rootobj)
        {
            try
            {
                rootobj.DIRECTION_ID = "2";
                rootobj.TYPEID = "108";

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

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
    }
}
