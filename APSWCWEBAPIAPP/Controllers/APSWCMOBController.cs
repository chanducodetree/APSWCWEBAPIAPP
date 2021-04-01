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

namespace APSWCWEBAPIAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APSWCMOBController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly SqlCon _hel;
        private ApplicationAPSWCCDbContext _context;
        private readonly ICaptchaService _authservice;
        public APSWCMOBController(ApplicationAPSWCCDbContext apcontext, IConfiguration config, ICaptchaService auth, SqlCon hel)
        {
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
                    reason=""
                }); 
            }
            else
            {
                response = Ok(new 
                    {
                    statusCode=102,
                    reason="Invalid Username and Password"
                });
            }
            return response;
        }
    }
}