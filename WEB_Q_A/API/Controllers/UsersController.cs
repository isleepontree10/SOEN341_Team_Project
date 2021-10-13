using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using System;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly IConfiguration _config;
        private readonly SignInManager<ApplicationUser> SignInManager;

        public UsersController(UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> SignInManager, IConfiguration config)
        {
            this.UserManager = UserManager;
            this.SignInManager = SignInManager;
            this._config = config;
        }
        [HttpGet]
        [Route("id")]
        //Get = api/users/id
        //Example = api/users/a0ec2c4f-d9ff-4627-b248-fb2f590148cd
        public async Task<object> GetUsers(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            ClientUser c1 = new ClientUser();
            c1.username = user.UserName;
            return c1;
        }
        [HttpGet]
        [Route("Logout")]
        public async void Logout()
        {
            await HttpContext.SignOutAsync();
        
        }
        [HttpPost]
        [Route("Login")]
        //Post request for login 
        //api/users/Login
        public async Task<Object> LogIn(LoginData model)
        {
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await SignInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                
                return BadRequest(result);
            }
            
            var claims = new List<Claim>();
            claims.Add(new Claim("username", model.UserName));
            claims.Add(new Claim("email", user.Email));
            claims.Add(new Claim("FirstName", user.FirstName));
            claims.Add(new Claim("LastName", user.LastName));
            claims.Add(new Claim("DateJoined", user.DateJoined));
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);
            await HttpContext.SignInAsync(claimPrincipal);
            return Ok(new
            {
                result = result
            }); ; ; ; ;; ; ; ;
        }
        [HttpPost]
        [Route("Register")]
        //POST = api/users/Register
        public async Task<object> PostApplicationUser(AppUser model) {
            var applicationUser = new ApplicationUser() {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateJoined = model.DateJoined
            };
            try
            {
                var result = await UserManager.CreateAsync(applicationUser, model.Password);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }


    }
}