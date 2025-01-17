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
        Task<MedicamentoDTO> GetMedicamentoById(int id);
        Task<MedicamentoSaveDTO> GetMedicamentoSaveDTOById(int id);
        Task<List<MedicamentoDTO>> GetByFiltro(MedicamentoFiltro oFiltro);
        Task<bool> Save(MedicamentoSaveDTO oMedicamento);
        Task<bool> Delete(int? id);
        Task<bool> Update(MedicamentoSaveDTO oMedicamento);

        Task<int> GetLastId();

    }
}
