$root = "../../"
$outputFolder = "artifacts"
$outputContents = Join-Path $outputFolder "*.*"
$libraryFolder =  Join-Path $outputFolder "Oisys"

Write-Output "Cleaning the artifacts folder..."
Remove-Item $outputFolder -Recurse

Write-Output "Build and publish the application..."
$outputPath = Join-Path $root $libraryFolder
dotnet publish -f netcoreapp2.1 -o $outputPath

Write-Output "Copying the docker folder to artifacts..."
Copy-Item "docker/Dockerfile" $outputFolder