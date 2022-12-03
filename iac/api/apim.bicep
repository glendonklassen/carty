param location string
param apimName string
param apiName string
param appInsightsName string

resource apimDeploy 'Microsoft.ApiManagement/service@2021-12-01-preview' = {
  name: apimName
  location: location
  sku: {
    capacity: 0
    name: 'Consumption'
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    publicNetworkAccess: 'Enabled'
    publisherEmail: 'gjklassen@hotmail.com'
    publisherName: 'Glendon Klassen'
  }
}

resource appInsightsAPIManagement 'Microsoft.ApiManagement/service/loggers@2020-06-01-preview' = {
  name: appInsightsName
  parent: apimDeploy
  properties: {
    loggerType: 'applicationInsights'
    resourceId: appInsights.id
    credentials: {
      instrumentationKey: appInsights.properties.InstrumentationKey
    }
  }
}
resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: appInsightsName
}
module cartyApi 'carty-api.bicep' = {
  name: 'cartyApi${guid('cartyApi')}'
  params: {
    apimName: apimName
    apiName: apiName
    loggerId: appInsightsAPIManagement.id
  }
  dependsOn:[
    apimDeploy
  ]
}
