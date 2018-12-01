using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Sqlite;
using Sqlite.EF;
using Sqlite.Model;
using System;
using System.IO;
using System.ServiceModel;
using WCF.Client;
using WCF.Server;
using WCF.Service;

namespace Test
{
    [TestFixture]
    public class Test
    {
        static readonly string TEST_ROOT = Path.Combine(Path.GetTempPath(), "iao_test_" + Path.GetRandomFileName());
        static readonly string DB_FILE = Path.Combine(TEST_ROOT, "db.sqlite");

        DatabaseProvider databaseProvider;

        DatabaseContext databaseContext;
        DatabaseContext DatabaseContext => databaseContext;//databaseProvider.GetConnection();
        DbSet<TableEntry> Entries => DatabaseContext.Entries;

        DirectSqliteDbAccess directSqliteDbAccess;

        IIPCService ipcService;

        [OneTimeSetUp]
        public void SetUp()
        {
            Directory.CreateDirectory(TEST_ROOT);

            databaseProvider = new DatabaseProvider(DB_FILE);
            databaseContext = databaseProvider.GetConnection();

            directSqliteDbAccess = new DirectSqliteDbAccess(DB_FILE);

            // IPC
            ipcService = new IPCServiceImpl();
            var ipcServer = new ServiceHostIPCServer<IIPCService>(ipcService);
            ipcServer.Start();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(TEST_ROOT, true);
        }

        [Test]
        public void Test1_DB_EF()
        {
            var entry = new TableEntry()
            {
                Id = 1L,
                Json = "{key1:value1, key2:value2}"
            };

            //using (var d = DatabaseContext)
            //{
                Entries.Add(entry);

                var storedEntry = Entries.Find(entry.Id);

                Assert.AreEqual(entry.Id, storedEntry.Id);
                Assert.AreEqual(entry.Json, storedEntry.Json);
            //}
        }

        [Test]
        public void Test5_DB_EF()
        {
            var entry = new TableEntry()
            {
                Id = 2L,
                Json = "{key1:value1, key2:value2}"
            };

            //using (var d = DatabaseContext)
            //{
            Entries.Add(entry);

            var storedEntry = Entries.Find(entry.Id);

            Assert.AreEqual(entry.Id, storedEntry.Id);
            Assert.AreEqual(entry.Json, storedEntry.Json);
            //}
        }

        [Test]
        public void Test3_DB_Sqlite()
        {
            var entry = new TableEntry()
            {
                Id = 3L,
                Json = "{key1:value1, key2:value2}"
            };

            directSqliteDbAccess.Add(entry);

            var storedEntry = directSqliteDbAccess.Find(entry.Id);

            Assert.AreEqual(entry.Id, storedEntry.Id);
            Assert.AreEqual(entry.Json, storedEntry.Json);
        }

        [Test]
        public void Test2_IPC()
        {
            bool executed = false;
            Action executeAction = () =>
            {
                executed = true;
            };

            ipcService.Providers.ExitAppProvider = executeAction;

            ExecuteIpc(c => c.ExitApp());
            Assert.True(executed);
        }

        void ExecuteIpc(Action<IIPCService> IpcAction)
        {
            IIPCService client = IPCClientFactory.Instantiate<IIPCService>(Constants.PORT);

            try
            {
                IpcAction(client);
            }
            finally
            {
                (client as IClientChannel).Close(TimeSpan.FromSeconds(2));
            }
        }
    }
}
