using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelService;
using Microsoft.Extensions.Configuration;
using APSWCWEBAPIAPP.DBConnection;
using AuthService;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using APSWCWEBAPIAPP.Services;
using System.IO;
using APSWCWEBAPIAPP.Models;
using System.Net.Http.Headers;

namespace APSWCWEBAPIAPP.Controllers
{
    //[Authorize(Policy = Policies.Mob)]
    [Route("api/[controller]")]
    [ApiController]
    public class APSWCMOBController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly SqlCon _hel;
        private ApplicationAPSWCCDbContext _context;
        private readonly ICaptchaService _authservice;
        private string saFolder = Path.Combine("SaveMobileLogs");
        private string saPathToSave = string.Empty;

        public APSWCMOBController(ApplicationAPSWCCDbContext apcontext, IConfiguration config, ICaptchaService auth, SqlCon hel)
        {
            saPathToSave = Path.Combine(Directory.GetCurrentDirectory(), saFolder);
            _context = apcontext;
            _config = config;
            _authservice = auth;
            _hel = hel;
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

        [HttpPost]
        [AllowAnonymous]
        [Route("Token")]
        public IActionResult Token([FromBody] User login)
        {
            IActionResult response = Unauthorized();

            User user = _authservice.AuthenticateUser(login);
            if (user != null)
            {
                var tokenString = _authservice.GenerateJWT(user);
                user.GToken = tokenString;
                user.FirstName = user.UserName;
                user.Password = "";
                response = Ok(new
                {
                    statusCode = 100,
                    userDetails = user,
                    statusMessage = ""
                });
            }
            else
            {
                response = Ok(new
                {
                    statusCode = 102,
                    statusMessage = "Invalid Username and Password"
                });
            }
            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Captcha")]
        public dynamic Captcha()
        {
            return _authservice.check_s_captch("");
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        [HttpPost]
        [AllowAnonymous]
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

        [HttpPost]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpPrimaryDetailsLogs", "SaveEmpPrimaryDetails : Input Data : " + value));
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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpCommuDetailsLogs", "SaveEmpCommuDetails : Input Data : " + value));
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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpFamilyDetailsLogs", "SaveEmpFamilyDetails : Input Data : " + value));
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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpPFDetailsLogs", "SaveEmpPFDetails : Input Data : " + value));
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

        [HttpGet]
        [Route("GetAvailable_Facilities")]
        public async Task<IActionResult> GetFacilitiesAvailable()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetFacilitiesAvailable());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Warehouse Available Facilities"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetWH_List")]
        public async Task<IActionResult> GetWHList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
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
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWH_View(rootobj));
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
        [Route("GetWH_History")]
        public async Task<IActionResult> GetWH_History(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
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
            try
            {
                string value = JsonConvert.SerializeObject(data);
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
            try
            {
                string value = JsonConvert.SerializeObject(data);
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
            try
            {
                string value = JsonConvert.SerializeObject(data);
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
        [Route("UpdateWareHouseDetails_all")]
        public async Task<IActionResult> UpdateWareHouseDetails_all(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
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
        [Route("GetEmployeeHistory")]
        public async Task<IActionResult> GetEmployeeHistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                return Ok(await _hel.GetEmployeeHistory(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Employee History"
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
        [Route("ProfileUpdation")]
        public async Task<IActionResult> ProfileUpdation(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "ProfileUpdationlogs", "ProfileUpdation : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.ProfileUpdation(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Updating Profile"
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

        [HttpPost, DisableRequestSizeLimit]
        [Route("GalleryUploadFileDetails")]
        public IActionResult GalleryUpload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("WareHouse", "Documents");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Route("SaveEmpLeave")]
        public async Task<IActionResult> SaveEmpLeave(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpLeave", "SaveEmpLeave : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveEmpLeave(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Leave Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmpLeaves")]
        public async Task<IActionResult> GetEmpLeaves(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetEmpLeaves", "GetEmpLeaves : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetEmpLeaves(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while GetEmpLeaves Details"
                });
                return response;
            }
        }
        [HttpPost]
        [Route("EmpLeave_Cancel")]
        public async Task<IActionResult> EmpLeave_Cancel(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "EmpLeave_Cancel", "EmpLeave_Cancel : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.EmpLeave_Cancel(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Employee Leave Cancel  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("EmpLeaveTypes_Get")]
        public async Task<IActionResult> EmpLeaveTypes_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "EmpLeaveTypes_Get", "EmpLeaveTypes_Get : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.EmpLeaveTypes_Get(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Employee LeaveTypes Get  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveOfficeTimings")]
        public async Task<IActionResult> SaveOfficeTimings(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveOfficeTimings", "SaveOfficeTimings : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveOfficeTimings(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save OfficeTimings  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateOfficeTimings")]
        public async Task<IActionResult> UpdateOfficeTimings(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateOfficeTimings", "UpdateOfficeTimings : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateOfficeTimings(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while UpdateOfficeTimings  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetOfficeTimings")]
        public async Task<IActionResult> GetOfficeTimings(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetOfficeTimings", "GetOfficeTimings : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetOfficeTimings(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while GetOfficeTimings  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmpLeaveDetails")]
        public async Task<IActionResult> GetEmpLeaveDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetEmpLeaveDetails", "GetEmpLeaveDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetEmpLeaveDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get EmpLeaveDetails  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetNoOfDays")]
        public async Task<IActionResult> GetNoOfDays(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetNoOfDays", "GetNoOfDays : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetNoOfDays(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get NoOfDays  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmployeeLeavesHistory")]
        public async Task<IActionResult> GetEmployeeLeavesHistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                return Ok(await _hel.GetEmployeeLeavesHistory(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Employee Leaves History"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveEmpAttendance")]
        public async Task<IActionResult> SaveEmpAttendance(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpAttendance", "SaveEmpAttendance : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveEmpAttendance(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Attendance Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmpAttendanceDetails")]
        public async Task<IActionResult> GetEmpAttendanceDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetEmpAttendanceDetails", "GetEmpAttendanceDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetEmpAttendanceDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Employee Attendance Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetCalendarDetails")]
        public async Task<IActionResult> GetCalendarDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetCalendarDetails", "GetCalendarDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCalendarDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Calendar Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Emp_Notes_Save")]
        public async Task<IActionResult> Emp_Notes_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Emp_Notes_Save", "Emp_Notes_Save : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Emp_Notes_Save(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Emp_Notes_Save  Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetWH_InspectionData")]
        public async Task<IActionResult> GetWH_InspectionData()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetWH_InspectionData());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while WareHouse Inspection List",

                });
                return response;
            }
        }
        [HttpPost]
        [Route("WH_Inspection_Save")]
        public async Task<IActionResult> WH_Inspection_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "WH_Inspection_SaveLogs", "WH_Inspection_Save : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.WH_Inspection_Save(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WareHouse Inspection Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Get_Login_Sheduled")]
        public async Task<IActionResult> Get_Login_Sheduled(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetLoginSheduled", "GetLoginSheduled : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Get_Login_Sheduled(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get User Login Sheduled Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Get_ReSheduled_Inspection")]
        public async Task<IActionResult> Get_ReSheduled_Inspection(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "ReSheduledInspection", "GetLoginSheduled : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Get_ReSheduled_Inspection(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Re-Sheduled Inspection Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Scheduled_Inspection_Review")]
        public async Task<IActionResult> Scheduled_Inspection_Review(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SheduledInspectionReviewLogs", "SheduledInspectionReview : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Scheduled_Inspection_Review(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Sheduled Inspection Review Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Review_Inspection")]
        public async Task<IActionResult> Review_Inspection(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Review_InspectionLogs", "Review_Inspection : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Review_Inspection(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Inspection Review Details"
                });
                return response;
            }
        }


        [HttpPost, DisableRequestSizeLimit]
        [Route("VirtualInspectiondFileDetails")]
        public IActionResult VirtualInspection()
        {
            try
            {
                var supportedTypes = new[] { "mp4" };
                var file = Request.Form.Files[0];
                var file1 = Request.Form.Files[1];
                var file2 = Request.Form.Files[2];
                var file3 = Request.Form.Files[3];
                var file4 = Request.Form.Files[4];
                var folderName = Path.Combine("WareHouse", "InspectionVideos");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0 && file1.Length > 0 && file2.Length > 0 && file3.Length > 0 && file4.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fileName1 = ContentDispositionHeaderValue.Parse(file1.ContentDisposition).FileName.Trim('"');
                    var fileName2 = ContentDispositionHeaderValue.Parse(file2.ContentDisposition).FileName.Trim('"');
                    var fileName3 = ContentDispositionHeaderValue.Parse(file3.ContentDisposition).FileName.Trim('"');
                    var fileName4 = ContentDispositionHeaderValue.Parse(file4.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var fullPath1 = Path.Combine(pathToSave, fileName1);
                    var fullPath2 = Path.Combine(pathToSave, fileName2);
                    var fullPath3 = Path.Combine(pathToSave, fileName3);
                    var fullPath4 = Path.Combine(pathToSave, fileName4);
                    var dbPath = Path.Combine(folderName, fileName);
                    var dbPath1 = Path.Combine(folderName, fileName1);
                    var dbPath2 = Path.Combine(folderName, fileName2);
                    var dbPath3 = Path.Combine(folderName, fileName3);
                    var dbPath4 = Path.Combine(folderName, fileName4);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    using (var stream1 = new FileStream(fullPath1, FileMode.Create))
                    {
                        file1.CopyTo(stream1);
                    }
                    using (var stream2 = new FileStream(fullPath2, FileMode.Create))
                    {
                        file2.CopyTo(stream2);
                    }
                    using (var stream3 = new FileStream(fullPath3, FileMode.Create))
                    {
                        file3.CopyTo(stream3);
                    }
                    using (var stream4 = new FileStream(fullPath4, FileMode.Create))
                    {
                        file4.CopyTo(stream4);
                    }
                    return Ok(new { dbPath, dbPath1, dbPath2, dbPath3, dbPath4 });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        [Route("GetWH_Regions")]
        public async Task<IActionResult> GetWHRegions()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetWHRegions());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get WareHouse Regions List",

                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetWH_Districts")]
        public async Task<IActionResult> GetWHDistricts(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetWHDistrictsLogs", "GetWHDistricts : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWHDistricts(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get WareHouse District Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetWH_InspectionList")]
        public async Task<IActionResult> GetWHInspectionList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetWHInspectionListLogs", "GetWHInspectionList : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWHInspectionList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get WareHouse Inspection Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("GetInspectionHistory")]
        public async Task<IActionResult> GetInspectionHistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                return Ok(await _hel.GetInspectionHistory(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Virtual Inspection History"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("DeadStockCategory")]
        public async Task<IActionResult> DeadStockCategory()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.DeadStockCategory());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Dead Stock Category Details",

                });
                return response;
            }
        }

        [HttpPost]
        [Route("DeadStocksubCategory")]
        public async Task<IActionResult> DeadStocksubCategory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "DeadStocksubCategoryLogs", "DeadStocksubCategory : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DeadStocksubCategory(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Dead Stock Sub Category List"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetItemTypes")]
        public async Task<IActionResult> GetItemTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetItemTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Item Types List",

                });
                return response;
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("DeadStockUploadFileDetails")]
        public IActionResult DeadStockUploadFileDetails()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("DeadStock", "Documents");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("DeadStockUploadImageDetails")]
        public IActionResult DeadStockUploadImageDetails()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("DeadStock", "Images");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Route("Deadstock_Details_Save")]
        public async Task<IActionResult> DeadstockDetailsSave(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "DeadstockDetailsSaveLogs", "DeadstockDetailsSave : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DeadstockDetailsSave(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Deadstock Insertion Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Get_DeadStock_Deatails")]
        public async Task<IActionResult> GetDeadStockDeatails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetDeadStockDeatailsLogs", "GetDeadStockDeatails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDeadStockDeatails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get WareHouse DeadStock Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("DeadStock_Regional_List")]
        public async Task<IActionResult> DeadStockRegionalList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "DeadStockRegionalListLogs", "DeadStockRegionalList : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DeadStockRegionalList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get WareHouse DeadStock Regional List"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("DeadStock_Section_List")]
        public async Task<IActionResult> DeadStockSectionList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "DeadStockSectionListLogs", "DeadStockSectionList : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DeadStockSectionList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get WareHouse DeadStock Section List"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Get_DeadStock_Status")]
        public async Task<IActionResult> GetDeadStockStatus(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetDeadStockStatusLogs", "GetDeadStockStatus : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDeadStockStatus(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while WareHouse DeadStock Status Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("Get_Tenders_Details")]
        public async Task<IActionResult> GetTendersDetails()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetTendersDetails());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Tenders Details",

                });
                return response;
            }
        }

        [HttpGet]
        [Route("Get_News_Details")]
        public async Task<IActionResult> GetNewsDetails()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetNewsDetails());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get News Details",

                });
                return response;
            }
        }

        [Route("GetWH_DeadStock_History")]
        public async Task<IActionResult> GetWHDeadStockHistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                return Ok(await _hel.GetWHDeadStockHistory(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Dead Stock History"
                });
                return response;
            }
        }

        #region Depositor Receipt In 

        [HttpPost]
        [Route("GetSpaceReservationDetails")]
        public async Task<IActionResult> GetSpaceReservationDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetSpaceReservationDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Space Reservation  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Savereceiptin")]
        public async Task<IActionResult> Savereceiptin(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Savereceiptin(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Saveing Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetReceiptInTokens")]
        public async Task<IActionResult> GetReceiptInTokens(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetReceiptInTokens(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetTransportDetails")]
        public async Task<IActionResult> GetTransportDetails()
        {
            IActionResult response = Unauthorized();
            try
            {

                return Ok(await _hel.GetTransportDetails());
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Trasport  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveGateIn")]
        public async Task<IActionResult> SaveGateIn(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveGateInLogs", "SaveGateIn : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveGateIn(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Gate IN Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetTokens")]
        public async Task<IActionResult> GetTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetTokens(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Token Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetTokenInfo")]
        public async Task<IActionResult> GetTokenInfo(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetTokenInfo(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Token Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveWeighmentIn")]
        public async Task<IActionResult> SaveWeighmentIn(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveWeighmentInLogs", "SaveWeighmentIn : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveWeighmentIn(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Weighment IN Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetDumpingTokens")]
        public async Task<IActionResult> GetDumpingTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDumpingTokens(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Tokens Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveDumping")]
        public async Task<IActionResult> SaveDumping(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveDumpingLogs", "SaveDumping : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveDumping(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Dumping Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetStackTokens")]
        public async Task<IActionResult> GetStackTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetStackTokens(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Tokens Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetStackTokenInfo")]
        public async Task<IActionResult> GetStackTokenInfo(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetStackTokenInfo(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Token Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetGodowns")]
        public async Task<IActionResult> GetGodowns(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetGodowns(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Godowns Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetCompartments")]
        public async Task<IActionResult> GetCompartments(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCompartments(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Compartments Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetStacks")]
        public async Task<IActionResult> GetStacks(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetStacks(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Stacks Information"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetStackTypes")]
        public async Task<IActionResult> GetStackTypes()
        {
            IActionResult response = Unauthorized();
            try
            {

                return Ok(await _hel.GetStackTypes());
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Stack Types  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveStacking")]
        public async Task<IActionResult> SaveStacking(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveStackingLogs", "SaveStacking : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveStacking(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Stacking Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetGateoutTokens")]
        public async Task<IActionResult> GetGateoutTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetGateoutTokens(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Tokens Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveGateOut")]
        public async Task<IActionResult> SaveGateOut(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveGateOutLogs", "SaveGateOut : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveGateOut(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Gate Out Information"
                });
                return response;
            }
        }

        #endregion

        [HttpPost]
        [Route("Weighment_OUTTokenList")]
        public async Task<IActionResult> Weighment_OUTTokenList(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Weighment_OUTTokenList(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveWeighmentOut")]
        public async Task<IActionResult> SaveWeighmentOut(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveWeighmentOut(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Saveing Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("WeighmentTokenList")]
        public async Task<IActionResult> WeighmentTokenList(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.WeighmentTokenList(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetVariety_GradeList")]
        public async Task<IActionResult> GetVarietyGradeList(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetVarietyGradeList(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetQualityPrameters")]
        public async Task<IActionResult> GetQualityPrameters(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetQualityPrameters(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SaveQualityChecking")]
        public async Task<IActionResult> SaveQualityChecking(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveQualityChecking(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Saveing Details"
                });
                return response;
            }
        }


        #region Depositor Receipt Out

        [HttpPost]
        [Route("GetStackinDetails")]
        public async Task<IActionResult> GetStackinDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetStackinDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Stack in Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveReceiptOutRequest")]
        public async Task<IActionResult> SaveReceiptOutRequest(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveReceiptOutRequestLogs", "SaveReceiptOutRequest : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveReceiptOutRequest(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Receipt Out Request Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetReceiptOutDetails")]
        public async Task<IActionResult> GetReceiptOutDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetReceiptOutDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Token  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetReceiptDetails")]
        public async Task<IActionResult> GetReceiptDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetReceiptDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Token  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetStackInCommodities")]
        public async Task<IActionResult> GetStackInCommodities(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetStackInCommodities(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Token  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("QC_OUT_Tokens")]
        public async Task<IActionResult> QC_OUT_Tokens(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.QC_OUT_Tokens(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Out_Gateouttokens")]
        public async Task<IActionResult> Out_Gateouttokens(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Out_Gateouttokens(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetWeighinTokens")]
        public async Task<IActionResult> GetWeighinTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWeighmentinToken(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Token Information"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SaveOutWeighmentIn")]
        public async Task<IActionResult> SaveOutWeighmentIn(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveOutWeighmentInLogs", "SaveOutWeighmentInLogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveOutWeighmentIn(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save OUTWeighment IN Information"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetWeighOutTokens")]
        public async Task<IActionResult> GetWeighOutTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWeighmentoutToken(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Token Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveOutWeighmentOut")]
        public async Task<IActionResult> SaveOutWeighmentOut(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveOutWeighmentInLogs", "SaveOutWeighmentInLogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveOutWeighmentout(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save OUTWeighment IN Information"
                });
                return response;
            }
        }


        #endregion

        [HttpPost]
        [Route("GetuserAccess_menu")]
        public async Task<IActionResult> GetMobuserAccessmenu(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetMobuserAccessmenu(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Menu Details"
                });
                return response;
            }
        }

        #region depositor receipt in (Others)
        [HttpPost]
        [Route("GetOtherReceiptInTokens")]
        public async Task<IActionResult> GetOtherReceiptInTokens(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetOtherReceiptInTokens(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("OtherWeighmentTokenList")]
        public async Task<IActionResult> OtherWeighmentTokenList(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.OtherWeighmentTokenList(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetOtherTokens")]
        public async Task<IActionResult> GetOtherTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetOtherTokens(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Token Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetOtherStackTokens")]
        public async Task<IActionResult> GetOtherStackTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetOtherStackTokens(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Tokens Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("OtherWeighment_OUTTokenList")]
        public async Task<IActionResult> OtherWeighment_OUTTokenList(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.OtherWeighment_OUTTokenList(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetOtherGateoutTokens")]
        public async Task<IActionResult> GetOtherGateoutTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetOtherGateoutTokens(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Tokens Information"
                });
                return response;
            }
        }

        #endregion


        #region General Booking


        [HttpPost]
        [Route("GetExistingUser_Details")]
        public async Task<IActionResult> GetExistingUserDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetExistingUserDetails(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load User Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetSubmitted_Docs")]
        public async Task<IActionResult> GetSubmittedDocs(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetSubmittedDocs(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetCommu_Details")]
        public async Task<IActionResult> GetCommuDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCommuDetails(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetQuantity_contract_Details")]
        public async Task<IActionResult> GetQntity_cntrct_Details(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetQntity_cntrct_Details(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetCommodityGroup_Details")]
        public async Task<IActionResult> GetCommodityGroupDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCommodityGroupDetails(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveSpaceReservation")]
        public async Task<IActionResult> SaveSpaceReservation(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveSpaceReservation(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Saveing Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveRegistrationDetails")]
        public async Task<IActionResult> SaveRegistrationDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveRegistrationDetailslogs", "SaveRegistrationDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveRegistrationDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Registration Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveRegDocDetails")]
        public async Task<IActionResult> SaveRegDocDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveRegDocDetailslogs", "SaveRegDocDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveRegDocDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Document Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveRegCommuDetails")]
        public async Task<IActionResult> SaveRegCommuDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveRegCommuDetailslogs", "SaveRegCommuDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveRegCommuDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Communication Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveFarCommuDetails")]
        public async Task<IActionResult> SaveFarCommuDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveFarCommuDetailslogs", "SaveFarCommuDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveFarCommuDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Communication Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetReqDocByReg")]
        public async Task<IActionResult> GetReqDocByReg(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetReqDocByReg(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured Load Document List"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetRegistrationTypes")]
        public async Task<IActionResult> GetRegistrationTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetRegistrationTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Registration Types"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetDocTypes")]
        public async Task<IActionResult> GetDocTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetDocTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Document Types"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetFarmerTypes")]
        public async Task<IActionResult> GetFarmerTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetFarmerTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Document Types"
                });
                return response;
            }
        }

        #endregion

        [HttpPost]
        [Route("GetPeriodicQCDetails")]
        public async Task<IActionResult> GetPeriodicQCDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPeriodicQCDetails(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SavePeriodicQualityChecking")]
        public async Task<IActionResult> SavePeriodicQualityChecking(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SavePeriodicQualityChecking(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Saveing Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("Savedisinfestation")]
        public async Task<IActionResult> Savedisinfestation(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Savedisinfestation(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Saveing Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Notifications_Get")]
        public async Task<IActionResult> Notifications_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Notifications_Get", "Notifications_Get : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Notifications_Get(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Notifications_Get  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Notifications_Update")]
        public async Task<IActionResult> Notifications_Update(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Notifications_Update", "Notifications_Get : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Notifications_Update(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Notifications_Update  Details"
                });
                return response;
            }
        }

        #region Past Attendance

        [HttpPost]
        [Route("PastAttendance_Save")]
        public async Task<IActionResult> PastAttendance_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "PastAttendance_Save", "PastAttendance_Save : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.PastAttendance_Save(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while PastAttendance_Save  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("PastAttendance_Update")]
        public async Task<IActionResult> PastAttendance_Update(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "PastAttendance_Update", "PastAttendance_Update : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.PastAttendance_Update(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while PastAttendance_Update  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("PastAttendance_Get")]
        public async Task<IActionResult> PastAttendance_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "PastAttendance_Get", "PastAttendance_Get : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.PastAttendance_Get(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while PastAttendance_Get  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetMastersHistory")]
        public async Task<IActionResult> GetMasterHistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetmastersHistorylist(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Master History Details"
                });
                return response;
            }
        }

        #endregion

        #region DeadStock
        [HttpPost]
        [Route("GetDeadStockDetails")]
        public async Task<IActionResult> GetDeadStockDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDeadStockDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("DeadStockInsertion")]
        public async Task<IActionResult> DeadStockInsertion(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "DeadStockInsertionlogs", "DeadStockInsertionlogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DeadStockInsertion(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetDeadStockHistory")]
        public async Task<IActionResult> GetDeadStockHistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDeadStockHistory(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Data"
                });
                return response;
            }
        }

        #endregion

        #region Help
        [HttpPost]
        [Route("GetHelpDetails")]
        public async Task<IActionResult> GetHelpDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHelpDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("HelpInsertion")]
        public async Task<IActionResult> HelpInsertion(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "HelpInsertionlogs", "HelpInsertionlogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.HelpInsertion(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHelpHistory")]
        public async Task<IActionResult> GetHelpHistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHelpHistory(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Data"
                });
                return response;
            }
        }

        #endregion

        [HttpPost]
        [Route("SaveTaxDetails")]
        public async Task<IActionResult> SaveTaxDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveTaxdetailsLogs", "SaveTaxdetailsLogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveTaxpaymentDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Tax Information"
                });
                return response;
            }
        }
        [HttpPost]
        [Route("GetTaxData")]
        public async Task<IActionResult> GetTaxData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetTaxDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Token Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveEmpTransferWorkDetails")]
        public async Task<IActionResult> SaveEmpTransferWorkDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpTransferWorkDetailsLogs", "SaveEmpTransferWorkDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveEmpTransferWorkDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Transfer Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetMDApprovalData")]
        public async Task<IActionResult> GetMDApprovalData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetMDApprovalDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Employee Information"
                });
                return response;
            }
        }

        [HttpPost]

        [Route("SaveMdlevelapprovalDetails")]
        public async Task<IActionResult> SaveMdlevelapprovalDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveMdlevelapprovalDetailsLogs", "SaveMdlevelapprovalDetailsLogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.SaveMDLEVELApprovalloan(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save EmployeeLoanApproved Information"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetImprestTypes")]
        public async Task<IActionResult> GetImprestTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetImprestTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Imprest Types",

                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetChemicalTypes")]
        public async Task<IActionResult> GetChemicalTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetChemicalTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Chemical Types",

                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetDisiHistory")]
        public async Task<IActionResult> GetDisiHistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDisiHistory(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveDisinfestationDetails")]
        public async Task<IActionResult> SaveDisinfestationDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveDisinfestationLogs", "SaveDisinfestationDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveDisinfestationDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Disinfestation Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveSpillingDetails")]
        public async Task<IActionResult> SaveSpillingDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveSpillingLogs", "SaveSpillingDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveSpillingDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Spilling Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetQualityiHistory")]
        public async Task<IActionResult> GetQualityiHistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetQualityiHistoryLogs", "GetQualityiHistory : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetQualityiHistory(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Quality History Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetPaymentApprovalData")]
        public async Task<IActionResult> GetPaymentApprovalData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetPaymentapprovalDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Payment Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetOfficerApprovalData")]
        public async Task<IActionResult> GetOfficerApprovalData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetOfficerApprovalDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Employee Information"
                });
                return response;
            }
        }

        [HttpPost]

        [Route("SaveemployeeofficerloanDetails")]
        public async Task<IActionResult> SaveemployeeofficerloanDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveemployeeloaninstallmentLogs", "SaveemployeeloaninstallmentLogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.Saveemployeeofficerloan(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save EmployeeLoanApproved Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmployeeMonthsData")]
        public async Task<IActionResult> GetEmployeeMonthsData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetEmployeeMonthsDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Months Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmployeeLoanData")]
        public async Task<IActionResult> GetEmployeeLoanData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetEmployeeLoanDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Loantype Information"
                });
                return response;
            }
        }


        [Route("SaveEmployeeloanDetails")]
        public async Task<IActionResult> SaveEmployeeloanDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                EmployeeMasterSp _rbroot = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmployeeloanrequestLogs", "SaveEmployeeloanrequestLogs : Input Data : " + value));

                return Ok(await _hel.SaveEmployeeloanreqDetails(_rbroot));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employeeloan Request Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmployeeData")]
        public async Task<IActionResult> GetEmployeeData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetEmployeeDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Loantype Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetWeigh_Bridge_Details")]
        public async Task<IActionResult> GetWeigh_Bridge_Details(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWeigh_Bridge_Details(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveWBDetails")]
        public async Task<IActionResult> SaveWBDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveWBDetailsLogs", "SaveWBDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveWBDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Weigh Bridge Details"
                });
                return response;
            }
        }

        #region Employee payroll

        [HttpPost]
        [Route("GetEmptypeData")]
        public async Task<IActionResult> GetEmptypeData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetEmptypeDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Token Information"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("CheckEmpCodeFound")]
        public async Task<IActionResult> CheckEmpCodeFound(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetEmpDetailsBycode(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load  Information"
                });
                return response;
            }
        }
        [HttpPost]

        [Route("SaveEmpSalDetails")]
        public async Task<IActionResult> SaveEmpSalDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveEmpSalDetailsLogs", "SaveEmpSalDetailsLogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.SaveEmployeesalDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Salary Information"
                });
                return response;
            }
        }

        [Route("UpdateEmpSalDetails")]
        public async Task<IActionResult> UpdateEmpSalDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateEmpSalDetailsLogs", "UpdateEmpSalDetailsLogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.UpdateEmployeesalaryDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Employee Salary Information"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetMonths")]
        public async Task<IActionResult> GetMonths(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetmonthsDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Months Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Getpayroledata")]
        public async Task<IActionResult> Getpayroledata(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetPayroleDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Employee Salary Information"
                });
                return response;
            }
        }


        [Route("SavePayroleDetails")]
        public async Task<IActionResult> SavePayroleDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                EmployeeMasterSp _rbroot = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SavePayroleDetailsLogs", "SavePayroleDetailsLogs : Input Data : " + value));

                return Ok(await _hel.SaveEmployeepayroleDetails(_rbroot));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Employee Salary Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Getemployeetypes")]
        public async Task<IActionResult> Getemployeetypes(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);

                return Ok(await _hel.GetEmpltypeDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Employee Type Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetEmployeetypesData")]
        public async Task<IActionResult> GetEmployeetypesData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetemployeetypeDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Employee Information"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetEmployeeviewdata")]
        public async Task<IActionResult> GetEmployeeviewdata(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetemployeeviewDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Employee Information"
                });
                return response;
            }
        }

        #endregion

        [HttpPost]
        [Route("GetGodown_Stack_cmprtdetails")]
        public async Task<IActionResult> GetGodown_Stack_cmprtdetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetGodown_Stack_cmprtdetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("GetStackRegisterDetails")]
        public async Task<IActionResult> GetStackRegisterDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetStackRegisterDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Stack Register Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetInvoiceModeDetails")]
        public async Task<IActionResult> GetInvoiceModeDetails()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetInvoiceModeDetails());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Invoice Modes",

                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetInvPriceDetails")]
        public async Task<IActionResult> GetInvPriceDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetInvPriceDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Price Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetRebateDetails")]
        public async Task<IActionResult> GetRebateDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetRebateDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Rebate Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetWHDetails")]
        public async Task<IActionResult> GetWHDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWHDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Warehouse Details"
                });
                return response;
            }
        }

    }
}