using FarmaceuticaBack.Data.Contracts;
using FarmaceuticaBack.Models;
using FarmaceuticaBack.Services.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmaceuticaBack.Data.Repositories
{
    public class PersonalEstablecimientoRepository : IPersonalEstablecimientoRepository
    {
        private readonly FarmaceuticaContext _context;
        private readonly JwtService _jwtService;
        public PersonalEstablecimientoRepository(FarmaceuticaContext context, JwtService jwtService)
        {
            this._context = context;
            _jwtService = jwtService;   
        }

        public async Task<string> Login(string nroDoc, string password)
        {
            var usuario = await _context.Personals.FirstOrDefaultAsync(u => u.NroDoc == nroDoc);

            if (usuario == null || !VerifyPassword(password, usuario.Psw))
            {
                return null;
            }

            return _jwtService.GenerateToken(usuario.NroDoc);
        }

        public async Task<bool> Register(Personal oPersonal)
        {
            var existe = await _context.Personals.AnyAsync(u => u.NroDoc == oPersonal.NroDoc);
            if (existe)
            {
                return false;
            }

            oPersonal.Psw = HashPassword(oPersonal.Psw);
            

            _context.Personals.Add(oPersonal);
            return await _context.SaveChangesAsync() > 0;
        }

        private string HashPassword(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        private bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }

        public async Task<bool> Add(PersonalCargosEstablecimiento oPersonal)
        {
            _context.PersonalCargosEstablecimientos.Add(oPersonal);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<PersonalCargosEstablecimiento>> GetAll()
        {
            List<PersonalCargosEstablecimiento> result = await _context.PersonalCargosEstablecimientos
                .Include(p => p.IdCargoNavigation)
                .Include(p => p.IdEstablecimientoNavigation)
                .Include(p => p.IdPersonalNavigation)
                .ToListAsync();
            return result;
        }

        public async Task<List<PersonalCargosEstablecimiento>> GetByEstablishment(int id)
        {
            return await _context.PersonalCargosEstablecimientos
                                    .Where(p => p.IdEstablecimiento == id)
                                    .Include(p => p.IdPersonalNavigation)
                                    .Include(p => p.IdCargoNavigation)
                                    .Include(p=> p.IdEstablecimientoNavigation)
                                    .ToListAsync();
        }

        public async Task<List<PersonalCargosEstablecimiento>> GetByFilter(int id, string nombre, string apellido, string documento)
        {
            IQueryable<PersonalCargosEstablecimiento> query = _context.PersonalCargosEstablecimientos
                                   .Include(s => s.IdPersonalNavigation)
                                   .Include(s => s.IdCargoNavigation)
                                   .Where(s => s.IdEstablecimiento == id);

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(s => s.IdPersonalNavigation.Nombre.Contains(nombre));
            }

            if (!string.IsNullOrEmpty(apellido))
            {
                query = query.Where(s => s.IdPersonalNavigation.Apellido.Contains(apellido));
            }

            if (!string.IsNullOrEmpty(documento))
            {
                query = query.Where(s => s.IdPersonalNavigation.NroDoc.Contains(documento));
            }

            List<PersonalCargosEstablecimiento> lst = await query.ToListAsync();

            return lst;
        }

        public async Task<PersonalCargosEstablecimiento> GetById(int id)
        {
            PersonalCargosEstablecimiento person = await _context.PersonalCargosEstablecimientos
                .Where(p => p.IdPersonalCargosEstablecimientos == id)
                .FirstOrDefaultAsync();
            return person;
        }

        public async Task<int> GetLastId()
        {
            var lastId = await _context.PersonalCargosEstablecimientos
            .OrderByDescending(m => m.IdPersonalCargosEstablecimientos)
            .Select(m => m.IdPersonalCargosEstablecimientos)
            .FirstOrDefaultAsync();

            return lastId;
        }

        string IPersonalEstablecimientoRepository.HashPassword(string plainPassword)
        {
            throw new NotImplementedException();
        }

        bool IPersonalEstablecimientoRepository.VerifyPassword(string plainPassword, string hashedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
