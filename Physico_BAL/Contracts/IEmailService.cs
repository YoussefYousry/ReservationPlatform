﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physico_BAL.Contracts
{
    public interface IEmailService
    {
        Task SendReservationEmail(string email, string name, TimeSpan time, DateOnly date);
    }
}
