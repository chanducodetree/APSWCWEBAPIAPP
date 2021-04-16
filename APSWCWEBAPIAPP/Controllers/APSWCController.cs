using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelService;
using Newtonsoft.Json;
using System.Web;

using APSWCWEBAPIAPP.Services;
using System.IO;
using AuthService;
using APSWCWEBAPIAPP.DBConnection;
using Microsoft.EntityFrameworkCore;
using APSWCWEBAPIAPP.Models;

namespace APSWCWEBAPIAPP.Controllers
{
   //[Authorize(Policy = Policies.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class APSWCController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SqlCon _hel;
        private ApplicationAPSWCCDbContext _context;
        private readonly ICaptchaService _authservice;
        private string saFolder = Path.Combine("SaveLogs");
        private string saPathToSave = string.Empty;
       
        private List<InspectionModel> InsUsers = new List<InspectionModel>
        {

            new InspectionModel {   WarehouseId = "VZ01101", WarehouseName = "Amdalavalasa -I (Own)",
            Description = "quanity checking", uploadeddate = DateTime.Now,FilePath="Inspection\\Images\\6.jpg" },
            new InspectionModel {   WarehouseName= "Amdalavalasa -Ii (Own-PEG)",
            WarehouseId= "VZ01102", Description = "building checking", uploadeddate = DateTime.Now,FilePath="Inspection\\Images\\6.jpg" },
        };


        public APSWCController(ApplicationAPSWCCDbContext apcontext,IConfiguration config, ICaptchaService auth , SqlCon hel)
        {
            saPathToSave = Path.Combine(Directory.GetCurrentDirectory(), saFolder);
            _context = apcontext;
            _config = config;
            _authservice = auth;
            _hel = hel;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Captcha")]
        public dynamic Captcha()
        {
            return _authservice.check_s_captch("");
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.CheckLogin(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Login"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("WareHouseMaster")]
        public List<WareHouseMaste> GetWareHouseMaster()
        {
            var res = ListWarehouse.ListWareHosueMaster.ToList();
            return res;
        }

        [HttpPost]
        [Route("GetServiceCharterDetails")]
        public async Task<IActionResult> GetServiceCharterDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetServiceCharterDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Service Charter Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("InsecpctionList")]
        public List<InspectionModel> GetInsecpctionList()
        {
            var res = InsUsers;
            return res;
        }

        [HttpPost]
        [Route("InspectionRegistration")]
        public async Task<IActionResult> InspectionRegistration([FromBody] MasterSp Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("InspectionLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            MasterSp obj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "InspectionRegistrationlogs", jsondata));

            return Ok(await _hel.SaveInspectionPhotos(obj));
           
        }


        [HttpPost]
        [Route("ContactRegistration")]
        public async Task<IActionResult> ContactRegistration([FromBody] MasterSp Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("APSWCLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            MasterSp obj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "ContactRegistrationLogs", jsondata));

            return Ok(await _hel.SaveContactRegisteration(obj));

        }

        [HttpPost]
        [Route("BoardofDirectorsRegistration")]
        public async Task<IActionResult> BoardofDirectorsRegistration([FromBody] MasterSp Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("APSWCLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            MasterSp obj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "BoardofDirectorsRegistrationLogs", jsondata));

            return Ok(await _hel.Saveboardofdirectors(obj));

        }

        [HttpPost]
        [Route("ServiceCharterInsert")]
        public async Task<IActionResult> ServiceCharterInsert([FromBody] MasterSp Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("APSWCLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            MasterSp obj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "ServiceCharterInsertLogs", jsondata));

            return Ok(await _hel.SaveServiceCharter(obj));

        }
        [HttpPost]
        [Route("HomePageConentInsert")]
        public async Task<IActionResult> HomePageConentInsert([FromBody] MasterSp Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("APSWCLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            MasterSp obj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "HomePageConentInsertLogs", jsondata));

            return Ok(await _hel.SaveHomePageConent(obj));

        }

        [HttpGet]
        [Route("GetEmpList")]
        public async Task<IActionResult> GetEmpList()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetEmpList());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Employee List",

                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetWorkLocations")]
        public async Task<IActionResult> GetWorkLocations()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetWorkLocations());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Work Locations",

                });
                return response;
            }
        }
        [HttpGet]
        [Route("GetBoardofDirectors")]
        public async Task<IActionResult> GetBoardofDirectors()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetBoardofDirectors());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Work Locations",

                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetContactList")]
        public async Task<IActionResult> GetContactList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetContactLists(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load States"
                });
                return response;
            }
        }
        [HttpGet]
        [Route("GetDesignations")]
        public async Task<IActionResult> GetDesignations()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetDesignations());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Designations",

                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetEmployeeTypes")]
        public async Task<IActionResult> GetEmployeeTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetEmployeeTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Employee Types"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetEducations")]
        public async Task<IActionResult> GetEducations()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetEducations());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Educations"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetDistricts")]
        public async Task<IActionResult> GetDistricts()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetDistricts());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Districts"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetAreaTypes")]
        public async Task<IActionResult> GetAreaTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetAreaTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Area Types"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetMandlas")]
        public async Task<IActionResult> GetMandlas(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetMandlas(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Mandals"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetVillages")]
        public async Task<IActionResult> GetVillages(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetVillages(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Villages"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetStorageTypes")]
        public async Task<IActionResult> GetStorageTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetStorageTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Storage Types"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetChargeDetails")]
        public async Task<IActionResult> GetChargeDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetChargeDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Charge Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetSections")]
        public async Task<IActionResult> GetSections()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetSections());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Sections"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetBloodGroups")]
        public async Task<IActionResult> GetBloodGroups()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetBloodGroups());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Blood Groups"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetExperianceYears")]
        public async Task<IActionResult> GetExperianceYears()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetExperianceYears());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Experiance Years"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetExperianceMonths")]
        public async Task<IActionResult> GetExperianceMonths()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetExperianceMonths());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Experiance Years"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetNationality")]
        public async Task<IActionResult> GetNationality()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetNationality());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Nationality"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetReligions")]
        public async Task<IActionResult> GetReligions()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetReligions());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Religions"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetCommunities")]
        public async Task<IActionResult> GetCommunities()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetCommunities());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Communities"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetMaritalStatus")]
        public async Task<IActionResult> GetMaritalStatus()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetMaritalStatus());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Marital Status"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetStates")]
        public async Task<IActionResult> GetStates()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetStates());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load States"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetLocations")]
        public async Task<IActionResult> GetLocations(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetLocations(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load States"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetRManagers")]
        public async Task<IActionResult> GetRManagers()
        {
            IActionResult response = Unauthorized();
            try
            {

                return Ok(await _hel.GetRManagers());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Reporting Managers"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetRelations")]
        public async Task<IActionResult> GetRelations()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetRelations());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Relations"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetSpaceDetails")]
        public async Task<IActionResult> GetSpaceDetails()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetSpaceDetails());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Space Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetFiveYearsReport")]
        public async Task<IActionResult> GetFiveYearsReport()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetFiveYearsReport());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Five Years Report"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetGenders")]
        public async Task<IActionResult> GetGenders()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetGenders());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Genders"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveEmpPrimaryDetails")]
        public async Task<IActionResult> SaveEmpPrimaryDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpPrimaryDetailslogs", "SaveEmpPrimaryDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveEmpPrimaryDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Genaral Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveEmpCommuDetails")]
        public async Task<IActionResult> SaveEmpCommuDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpCommuDetailslogs", "SaveEmpCommuDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveEmpCommuDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Communication Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveEmpWorkDetails")]
        public async Task<IActionResult> SaveEmpWorkDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpWorkDetailsLogs", "SaveEmpWorkDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveEmpWorkDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Work Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveEmpBankDetails")]
        public async Task<IActionResult> SaveEmpBankDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpBankDetailsLogs", "SaveEmpBankDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveEmpBankDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Bank Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveEmpFamilyDetails")]
        public async Task<IActionResult> SaveEmpFamilyDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpFamilyDetailslogs", "SaveEmpFamilyDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveEmpFamilyDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Work Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveEmpPFDetails")]
        public async Task<IActionResult> SaveEmpPFDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpPFDetailslogs", "SaveEmpPFDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveEmpPFDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Providend Fund Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetIFSCCodeDetails")]
        public async Task<IActionResult> GetIFSCCodeDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetIFSCCodeDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get IFSC Code Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmpFamilyDetails")]
        public async Task<IActionResult> GetEmpFamilyDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetEmpFamilyDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Employee Family Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmployeeDetails")]
        public async Task<IActionResult> GetEmployeeDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetEmployeeDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Employee Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdatedEmployeeDetails")]
        public async Task<IActionResult> UpdatedEmployeeDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdatedEmployeeDetailsLogs", "UpdatedEmployeeDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdatedEmployeeDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Employee Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateEmpPrimaryDetails")]
        public async Task<IActionResult> UpdateEmpPrimaryDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateEmpPrimaryDetailsLogs", "UpdateEmpPrimaryDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateEmpPrimaryDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Employee General Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateAdminEmpPrimaryDetails")]
        public async Task<IActionResult> UpdateAdminEmpPrimaryDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateAdminEmpPrimaryDetailsLogs", "UpdateAdminEmpPrimaryDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateAdminEmpPrimaryDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Employee General Details"
                });
                return response;
            }
        }


        public async Task<bool> IsCaptchaValid(string token)
        {
            var result = false;

            var googleVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";

            try
            {
                string secretKey = "6Lfb2XkUAAAAAMGZT_WidYXjlGPYSSBilRfO3EkA";
                using var client = new HttpClient();

                var response = await client.PostAsync($"{googleVerificationUrl}?secret={secretKey}&response={token}", null);
                var jsonString = await response.Content.ReadAsStringAsync();
                var captchaVerfication = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(jsonString);

                result = captchaVerfication.success;
            }
            catch (Exception e)
            {
                // fail gracefully, but log
                //  logger.LogError("Failed to process captcha validation", e);
            }

            return result;
        }

        public class CaptchaVerificationResponse
        {
            public bool success { get; set; }
        }


        [HttpGet]
        [Route("GetWH_Regionalofficers")]
        public async Task<IActionResult> GetWHRegionalofficers()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetWHRegionalofficers());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Regional Offices"
                });
                return response;
            }
        }


        [HttpGet]
        [Route("GetWH_Type")]
        public async Task<IActionResult> GetWHType()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetWHType());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Warehouse Types"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetWH_List")]
        public async Task<IActionResult> GetWHList(dynamic data)
        {
            IActionResult response = Unauthorized();
            string value = EncDecrpt.Decrypt_Data(data);
            try
            {
                //string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWHList(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load WarehouseList"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetWH_View")]
        public async Task<IActionResult> GetWH_View(dynamic data)
        {
            IActionResult response = Unauthorized();
            string value = EncDecrpt.Decrypt_Data(data);
            try
            {
                //string value = JsonConvert.SerializeObject(jsondata);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWH_View(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load WarehouseList"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetWH_History")]
        public async Task<IActionResult> GetWH_History(dynamic data)
        {
            IActionResult response = Unauthorized();
            string value = EncDecrpt.Decrypt_Data(data);
            try
            {
                //string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWH_History(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Warehouse History"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("GetRegionDistricts")]
        public async Task<IActionResult> GetRegionDistricts(dynamic data)
        {
            IActionResult response = Unauthorized();
            string value = EncDecrpt.Decrypt_Data(data);
            try
            {
                //string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetRegionDistricts(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Mandals"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SaveWareHouseDetails")]
        public async Task<IActionResult> SaveWareHouseDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            string value = EncDecrpt.Decrypt_Data(data);
            try
            {
               // string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveWareHouseDetailslogs", "SaveWareHouseDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveWareHouseDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WareHouse Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("updateWareHouseDetails")]
        public async Task<IActionResult> updateWareHouseDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            string value = EncDecrpt.Decrypt_Data(data);
            try
            {
                //string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateWareHouseDetailslogs", "UpdateWareHouseDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateWareHouseDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update WareHouse Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveChangePassword")]
        public async Task<IActionResult> SaveChangePassword(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveChangePasswordlogs", "SaveChangePassword : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveChangePassword(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Change Password"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetMasterDetails")]
        public async Task<IActionResult> GetMasterDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Getmasterslist(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("employeemasterreg")]
        public IActionResult employeemasterreg(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("employeemasterregLogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "employeemasterRegistrationlogs", jsondata));
                return Ok(_hel.SaveEmpmasterreg(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {


                    StatusCode = "102",
                    StatusMessage = "Error Occured while Save Master Details",

                });
                return response;
            }
        }


        [HttpPost]
        [Route("updateEmpmasterregs")]
        public IActionResult updateEmpmasterregs(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("employeemasterupdateLogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "employeemasterUpdatelogs", jsondata));
                return Ok(_hel.updateEmpmasterreg(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while update Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("updateWareHouseDetails_ALL")]
        public async Task<IActionResult> updateWareHouseDetails_ALL(dynamic data)
        {
            IActionResult response = Unauthorized();
            string value = EncDecrpt.Decrypt_Data(data);
            try
            {
               // string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateWareHouseDetails_alllogs", "UpdateWareHouseDetails_all : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateWareHouseDetails_all(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update WareHouse Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetPageDetails")]
        public async Task<IActionResult> GetPageDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
           
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPageDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Page Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetPageAccessDetails")]
        public async Task<IActionResult> GetPageAccessDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPageAccessDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Page Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Get_DesigPageAccessDetails")]
        public async Task<IActionResult> Get_DesigPageAccessDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Get_DesigPageAccessDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Page Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SavePageDetails")]
        public async Task<IActionResult> SavePageDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SavePageDetailslogs", "SavePageDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SavePageDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Page Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveUseraccessDetails")]
        public async Task<IActionResult> SaveUseraccessDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveUseraccessDetailslogs", "SaveUseraccessDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveUseraccessDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Useraccess Details"
                });
                return response;
            }
        }

    }

}