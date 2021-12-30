# Initialization script. This script needs to run the very first the application starts.
# pass "dev" to setup local environment
# after susccessful setup, go to <localhost>/init/ to continue with the setup

$env=$args[0]

$AcceptedEnvs = "prod", "dev"
$CheckArg = $AcceptedEnvs.Contains($env)

if (!$CheckArg) {
	return "ENV not accepted: MUST be 'prod' OR 'dev'"
}

Write-Host "Preparing to update database for: " $env

if ($env -eq "prod") {
Write-Host "Targetting Azure: "
$connectionString = "Server=tcp:vpm-app.database.windows.net,1433;Initial Catalog=vpm-db;Persist Security Info=False;User ID=serveradmin;Password=rSUm4gEKrPVOjgSwDCau;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
} 

if ($env -eq "dev") {
Write-Host "Targetting LocalDB: "
$connectionString = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VPM;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False"
}

Write-Host "Connection String: "
Write-Host $connectionString

if (Test-Path -Path Migrations) {
	# Delete old migrations
	rm Migrations -r
}

# Recreate migrations
dotnet ef migrations add InitialCreate
# Setup the ConnectionStrings based on the selected ENV
$env:ConnectionStrings:DefaultConnection=$connectionString
# Update the targeting database
dotnet ef database update