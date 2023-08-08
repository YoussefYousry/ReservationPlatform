using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Physico_BAL.Contracts;
using Physico_BAL.DTO;
using Physico_DAL.Data;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Repoisitories
{
    public class DoctorRepoistory : RepositoryBase<Doctor> , IDoctorRepoistory
    {
        private readonly IFilesManager _filesManager;
        private readonly IMapper _mapper;
        public DoctorRepoistory(AppDbContext context , IFilesManager filesManager , IMapper mapper) :base(context)
        {
                _filesManager = filesManager;
                _mapper = mapper;
        }
        public async Task<DoctorDto?> GetDoctorById(string doctorId, bool trackChanges)
            => await FindByCondition(d => d.Id == doctorId, trackChanges)
            .ProjectTo<DoctorDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        public async Task<IEnumerable<DoctorDto?>> GetAllDoctors(bool trackChanges)
            => await FindAll(trackChanges)
            .ProjectTo<DoctorDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
