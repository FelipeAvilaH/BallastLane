using System.Data;
using System.Data.Common;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly string _connectionString;
    protected readonly string _name;

    public Repository(string connectionString, string name)
    {
        _connectionString = connectionString;
        _name = name;
    }
    

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = string.Format("SELECT *from public.get_{0}_by_id({1})", _name, id);
        //AddParameter(command, "p_id", id);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapEntity(reader);
        }
        return null;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = new List<TEntity>();
        using (var connection = CreateConnection())
        {
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = string.Format("SELECT *from public.get_all_{0}()",_name);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                entities.Add(MapEntity(reader));
            }
        }
        return entities;
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "insert_" + _name;

        AddParameters(command, entity);

        await command.ExecuteNonQueryAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "update_" + _name;

        AddParameters(command, entity);

        await command.ExecuteNonQueryAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = "delete_" + _name;
        AddParameter(command, "p_id", id);

        await command.ExecuteNonQueryAsync();
    }

    protected virtual void AddParameter(DbCommand command, string name, object value)
{
    var parameter = command.CreateParameter();
    parameter.ParameterName = name;
    parameter.Value = value ?? DBNull.Value; // Tratar los valores nulos
    command.Parameters.Add(parameter);
}

    protected abstract DbConnection CreateConnection();
    protected abstract void AddParameters(DbCommand command, TEntity entity);
    protected abstract TEntity MapEntity(DbDataReader reader);
}
