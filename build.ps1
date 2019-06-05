$root = "../../"
$outputFolder = "artifacts"
$outputContents = Join-Path $outputFolder "*.*"
$libraryFolder =  Join-Path $outputFolder "Oisys"
$srcFolder = "src/Oisys.Web"

Write-Output "Cleaning the artifacts folder..."
Remove-Item $outputFolder -Recurse

Write-Output "Build and publish the application..."
$outputPath = Join-Path $root $libraryFolder
dotnet publish $srcFolder -f netcoreapp2.1 -c Release --force -o $outputPath

Write-Output "Copying the docker folder to artifacts..."
Copy-Item "docker/Dockerfile" $outputFolder