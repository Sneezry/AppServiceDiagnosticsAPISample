# App Service Diagnostics API Sample
This project shows a sample on how to call App service diagnostics APIs using Azure AD App authentication.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

- [dotnet core sdk](https://dotnet.microsoft.com/download)
- Client Cert that is trusted by App services team, (Read instructions on that in the internal wiki), OR
- AD App with AppId in allowed list by App Services Team. [Learn How to Create an Azure AD App](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal)


### Building the project

```
cd src\SampleAPIServer
dotnet build
```

## Running Project Locally

1. Update the Settings in `src\SampleAPIServer\appsettings.json` as per internal wiki shared.

2. Run the SampleAPIServer:

    ```
    cd src\SampleAPIServer
    dotnet run

    ```

The SampleAPIServer will start and requests will be serve at `http://localhost:50616`

## Testing the APIs

The SampleAPIServer exposes two APIs. You can just the APIs using Postman or in Browser:

- #### List Detectors API:
    - Method : GET
    - Url : `http://localhost:50616/subscriptions/<your subid>/resourceGroups/<your resource group>/Microsoft.<RPName>/<service>/<your service name>/detectors`

    Example:

    - AKS : `http://localhost:50616/subscriptions/<your subid>/resourceGroups/<your resource group>/Microsoft.ContainerService/managedClusters/<your cluster name>/detectors`
    - Logic App : `http://localhost:50616/subscriptions/<your subid>/resourceGroups/<your resource group>/Microsoft.Logic/workflows/<your cluster name>/detectors`


- #### Get Detector API:
    - Method : GET
    - Url : `http://localhost:50616/subscriptions/<your subid>/resourceGroups/<your resource group>/Microsoft.<RPName>/<service>/<your service name>/detectors/<detectorId>`

