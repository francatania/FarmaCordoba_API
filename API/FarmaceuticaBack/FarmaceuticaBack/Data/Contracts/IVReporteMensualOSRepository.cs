﻿using FarmaceuticaBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Data.Contracts
{
    public interface IVReporteMensualOSRepository
    {
        Task<List<VReporteMensualObraSocial>> GetAll();

        Task<List<VReporteMensualObraSocial>> GetByFilter(string os, int year, int month);
    }
}
