param apimName string
param cartyPath string = 'carty'

resource apimDeploy 'Microsoft.ApiManagement/service@2021-12-01-preview' existing = {
  name: apimName
}

resource cartyApiv1 'Microsoft.ApiManagement/service/apiVersionSets@2021-12-01-preview' = {
  name: 'cartyv1'
  parent: apimDeploy
  properties: {
    description: 'The first iteration of Carty APIs.'
    displayName: 'Carty v1'
    versionHeaderName: 'x-api-version'
    versioningScheme: 'Header'
  }
}

resource cartyApiDeploy 'Microsoft.ApiManagement/service/apis@2022-04-01-preview' = {
  name: 'cartyapi'
  parent: apimDeploy
  properties:{
    displayName: 'Carty'
    protocols:[
      'https'
    ]
    apiVersionSetId: cartyApiv1.id
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
