using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> CreateAsync(Usuario usuario);
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int idUsuario);
    }
}
