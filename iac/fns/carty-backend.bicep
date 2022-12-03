param apimName string
param url string
param appName string
resource apimDeploy 'Microsoft.ApiManagement/service@2021-12-01-preview' existing = {  
  name: apimName
}
resource cartyApiBackend 'Microsoft.ApiManagement/service/backends@2022-04-01-preview' = {
  name: '${appName}-backend'
  parent: apimDeploy
  properties:{
    url: url
    protocol: 'http'
  }
}
