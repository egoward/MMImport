Please refer to "Index.html" file in the "Doc" folder for instructions.

2009 April 25th
===============
Added support for SQL Server 2008

2008/06/01
==========
Fix problem with default PSQL command line 

2008/05/27
==========
Some file reoganision.
Added switch to split input files to separate directories in output
Added switch to append input files to a single output file for OGR driver.

2007/11/11
==========
Added support for additional item types in OSTN02 map reprojection.

2007/11/08
==========
Fixed bug with OSTN02 conversion again.  Data file was corrupt


2007/10/28
==========
Fixed bug with OSTN02 conversion


2007/10/21
==========
Added initial Integrated Transport Network / ITN / Road network XSL/XSD

2007/10/16
==========
Fixed a problem with open file handles on ESRI Shape export (each tile goes to it's own directory now)
Fixed some leaks
Fixed command line processing

2007/10/15
==========
Finished OSGB1936 <-> WGS84 conversion using OSTN02
Tidied up SHP Export to include SHP filename if you import multiple files
Added 'ReferenceToTopographicArea' to Address Layer 1


2007/10/14
==========
Added GDAL dependency and 5Mb (!) of DLL
Added ability to write SHP maps from MasterMap
Actually generic GDAL output support so can probably do MIF etc.
Added AddressLayer 2 and Address Layer 1 thanks to James / Calnea
Select the appropriate XSL and XSD files before running the import.
Started implementing function to reproject maps using OSTN02 and read/write using GDAL
TODO: Finish reprojection and driver picker for GDAL output.


2007/10/06
==========
Added support for XSL and XSD for importing Address Layer 2

2007/09/25
==========
Added OSTN02 library for accurate OSGB1936 <-> WGS84 conversions
Added option to import OSTN grid correction data from OS.
Added support for OSGB1936->WGS84 Support (EPSG:27700 -> EPSG:4326)
Make sure you change the SRID when you set it to WGS84 mode.

2007/06/08
==========
Wired up File > Exit menu
Provided CommandTimeout override

2007/06/06
==========
Updated documentation about installing Postgres
Fixed bug when you press the Cancel button
Added logging to a file.
Improved logging in the PostGres driver
Improved logging in the progress window.

2007/06/03
==========
Initial Release


