# ClearBank.DeveloperTest

## Getting Started With ClearBank.DeveloperTest.API

1. Clone the repo
2. Get the local.settings.json with the configuration for the specific environment you want to work on

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "PaymentServiceOptions:DataStoreType": "Backup"
  }
}
```

