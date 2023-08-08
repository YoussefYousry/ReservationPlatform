using AutoMapper;
using Physico_BAL.DTO;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<DoctorForRegistrationDto, Doctor>();
            CreateMap<UserForLoginDto , User>();
            CreateMap<Doctor, DoctorDto>();
        }
    }
}
