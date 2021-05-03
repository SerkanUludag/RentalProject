using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Concrete.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WepAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin);
            }

            var result = _authService.CreateAccessToken(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authService.UserExists(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists);
            }

            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getloggeduser")]
        public IActionResult GetLoggedUser(string email)
        {
            var user = _userService.GetByMail(email);
            UserInformationDto userInformation = new UserInformationDto { UserId = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
            if (user != null)
            {
                return Ok(userInformation);
            }
            return BadRequest("Wrong email");
        }

        [HttpPost("updateuser")]
        public IActionResult UpdateUser(UserInformationDto userInfo)
        {
            var userToUpdate = _userService.GetByMail(userInfo.Email);
            userToUpdate.FirstName = userInfo.FirstName;
            userToUpdate.LastName = userInfo.LastName;
            userToUpdate.Email = userInfo.Email;
            _userService.Update(userToUpdate);
            return Ok(new SuccessResult("Updated"));
        }

        [HttpPost("updatepassword")]
        public IActionResult UpdatePassword([FromForm(Name = "password")] string password, [FromForm(Name = "email")] string email)
        {
            _userService.UpdatePassword(password, email);
            return Ok(new SuccessResult("Password changed"));
            
        }
    }
}
