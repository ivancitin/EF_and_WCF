using Microsoft.EntityFrameworkCore;

namespace Sqlite.EF
{
    public class DatabaseProvider
    {
        public readonly string DatabasePath;

        public DatabaseProvider(string databasePath)
        {
            this.DatabasePath = databasePath;

            using (var db = GetConnection())
            {
                db.Database.Migrate();
            }
        }

        public DatabaseContext GetConnection()
        {
            return new DatabaseContext(DatabasePath);
        }
    }
}
