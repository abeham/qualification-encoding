# find ms build
$programFilesX86Dir = ($Env:ProgramFiles, ${Env:ProgramFiles(x86)})[[bool]${Env:ProgramFiles(x86)}]
$vsDir = [System.IO.Path]::Combine($programFilesX86Dir, "Microsoft Visual Studio")
$years = @("2019", "2017")
$editions = @("Enterprise", "Professional", "Community", "BuildTools")
$versions = @("Current", "15.0")

$msBuildPath = $undefined
:search Foreach ($year in $years) {
  $loc = [System.IO.Path]::Combine($vsDir, $year)
  Foreach ($edition in $editions) {
    $edLoc = [System.IO.Path]::Combine($loc, $edition, "MSBuild")
    Foreach ($version in $versions) {
      $binLoc = [System.IO.Path]::Combine($edLoc, $version, "Bin")
      $loc64 = [System.IO.Path]::Combine($binLoc, "amd64", "MSBuild.exe")
      $loc32 = [System.IO.Path]::Combine($binLoc, "MSBuild.exe")

      If ([System.IO.File]::Exists($loc64)) {
        $msBuildPath = $loc64
        Break search;
      }
      If ([System.IO.File]::Exists($loc32)) {
        $msBuildPath = $loc32
        Break search;
      }
    }
  }
}

Try {
  If ($msBuildPath -eq $undefined) {
    "Could not locate MSBuild, ABORTING ..."
    Return
  }

  "MSBuild located at `"{0}`"." -f $msBuildPath
  
  $config = "Release"
  $input = Read-Host "Which configuration to build? [$($config)]"
  $config = ($config, $input)[[bool]$input]

  # query clean desire
  Do {
      $trunk = $true
      $input = Read-Host "Would you like to build the trunk? [Y/n]"
      $input = ([string]("y", $input)[[bool]$input]).ToLowerInvariant()
      If ($input -eq "n") {
        $trunk = $false
        $success = $true
      }
      $success = $success -or ($input -eq "y")
  } While (-Not $success)
  
  if ($trunk) {
    Push-Location trunk
     "Building `"HeuristicLab.ExtLibs.sln`" ($config) ..."
    $args = @(
      "HeuristicLab.ExtLibs.sln",
      "/t:Restore,Build",
      "/p:Configuration=`"$config`"",
      "/m", "/nologo", "/verbosity:q", "/clp:ErrorsOnly"
    )
    & $msBuildPath $args
     "Building `"HeuristicLab 3.3.sln`" ($config) ..."
    $args = @(
      "HeuristicLab 3.3.sln",
      "/t:Restore,Build",
      "/p:Configuration=`"$config`"",
      "/m", "/nologo", "/verbosity:q", "/clp:ErrorsOnly"
    )
    & $msBuildPath $args
    
    Pop-Location
  }
  
  Push-Location addons\2987_MOEAD_Algorithm
 "Building `"HeuristicLab.Algorithms.MOEAD-3.4.sln`" ($config) ..."
  $args = @(
    "HeuristicLab.Algorithms.MOEAD-3.4.sln",
    "/t:Restore,Build",
    "/p:Configuration=`"$config`"",
    "/m", "/nologo", "/verbosity:q", "/clp:ErrorsOnly"
  )
  & $msBuildPath $args
  
  Pop-Location

  Push-Location branches
 "Building `"WorkerCrosstraining.sln`" ($config) ..."
  $args = @(
    "WorkerCrosstraining.sln",
    "/t:Restore,Build",
    "/p:Configuration=`"$config`"",
    "/m", "/nologo", "/verbosity:q", "/clp:ErrorsOnly"
  )
  & $msBuildPath $args
  
  Pop-Location
} Finally {
  ""

  Write-Host -NoNewline "Press any key to continue ... "

  [void][System.Console]::ReadKey($true)
}
