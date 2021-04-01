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
namespace APSWCWEBAPIAPP.DBConnection
{
    public class SqlCon
    {
        private readonly string _connectionString;
       private string exFolder = Path.Combine("ExceptionLogs");
        private string exPathToSave = string.Empty;
        dynamic resultobj = new ExpandoObject();
        public SqlCon(IConfiguration configuration)
        {
           exPathToSave= Path.Combine(Directory.GetCurrentDirectory(), exFolder);
            _connectionString = configuration.GetConnectionString("apswcproddb");
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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetApswcWareHouseMaster:Method:"+jsondata));
                return ex.Message.ToString();
            }
            
        }

       public  async Task<dynamic> GetBoardofDirectors()
        {
            try
            {
                MasterSp m = new MasterSp();
                m.DIRECTION_ID = "1";
                m.TYPEID = "BOARD_OF_DIRECTORS";
                return await APSWCMasterSp(m);
            }
            catch(Exception ex)
            {
                
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetBoardofDirectors:Method:"+jsondata));
                return ex.Message.ToString();
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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetWorkLocations : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetEmployeeTypes : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetEducations : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetDistricts : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetAreaTypes : Method:" + jsondata + " , Input Data : " + inputdata));

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

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Loaded Successfully";
                resultobj.Details = await APSWCMasterSp(rootobj);

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetMandlas : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetVillages : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetStorageTypes : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetChargeDetails : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetSections : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while load Sections";
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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetBloodGroups : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetExperianceYears : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetExperianceMonths : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetNationality : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetReligions : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetCommunities : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetMaritalStatus : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetStates : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetSpaceDetails : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetFiveYearsReport : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetRManagers : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetRelations : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetGenders : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveEmpPrimaryDetails : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveEmpCommuDetails : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveEmpWorkDetails : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveEmpBankDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Save Employee Bank Details";
                return resultobj;

            }
        }

        public async Task<dynamic> SaveEmpFamilyDetails(FamilyListCls rootobj)
        {
            try
            {
                foreach (var family in rootobj.FamilyList)
                {
                    var obj = new MasterSp();
                    obj.DIRECTION_ID = "2";
                    obj.TYPEID = "301";

                    obj.INPUT_01 = family.INPUT_01;
                    obj.INPUT_02 = family.INPUT_02;
                    obj.INPUT_03 = family.INPUT_03;
                    obj.INPUT_04 = family.INPUT_04;
                    obj.INPUT_05 = family.INPUT_05;
                    obj.INPUT_06 = family.INPUT_06;

                    resultobj.Details = await APSWCMasterSp(obj);
                }

                resultobj.StatusCode = 100;
                resultobj.StatusMessage = "Data Inserted Successfully";

                return resultobj;
            }
            catch (Exception ex)
            {
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                string inputdata = JsonConvert.SerializeObject(rootobj);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveEmpFamilyDetails : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "SaveEmpPFDetails : Method:" + jsondata + " , Input Data : " + inputdata));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log_Exception(exPathToSave, "GetIFSCCodeDetails : Method:" + jsondata + " , Input Data : " + inputdata));

                resultobj.StatusCode = 102;
                resultobj.StatusMessage = "Error Occured while Get IFSC Code Details";
                return resultobj;

            }
        }

        public async Task<DataTable> APSWCMasterSp(MasterSp objMa)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("SP_MASTER_PROC", sqlcon);
                SqlDataAdapter adp = new SqlDataAdapter();
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
                return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }

          }
    }
}
