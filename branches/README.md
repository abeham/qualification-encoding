# Worker Cross Training Problem and Encoding

## Build

You need to have .NET Framework 4.6.1 and at least Visual Studio 2017.
It has been tested on Windows 10.

 1. Create folder trunk and checkout revision 2588b5ad0488b251f81ca96f67d87b86f58c7354 of https://github.com/HeuristicLab/HeuristicLab.git
 1. Build trunk/HeuristicLab.ExtLibs.sln
 2. Build trunk/HeuristicLab.sln
 3. Build Crosstraining/Crosstraining.sln

## Run Experiments

1. Start trunk/bin/HeuristicLab 3.3.exe
2. Select File>New... and create a NSGA-II
3. In the Problem Tab, click new Problem Button and set either the bi-objective or tri-objective problem
