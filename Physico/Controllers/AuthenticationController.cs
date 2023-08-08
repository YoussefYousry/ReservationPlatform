using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Physico_BAL.Contracts;
using Physico_BAL.DTO;
using Physico_DAL.Models;

namespace Physico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<Doctor> _doctorManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthenticationController(UserManager<Doctor> doctorManager, IAuthService authService, IMapper mapper)
        {
            _doctorManager = doctorManager;
            _authService = authService;
            _mapper = mapper;
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] DoctorForRegistrationDto registrationDto)
        {
            var user = _mapper.Map<Doctor>(registrationDto);
            var result = await _doctorManager.CreateAsync(user, registrationDto.Password!);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors) {
                ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _doctorManager.AddToRoleAsync(user, "Doctor");
            var doctor = await _doctorManager.FindByNameAsync(user.UserName!);
            return Ok(new { DoctorId = await _doctorManager.GetUserIdAsync(doctor!) });
        }
        [HttpPost("Login")]

        public async Task<IActionResult> Authenticate([FromBody] UserForLoginDto user)
        {
            if (!await _authService.ValidateUser(user))
                return Unauthorized();


            var doctor = await _doctorManager.FindByEmailAsync(user.Email!);
            var token = await _authService.CreateToken();
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(20),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("DoctorId", doctor!.Id, cookieOptions);
            Response.Cookies.Append("Token", token, cookieOptions);
            return Ok(
            new
            {
                Token = token,
                UserId = await _doctorManager.GetUserIdAsync(doctor!)
            }
            );
        }
    }
}
