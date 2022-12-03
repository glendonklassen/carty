$sharedRgName = 'gk-shared-cc'
$cartyRgName = 'gk-carty-cc'
$location = 'canadacentral'
$templateFile = 'carty.bicep'
$paramsFile = 'carty-params.json'
$timestamp =  (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHHmmssK")
$deploymentName = 'cartyDeployment' + $timestamp

az group create -n $sharedRgName -l $location
az group create -n $cartyRgName -l $location

$title    = 'Azure Resource Deployment'
$question = 'Are you sure you want to proceed?'
$choices  = '&Yes', '&No'

az deployment group what-if --name $deploymentName --resource-group $sharedRgName --template-file $templateFile --parameters $paramsFile

$decision = $Host.UI.PromptForChoice($title, $question, $choices, 1)
if ($decision -eq 0) {
    az deployment group create --name $deploymentName --resource-group $sharedRgName --template-file $templateFile --parameters $paramsFile
} else {
    Write-Host 'Cancelled deployment operation.'
}