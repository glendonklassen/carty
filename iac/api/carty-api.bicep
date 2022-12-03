param apimName string
param apiName string
param cartyPath string = 'carty'

resource apimDeploy 'Microsoft.ApiManagement/service@2021-12-01-preview' existing = {
  name: apimName
}

resource cartyApiVersionSet 'Microsoft.ApiManagement/service/apiVersionSets@2021-12-01-preview' = {
  name: 'cartyApiVersionSet'
  parent: apimDeploy
  properties: {
    description: 'Carty API Version Set'
    displayName: 'Carty'
    versionHeaderName: 'x-api-version'
    versioningScheme: 'Header'
  }
}

resource cartyApiDeploy 'Microsoft.ApiManagement/service/apis@2022-04-01-preview' = {
  name: apiName
  parent: apimDeploy
  properties:{
    displayName: 'Carty'
    protocols:[
      'https'
    ]
    apiVersion: 'v1'
    isCurrent: true
    apiVersionSetId: cartyApiVersionSet.id
    path: cartyPath
    subscriptionRequired: true
  }
}

resource cartyAdminSubscription 'Microsoft.ApiManagement/service/subscriptions@2021-12-01-preview' = {
  name: 'cartyAdminSubscription'
  parent: apimDeploy
  properties: {
    allowTracing: true
    displayName: 'CartyAdmin'
    scope: 'apis/${cartyApiDeploy.id}'
    state: 'active'
  }
}
