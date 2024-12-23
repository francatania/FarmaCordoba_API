﻿using FarmaceuticaBack.Data.Models;
using FarmaceuticaBack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Data.Contracts
{
    public interface IInventarioRepository
    {
        Task<List<Inventario>> GetAll();


        Task<List<Inventario>> GetInventarioByFactura(int idFactura, DateTime from, DateTime to);

        Task<List<Inventario>> GetInventarioByPedido(int idPedido, DateTime from, DateTime to);

        Task<List<Inventario>> GetInventarioByFilter(InventarioFiltro oFiltro);

        Task<bool> CreateInventario(Inventario inv);

        Task<List<TiposMovimiento>> GetAllMovements();

    }
}
