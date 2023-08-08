using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Physico_BAL.Contracts;
using Physico_BAL.DTO;
using Physico_DAL.Models;

namespace Physico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IFilesRepository _filesRepository;

        public DoctorsController(IRepositoryManager repositoryManager, IMapper mapper, IFilesRepository filesRepository)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _filesRepository = filesRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _repositoryManager.Doctor.GetAllDoctors(trackChanges: false);
            if (doctors is null)
            {
                return NotFound("There Are no Doctors in the system yet!");
            }
            var doctorsToReturn = _mapper.Map<IEnumerable<DoctorDto>>(doctors);
            return Ok(doctorsToReturn);
        }
        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetDoctor(string doctorId)
        {
            var doctor = await _repositoryManager.Doctor.GetDoctorById(doctorId,trackChanges: false);
            if (doctor is null)
            {
                return NotFound($"Doctor with ID {doctorId} doesn't exist in the database");
            }
            return Ok(doctor);
        }
        [HttpPost("UploadImage"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadImage([FromForm] ImageForUploadViewModel img, string userId)
        {
            var doctor = await _repositoryManager.Doctor.GetDoctorById(userId, trackChanges: false);
            if (doctor is null)
            {
                return NotFound($"Doctor with ID: {userId} doesn't exist in the database ");
            }
            _filesRepository.UploadImageToUser(userId, img.File!);
            await _repositoryManager.SaveChangesAsync();
            return StatusCode(201, "Image Uploaded Successfully");
        }
        [HttpGet("Image/{userId}")]
        public async Task<IActionResult> GetImage(string userId)
        {
            try
            {
                FileStream file = await _filesRepository.GetImageToUser(userId);
                return new FileStreamResult(file, "image/png");
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest($"User doesn't have an Image : {ex}");
            }

        }

    }
}
