﻿using FarmaceuticaBack.Data.Contracts;
using FarmaceuticaBack.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Data.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly FarmaceuticaContext _context;
        public StockRepository(FarmaceuticaContext context)
        {
            this._context = context;
        }
        public async Task<bool> Add(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Stock>> GetByEstablishment(int id)
        {
            List<Stock> stocks =await _context.Stocks
                .Include(s => s.IdMedicamentoLoteNavigation)
                .Include(s => s.IdMedicamentoLoteNavigation.IdMedicamentoNavigation)
                .Include(s => s.IdMedicamentoLoteNavigation.IdMedicamentoNavigation.IdPresentacionNavigation)
                .Include(s => s.IdProductoNavigation)
                .Where(s => s.IdEstablecimiento == id)
                .ToListAsync();
            List<Stock> result = new List<Stock>();
            foreach (Stock s in stocks)
            {
                Stock stockAppend = s;
                foreach(Stock stk in stocks)
                {
                    if (stk.IdStock != s.IdStock && stk.IdProducto == s.IdProducto && stk.IdMedicamentoLote == s.IdMedicamentoLote)
                        stockAppend = (stk.Fecha > s.Fecha) ? stk : s;
                }
                result.Add(stockAppend);
            }
            return result;
        }

        public async Task<List<Stock>> GetByEstablishmentAndArticle(int id, string? product, string? medicine)
        {
            List<Stock> stocks;
            if (medicine == null && product != null)
            {
                stocks = await _context.Stocks
                .Include(p => p.IdProductoNavigation)
                .Where(s => s.IdEstablecimiento == id && s.IdProductoNavigation.Nombre.Contains(product))
                .ToListAsync();
            } else if (medicine != null && product == null)
            {
                stocks = await _context.Stocks
                .Include(p => p.IdMedicamentoLoteNavigation)
                .Include(m => m.IdMedicamentoLoteNavigation.IdMedicamentoNavigation)
                .Where(s => s.IdEstablecimiento == id && s.IdProductoNavigation.Nombre.Contains(medicine))
                .ToListAsync();
            }
            else
            {
                stocks = await _context.Stocks
                .Where(s => s.IdEstablecimiento == id)
                .ToListAsync();
            }
            List<Stock> result = new List<Stock>();
            foreach (Stock s in stocks)
            {
                Stock stockAppend = s;
                foreach (Stock stk in stocks)
                {
                    if (stk.IdStock != s.IdStock && stk.IdProducto == s.IdProducto && stk.IdMedicamentoLote == s.IdMedicamentoLote)
                        stockAppend = (stk.Fecha > s.Fecha) ? stk : s;
                }
                result.Add(stockAppend);
            }
            return result;
        }

        public async Task<List<Stock>> GetStockLotesByEstablishment(int id)
        {
            List<Stock> stocks = await _context.Stocks
                         .Include(s => s.IdMedicamentoLoteNavigation)
                         .Include(s => s.IdMedicamentoLoteNavigation.IdMedicamentoNavigation)
                         .Include(s => s.IdMedicamentoLoteNavigation.IdMedicamentoNavigation.IdPresentacionNavigation)
                         .Where(s => s.IdEstablecimiento == id && s.IdProducto == null)
                         .ToListAsync();
            List<Stock> result = new List<Stock>();
            foreach (Stock s in stocks)
            {
                Stock stockAppend = s;
                foreach (Stock stk in stocks)
                {
                    if (stk.IdStock != s.IdStock && stk.IdProducto == s.IdProducto && stk.IdMedicamentoLote == s.IdMedicamentoLote)
                        stockAppend = (stk.Fecha > s.Fecha) ? stk : s;
                }
                result.Add(stockAppend);
            }
            return result;
        }

        public async Task<List<Stock>> GetStockLotesByEstablishmentAndFilter(int id, string medicamento, string lote, bool active)
        {
            IQueryable<Stock> query = _context.Stocks
                                               .Include(s => s.IdMedicamentoLoteNavigation)
                                               .Include(s => s.IdMedicamentoLoteNavigation.IdMedicamentoNavigation)
                                               .Where(s => s.IdEstablecimiento == id && s.IdProducto == null);

            if (!string.IsNullOrEmpty(medicamento))
            {
                query = query.Where(s => s.IdMedicamentoLoteNavigation.IdMedicamentoNavigation.NombreComercial.Contains(medicamento));
            }

            if (!string.IsNullOrEmpty(lote))
            {
                query = query.Where(s => s.IdMedicamentoLoteNavigation.Lote.Contains(lote));
            }

            query = query.Where(x => x.IdMedicamentoLoteNavigation.Activo == active);

            List<Stock> stocks = await query.ToListAsync();

            List<Stock> result = new List<Stock>();
            foreach (Stock s in stocks)
            {
                Stock stockAppend = s;
                foreach (Stock stk in stocks)
                {
                    if (stk.IdStock != s.IdStock && stk.IdProducto == s.IdProducto && stk.IdMedicamentoLote == s.IdMedicamentoLote)
                        stockAppend = (stk.Fecha > s.Fecha) ? stk : s;
                }
                result.Add(stockAppend);
            }
            return result;
        }

        public async Task<bool> Update(Stock stock)
        {
            _context.Stocks.Update(stock);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Stock>> GetAllStockLotesByEstablishmentAndFilter(int establecimiento, int medicamento, int producto)
        {
            IQueryable<Stock> query = _context.Stocks
                                               .Include(s => s.IdMedicamentoLoteNavigation)
                                               .Include(s => s.IdMedicamentoLoteNavigation.IdMedicamentoNavigation)
                                               .Include(s => s.IdProductoNavigation)
                                               .Include(s => s.IdEstablecimientoNavigation)
                                               .Where(s => s.IdEstablecimiento == establecimiento);

            if (medicamento > 0)
            {
                query = query.Where(s => s.IdMedicamentoLoteNavigation != null &&
                                         s.IdMedicamentoLoteNavigation.IdMedicamentoNavigation != null &&
                                         s.IdMedicamentoLoteNavigation.IdMedicamentoNavigation.IdMedicamento == medicamento);
            }

            if (producto > 0)
            {
                query = query.Where(s => s.IdProductoNavigation != null &&
                                         s.IdProductoNavigation.IdProducto == producto);
            }

            var stocks = await query.ToListAsync();
            var resultado = new List<Stock>();

            foreach (var stock in stocks)
            {
                var existente = resultado.FirstOrDefault(r =>
                    (stock.IdProducto != null && r.IdProducto == stock.IdProducto) ||
                    (stock.IdMedicamentoLoteNavigation != null && stock.IdMedicamentoLoteNavigation.IdMedicamento > 0 &&
                     r.IdMedicamentoLoteNavigation != null && r.IdMedicamentoLoteNavigation.IdMedicamento == stock.IdMedicamentoLoteNavigation.IdMedicamento)
                );

                if (existente != null)
                {
                    if (stock.Fecha > existente.Fecha)
                    {
                        resultado.Remove(existente);
                        resultado.Add(stock);
                    }
                }
                else
                {
                    resultado.Add(stock);
                }
            }

            return resultado;
        }


    }
}
