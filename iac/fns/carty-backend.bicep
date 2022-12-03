param apimName string
param url string
param appName string
param apiName string
var fnKeyValueName = 'key${appName}'
@secure()
param functionKey string
resource apim 'Microsoft.ApiManagement/service@2021-12-01-preview' existing = {  
  name: apimName
}
resource cartyApiDeploy 'Microsoft.ApiManagement/service/apis@2022-04-01-preview' existing = {
  name: apiName
  parent: apim
}

resource functionKeyNamedValue 'Microsoft.ApiManagement/service/namedValues@2022-04-01-preview' = {
  name: fnKeyValueName
  parent: apim
  properties: {
    secret: true
    value: functionKey
    displayName: fnKeyValueName
  }
}
var backendId = '${appName}backend'
resource cartyApiBackend 'Microsoft.ApiManagement/service/backends@2022-04-01-preview' = {
  name: backendId
  parent: apim
  properties:{
    url: url
    protocol: 'http'
  }
}
resource cartyOperation 'Microsoft.ApiManagement/service/apis/operations@2022-04-01-preview' = {
  name: appName
  parent: cartyApiDeploy
  properties: {
    displayName: 'CartyMap'
    method: 'POST'
    urlTemplate: 'map'
  }
}
var policyDoc = replace(replace(loadTextContent('policy/policy.xml'), '{{backendId}}', backendId), 'FunctionKeyNamedValue', fnKeyValueName)
resource cartyOperationPolicy 'Microsoft.ApiManagement/service/apis/operations/policies@2021-12-01-preview' = {
  name: 'policy'
  parent: cartyOperation
  properties: {
    format: 'rawxml'
    value: policyDoc
  }
}
