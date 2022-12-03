param location string
param apimName string
param apiName string
param appInsightsName string = 'cartyInsights'


module cartyShared 'shared/app-insights.bicep' = {
  name: 'cartyShared${uniqueString('cartyShared')}'
  params:{
    appInsightsName: appInsightsName
    location: location
  }
}
module apim 'api/apim.bicep' = {
  name: 'apim${uniqueString('apim')}'
  params:{
    location: location
    apimName: apimName
    apiName: apiName
    appInsightsName: appInsightsName
  }
  dependsOn: [
    cartyShared
  ]
}
