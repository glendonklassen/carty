param location string
param apimName string

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

module cartyApi 'carty-api.bicep' = {
  name: 'cartyApi${guid('cartyApi')}'
  params: {
    apimName: apimName
  }
  dependsOn:[
    apimDeploy
  ]
}
