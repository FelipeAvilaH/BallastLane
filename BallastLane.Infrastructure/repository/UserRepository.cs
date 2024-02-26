using System.Data;
using System.Data.Common;
using Npgsql;

public class UserRepository(string connectionString) : Repository<UserEntity>(connectionString, "user"), IUserRepository
{
    protected override DbConnection CreateConnection()
    {
         return new NpgsqlConnection(_connectionString);
    }

    protected override void AddParameters(DbCommand command, UserEntity entity)
    {      
        var npgsqlCommand = (NpgsqlCommand)command;

        npgsqlCommand.Parameters.AddWithValue("p_id", entity.Id);
        npgsqlCommand.Parameters.AddWithValue("p_username", entity.Username);
        npgsqlCommand.Parameters.AddWithValue("p_password", entity.Password);
        npgsqlCommand.Parameters.AddWithValue("p_email", entity.Email);
    }

    protected override UserEntity MapEntity(DbDataReader reader)
    {
        return new UserEntity
        {
             Id = Convert.ToInt32(reader["id"]),
             Username = reader["username"].ToString(),
             Password = reader["password"].ToString(),
             Email = reader["email"].ToString()
        };
        
    }

    public async Task<UserEntity> GetByUserName(string username)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = string.Format("SELECT *from public.get_{0}_by_username('{1}')", _name, username);
        
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return MapEntity(reader);
        }
        return null;
    }
}
