using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_DAL.Models
{
    public class User : IdentityUser
    {
        public string ImageUrl { get; set; } = "NOT FOUND";
    }
}
