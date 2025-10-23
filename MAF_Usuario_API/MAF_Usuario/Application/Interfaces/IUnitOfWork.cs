using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Propiedad para acceder al Repositorio de Usuarios
        IUsuarioRepository Usuarios { get; }

        // Método para confirmar (commit) los cambios en la transacción
        Task<int> CommitAsync();

        // Método para revertir (rollback) si es necesario (opcional)
        // void Rollback();
    }
}
