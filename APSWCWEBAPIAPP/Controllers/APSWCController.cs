
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
using System.Dynamic;

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


        public APSWCController(ApplicationAPSWCCDbContext apcontext, IConfiguration config, ICaptchaService auth, SqlCon hel)
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
        [Route("GalleryRegistration")]
        public async Task<IActionResult> GalleryRegistration([FromBody] MasterSp Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("APSWCLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            MasterSp obj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "GalleryRegistrationLogs", jsondata));

            return Ok(await _hel.SaveGalleryRegisteration(obj));

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

        [HttpPost]
        [Route("ScrollNewMessageInsert")]
        public async Task<IActionResult> ScrollNewMessageInsert([FromBody] MasterSp Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("APSWCLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            MasterSp obj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "ScrollNewMessageInsertLogs", jsondata));

            return Ok(await _hel.SaveNewsScrollMessage(obj));

        }

        [HttpPost]
        [Route("GetScrollNewMessage")]
        public async Task<IActionResult> GetScrollNewMessage([FromBody] MasterSp Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("APSWCLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            MasterSp obj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
            //Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "ScrollNewMessageInsertLogs", jsondata));

            return Ok(await _hel.GetNewsScrollMessageData(obj));

        }

        [HttpGet]
        [Route("GetHomePageConent")]
        public async Task<IActionResult> GetHomePageConent()
        {

            return Ok(await _hel.GetHomepageContent());

        }


        [HttpGet]
        [Route("GetGalleryImages")]
        public async Task<IActionResult> GetGalleryImages()
        {

            return Ok(await _hel.GetGalleryImages());

        }

        [HttpGet]
        [Route("GetVisitorsCount")]
        public async Task<IActionResult> GetVisitorsCount()
        {

            return Ok(await _hel.GetVisitorsCount());

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


        [HttpPost]
        [Route("GetPincodeDetails")]
        public async Task<IActionResult> GetPincodeDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPincodeDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Pincode Details"
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
        [Route("updateWareHouseDetails_ALL")]
        public async Task<IActionResult> updateWareHouseDetails_ALL(dynamic data)
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



        [HttpPost]
        [Route("InsuranceCmpnywiseDetails")]
        public async Task<IActionResult> InsuranceCmpnywiseDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.InsuranceCmpnywiseDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Insurance Company Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SaveWH_InsuranceDetails")]
        public async Task<IActionResult> SaveWH_InsuranceDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveWH_InsuranceDetailsLogs", "SaveWH_InsuranceDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveWH_InsuranceDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save  Warehouse Insurance Details"
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
        [Route("GetPageDetailsByID")]
        public async Task<IActionResult> GetPageDetailsByID(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPageDetailsByID(rootobj));
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
        [Route("UpdatePageDetails")]
        public async Task<IActionResult> UpdatePageDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdatePageDetailslogs", "UpdatePageDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdatePageDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Page Details"
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

        [HttpGet]
        [Route("GetLeaveTypes_Settings")]
        public async Task<IActionResult> GetLeaveTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetLeaveTypes_Settings());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while GetLeaveTypes_Settings Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveLeaveMaster")]
        public async Task<IActionResult> SaveLeaveMaster(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveLeaveMasterLogs", "SaveLeaveMasterLogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveLeaveMaster(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Leave Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateLeaveMaster")]
        public async Task<IActionResult> UpdateLeaveMaster(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateLeaveMaster", "UpdateLeaveMaster : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateLeaveMaster(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Leave Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveHolidayMaster")]
        public async Task<IActionResult> SaveHolidayMaster(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveHolidayMaster", "SaveHolidayMaster : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveHolidayMaster(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Holiday Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateHolidayMaster")]
        public async Task<IActionResult> UpdateHolidayMaster(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateHolidayMaster", "UpdateHolidayMaster : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateHolidayMaster(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Holiday Master Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetHolidayMaster")]
        public async Task<IActionResult> GetHolidayMaster(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetHolidayMaster", "GetHolidayMaster : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHolidayMaster(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get HolidayMaster Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetLeaveDetails")]
        public async Task<IActionResult> GetLeaveDetails()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetLeaveDetails());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Leave Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHolidayMaster_VIEW")]
        public async Task<IActionResult> GetHolidayMaster_VIEW(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetHolidayMaster_VIEW", "GetHolidayMaster_VIEW : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHolidayMaster_VIEW(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get HolidayMaster_VIEW Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetLeaveMaster_VIEW")]
        public async Task<IActionResult> GetLeaveMaster_VIEW(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetLeaveMaster_VIEW", "GetLeaveMaster_VIEW : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetLeaveMaster_VIEW(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get GetLeaveMaster_VIEW Details"
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
                    StatusMessage = "Error Occured while Save Employee Leave Master Details"
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

        #region Out sourcing Agencies

        [HttpPost]
        [Route("SaveOutsourcingAgency")]
        public async Task<IActionResult> SaveOutsourcingAgency(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SSaveOutsourcingAgencyLogs", "SaveOutsourcingAgency : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveOutsourcingAgency(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Outsourcing Agency  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateOutsourcingAgency")]
        public async Task<IActionResult> UpdateOutsourcingAgency(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateOutsourcingAgencyLogs", "UpdateOutsourcingAgency : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateOutsourcingAgency(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Outsourcing Agency  Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetOutsourcingAgencies")]
        public async Task<IActionResult> GetOutsourcingAgencies()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetOutsourcingAgencies());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Outsourcing Agencies"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetDetailsofOutsourcing")]
        public async Task<IActionResult> GetDetailsofOutsourcing(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDetailsofOutsourcing(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of Outsourcing"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetOutsourceContactDetails")]
        public async Task<IActionResult> GetOutsourceContactDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetOutsourceContactDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Outsource Contact Details"
                });
                return response;
            }
        }

        #endregion

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
        [Route("GetuserAccess_menu")]
        public async Task<IActionResult> GetuserAccessmenu(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetuserAccessmenu(rootobj));

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
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveSpaceReservationLogs", "SaveSpaceReservation : Input Data : " + value));
               
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

        [HttpPost]
        [Route("SaveQuantityChecking")]
        public async Task<IActionResult> SaveQuantityChecking(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveQuantityCheckingLogs", "SaveQuantityChecking : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveQuantityChecking(rootobj));

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
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveWeighmentOutLogs", "SaveWeighmentOut : Input Data : " + value));
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
        [Route("Getcommodities")]
        public async Task<IActionResult> Getcommodities(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Getcommoditieslist(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Master  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("insertCommodities")]
        public IActionResult insertCommodities(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("employeemasterregLogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "employeemasterregLogs", jsondata));
                return Ok(_hel.insertcommodity(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }
        [HttpPost]
        [Route("updateCommodities")]
        public IActionResult updateCommodities(dynamic data)
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
                return Ok(_hel.updateCommodities(rootobj));
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
        [Route("GetRegionMasterDetails")]
        public async Task<IActionResult> GetRegionDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetRegionmasterslist(rootobj));
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
        [Route("GetRegionDetailsbydistcode")]
        public async Task<IActionResult> GetRegionDetailsbydistcode(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetRegionbydistcode(rootobj));
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
        [Route("Emp_CheckIn_Out")]
        public async Task<IActionResult> Emp_CheckIn_Out(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Emp_CheckIn_Out", "Emp_CheckIn_Out : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Emp_CheckIn_Out(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Emp_CheckIn_Out  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("Get_Emp_CheckIn_Out_Details")]
        public async Task<IActionResult> Get_Emp_CheckIn_Out_Details(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GET Emp_CheckIn_Out_Details", "Emp_CheckIn_Out : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Get_Emp_CheckIn_Out_Details(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Emp_CheckIn_Out_Details  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Emp_All_Events")]
        public async Task<IActionResult> Emp_All_Events(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Emp_All_Events", "Emp_All_Events : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Emp_All_Events(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Emp_All_Events  Details"
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

        #region Mail
        [HttpPost]
        [Route("SendMail")]
        public async Task<IActionResult> SendMail(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SendMail(rootobj));
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
        [Route("TestMail")]
        public async Task<IActionResult> TestMail(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.TestMail(rootobj));
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
        [Route("GetLayoutConfiguration")]
        public async Task<IActionResult> GetLayoutConfiguration(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetLayoutConfiguration(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while get Layout Configuration"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveLayoutConfiguration")]
        public async Task<IActionResult> SaveLayoutConfiguration(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveLayoutConfigurationLogs", "SaveLayoutConfiguration : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveLayoutConfiguration(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Layout Configuration"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateLayoutApproveorRejectStatus")]
        public async Task<IActionResult> UpdateLayoutApproveorRejectStatus(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateLayoutConfigurationLogs", "UpdateLayoutStatus : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateLayoutApproveorRejectStatus(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Layout Approve/Reject Status"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetWHDetailsByRM")]
        public async Task<IActionResult> GetWHDetailsByRM(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWHDetailsByRM(rootobj));
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
        [Route("GetWHCapacityDetails")]
        public async Task<IActionResult> GetWHCapacityDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWHCapacityDetails(rootobj));

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
        [Route("GetCompartmentlistforleased")]
        public async Task<IActionResult> GetCompartmentlistforleased(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCompartmentlistforleased(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading Compartment Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Emp_Leave_Balance")]
        public async Task<IActionResult> Emp_Leave_Balance(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Emp_Leave_Balance", "Emp_Leave_Balance : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                OkObjectResult okObjectResult = Ok(await _hel.Emp_Leave_Balance(rootobj));

                return okObjectResult;
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Emp_Leave_Balance  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("Emp_Leave_Balance_Save")]
        public async Task<IActionResult> Emp_Leave_Balance_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Emp_Leave_Balance_Save", "Emp_Leave_Balance_Save : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Emp_Leave_Balance_Save(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Emp_Leave_Balance_Save  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Emp_In_Out_Get")]
        public async Task<IActionResult> Emp_In_Out_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Emp_In_Out_Get", "Emp_In_Out_Get : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Emp_In_Out_Get(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Emp_In_Out_Get  Details"
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
        [Route("GetQuantityTokens")]
        public async Task<IActionResult> GetQuantityTokens(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetQuantityTokens(rootobj));
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

        [HttpGet]
        [Route("GetInsurancesList")]
        public async Task<IActionResult> GetInsurancesList()
        {
            IActionResult response = Unauthorized();
            try
            {

                return Ok(await _hel.GetInsurancesList());
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Insurances  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetInsuranceByID")]
        public async Task<IActionResult> GetInsuranceByID(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetInsuranceByID(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Insurance Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveInsuranceDetails")]
        public async Task<IActionResult> SaveInsuranceDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveInsuranceDetailsLogs", "SaveInsuranceDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveInsuranceDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Insurance Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateInsuranceDetails")]
        public async Task<IActionResult> UpdateInsuranceDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateInsuranceDetailsLogs", "UpdateInsuranceDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateInsuranceDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Insurance Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetVarietylist")]
        public async Task<IActionResult> GetVarietylist(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Getvarietylist(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Master  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("Getcommoditygrpbycomdty")]
        public async Task<IActionResult> Getcommoditygrpbycomdty(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Getcommodtilistbycomdid(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting Master  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SavecommodityVarietyMaster")]
        public IActionResult SavecommodityVarietyMaster(dynamic data)
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
                return Ok(_hel.Savecommoditymasterreg(rootobj));
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
        [Route("updateCommodityVariety")]
        public IActionResult updateCommodityVariety(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("employeemasterupdateLogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMqualityobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "employeemasterUpdatelogs", jsondata));
                return Ok(_hel.updateCommodityvarietyreg(rootobj));
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
        [Route("SaveQualityParameter")]
        public IActionResult SaveQualityParameter(dynamic data)
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
                return Ok(_hel.SaveQualityparams(rootobj));
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
        [Route("updateQualityParameter")]
        public IActionResult updateQualityParameter(dynamic data)
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
                return Ok(_hel.updateQualityparams(rootobj));
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
        [Route("SaveCommodityGroupDetails")]
        public IActionResult SaveCommodityGroupDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("employeemasterregLogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "employeemasterregLogs", jsondata));
                return Ok(_hel.SavecommodityGroup(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }
        [HttpPost]
        [Route("updateCommodityGroupDetails")]
        public IActionResult updateCommodityGroupDetails(dynamic data)
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
                return Ok(_hel.updateCommodityGroup(rootobj));
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
        [Route("UpdateUseraccessDetails")]
        public async Task<IActionResult> UpdateUseraccessDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "updateUseraccessDetailslogs", "SaveUseraccessDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateUseraccessDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Updateing Useraccess Details"
                });
                return response;
            }
        }

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

        [HttpPost]
        [Route("GetWeighbridgedata")]
        public async Task<IActionResult> GetWeighbridgedata(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWeighmentdataDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Weighbridge Information"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SaveWeighbridgeDetails")]
        public async Task<IActionResult> SaveWeighbridgeDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveWeighbridgedetailsLogs", "SaveWeighbridgedetailsLogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveWeghbridgeDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Weighbridge Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("updateWeighbridgedetails")]
        public IActionResult updateWeighbridgedetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("WeighbridgeupdateLogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "WeighbridgeUpdatelogs", jsondata));
                return Ok(_hel.updateWeighbridge(rootobj));
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

        #region Warehouse Receipt

        [HttpPost]
        [Route("GetPendingWHReceipts")]
        public async Task<IActionResult> GetPendingWHReceipts(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPendingWHReceipts(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Warehouse Receipts  Details"
                });
                return response;
            }
        }

        [HttpGet]
        [Route("GetInsuranceList")]
        public async Task<IActionResult> GetInsuranceList()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetInsuranceList());
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Insurance  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveReceiptDetails")]
        public async Task<IActionResult> SaveReceiptDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveReceiptDetailsLogs", "SaveReceiptDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveReceiptDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Generate Receipt"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetGeneratedWHReceipts")]
        public async Task<IActionResult> GetGeneratedWHReceipts(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetGeneratedWHReceipts(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Warehouse Receipts  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("DownloadReceipts")]
        public async Task<IActionResult> DownloadReceipts(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DownloadReceipts(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Download Receipt"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("APSWCMapsServiceConsume")]
        public async Task<IActionResult> APSWCMapsServiceConsume(DigiLocker root)
        {
            IActionResult response = Unauthorized();
            string jsondata = JsonConvert.SerializeObject(root);
            try
            {
                return Ok(await _hel.APSWCMapsServiceConsume(root));
            }
            catch (Exception ex)
            {
                response = BadRequest(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Issued Files " + ex.Message

                });

                return response;
            }
        }

        [HttpPost]
        [Route("UpdateLoanDetails")]
        public async Task<IActionResult> UpdateLoanDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateLoanDetailsLogs", "UpdateLoanDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateLoanDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Bank Loan Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateLoanRepaymentDetails")]
        public async Task<IActionResult> UpdateLoanRepaymentDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateLoanRepaymentDetailsLogs", "UpdateLoanRepaymentDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateLoanRepaymentDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Bank Loan Re-payment Details"
                });
                return response;
            }
        }

        #endregion

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

        #region User Access Permission for Mobile

        [HttpPost]
        [Route("GetPageMobDetails")]
        public async Task<IActionResult> GetPageMobDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPageMobDetails(rootobj));
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
        [Route("GetPageMobDetailsByID")]
        public async Task<IActionResult> GetPageMobDetailsByID(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPageMobDetailsByID(rootobj));
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
        [Route("GetPageMobAccessDetails")]
        public async Task<IActionResult> GetPageMobAccessDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPageMobAccessDetails(rootobj));
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
        [Route("Get_DesigPageMobAccessDetails")]
        public async Task<IActionResult> Get_DesigPageMobAccessDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Get_DesigPageMobAccessDetails(rootobj));
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
        [Route("SavePageMobDetails")]
        public async Task<IActionResult> SavePageMobDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SavePageDetailslogs", "SavePageDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SavePageMobDetails(rootobj));
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
        [Route("UpdatePageMobDetails")]
        public async Task<IActionResult> UpdatePageMobDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdatePageDetailslogs", "UpdatePageDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdatePageMobDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Page Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveUserMobaccessDetails")]
        public async Task<IActionResult> SaveUserMobaccessDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveUseraccessDetailslogs", "SaveUseraccessDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveUserMobaccessDetails(rootobj));
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

        [HttpPost]
        [Route("UpdateUserMobaccessDetails")]
        public async Task<IActionResult> UpdateUserMobaccessDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateUserMobaccessDetailslogs", "UpdateUserMobaccessDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateUserMobaccessDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Updateing User access Details"
                });
                return response;
            }
        }


        #endregion

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

        #region employee transfer
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
        #endregion

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

        #region Warehouse Reports
        [HttpPost]
        [Route("GetLorryWBRegister")]
        public async Task<IActionResult> GetLorryWBRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetLorryWBRegister(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Lorry Weighbridge List"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetStockList")]
        public async Task<IActionResult> GetStockList(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetStockList(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Lorry Weighbridge List"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetDailyValuationStock")]
        public async Task<IActionResult> GetDailyValuationStock(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDailyValuationStock(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Daily Valuation Stock"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetOpeningBalance")]
        public async Task<IActionResult> GetOpeningBalance(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetOpeningBalance(rootobj));
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
        [Route("GetImprestRegister")]
        public async Task<IActionResult> GetImprestRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetImprestRegister(rootobj));
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
        [Route("GetFarmersAndCommodites")]
        public async Task<IActionResult> GetFarmersAndCommodites(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetFarmersAndCommodites(rootobj));
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
        [Route("GetMonthsAndYears")]
        public async Task<IActionResult> GetMonthsAndYears(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetMonthsAndYears(rootobj));
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
        [Route("GetDepostCmdtys")]
        public async Task<IActionResult> GetDepostCmdtys(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDepostCmdtys(rootobj));
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
        [Route("GetBankloanRegister")]
        public async Task<IActionResult> GetBankloanRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetBankloanRegister(rootobj));
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
        [Route("GetDailyTransactionRegister")]
        public async Task<IActionResult> GetDailyTransactionRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDailyTransactionRegister(rootobj));
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
        [Route("GetDepositerLedger")]
        public async Task<IActionResult> GetDepositerLedger(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDepositerLedger(rootobj));
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
        [Route("GetWHspillage")]
        public async Task<IActionResult> GetWHspillage(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWHspillage(rootobj));
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
        [Route("GetChemicals")]
        public async Task<IActionResult> GetChemicals(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetChemicals(rootobj));
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
        [Route("GetChemicalConsumption")]
        public async Task<IActionResult> GetChemicalConsumption(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetChemicalConsumption(rootobj));
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


        [HttpPost]
        [Route("DashboardData")]
        public async Task<IActionResult> DashboardData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DashboardDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Dashboard Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("HRDashboardData")]
        public async Task<IActionResult> HRDashboardData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.HRDashboardDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Dashboard Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetPriorityData")]
        public async Task<IActionResult> GetPriorityData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPriorityData(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Priority Register Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetStorageChargesData")]
        public async Task<IActionResult> GetStorageChargesData(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetStorageChargesData(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Storage Charges Details"
                });
                return response;
            }
        }

        #endregion

        #region Periodic Quality Examination

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

        [HttpPost]
        [Route("SaveDisinfestationDetails")]
        public async Task<IActionResult> SaveDisinfestationDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveDisinfestationDetailsLogs", "SaveDisinfestationDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveDisinfestationDetails(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Saving Disinfestation Details"
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
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDisiHistory(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Disinfestation Details"
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
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetQualityiHistory(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Quality Details"
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
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveSpillingDetailsLogs", "SaveSpillingDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveSpillingDetails(rootobj));

            }
            catch (Exception)
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
        [Route("GetSpillageStack")]
        public async Task<IActionResult> GetSpillageStack(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetSpillageStack(rootobj));

            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Stacks Details"
                });
                return response;
            }
        }

        #endregion

        #region SERVICE REGISTRAR SR_LeaveLedger_Save

        [HttpGet]
        [Route("GetMasterLeaveTypes")]
        public async Task<IActionResult> GetMasterLeaveTypes()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetLeaveTypes());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while GetLeaveTypes Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SR_LeaveLedger_Save")]
        public async Task<IActionResult> SR_LeaveLedger_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR LeaveLedger Save  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SR_LeaveLedger_Get")]
        public async Task<IActionResult> SR_LeaveLedger_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR LeaveLedger Get  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SR_EmpTransfer_Save")]
        public async Task<IActionResult> SR_EmpTransfer_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR EmpTransfer Save  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SR_EmpTransfer_Get")]
        public async Task<IActionResult> SR_EmpTransfer_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_EmpTransfer_Get  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SR_Suspention_Save")]
        public async Task<IActionResult> SR_Suspention_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_Suspention_Save  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SR_Suspention_Get")]
        public async Task<IActionResult> SR_Suspention_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_Suspention_Get  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SR_StoppageIncrement_Save")]
        public async Task<IActionResult> SR_StoppageIncrement_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_StoppageIncrement_Save  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SR_StoppageIncrement_Get")]
        public async Task<IActionResult> SR_StoppageIncrement_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_StoppageIncrement_Get  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SR_ReinstatingDuties_Save")]
        public async Task<IActionResult> SR_ReinstatingDuties_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_ReinstatingDuties_Save  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SR_ReinstatingDuties_Get")]
        public async Task<IActionResult> SR_ReinstatingDuties_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_ReinstatingDuties_Get  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SR_EmployeePromotion_Save")]
        public async Task<IActionResult> SR_EmployeePromotion_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_EmployeePromotion_Save  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SR_EmployeePromotion_Get")]
        public async Task<IActionResult> SR_EmployeePromotion_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_EmployeePromotion_Get  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SR_AdditionalCharge_Save")]
        public async Task<IActionResult> SR_AdditionalCharge_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_AdditionalCharge_Save  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SR_AdditionalCharge_Get")]
        public async Task<IActionResult> SR_AdditionalCharge_Get(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_AdditionalCharge_Get  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SR_GETALLLOCATIONS")]
        public async Task<IActionResult> SR_GETALLLOCATIONS(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_GETALLLOCATIONS  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SERVICE_EVENTS_GET")]
        public async Task<IActionResult> SERVICE_EVENTS_GET(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured  SERVICE_EVENTS_GET Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SR_Common_Insert_Save")]
        public async Task<IActionResult> SR_Common_Insert_Save(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_GET_INSERT_COMMON(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while SR_Common_Insert_Save  Details"
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

        #region Digilocker
        [HttpPost]
        [Route("DLAccessToken")]
        public async Task<IActionResult> DLAccessToken(DigiLocker root)
        {
            IActionResult response = Unauthorized();
            string jsondata = JsonConvert.SerializeObject(root);
            try
            {
                return Ok(await _hel.DigiLockerAccessToken(root));
            }
            catch (Exception ex)
            {
                response = BadRequest(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Access Token"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("DLIssuedfiles")]
        public async Task<IActionResult> DLIssuedfiles(DigiLocker root)
        {
            IActionResult response = Unauthorized();
            string jsondata = JsonConvert.SerializeObject(root);
            try
            {
                return Ok(await _hel.DLIssuedfiles(root));
            }
            catch (Exception ex)
            {
                response = BadRequest(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Issued Files"
                });
                return response;
            }
        }

        #endregion

        #region DigiLocker
        [HttpPost]
        [Route("DigiLocker")]
        public async Task<IActionResult> DigiLocker(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DigiLocker(rootobj));
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
        [Route("DLCertfIssuerData")]
        public async Task<IActionResult> DLCertfIssuerData(DigiLocker root)
        {
            IActionResult response = Unauthorized();
            string jsondata = JsonConvert.SerializeObject(root);
            try
            {
                return Ok(await _hel.DLCertfIssuerData(root));
            }
            catch (Exception ex)
            {
                response = BadRequest(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Issued Files"
                });
                return response;
            }
        }

        #endregion


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


        #region Invoice Masters

        [HttpPost]
        [Route("GetInvoiceMasterDetails")]
        public async Task<IActionResult> GetInvoiceMasterDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetInvoiceMasterDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Warehouse Invoice Master Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("Saveinvoicemasterdetails")]
        public IActionResult Saveinvoicemasterdetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("Invoicemasterlogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "Invoicemasterlogs", jsondata));
                return Ok(_hel.SaveInvoiceModeDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("Getcommoditiesgrouplist")]
        public async Task<IActionResult> Getcommoditiesgrouplist(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCommpodityGrouplist(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting CommodityGroup Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetcommoditiesNamelist")]
        public async Task<IActionResult> GetcommoditiesNamelist(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCommpoditynamelist(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting CommodityGroup Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveOverandAboveDetails")]
        public IActionResult SaveOverandAboveDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("SaveOverandAbovelogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "SaveOverandAbovelogs", jsondata));
                return Ok(_hel.SaveOverandAboveDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("updatecommoditypricededtails")]
        public IActionResult updatecommoditypricededtails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("updatecommoditypricelogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "updatecommoditypricelogs", jsondata));
                return Ok(_hel.updatecommodprcDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }


        [HttpPost]

        [Route("GetDepositoryDetails")]
        public async Task<IActionResult> GetDepositoryDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDepositoryDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Warehouse Invoice Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveRebateDetails")]
        public IActionResult SaveRebateDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("RebateDetailslogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "RebateDetailslogs", jsondata));
                return Ok(_hel.SaveRebatepriceDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }

        [HttpPost]

        [Route("GetRebateListDetails")]
        public async Task<IActionResult> GetRebateListDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetrebatelistDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Warehouse Invoice Master Details"
                });
                return response;
            }
        }
        [HttpPost]

        [Route("Getoverandabovedetails")]
        public async Task<IActionResult> Getoverandabovedetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetoverandabvDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Warehouse Invoice Master Details"
                });
                return response;
            }
        }
        [HttpPost]
        [Route("updateRebatededtails")]
        public IActionResult updateRebatededtails(dynamic data)
        {

            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("updateRebatededtailslogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "updateRebatededtailslogs", jsondata));
                return Ok(_hel.updaterebatDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("updateoverandabovedetails")]
        public IActionResult updateoverandabovedetails(dynamic data)
        {

            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("updateoverandabovelogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "updateoverandabovelogs", jsondata));
                return Ok(_hel.updateoverandaboveDetails(rootobj));
            }
            catch (Exception ex)
            {

                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }

        [HttpPost]

        [Route("GetHospitalslist")]
        public async Task<IActionResult> GetHospitalslist(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Getmedicalreimbursement(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Medical Reimbursement Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveHospitalDetails")]
        public IActionResult SaveHospitalDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("SaveHospitalDetailslogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "SaveHospitalDetailslogs", jsondata));
                return Ok(_hel.SaveHospitalsDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("SaveDiseaseDetails")]
        public IActionResult SaveDiseaseDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("SaveDiseaseDetailslogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "SaveDiseaseDetailslogs", jsondata));
                return Ok(_hel.SaveDiseaseDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("updatehospitaldetails")]
        public IActionResult updatehospitaldetails(dynamic data)
        {

            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("updatehospitallogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "updatehospitallogs", jsondata));
                return Ok(_hel.updatehospitallistDetails(rootobj));
            }
            catch (Exception ex)
            {

                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("updateDiseasedetails")]
        public IActionResult updateDiseasedetails(dynamic data)
        {

            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("updateDiseaselogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "updateDiseaselogs", jsondata));
                return Ok(_hel.updateDiseaselistDetails(rootobj));
            }
            catch (Exception ex)
            {

                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Master Details"
                });
                return response;
            }
        }



        [HttpPost]

        [Route("GetHospitalslistbydistcode")]
        public async Task<IActionResult> GetHospitalslistbydistcode(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.GetHospitaldetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Medical Reimbursement Master Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetTreatmenttypebytrtamount")]
        public async Task<IActionResult> GetTreatmenttypebytrtamount(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.Gettreatmentdetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Treatmenttype  Details"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("SaveMedicalRequest")]
        public async Task<IActionResult> SaveMedicalRequest(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveMedicalRequestLogs", "SaveMedicalRequestLogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.SaveMedicalRequest(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Medical Reimbursement Information"
                });
                return response;
            }
        }


        [HttpPost]

        [Route("SaveMedivcalDetails")]
        public async Task<IActionResult> SaveMedivcalDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveMedivcalDetailsLogs", "SaveMedivcalDetailsLogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.Savemedicaldetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Medical Approval Information"
                });
                return response;
            }
        }


        #endregion


        #region Warehouse Invoices

        [HttpPost]
        [Route("GetInvoiceDetails")]
        public async Task<IActionResult> GetInvoiceDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWHInvoiceDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Invoice List"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetCreditorDebitInvoiceDetails")]
        public async Task<IActionResult> GetCreditorDebitInvoiceDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCreditorDebitInvoiceDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Invoice List"
                });
                return response;
            }

        }

        [HttpPost]
        [Route("GetInvoicesList")]
        public async Task<IActionResult> GetInvoicesList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetInvoicesList(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Invoice List"
                });
                return response;
            }

        }

        [HttpPost]
        [Route("GetInvoiceIDDetails")]
        public async Task<IActionResult> GetInvoiceIDDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetInvoiceIDDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Invoice Details"
                });
                return response;
            }

        }

        [HttpPost]
        [Route("SaveDeductionDetails")]
        public async Task<IActionResult> SaveDeductionDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveDeductionDetailsLogs", "SaveDeductionDetails : Input Data : " + value));

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);

                return Ok(await _hel.SaveDeductionDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Deductions Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetDeductionDetails")]
        public async Task<IActionResult> GetDeductionDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDeductionDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Deductions Details"
                });
                return response;
            }

        }
        #endregion


        #region H & T Contractors

        [HttpPost]
        [Route("SaveHTDetails")]
        public async Task<IActionResult> SaveHTDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveHTDetailsLogs", "SaveHTDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveHTDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save H & T Contractors  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateHTDetails")]
        public async Task<IActionResult> UpdateHTDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateHTDetailsLogs", "UpdateHTDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateHTDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update H&T Contractor  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHTDetails")]
        public async Task<IActionResult> GetHTDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHTDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of H&T Contractor"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHTHistoryDetails")]
        public async Task<IActionResult> GetHTHistoryDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHTHistoryDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get History Of H&T Contractor"
                });
                return response;
            }
        }

        #endregion

        #region Medical Reambersment

        [HttpPost]

        [Route("SaveApprovalDetails")]
        public async Task<IActionResult> SaveApprovalDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveApprovalDetailsLogs", "SaveApprovalDetailsLogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.Saveapprovaldetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Approval Information"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetBankdetails")]
        public async Task<IActionResult> GetBankdetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Getbanklist(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting bankaccount Details"
                });
                return response;
            }
        }

        [HttpPost]

        [Route("UpdateMedicalDetails")]
        public async Task<IActionResult> UpdateMedicalDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "RejectMedicalLogs", "RejectMedicalLogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.updatemedicaldetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Medical Approval Information"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("GetFamilyrelation")]
        public async Task<IActionResult> GetGetFamilyrelation(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.Getfamilyrelationdetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Treatmenttype  Details"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("GetFamilynames")]
        public async Task<IActionResult> GetGetFamilynames(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.Getfamilydetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Treatmenttype  Details"
                });
                return response;
            }
        }

        #endregion

        [HttpPost]
        [Route("GetAssetmanagementSubcategoryDetails")]
        public async Task<IActionResult> GetAssetmanagementSubcategoryDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "GetAssetmanagementSubcategoryDetails", "GetAssetmanagementSubcategory : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetAssetmanagementsubcategory(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get AssetmanagementSubcategory Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SERVICE_PROFILE")]
        public async Task<IActionResult> SERVICE_PROFILE(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SR_SERVICE_PROFILE(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured  SERVICE_PROFILE Details"
                });
                return response;
            }
        }

        #region Hamalies Services Details

        [HttpPost]
        [Route("SaveHamaliesServices")]
        public async Task<IActionResult> SaveHamaliesServices(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveHamaliesServicesLogs", "SaveHamaliesServices : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveHamaliesServices(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Hamalies Service  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UpdateHamaliesServices")]
        public async Task<IActionResult> UpdateHamaliesServices(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UpdateHamaliesServicesLogs", "UpdateHamaliesServices : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.UpdateHamaliesServices(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update Hamalies Service  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHamaliesServices")]
        public async Task<IActionResult> GetHamaliesServices(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHamaliesServices(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of Hamalies Services"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHMHistoryDetails")]
        public async Task<IActionResult> GetHMHistoryDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHMHistoryDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get History Of Hamalies Services"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHTContractorsDetails")]
        public async Task<IActionResult> GetHTContractorsDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHTContractorsDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of H&T Contractors Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHTServicesDetails")]
        public async Task<IActionResult> GetHTServicesDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHTServicesDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of H&T Services Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveWHHTMapping")]
        public async Task<IActionResult> SaveWHHTMapping(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveWHHTMappingLogs", "SaveWHHTMapping : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveWHHTMapping(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Warehouse ANd H&T Mapping  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHTMappingDetails")]
        public async Task<IActionResult> GetHTMappingDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHTMappingDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of H&T Mapping Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHTMapHistoryDetails")]
        public async Task<IActionResult> GetHTMapHistoryDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHTMapHistoryDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of H&T Mapping History Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHTServicesList")]
        public async Task<IActionResult> GetHTServicesList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHTServicesList(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of H&T Services List"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetHTServicesPrice")]
        public async Task<IActionResult> GetHTServicesPrice(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHTServicesPrice(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of H&T Services Price"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetWHHTContractors")]
        public async Task<IActionResult> GetWHHTContractors(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetWHHTContractors(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Get Details of Warehouse H&T Contractors Details"
                });
                return response;
            }
        }

        #endregion

        [HttpPost]
        [Route("SaveDeadstockApproveDetails")]
        public async Task<IActionResult> SaveDeadstockApproveDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveDeadstockApprovelogs", "SaveDeadstockApprovelogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DeadStockApproval(rootobj));
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
        [Route("updateDeadstockApproveDetails")]
        public async Task<IActionResult> updateDeadstockApproveDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "updateDeadstockApprovelogs", "updateDeadstockApprovelogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DeadStockupdateApproval(rootobj));
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
        [Route("GetDeadstockhistory1")]
        public async Task<IActionResult> GetDeadstockhistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Historydetails(rootobj));
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

        #region Warehouse Weight Check Memos

        [HttpPost]
        [Route("GetPendingWHMemos")]
        public async Task<IActionResult> GetPendingWHMemos(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetPendingWHMemos(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Warehouse Weight Check Memo  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetCompletedWHMemos")]
        public async Task<IActionResult> GetCompletedWHMemos(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCompletedWHMemos(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Warehouse Weight Check Memo  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SavePendingWHMemosDetails")]
        public async Task<IActionResult> SavePendingWHMemosDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SavePendingWHMemosDetailsLogs", "SavePendingWHMemosDetails , Inpuut Data :  " + jsondata));
                return Ok(await _hel.SavePendingWHMemosDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = "102",
                    StatusMessage = "Error Occured while Save  Save Pending Warehouse Weight Memo Details",

                });
                return response;
            }
        }

        #endregion


        [HttpPost]
        [Route("SaveImprestmaster")]
        public IActionResult SaveImprestmaster(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("imprestmasterLogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "imprestmasterLogs", jsondata));
                return Ok(_hel.SaveImprestparams(rootobj));
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
        [Route("GetDEEODetails")]
        public async Task<IActionResult> GetDEEODetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.DEEOdetails(rootobj));
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
        [Route("UpdateImprestmaster")]
        public IActionResult UpdateImprestmaster(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                var folderName = Path.Combine("UpdateImprestmasterLogs");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "UpdateImprestmasterLogs", jsondata));
                return Ok(_hel.UpdateImprestparams(rootobj));
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

        [Route("SaveServicereqApprovalDetails")]
        public async Task<IActionResult> SaveServicereqApprovalDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveServicereqApprovalLogs", "SaveServicereqApprovalLogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveServicereqapprovaldetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Approval Information"
                });
                return response;
            }
        }

        #region Warehouse Manual Existing Stock Entry

        [HttpPost]
        [Route("GetStocksList")]
        public async Task<IActionResult> GetStocksList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetStocksList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Stcks Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetDepositorList")]
        public async Task<IActionResult> GetDepositorList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDepositorList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Depositor Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetBookingList")]
        public async Task<IActionResult> GetBookingList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetBookingList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Bookings  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetCommodityList")]
        public async Task<IActionResult> GetCommodityList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);

                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCommodityList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Load Commodity  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveExistingStockDetails")]
        public async Task<IActionResult> SaveExistingStockDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string jsondata = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(jsondata);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveExistingStockDetailsLogs", "SaveExistingStockDetails , Inpuut Data :  " + jsondata));
                return Ok(await _hel.SaveExistingStockDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {


                    StatusCode = "102",
                    StatusMessage = "Error Occured while Save Existing Stock Details",

                });
                return response;
            }

        }

        #endregion

        [HttpPost]
        [Route("SaveemployeeloanApproveDetails")]
        public async Task<IActionResult> SaveemployeeloanApproveDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveemployeeloanApprovelogs", "SaveemployeeloanApprovelogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.saveempapprovalApproval(rootobj));
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
        [Route("updateemployeeloanApproveDetails")]
        public async Task<IActionResult> updateemployeeloanApproveDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "updateemployeeloanApprovelogs", "updateemployeeloanApprovelogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.loanapprovalupdateApproval(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while update Data"
                });
                return response;
            }
        }




        [HttpPost]
        [Route("GetCashBook")]
        public async Task<IActionResult> GetCashBook(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetCashBook(rootobj));
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
        [Route("GetHandTReg")]
        public async Task<IActionResult> GetHandTReg(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetHandTReg(rootobj));
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
        [Route("GetInvoiceDepositers")]
        public async Task<IActionResult> GetInvoiceDepositers(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetInvoiceDepositers(rootobj));
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

        [Route("SaveemployeeloanSDetails")]
        public async Task<IActionResult> SaveemployeeloanSDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveemployeeloanSDetailslogs", "SaveemployeeloanSDetailslogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.Saveemployeeloandetais(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save EmployeeLoan Information"
                });
                return response;
            }
        }

        #region Warehouse Reports

        [HttpPost]
        [Route("GetFumigationSprayingRegister")]
        public async Task<IActionResult> GetFumigationSprayingRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetFumigationSprayingRegister(rootobj));
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
        [Route("GetTendersList")]
        public async Task<IActionResult> GetTendersList(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveTendersMasterLogs", "SaveTendersLogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);

                return Ok(await _hel.GetTendersList(rootobj));
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
        [Route("GetGateRegister")]
        public async Task<IActionResult> GetGateRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetGateRegister(rootobj));
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
        [Route("GetReceiptIssueCancellation")]
        public async Task<IActionResult> GetReceiptIssueCancellation(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetReceiptIssueCancellation(rootobj));
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
        [Route("GetGunnyRegister")]
        public async Task<IActionResult> GetGunnyRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetGunnyRegister(rootobj));
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
        [Route("GetDepositorCommodities")]
        public async Task<IActionResult> GetDepositorCommodities(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDepositorCommodities(rootobj));
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
        [Route("GetAnalysisRegister")]
        public async Task<IActionResult> GetAnalysisRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetAnalysisRegister(rootobj));
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
        [Route("GetJointSampleRegister")]
        public async Task<IActionResult> GetJointSampleRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetJointSampleRegister(rootobj));
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
        [Route("GetDeadStockRegister")]
        public async Task<IActionResult> GetDeadStockRegister(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetDeadStockRegister(rootobj));
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
        #endregion

        #region Warehouse construction Engg section
        [HttpPost]

        [Route("SaveFinWHCDetails")]
        public async Task<IActionResult> SaveFinWHCDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveFinWHCDetailsLogs", "SaveFinWHCDetailsLogs : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.SaveWarehousereqdetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WH construction Information"
                });
                return response;
            }
        }

        [HttpPost]

        [Route("GETFinWHCDetails")]
        public async Task<IActionResult> GETFinWHCDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                //Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveFinWHCDetailsLogs", "SaveFinWHCDetailsLogs : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFinWHCDetailsli(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading WH construction Information"
                });
                return response;
            }
        }


        [HttpGet]
        [Route("GetWHRegionslist")]
        public async Task<IActionResult> GetWHRegionslist()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetWHRegionsli());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load Regions"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetFWHDistrictlist")]
        public async Task<IActionResult> GetFWHDistrictlist(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                warehousereq rootobj = JsonConvert.DeserializeObject<warehousereq>(value);
                return Ok(await _hel.GetFWHDistrictsli(rootobj));
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
        [Route("GetFWHRUlist")]
        public async Task<IActionResult> GetFWHRUlist()
        {
            IActionResult response = Unauthorized();
            try
            {
                return Ok(await _hel.GetFWHRUli());
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load RuralUrban"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetFWHMandallist")]
        public async Task<IActionResult> GetFWHMandallist(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                warehousereq rootobj = JsonConvert.DeserializeObject<warehousereq>(value);
                return Ok(await _hel.GetFWHMandalli(rootobj));
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

        #endregion

        #region Warehouse Construction Loan Details

        [HttpPost]
        [Route("SaveFWHCLoanDetails")]
        public async Task<IActionResult> SaveFWHCLoanDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveFWHCLoanDetailsLogs", "SaveFWHCLoanDetailsLogs : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);

                string dd = rootobj.INPUT_01;
                DateTime LApdDate;
                string[] formats = { "dd-MM-yyyy", "yyyy-MM-dd", "dd/MM/yyyy" };

                DateTime.TryParseExact(dd, formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out LApdDate);

                rootobj.INPUT_01 = LApdDate.ToString("yyyy-MM-dd");

                string dd14 = rootobj.INPUT_14;
                DateTime SandDate;

                DateTime.TryParseExact(dd14, formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out SandDate);

                rootobj.INPUT_14 = SandDate.ToString("yyyy-MM-dd");

                string dd34 = rootobj.INPUT_34;
                DateTime CRDDate;

                DateTime.TryParseExact(dd34, formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out CRDDate);

                rootobj.INPUT_34 = CRDDate.ToString("yyyy-MM-dd");

                return Ok(await _hel.SaveFWHCLoandetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WHC Loan Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GETFWHLInstallmentDetails")]
        public async Task<IActionResult> GETFWHLInstallmentDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLInstallmentdetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Fetching data"
                });
                return response;
            }
        }
        [HttpPost]
        [Route("GETFWHLInterestDetails")]
        public async Task<IActionResult> GETFWHLInterestDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLIntrestdetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Fetching data"
                });
                return response;
            }
        }



        #endregion

        #region WareHouse Contractor Master Details
        [HttpPost]
        [Route("SaveFWHCContractorDetails")]
        public async Task<IActionResult> SaveFWHCContractorDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveFWHCLoanDetailsLogs", "SaveFWHCLoanDetailsLogs : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.SaveFWHCContractordetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WHC Contractor Information"
                });
                return response;
            }
        }

        [HttpPost]

        [Route("GETFWHCTDetails")]
        public async Task<IActionResult> GETFWHCTDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                //Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveFinWHCDetailsLogs", "SaveFinWHCDetailsLogs : Input Data : " + value));
                warehousereq rootobj = JsonConvert.DeserializeObject<warehousereq>(value);
                return Ok(await _hel.GETFWHCTDetailsli(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while loading WH construction Information"
                });
                return response;
            }
        }
        #endregion

        #region WareHouse Loan Provided Bank Details MAster
        [HttpPost]
        [Route("SaveFWHBANKMasterDetails")]
        public async Task<IActionResult> SaveFWHBANKMasterDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveFWHBANKMasterDetails", "SaveFWHBANKMasterDetails : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.SaveFWHBANKMasterdetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WHC Bank Information"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GETFWHBANKMasterDetails")]
        public async Task<IActionResult> GETFWHBANKMasterDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHBANKMasterdetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WHC Bank Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GETFWHBANKList")]
        public async Task<IActionResult> GETFWHBANKList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLBANKList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WHC Bank Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GETFWHBANKDetailsList")]
        public async Task<IActionResult> GETFWHBANKDetailsList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLBANKDetailsList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WHC Bank Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GETFWHNOOFQUATERS")]
        public async Task<IActionResult> GETFWHNOOFQUATERS(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLNOQUATERS(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Fetching WHC Bank Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GETFWHREQUESTList")]
        public async Task<IActionResult> GETFWHREQUESTList(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLBANKDetailsList(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WHC Bank Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveFWHLREPAYDETAILS")]
        public async Task<IActionResult> SaveFWHLREPAYDETAILS(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveFWHLREPAYDETAILS", "SaveFWHLREPAYDETAILS : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.SaveFWHBANKMasterdetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WHC Bank Information"
                });
                return response;
            }
        }


        #endregion


        #region WareHouse CT Loan Approval and Reject details

        [HttpPost]
        [Route("GETFWHLREQUESTLIST")]
        public async Task<IActionResult> GETFWHLREQUESTLIST(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                warehousereq rootobj = JsonConvert.DeserializeObject<warehousereq>(value);
                return Ok(await _hel.GETFWHLREQUESTLI(rootobj));
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
        #endregion


        #region WH CT Loan Repay Details
        [HttpPost]
        [Route("GETFWHLDRPLIST")]
        public async Task<IActionResult> GETFWHLDRPLIST(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                warehousereq rootobj = JsonConvert.DeserializeObject<warehousereq>(value);
                return Ok(await _hel.GETFWHLDRPLIST(rootobj));
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
        [Route("GETFWHLSDATALIST")]
        public async Task<IActionResult> GETFWHLSDATALIST(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                warehousereq rootobj = JsonConvert.DeserializeObject<warehousereq>(value);

                string dd = rootobj.INPUT_03;
                DateTime startDate;
                string[] formats = { "dd-MM-yyyy", "yyyy-MM-dd", "dd/MM/yyyy" };

                DateTime.TryParseExact(dd, formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out startDate);

                rootobj.INPUT_03 = startDate.ToString("yyyy-MM-dd");
                return Ok(await _hel.GETFWHLSDATALIST(rootobj));
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
        #endregion

        [HttpPost]
        [Route("GETFWHLoanDetails")]
        public async Task<IActionResult> GETFWHLoanDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLoanDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Fetching data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GETFWHLEMIDetails")]
        public async Task<IActionResult> GETFWHLEMIDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLEMIDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Fetching data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UPDFWHBANKMasterDetails")]
        public async Task<IActionResult> UPDFWHBANKMasterDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UPDFWHBANKMasterDetails", "UPDFWHBANKMasterDetails : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.UPDFWHBANKMasterdetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update WHC Bank Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("WHSaveemployeeloanApproveDetails")]
        public async Task<IActionResult> WHSaveemployeeloanApproveDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "WHSaveemployeeloanApprovelogs", "WHSaveemployeeloanApprovelogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.WHsaveempapprovalApproval(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save the Data"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("WHupdateemployeeloanApproveDetails")]
        public async Task<IActionResult> WHupdateemployeeloanApproveDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "WHupdateemployeeloanApprovelogs", "WHupdateemployeeloanApprovelogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.WHloanapprovalupdateApproval(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while update Data"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetWhhistory")]
        public async Task<IActionResult> GetWhhistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.whHistorydetails(rootobj));
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
        [Route("servicerequestRefunddetails")]
        public async Task<IActionResult> servicerequestRefunddetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "servicerequestRefunddetailslogs", "servicerequestRefunddetailslogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.servicePayrefund(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save the Data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GETFWHLPREEMICHAT")]
        public async Task<IActionResult> GETFWHLPREEMICHAT(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                string dd = rootobj.INPUT_01;
                DateTime startDate;
                string[] formats = { "dd-MM-yyyy", "yyyy-MM-dd", "dd/MM/yyyy" };

                DateTime.TryParseExact(dd, formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out startDate);

                rootobj.INPUT_01 = startDate.ToString("yyyy-MM-dd");
                return Ok(await _hel.GETFWHPREEMICHAT(rootobj));

                //if (rootobj.INPUT_05 == "NIDA")
                //{
                //    return Ok(await _hel.GETFWHPREEMICHAT(rootobj));
                //}
                //else
                //{
                //    return Ok(await _hel.GETFWHPREWIFEMICHAT(rootobj));
                //}
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Fetching data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GETFWHLEMPREQHIST")]
        public async Task<IActionResult> GETFWHLEMPREQHIST(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLEMPREQHIST(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Fetching data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SendOTP")]
        public async Task<IActionResult> SendOTP(dynamic data)
        {

            IActionResult response = Unauthorized();

            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);

                return Ok(await _hel.SendSMS(rootobj));


            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while sending OTP"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveFWHLFDDETAILS")]
        public async Task<IActionResult> SaveFWHLFDDETAILS(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveFWHLFDDETAILS", "SaveFWHLFDDETAILS : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.SaveFWHLFDDETAILS(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save WHC Bank FD Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GETFWHLFDDetails")]
        public async Task<IActionResult> GETFWHLFDDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLFDDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Fetching data"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP(dynamic data)
        {

            IActionResult response = Unauthorized();

            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);

                return Ok(await _hel.VerifyOTPNumber(rootobj));


            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while sending OTP"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("GETFWHLFDMonths")]
        public async Task<IActionResult> GETFWHLFDMonths(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.GETFWHLFDMonths(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Fetching data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UPDFWHLFDDETAILS")]
        public async Task<IActionResult> UPDFWHLFDDETAILS(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UPDFWHLFDDETAILS", "UPDFWHLFDDETAILS : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.UPDFWHLFDDETAILS(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update WHC Bank FD Information"
                });
                return response;
            }
        }


        [HttpPost]
        [Route("GetImprestDetails")]
        public async Task<IActionResult> GetImprestDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.whImprestdetails(rootobj));
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
        [Route("SaveimprestreqDetails")]
        public async Task<IActionResult> SaveimprestreqDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "saveImprestApprovallogs", "saveImprestApprovallogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.saveImprestApproval(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save the Data"
                });
                return response;
            }
        }



        [HttpPost]
        [Route("updateimprestapproval")]
        public async Task<IActionResult> updateimprestapproval(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "updateimprestapprovallogs", "updateimprestapprovallogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.updateimprestapproval(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while update Data"
                });
                return response;
            }
        }


        [HttpPost]

        [Route("SaveimprestApprovalDetails")]
        public async Task<IActionResult> SaveimprestApprovalDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveimprestApprovalDetailsLogs", "SaveimprestApprovalDetailsLogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Saveimprestpprovaldetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save Approval Information"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("SaveimprestpushbackDetails")]
        public async Task<IActionResult> Saveimprestdetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "Saveimprestpushbacklogs", "Saveimprestpushbacklogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.savepushbackImprestApproval(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save the Data"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("GetIMPRESThistory")]
        public async Task<IActionResult> GetIMPRESThistory(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.Historydetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Getting History details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("Getregionsections")]
        public async Task<IActionResult> Getregionsections(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {

                string value = JsonConvert.SerializeObject(data);
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.GetregsecDetails(rootobj));
            }
            catch (Exception)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while load  Details"
                });
                return response;
            }
        }

        [HttpPost]
        [Route("UPDFWHCONTRDETAILS")]
        public async Task<IActionResult> UPDFWHCONTRDETAILS(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "UPDFWHCONTRDETAILS", "UPDFWHCONTRDETAILS : Input Data : " + value));
                WHLoancl rootobj = JsonConvert.DeserializeObject<WHLoancl>(value);
                return Ok(await _hel.UPDFWHCONTRDETAILS(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {
                    StatusCode = 102,
                    StatusMessage = "Error Occured while Update WH Contractor Information"
                });
                return response;
            }
        }


        [HttpPost]

        [Route("SaveSendDetails")]
        public async Task<IActionResult> SaveSendDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SendDetailslogs", "SendDetailslogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.SendDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save the Details"
                });
                return response;
            }
        }


        [HttpPost]

        [Route("SubmitCancelDetails")]
        public async Task<IActionResult> SubmitCancelDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "CancelDetailslogs", "CancelDetailslogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.CancelDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save the Information"
                });
                return response;
            }
        }





        [HttpPost]

        [Route("SavePushbackDetails")]
        public async Task<IActionResult> SavePushbackDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SavePushbacklogs", "SavePushbacklogs : Input Data : " + value));
                EmployeeMasterSp rootobj = JsonConvert.DeserializeObject<EmployeeMasterSp>(value);
                return Ok(await _hel.pushbackDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save the Information"
                });
                return response;
            }
        }


        [HttpPost]

        [Route("SaveserviceSendDetails")]
        public async Task<IActionResult> SaveserviceSendDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveserviceSendlogs", "SaveserviceSendlogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.serviceSendDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Send the Details"
                });
                return response;
            }
        }


        [HttpPost]

        [Route("SubmitserviceCancelDetails")]
        public async Task<IActionResult> SubmitserviceCancelDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "serviceCancelDetailslogs", "serviceCancelDetailslogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.serviceCancelDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while Save the Information"
                });
                return response;
            }
        }





        [HttpPost]

        [Route("SaveservicePushbackDetails")]
        public async Task<IActionResult> SaveservicePushbackDetails(dynamic data)
        {
            IActionResult response = Unauthorized();
            try
            {
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveservicePushbacklogs", "SaveservicePushbacklogs : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.servicepushbackDetails(rootobj));
            }
            catch (Exception ex)
            {
                response = Ok(new
                {

                    StatusCode = 102,
                    StatusMessage = "Error Occured while pushback the Information"
                });
                return response;
            }
        }

    }

}
