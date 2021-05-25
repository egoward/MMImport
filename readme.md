## Edonica MasterMap Importer

See https://www.edonica.com/MMImport/index.html for details.

Uploading MMImport.20091029.zip here and converting source code in this repository to the MIT license.  Naturally this license applies to source files in this repository which I am the sole author of.

Notes:
- This code was last compiled with Visual Studio 2008 and will take a little work to migrate

## Change log May 25 2021

Migrated to .Net 472 / NuGet / High DPI and tested with:
- Visual Studio 2019 (16.9.6)
- SQL Server 2019 Developer (some faff with native DLLs)
- PostGres 13 (13.3 running locally)

All still works with geometry visible in PGAdmin and SQL Server Management Console.

Known issues:
- Migration to .Net Core looks preferable
- File layout worked well as a Zip, not so much in GIT
- Native dependencies require copying files around in a non-obvious way:
  - Copy from %HOMEPATH%\\.nuget\packages\gdal.native\2.4.4\build\gdal\x64\*
  - To ~\Source\bin\Debug\*
