using Dapper;
using System.Data;
using Domain.Entities;
using Application.Interfaces.Repositories;


namespace Infrastructure.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {

        private readonly IDbConnection _dbConnection;
        private readonly IDbTransaction _dbTransaction;
        public UsuarioRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _dbConnection = connection;
            _dbTransaction = transaction;
        }
        public async Task<Usuario?> CreateAsync(Usuario usuario)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@Nombre", usuario.Nombre);
            parameters.Add("@Apellido", usuario.Apellido);
            parameters.Add("@Correo", usuario.Correo);

            var result = await _dbConnection.QueryFirstOrDefaultAsync<Usuario>(
                "SP_Usuario_Crear",
                parameters,
                transaction: _dbTransaction,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<bool> DeleteAsync(int idUsuario)
        {

            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", idUsuario);

            var filasAfectadas = await _dbConnection.ExecuteScalarAsync<int>(
                "SP_Usuario_Eliminar",
                parameters,
                transaction: _dbTransaction,
                commandType: CommandType.StoredProcedure
            );

            return filasAfectadas > 0;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _dbConnection.QueryAsync<Usuario>(
                "SP_Usuario_ListarTodos",
                transaction: _dbTransaction,
                commandType: CommandType.StoredProcedure
            );
        }

      

        public async Task<Usuario?> UpdateAsync(Usuario usuario)
        {
            // Parámetros para el SP
            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", usuario.IdUsuario);
            parameters.Add("@Nombre", usuario.Nombre);
            parameters.Add("@Apellido", usuario.Apellido);
            parameters.Add("@Correo", usuario.Correo);
            parameters.Add("@Activo", usuario.Activo);

            var result = await _dbConnection.QueryFirstOrDefaultAsync<Usuario>(
                "SP_Usuario_Actualizar",
                parameters,
                transaction: _dbTransaction,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}
