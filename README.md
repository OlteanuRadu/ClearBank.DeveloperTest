# ClearBank.DeveloperTest

Getting Started
Clone the repo
Get the local.settings.json with the configuration for the specific environment you want to work on
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "PaymentServiceOptions:DataStoreType": "Backup"
  }
}
