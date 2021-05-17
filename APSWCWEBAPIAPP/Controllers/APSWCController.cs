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
        [Route("SaveWeighBridgeDetails")]
        public async Task<IActionResult> SaveWeighBridgeDetails(dynamic data)
        {
            IActionResult response = Unauthorized();

            try
            {
                //string value = EncDecrpt.Decrypt_Data(data);
                string value = JsonConvert.SerializeObject(data);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(saPathToSave, "SaveWeighBridgeDetailslogs", "SaveWareHouseDetails : Input Data : " + value));
                MasterSp rootobj = JsonConvert.DeserializeObject<MasterSp>(value);
                return Ok(await _hel.SaveWeighBridgeDetails(rootobj));
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

    }

}