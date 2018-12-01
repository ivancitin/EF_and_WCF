using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Sqlite.Model;
using System.Data;

namespace Sqlite
{
    public class DirectSqliteDbAccess
    {
        readonly string dbPath;

        public DirectSqliteDbAccess(string dbPath)
        {
            this.dbPath = dbPath;
        }

        public TableEntry Find(long key)
        {
            lock (connectionLock)
            {
                return ReadEntry(key, GetDbConnection());
            }
        }

        public void Add(TableEntry entry)
        {
            string json = JsonConvert.SerializeObject(entry);

            lock (connectionLock)
            {
                SaveEntry(entry.Id, json, GetDbConnection());
            }
        }

        public void Dispose()
        {
            TryCloseConnection(dbConnection);
        }

        // Using a single connection, its use will be serialized
        readonly object connectionLock = new object();
        SqliteConnection dbConnection;
        SqliteConnection GetDbConnection()
        {
            if (dbConnection == null || dbConnection.State == ConnectionState.Closed || dbConnection.State == ConnectionState.Broken)
            {
                TryCloseConnection(dbConnection);
                dbConnection = null;

                dbConnection = new SqliteConnection(GetConnectionString());
                dbConnection.Open();
            }

            return dbConnection;
        }

        string GetConnectionString()
        {
            return $"Data Source=\"{dbPath}\";";
        }

        static void TryCloseConnection(SqliteConnection conn)
        {
            try
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                    conn = null;
                }
            }
            finally
            {
                conn?.Dispose();
            }
        }

        static readonly string INSERT_SQL = "INSERT INTO TableEntry (Id, Json) VALUES (@id, @json)";
        static readonly string SELECT_SQL = "SELECT * FROM TableEntry WHERE Id = @id";

        static void SaveEntry(long id, string json, SqliteConnection conn)
        {
            using (var command = new SqliteCommand(INSERT_SQL, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@json", json);

                command.ExecuteNonQuery();
            }
        }

        static TableEntry ReadEntry(long id, SqliteConnection conn)
        {
            TableEntry result = null;
            using (var command = new SqliteCommand(SELECT_SQL, conn))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = JsonConvert.DeserializeObject<TableEntry>(reader.GetString(1));
                    }
                }
            }

            return result;
        }
    }
}
