# Getting started

## Setting up your machine to write K1 apps

### If you haven't cloned yet this repo yet
This approach will clone the repository, build it, and configure your environment so you can run K1:

1. Open a VS dev command prompt (standard or PowerShell)
2. Navigate to the folder where you'd like to clone the project, e.g. ```C:\src```
3.  Copy and paste the following and hit Enter:
    - powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "iex ((new-object net.webclient).DownloadString('https://raw.github.com/Katana/ProjectSystem/master/install.ps1?token=249088__eyJzY29wZSI6IlJhd0Jsb2I6S2F0YW5hL1Byb2plY3RTeXN0ZW0vbWFzdGVyL2luc3RhbGwucHMxIiwiZXhwaXJlcyI6MTM4NDY2MzkwMn0%3D--928efa4c77331f2a69d53ab23cd1e7a2adaa989d'))"
4. Remember to close and re-open your command prompt to get the updated environment settings

### If you've already cloned this repo
This approach will build the project and configure your environment so you can run K1:

1. Open a VS dev cmd prompt (standard or PowerShell)
2. Navigate to the folder where you cloned the project, e.g. ```C:\src\ProjectSystem```
3. Run build.cmd
4. Modify the PATH to include the scripts folder:
    - In PowerShell, type ```[Environment]::SetEnvironmentVariable("PATH", "$env:PATH;$pwd\scripts", "User")``` and hit Enter
    - In cmd prompt, type ```SET PATH=%PATH%;%CD%\scripts``` and hit Enter

## Writing your first K1 web application
Coming soon...