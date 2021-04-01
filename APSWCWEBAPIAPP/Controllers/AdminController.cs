using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APSWCWEBAPIAPP.DBConnection;
using APSWCWEBAPIAPP.Models;
using APSWCWEBAPIAPP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ModelService;
using Newtonsoft.Json;

namespace APSWCWEBAPIAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   //[Authorize(Policy = Policies.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SqlCon _sqlrepository;
        private List<InspectionModel> InsUsers = new List<InspectionModel>
        {

            new InspectionModel {   WarehouseId = "VZ01101", WarehouseName = "Amdalavalasa -I (Own)",
            Description = "quanity checking", uploadeddate = DateTime.Now,FilePath="Inspection\\Images\\6.jpg" },
            new InspectionModel {   WarehouseName= "Amdalavalasa -Ii (Own-PEG)",
            WarehouseId= "VZ01102", Description = "building checking", uploadeddate = DateTime.Now,FilePath="Inspection\\Images\\6.jpg" },
        };

        public AdminController(IConfiguration config,SqlCon sqlcon)
        {
            _config = config;
            _sqlrepository = sqlcon;

        }

        [HttpGet]
        [Route("GetapswcMaster")]
        public async Task<ResponseStatusInfoModel> GetapswcMaster()
        {
            ResponseStatusInfoModel obj = new ResponseStatusInfoModel();
            obj.StatusCode = System.Net.HttpStatusCode.OK;
            obj.Message = "Data Success";
            obj.DataList= await _sqlrepository.GetApswcWareHouseMaster();

            return obj;
        }
        [HttpGet]
        [Route("GetBoardofDirectorMaster")]
        public async Task<ResponseStatusInfoModel> GetBoardofDirectorMaster()
        {
            ResponseStatusInfoModel obj = new ResponseStatusInfoModel();
            obj.StatusCode = System.Net.HttpStatusCode.OK;
            obj.Message = "Data Success";
            obj.DataList = await _sqlrepository.GetBoardofDirectors();

            return obj;
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
        public IActionResult InspectionRegistration([FromBody] InspectionModel Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("InspectionLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "InspectionRegistrationlogs", jsondata));


            response = Ok(new
            {
                StatusCode = 100,
                StatusMessage = "Data Submitted Succssfully"
            });
            return response;
        }

        [HttpPost]
        [Route("GalleryRegistration")]
        public IActionResult GalleryRegistration([FromBody] InspectionModel Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("GalleryLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "GalleryRegistrationlogs", jsondata));


            response = Ok(new
            {
                StatusCode = 100,
                StatusMessage = "Data Submitted Succssfully"
            });
            return response;
        }

        [HttpPost]
        [Route("TendersRegistration")]
        public IActionResult TendersRegistration([FromBody] InspectionModel Ins)
        {
            IActionResult response = Unauthorized();
            var folderName = Path.Combine("TenderLogs");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
            string jsondata = JsonConvert.SerializeObject(Ins);
            Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(pathToSave, "TendersRegistrationlogs", jsondata));


            response = Ok(new
            {
                StatusCode = 100,
                StatusMessage = "Data Submitted Succssfully"
            });
            return response;
        }


    }
}