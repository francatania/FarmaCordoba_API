﻿using FarmaceuticaBack.Data.Models;
using FarmaceuticaBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Data.Contracts
{
    public interface IMedicamentoRepository
    {
        Task<List<MedicamentoDTO>> GetAll();
        Task<Medicamento> GetMedicamentoById(int id);
        Task<List<Medicamento>> GetByFiltro(MedicamentoFiltro oFiltro);
        Task<bool> Save(Medicamento oMedicamento);
        Task<bool> Delete(int? id);
        Task<bool> Update(Medicamento oMedicamento);

        Task<int> GetLastId();

    }
}
