using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Physico_BAL.Contracts;
using Physico_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Repoisitories
{
    public class FilesRepository : IFilesRepository
    {
        private readonly AppDbContext _context;
        private readonly IFilesManager _filesManager;
        private readonly IMapper _mapper;

        public FilesRepository(AppDbContext context, IFilesManager filesManager, IMapper mapper)
        {
            _context = context;
            _filesManager = filesManager;
            _mapper = mapper;
        }

        public void UploadImageToUser(string userId, IFormFile img)
        {
            var url = _filesManager.UploadFiles(img);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            user!.ImageUrl = url;
        }
        public async Task<FileStream> GetImageToUser(string userId)
        {
            var img = await _context.Users.AsNoTracking().Where(u => u.Id.Equals(userId)).Select(u => u.ImageUrl).FirstOrDefaultAsync();
            return _filesManager.GetFile(img!);
        }
    }
}
