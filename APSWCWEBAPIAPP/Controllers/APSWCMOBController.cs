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

namespace APSWCWEBAPIAPP.Controllers
{
    [Authorize(Policy = Policies.Mob)]
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
                    statusCode=102,
                    statusMessage = "Invalid Username and Password"
                });
            }
            return response;
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

        [HttpPost]
        [Route("GetServiceCharterDetails")]
        public async Task<IActionResult> GetServiceCharterDetails()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetServiceCharterDetails());
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
                FamilyListCls rootobj = JsonConvert.DeserializeObject<FamilyListCls>(value);
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

    }
}