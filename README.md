# carty

To deploy, run [deploy-shared.ps1](iac/deploy-shared.ps1) then [deploy-fns.ps1](iac/deploy-fns.ps1).

To run, use `func start` in the [api](/api/) directory.

## TODO

- Enable continuous deployment from GitHub using [sites/sourcecontrols](https://learn.microsoft.com/en-us/azure/templates/microsoft.web/sites/sourcecontrols?pivots=deployment-language-bicep).
- Create a contract for map generation algorithms
- Create a decent map generation algorithm
- Start playing a game on the board that gets generated
