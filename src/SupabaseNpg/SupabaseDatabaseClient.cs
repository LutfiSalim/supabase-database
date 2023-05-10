using Npgsql;

namespace SupabaseNpg
{
    internal class SupabaseDatabaseClient
    {
        private readonly string _connectionString;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="connectionString"></param>
        public SupabaseDatabaseClient(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Guid> CreateData(string name, string email, string password)
        {
            var user = new Users
            {
                Name = name,
                Email = email,
                Password = password
            };

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand("INSERT INTO users( name, email, password) VALUES ( @name, @email, @password) RETURNING user_id");
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Connection = connection;
            var objectId = await command.ExecuteScalarAsync();
            Guid result = (Guid)objectId!;
            return result;
        }

        public async Task<List<Dictionary<string, object>>> ListData()
        {
            var data = new List<Dictionary<string, object>>();

            // sample of nested using statements
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM users"))
                {
                    command.Connection = connection;
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        var dict = new Dictionary<string, object>();
                        dict[reader.GetName(0)] = reader[0];
                        dict[reader.GetName(1)] = reader[1];
                        dict[reader.GetName(2)] = reader[2];
                        dict[reader.GetName(3)] = reader[3]; // depends on how many columns ada dlm table

                        data.Add(dict);
                    }

                    command.Dispose(); //reader = null
                }
            }

            return data;
        }

        public async Task<List<Dictionary<string, object>>> GetData(string userIdStringGet)
        {
            if (Guid.TryParse(userIdStringGet, out Guid userId))
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using var command = new NpgsqlCommand("SELECT * FROM users WHERE user_id = @userId");
                command.Parameters.AddWithValue("@userId", userId);
                command.Connection = connection;
                var reader = await command.ExecuteReaderAsync();

                var dataGet = new List<Dictionary<string, object>>();

                while (await reader.ReadAsync())
                {
                    var dict = new Dictionary<string, object>();
                    dict[reader.GetName(0)] = reader[0];
                    dict[reader.GetName(1)] = reader[1];
                    dict[reader.GetName(2)] = reader[2];
                    dict[reader.GetName(3)] = reader[3]; // depends on how many columns ada dlm table

                    dataGet.Add(dict);
                }

                return dataGet;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Dictionary<string, object>>> GetData2(string userIdStringGet)
        {
            var dataGet = new List<Dictionary<string, object>>();

            if (Guid.TryParse(userIdStringGet, out Guid userId))
            {
                return null!;
            }

            NpgsqlConnection connection = null!;
            NpgsqlCommand command = null!;

            try
            {
                connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                command = new NpgsqlCommand("SELECT * FROM users WHERE user_id = @userId");
                command.Parameters.AddWithValue("@userId", userId);
                command.Connection = connection;
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var dict = new Dictionary<string, object>();
                    dict[reader.GetName(0)] = reader[0];
                    dict[reader.GetName(1)] = reader[1];
                    dict[reader.GetName(2)] = reader[2];
                    dict[reader.GetName(3)] = reader[3]; // depends on how many columns ada dlm table

                    dataGet.Add(dict);
                }

                return dataGet;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return dataGet;
            }
            finally
            {
                connection?.Dispose();
                connection = null!;

                command?.Dispose();
                command = null!;
            }
        }

        public async Task<int> UpdateData(string userIdStringUpdate, string nameupdate, string emailupdate, string passwordupdate)
        {

            string strGuid = "" + userIdStringUpdate;
            Guid userId = Guid.Parse(strGuid);

            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            using var command = new NpgsqlCommand("UPDATE users SET name = @name, email = @email, password = @password WHERE user_id = @userId");
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@name", nameupdate);
            command.Parameters.AddWithValue("@email", emailupdate);
            command.Parameters.AddWithValue("@password", passwordupdate);
            command.Connection = connection;
            var rowsUpdated = await command.ExecuteNonQueryAsync();

            return rowsUpdated;
        }

        public async Task<int> DeleteData(string userIdStringDelete)
        {
            if (Guid.TryParse(userIdStringDelete, out Guid userId))
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.Open();
                using var command = new NpgsqlCommand("DELETE FROM users WHERE user_id = @userId");
                command.Parameters.AddWithValue("@userId", userId);
                command.Connection = connection;
                var rowAffected = await command.ExecuteNonQueryAsync();
                return rowAffected;
            }
            else
            {
                int rowsAffected = 0;

                return rowsAffected;
            }
        }
    }
}
