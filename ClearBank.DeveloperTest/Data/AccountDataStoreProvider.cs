using ClearBank.DeveloperTest.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStoreProvider : IAccountDataStoreProvider
    {
        public IAccountDataStore GetAccountDataStore(string dataStoreType) =>
            dataStoreType == Constants.BackupDataStoreType ?
            new BackupAccountDataStore() :
            new AccountDataStore();
    }
}
