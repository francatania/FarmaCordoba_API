using FarmaceuticaBack.Data.Contracts;
using FarmaceuticaBack.Data.Models;
using FarmaceuticaBack.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Data.Repositories
{
    public class MedicamentoRepository : IMedicamentoRepository
    {
        private readonly FarmaceuticaContext _context;
        public MedicamentoRepository(FarmaceuticaContext context)
        {
            _context = context;
        }
        public async Task<bool> Delete(int? id)
        {
            var oMedicamento = await _context.Medicamentos.FindAsync(id);
            if (oMedicamento != null)
            {
                oMedicamento.Activo = false;
                _context.Update(oMedicamento);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        public async Task<List<MedicamentoDTO>> GetAll()
        {
            var List = await _context.Medicamentos
                            .Include(m => m.IdLaboratorioNavigation)
                            .Include(m=> m.IdMonodrogaNavigation)
                            .Include(m => m.IdMarcaNavigation)
                            .Include(m => m.IdPresentacionNavigation)
                            .Select(m => new MedicamentoDTO { 
                                IdMedicamento = m.IdMedicamento,
                                NombreMedicamento = m.NombreComercial,
                                Laboratorio = m.IdLaboratorioNavigation.NombreLaboratorio,
                                Marca = m.IdMarcaNavigation.NombreMarca,
                                Presentacion = m.IdPresentacionNavigation.NombrePresentacion,
                                Monodroga = m.IdMonodrogaNavigation.Monodroga1,
                                Descripcion = m.Descripcion,
                                VentaLibre = m.VentaLibre,
                                Activo = m.Activo,
                                Precio = m.Precio
        
                            })
                            .ToListAsync();
            return List;
        }

        public async Task<List<MedicamentoDTO>> GetByFiltro(MedicamentoFiltro oFiltro)
        {
            IQueryable<Medicamento> query = _context.Medicamentos.AsQueryable();

            if (oFiltro.IdLaboratorio != 0)
            {
                query = query.Where(x => x.IdLaboratorio == oFiltro.IdLaboratorio);
            }
            if (oFiltro.IdMarca != 0)
            {
                query = query.Where(x => x.IdMarca == oFiltro.IdMarca);
            }
            if(oFiltro.IdMonodroga != 0)
            {
                query = query.Where(x => x.IdMonodroga == oFiltro.IdMonodroga);
            }
            if (oFiltro.IdPresentacion != 0)
            {
                query = query.Where(x => x.IdPresentacion == oFiltro.IdPresentacion);
            }

            if(oFiltro.NombreComercial != null)
            {
                query = query.Where(x => x.NombreComercial.Contains(oFiltro.NombreComercial));
            }

         
            
                query = query.Where(x => x.Activo == oFiltro.Activo);
           

            return await query.Include(m => m.IdLaboratorioNavigation)
                              .Include(m => m.IdMarcaNavigation)
                              .Include(m => m.IdMonodrogaNavigation)
                              .Include(m => m.IdPresentacionNavigation)
                              .Select(m => new MedicamentoDTO
                               {
                               IdMedicamento = m.IdMedicamento,
                               NombreMedicamento = m.NombreComercial,
                               Laboratorio = m.IdLaboratorioNavigation.NombreLaboratorio,
                               Marca = m.IdMarcaNavigation.NombreMarca,
                               Presentacion = m.IdPresentacionNavigation.NombrePresentacion,
                               Monodroga = m.IdMonodrogaNavigation.Monodroga1,
                               Descripcion = m.Descripcion,
                               VentaLibre = m.VentaLibre,
                               Activo = m.Activo,
                               Precio = m.Precio
                               })
                              .ToListAsync();
        }

        public async Task<int> GetLastId()
        {
            var lastId = await _context.Medicamentos
                .OrderByDescending(m => m.IdMedicamento)
                .Select(m => m.IdMedicamento)            
                .FirstOrDefaultAsync();       

                 return lastId;
        }

        public async Task<MedicamentoDTO> GetMedicamentoById(int id)
        {
            return await _context.Medicamentos
                                 .Include(m => m.IdLaboratorioNavigation)
                                .Include(m => m.IdMonodrogaNavigation)
                                .Include(m => m.IdMarcaNavigation)
                                .Include(m => m.IdPresentacionNavigation)
                               .Select(m => new MedicamentoDTO
                               {
                               IdMedicamento = m.IdMedicamento,
                               NombreMedicamento = m.NombreComercial,
                               Laboratorio = m.IdLaboratorioNavigation.NombreLaboratorio,
                               Marca = m.IdMarcaNavigation.NombreMarca,
                               Presentacion = m.IdPresentacionNavigation.NombrePresentacion,
                               Monodroga = m.IdMonodrogaNavigation.Monodroga1,
                               Descripcion = m.Descripcion,
                               VentaLibre = m.VentaLibre,
                               Activo = m.Activo,
                               Precio = m.Precio
                               })
                                .Where(m => m.IdMedicamento == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<MedicamentoSaveDTO> GetMedicamentoSaveDTOById(int id)
        {
            return await _context.Medicamentos
                               .Select(m => new MedicamentoSaveDTO
                               {
                                   IdMedicamento = m.IdMedicamento,
                                   NombreMedicamento = m.NombreComercial,
                                   IdLaboratorio = m.IdLaboratorio,
                                   IdMarca = m.IdMarca,
                                   IdPresentacion = m.IdPresentacion,
                                   IdMonodroga = m.IdMonodroga,
                                   Descripcion = m.Descripcion,
                                   VentaLibre = m.VentaLibre,
                                   Activo = m.Activo,
                                   Precio = m.Precio
                               })
                                .Where(m => m.IdMedicamento == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<bool> Save(MedicamentoSaveDTO oMedicamento)
        {
            var newId = await _context.Medicamentos
                .OrderByDescending(m => m.IdMedicamento)
                .Select(m => m.IdMedicamento + 1)
                .FirstOrDefaultAsync();

            Medicamento oMed = new Medicamento();
            oMed.IdMedicamento = newId;
            oMed.IdMonodroga = oMedicamento.IdMonodroga;
            oMed.NombreComercial = oMedicamento.NombreMedicamento;
            oMed.IdLaboratorio = oMedicamento.IdLaboratorio;
            oMed.IdMarca = oMedicamento.IdMarca;
            oMed.VentaLibre = oMedicamento.VentaLibre;
            oMed.IdPresentacion = oMedicamento.IdPresentacion;
            oMed.Descripcion = oMedicamento.Descripcion;
            oMed.Precio = oMedicamento.Precio;
            oMed.Activo = oMedicamento.Activo;
            _context.Medicamentos.Add(oMed);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(MedicamentoSaveDTO oMedicamento)
        {
            var bMedicamento = await _context.Medicamentos.FindAsync(oMedicamento.IdMedicamento);
            if (bMedicamento != null)
            {
                bMedicamento.IdMonodroga = oMedicamento.IdMonodroga;
                bMedicamento.NombreComercial = oMedicamento.NombreMedicamento;
                bMedicamento.IdLaboratorio = oMedicamento.IdLaboratorio;
                bMedicamento.IdMarca = oMedicamento.IdMarca;
                bMedicamento.VentaLibre = oMedicamento.VentaLibre;
                bMedicamento.IdPresentacion = oMedicamento.IdPresentacion;
                bMedicamento.Descripcion = oMedicamento.Descripcion;
                bMedicamento.Precio = oMedicamento.Precio;
                bMedicamento.Activo = oMedicamento.Activo;
                _context.Update(bMedicamento);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
