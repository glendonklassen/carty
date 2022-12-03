param location string
param apimName string
param apiName string
param appInsightsName string = 'cartyInsights'
param sharedResourceGroup string
param cartyResourceGroupName string
param uniqueName string = uniqueString(resourceGroup().id, appPrefix, location)
@maxLength(10)
param appPrefix string
@description('The name of the function app that you wish to create.')
param appName string = '${appPrefix}${uniqueName}'

module cartyFns 'fns/carty-fns.bicep' = {
  scope: resourceGroup(cartyResourceGroupName)
  name: 'cartyFns${uniqueString('cartyFns')}'
  params:{
    location: location
    appName: appName
    apiName: apiName
    apimName: apimName
    uniqueName: uniqueName
    sharedResourceGroup: sharedResourceGroup
    applicationInsightsName: appInsightsName
  }
}
