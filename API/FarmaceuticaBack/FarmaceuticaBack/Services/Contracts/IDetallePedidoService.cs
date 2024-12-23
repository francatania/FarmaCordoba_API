﻿using FarmaceuticaBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Services.Contracts
{
    public interface IDetallePedidoService
    {
        Task<List<DetallesPedido>> GetByPedido(int id);
        Task<DetallesPedido> GetByDetallePedido(int idPedido, int idDetalleP);
        Task<bool> Delete(int idPedido, int idDetalleP);
        Task<bool> Save(DetallesPedido dp);
    }
}
