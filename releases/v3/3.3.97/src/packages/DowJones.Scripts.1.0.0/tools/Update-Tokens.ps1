param(
	[Parameter(Mandatory=$true,ValueFromPipeline=$true)][string]$Product,
	[Parameter(Mandatory=$false)][string]$DestinationFolder
)

if ($DestinationFolder -eq "")
{
	try {
		$project = Get-Project

		Write-Host "Updating tokens for" $project.Name

		$DestinationFolder = $project.FullName | Split-Path -parent
	}
	catch {
		$DestinationFolder = (Get-Location).Path
	}
}

$url = "http://translate.factiva.com/V2/data/${Product}/bin.zip"
$localFilename = "${DestinationFolder}\bin.zip"


# Download the tokens file
$request = new-object System.Net.WebClient
Write-Host "Downloading ${url} to ${localFilename}..."
$request.DownloadFile($url, $localFilename)
Write-Host "Downloaded ${localFilename}."


# Unzip the downloaded zip
$shell_app=new-object -com shell.application 
$zip_file = $shell_app.namespace($localFilename) 
$destination = $shell_app.namespace($DestinationFolder) 

Write-Host "Unzipping ${localFilename} to ${DestinationFolder}..."
$destination.CopyHere($zip_file.items(), 0x14)

Write-Host "Deleting ${localFilename}..."
rm -Recurse $localFilename


Write-Host "Done!"