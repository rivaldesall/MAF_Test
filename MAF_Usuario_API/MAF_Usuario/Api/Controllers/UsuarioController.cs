using Api.DTO;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("V1/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsuarioController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UsuarioCreateDto dto)
        {
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo
            };

            var nuevoUsuario = await _unitOfWork.Usuarios.CreateAsync(usuario);

            if (nuevoUsuario == null)
            {
                return BadRequest("No se pudo crear el usuario. El correo ya podría estar registrado.");
            }

            await _unitOfWork.CommitAsync();
            return CreatedAtAction(nameof(getAll), new { id = nuevoUsuario.IdUsuario }, nuevoUsuario);
        }

        
        [HttpPost("getAll")]
        public async Task<IActionResult> getAll()
        {
            var usuarios = await _unitOfWork.Usuarios.GetAllAsync();
            return Ok(usuarios);
        }

        
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UsuarioUpdateDto dto)
        {
            var usuario = new Usuario
            {
                IdUsuario = dto.IdUsuario,
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Correo = dto.Correo,
                Activo = dto.Activo
            };

            var usuarioActualizado = await _unitOfWork.Usuarios.UpdateAsync(usuario);

            if (usuarioActualizado == null)
            {
                return NotFound($"Usuario con IdUsuario {dto.IdUsuario} no encontrado.");
            }

            await _unitOfWork.CommitAsync();
            return Ok(usuarioActualizado);
        }

        
        [HttpPost("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _unitOfWork.Usuarios.DeleteAsync(id);

            if (!resultado)
            {
                return NotFound($"Usuario con IdUsuario {id} no encontrado.");
            }

            await _unitOfWork.CommitAsync();
            return NoContent(); 
        }
    }
}
