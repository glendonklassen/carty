@description('Storage Account type')
@allowed([
  'Standard_LRS'
  // $$$
  //'Standard_GRS'
  //'Standard_RAGRS'
])
param storageAccountType string = 'Standard_LRS'
param location string
param appName string
param apiName string
param apimName string
param uniqueName string
param sharedResourceGroup string
param runtime string = 'dotnet'

param applicationInsightsName string
var storageAccountName = 'stg${uniqueName}'
var functionWorkerRuntime = runtime

resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  scope: resourceGroup(sharedResourceGroup)
  name: applicationInsightsName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountType
  }
  kind: 'Storage'
}

resource hostingPlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appName
  location: location
  kind: 'linux'
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {
    reserved: true
  }
}

resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: appName
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNET|6.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTSHARE'
          value: toLower(appName)
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionWorkerRuntime
        }
      ]
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
    }
    httpsOnly: true
  }
}
module backend 'carty-backend.bicep' = {
  scope: resourceGroup(sharedResourceGroup)
  name: 'backend${appName}'
  params:{
    appName: appName
    functionKey: listkeys('${functionApp.id}/host/default', '2016-08-01').functionKeys.default
    apimName: apimName
    url: 'https://${functionApp.properties.defaultHostName}/api'
    apiName: apiName
  }
}
