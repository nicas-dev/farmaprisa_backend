using FarmaPrisa.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmaPrisa.Controllers
{
    [ApiController]
    [Route("[controller]")]

    //[Authorize(Roles = "administrador")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchReporsitory _branchRepo;

        public BranchController(IBranchReporsitory branchRepo)
        {
            _branchRepo = branchRepo;
        }

        [HttpGet("with-users")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _branchRepo.GetAllBranchesWithUsersAsync();
            return Ok(result);
        }

        [HttpGet("with-users/{idBranch}")]
        public async Task<IActionResult> GetById(int idBranch)
        {
            var result = await _branchRepo.GetBranchWithUsersByIdAsync(idBranch);
            if (result == null)
                return NotFound("Sucursal no encontrada");

            return Ok(result);
        }

        [HttpPost("{idBranch}/assign-user/{idUsuario}")]
        public async Task<IActionResult> AssignUserToBranch(int idBranch, int idUsuario)
        {
            var success = await _branchRepo.AssignUserToBranchAsync(idBranch, idUsuario);
            if (!success)
                return BadRequest("No fue posible asignar el usuario");

            return Ok("Usuario asignado correctamente");
        }
    }
}
