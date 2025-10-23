using Application.Interfaces;
using Application.Interfaces.Repositories;
using Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;

        // Repositorios
        public IUsuarioRepository Usuarios { get; }

        public UnitOfWork(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            _connection = new SqlConnection(connectionString);
            _connection.Open();
            // Inicia la transacción al crear la UoW
            _transaction = _connection.BeginTransaction();

            // Inicializa los repositorios pasando la conexión y la transacción
            Usuarios = new UsuarioRepository(_connection, _transaction);
        }

        public async Task<int> CommitAsync()
        {
            try
            {
                // Confirma la transacción
                _transaction.Commit();
                return 1; // O el número de filas afectado si se pudiera rastrear
            }
            catch
            {
                // En caso de error, revierte
                _transaction.Rollback();
                throw; // Relanza la excepción para que la API lo maneje
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction(); // Inicia una nueva transacción
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Intentar hacer rollback si no se ha confirmado (para evitar locks)
                    try
                    {
                        _transaction?.Rollback();
                    }
                    catch { /* Ignorar errores de rollback en dispose */ }

                    _transaction?.Dispose();
                    _connection?.Close();
                    _connection?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
