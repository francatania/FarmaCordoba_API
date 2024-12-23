﻿using FarmaceuticaBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Services.Contracts
{
    public interface IMedicamentoLoteService
    {
        Task<List<MedicamentosLote>> GetAll();

        Task<bool> Add(MedicamentosLote oMedicamento);
        Task<int> GetLastId();

        Task<bool> Delete(int id);

    }
}
