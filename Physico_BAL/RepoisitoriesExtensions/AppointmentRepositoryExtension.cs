using Microsoft.EntityFrameworkCore;
using Physico_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Physico_BAL.RepoisitoriesExtensions
{
    public static class AppointmentRepositoryExtension
    {
        public static IQueryable<Appointment>Search(
            this IQueryable<Appointment> appointment , string searchTerm , DateOnly date)
        {
            var result = appointment;
            if (!string.IsNullOrEmpty(date.ToString("yyyy-MM-dd")))
            {
                result = result.Where(a => a.Date == date);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerCaseTerm = searchTerm.Trim().ToLower();
                result = result.Where(a => a.PatientName.Contains(searchTerm) || a.PatientEmail.Contains(searchTerm));
            }
            return result;
        }
    }
}
