using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.DTO
{
    public class ImageForUploadViewModel
    {
        public IFormFile? File { get; set; }
    }
}
