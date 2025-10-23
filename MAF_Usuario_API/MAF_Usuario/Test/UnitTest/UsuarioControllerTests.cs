using Moq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Api.DTO;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Api.Controllers;

namespace Test.UnitTest
{
    public class UsuarioControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();

            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(uow => uow.Usuarios).Returns(_mockUsuarioRepository.Object);

            _controller = new UsuarioController(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Crear_Usuario_Valido_DebeLlamarCommitYRetornarCreated()
        {
            var dto = new UsuarioCreateDto{ Nombre = "Test", Apellido = "User", Correo = "test@prueba.com" };
            var usuarioSimulado = new Usuario { IdUsuario = 1, Nombre = "Test", Correo = "test@prueba.com" };
            _mockUsuarioRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Usuario>()))
                .ReturnsAsync(usuarioSimulado);

            var result = await _controller.Create(dto);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            _mockUsuarioRepository.Verify(repo => repo.CreateAsync(It.IsAny<Usuario>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Crear_Usuario_FalloDeCreacion_DebeRetornarBadRequestYNoLlamarCommit()
        {
            var dto = new UsuarioCreateDto { Nombre = "Falla", Apellido = "User", Correo = "fail@prueba.com" };
            _mockUsuarioRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Usuario>()))
                .ReturnsAsync((Usuario)null);

            var result = await _controller.Create(dto);
            Assert.IsType<BadRequestObjectResult>(result);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }
        [Fact]
        public async Task Listar_UsuariosExistentes_DebeRetornarOkConDatos()
        {
            var listaSimulada = new List<Usuario>
            {
                new Usuario { IdUsuario = 1, Nombre = "User1" },
                new Usuario { IdUsuario = 2, Nombre = "User2" }
            };
            _mockUsuarioRepository.Setup(repo => repo.GetAllAsync())
                                  .ReturnsAsync(listaSimulada);

            var result = await _controller.getAll();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var usuarios = Assert.IsAssignableFrom<IEnumerable<Usuario>>(okResult.Value);
            Assert.Equal(2, usuarios.Count());
            _mockUsuarioRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Listar_NoHayUsuarios_DebeRetornarOkConListaVacia()
        {
            _mockUsuarioRepository.Setup(repo => repo.GetAllAsync())
                                  .ReturnsAsync(new List<Usuario>());

            var result = await _controller.getAll();
            var okResult = Assert.IsType<OkObjectResult>(result);
            var usuarios = Assert.IsAssignableFrom<IEnumerable<Usuario>>(okResult.Value);
            Assert.Empty(usuarios);
        }
        [Fact]
        public async Task Actualizar_UsuarioExistente_DebeRetornarOkYLlamarCommit()
        {
            var dto = new UsuarioUpdateDto { IdUsuario = 5, Nombre = "Updated", Correo = "upd@test.com", Activo = true };
            var usuarioActualizado = new Usuario { IdUsuario = 5, Nombre = dto.Nombre };
            _mockUsuarioRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Usuario>()))
                                  .ReturnsAsync(usuarioActualizado);

            var result = await _controller.Update(dto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockUsuarioRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Usuario>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Actualizar_UsuarioNoExistente_DebeRetornarNotFoundYNoLlamarCommit()
        {
            var dto = new UsuarioUpdateDto { IdUsuario = 999, Nombre = "Missing", Correo = "miss@test.com", Activo = false };
            _mockUsuarioRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Usuario>()))
                                  .ReturnsAsync((Usuario)null);

            var result = await _controller.Update(dto);
            Assert.IsType<NotFoundObjectResult>(result);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }
        [Fact]
        public async Task Eliminar_UsuarioExistente_DebeRetornarNoContentYLLamarCommit()
        {
            const int idAEliminar = 10;
            _mockUsuarioRepository.Setup(repo => repo.DeleteAsync(idAEliminar))
                                  .ReturnsAsync(true);

            var result = await _controller.Delete(idAEliminar);
            Assert.IsType<NoContentResult>(result);
            _mockUsuarioRepository.Verify(repo => repo.DeleteAsync(idAEliminar), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Eliminar_UsuarioNoExistente_DebeRetornarNotFoundYNoLlamarCommit()
        {
            const int idAEliminar = 999;
            _mockUsuarioRepository.Setup(repo => repo.DeleteAsync(idAEliminar))
                                  .ReturnsAsync(false);

            var result = await _controller.Delete(idAEliminar);
            Assert.IsType<NotFoundObjectResult>(result);
            _mockUnitOfWork.Verify(uow => uow.CommitAsync(), Times.Never);
        }
    }
}