using FarmaPrisa.Data;
using FarmaPrisa.Models.Dtos.Branch;
using FarmaPrisa.Repository.Interface;
using FarmaPrisa.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FarmaPrisa.Repository.Service
{
    public class BranchService : IBranchReporsitory
    {
        private readonly FarmaPrisaContext _context;

        public BranchService(FarmaPrisaContext context)
        {
            _context = context;
        }

        public async Task<List<BranchWithUsersDto>> GetAllBranchesWithUsersAsync()
        {
            return await _context.Branches
                .Include(b => b.Company)  // opcional
                .Include(b => b.Usuarios) 
                .Select(b => new BranchWithUsersDto
                {
                    IdBranch = b.IdBranch,
                    BranchName = b.BranchName,
                    Address = b.Address,
                    PhoneNumber = b.PhoneNumber,
                    Usuarios = b.Usuarios.Select(u => new UserBranchDto
                    {
                        Id = u.Id,
                        NombreCompleto = u.NombreCompleto,
                        Email = u.Email,
                        Telefono = u.Telefono
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<BranchWithUsersDto?> GetBranchWithUsersByIdAsync(int idBranch)
        {
            return await _context.Branches
                .Where(b => b.IdBranch == idBranch)
                .Include(b => b.Usuarios)
                .Select(b => new BranchWithUsersDto
                {
                    IdBranch = b.IdBranch,
                    BranchName = b.BranchName,
                    Address = b.Address,
                    PhoneNumber = b.PhoneNumber,
                    Usuarios = b.Usuarios.Select(u => new UserBranchDto
                    {
                        Id = u.Id,
                        NombreCompleto = u.NombreCompleto,
                        Email = u.Email,
                        Telefono = u.Telefono
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> AssignUserToBranchAsync(int idBranch, int idUsuario)
        {
            var branch = await _context.Branches
                .Include(b => b.Usuarios)
                .FirstOrDefaultAsync(b => b.IdBranch == idBranch);

            if (branch == null)
                return false;

            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
                return false;

            if (!branch.Usuarios.Any(u => u.Id == idUsuario))
                branch.Usuarios.Add(usuario);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
