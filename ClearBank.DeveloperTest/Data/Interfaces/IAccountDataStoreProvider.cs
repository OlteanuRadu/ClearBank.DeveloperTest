namespace ClearBank.DeveloperTest.Data.Interfaces
{
    public interface IAccountDataStoreProvider
    {
        IAccountDataStore GetAccountDataStore(string dataStoreType);
    }
}
