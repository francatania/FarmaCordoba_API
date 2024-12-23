﻿using FarmaceuticaBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Services.Contracts
{
    public interface IReporteMensualObraSocialService
    {
        Task<List<VReporteMensualObraSocial>> GetAll();

        Task<List<VReporteMensualObraSocial>> GetByFilters(string? OS, int? year, int? month);
    }
}