<?xml version="1.0" encoding="utf-8" ?>
<!-- 
  Copyright Edonica
  
  Based on a best effort reproduction of the Ordnance Survey XSL style guide.
  Style guide is Ordnance Survey (c) Crown Copyright, All rights reserved
-->
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:osgb="http://www.ordnancesurvey.co.uk/xml/namespaces/osgb"
                xmlns:gml="http://www.opengis.net/gml"
                xmlns:xmlin="http://www.edonica.com/xmlin1"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                >
  <xsl:output method="xml" indent="yes" />

  <!-- 
  ########################################################################
  # Items matched in "/"
  #
  -->
  <!--Things we can quietly ignore-->
  <xsl:template match="gml:description">
    <TransformOutput>
      <TransformLog>
        GML Description : <xsl:value-of select="normalize-space(.)"/>
      </TransformLog>
    </TransformOutput>
  </xsl:template>

  <xsl:template match="gml:boundedBy">
    <TransformOutput>
      <TransformLog>
        GML bounded by : <xsl:value-of select="normalize-space(.)"/>
      </TransformLog>
    </TransformOutput>
  </xsl:template>
  <xsl:template match="osgb:queryTime">
    <TransformOutput>
      <TransformLog>
        Query time : <xsl:value-of select="normalize-space(.)"/>
      </TransformLog>
    </TransformOutput>

  </xsl:template>
  <xsl:template match="osgb:queryExtent">
    <TransformOutput>
      <TransformLog>
        OS Query extent : <xsl:value-of select="normalize-space(.)"/>
      </TransformLog>
    </TransformOutput>
  </xsl:template>
  <xsl:template match="osgb:boundedBy">
    <TransformOutput>
      <TransformLog>
        OS Bounded By : <xsl:value-of select="normalize-space(.)"/>
      </TransformLog>
    </TransformOutput>
  </xsl:template>
  <xsl:template match="osgb:queryChangeSinceDate">
    <TransformOutput>
      <TransformLog>
        OS Query Change Since Date : <xsl:value-of select="normalize-space(.)"/>
      </TransformLog>
    </TransformOutput>
  </xsl:template>

  <xsl:template match="osgb:boundaryMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:BoundaryLine" mode="boundaryMember"/>
    </TransformOutput>
  </xsl:template>

  <xsl:template match="osgb:cartographicMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:CartographicText" mode="cartographicMember"/>
      <xsl:apply-templates select="./osgb:CartographicSymbol" mode="cartographicMember"/>
    </TransformOutput>
  </xsl:template>

  <xsl:template match="osgb:topographicMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:TopographicPoint" mode="topographicMember"/>
      <xsl:apply-templates select="./osgb:TopographicLine" mode="topographicMember"/>
      <xsl:apply-templates select="./osgb:TopographicArea" mode="topographicMember"/>
    </TransformOutput>
  </xsl:template>

  <xsl:template match="osgb:departedMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:DepartedFeature" mode="departedMember"/>
    </TransformOutput>  
  </xsl:template>

  <xsl:template match="osgb:TopographicPoint" mode="topographicMember">
    <xsl:variable name="descriptiveGroups">
      <xsl:for-each select="./osgb:descriptiveGroup">
        <xsl:value-of select="normalize-space(.)"/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
    </xsl:variable>
    <xsl:variable name="descriptiveTerms">
      <xsl:for-each select="./osgb:descriptiveTerm">
        <xsl:value-of select="normalize-space(.)"/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
    </xsl:variable>
    <!-- work out what symbol to draw -->
    <xsl:variable name="symbol">
      <xsl:choose>
        <xsl:when test="contains($descriptiveTerms,'Positioned Nonconiferous Tree;')">
          positionedNonconiferousTreeSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Positioned Coniferous Tree;')">
          positionedConiferousTreeSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Positioned Boulder;')">
          positionedBoulderSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Site Of Heritage;')">
          heritageSiteOfSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveGroups,'Inland Water;')">
          waterPointSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Disused Feature;')">
          landformDisusedSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Boundary Post Or Stone;')">
          boundaryPostSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Switch;')">
          railwaySwitchSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Air Height;')">
          airHeightSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Spot Height;')">
          spotHeightSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Triangulation Point Or Pillar;')">
          triangulationStationSymbol
        </xsl:when>
        <xsl:otherwise>
          pointSymbol
        </xsl:otherwise>

      </xsl:choose>
    </xsl:variable>
    <TopographicPoint>
      <TOID>
        <xsl:value-of select="./@fid"/>
      </TOID>
      <FeatureCode>
        <xsl:value-of select="./osgb:featureCode/."/>
      </FeatureCode>
      <PointClass>
        <!--
        Groups: <xsl:value-of select="$descriptiveGroups"/>
        Temps: <xsl:value-of select="$descriptiveTerms"/>
        -->
        <xsl:value-of select="$symbol"/>
      </PointClass>
      <Geom>
        <xsl:copy-of select="./osgb:point/*"/>
      </Geom>
    </TopographicPoint>
  </xsl:template>

  <xsl:template match="osgb:TopographicLine" mode="topographicMember">
    <xsl:variable name="descriptiveGroups">
      <xsl:for-each select="./osgb:descriptiveGroup">
        <xsl:value-of select="normalize-space(.)"/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
    </xsl:variable>
    <xsl:variable name="descriptiveTerms">
      <xsl:for-each select="./osgb:descriptiveTerm">
        <xsl:value-of select="normalize-space(.)"/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
    </xsl:variable>
    <xsl:variable name="physicalPresence">
      <xsl:value-of select="normalize-space(./osgb:physicalPresence)"/>
    </xsl:variable>
    <xsl:variable name="class">
      <xsl:choose>
        <!-- TopographicLine and BoundaryLine types -->
        <xsl:when test="contains($descriptiveGroups,'Building;') and ($physicalPresence = 'Overhead')">buildingOverheadLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Course Of Heritage;')">defaultUndergroundLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Overhead Construction;')">structureOverheadLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Tunnel Edge;')">defaultUndergroundLine</xsl:when>
        <xsl:when test="contains($descriptiveGroups,'Building;')">buildingLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Mean High Water (Springs);')">waterBoldLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Mean Low Water (Springs);')">waterDashedLine</xsl:when>
        <xsl:when test="contains($descriptiveGroups,'Inland Water;')">waterLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Narrow Gauge;')">narrowGaugeRailwayAlignmentLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Standard Gauge Track;')">standardGaugeRailLine</xsl:when>
        <xsl:when test="contains($descriptiveGroups,'Landform;') and (contains($descriptiveTerms,'Top Of Slope;'))">landformBoldLine</xsl:when>
        <xsl:when test="contains($descriptiveGroups,'Landform;') and (contains($descriptiveTerms,'Top Of Cliff;'))">landformBoldLine</xsl:when>
        <xsl:when test="contains($descriptiveGroups,'Landform;') and (contains($descriptiveTerms,'Bottom Of Slope;'))">landformLine</xsl:when>
        <xsl:when test="contains($descriptiveGroups,'Landform;') and (contains($descriptiveTerms,'Bottom Of Cliff;'))">landformLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Parish;')">parishLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Electoral;')">electoralLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'County;')">countyLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Parliamentary;')">parliamentaryLine</xsl:when>
        <xsl:when test="contains($descriptiveTerms,'District;')">districtLine</xsl:when>
        <xsl:when test="$physicalPresence = 'Edge / Limit'">defaultDashedLine</xsl:when>
        <xsl:when test="$physicalPresence = 'Underground'">defaultUndergroundLine</xsl:when>
        <xsl:when test="$physicalPresence = 'Closing'">closingLine</xsl:when>
        <xsl:otherwise>defaultLine</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>


    <TopographicLine>
      <TOID>
        <xsl:value-of select="./@fid"/>
      </TOID>
      <FeatureCode>
        <xsl:value-of select="./osgb:featureCode/."/>
      </FeatureCode>
      <LineClass>
        <xsl:value-of select="$class"/>
      </LineClass>
      <Geom>
        <xsl:copy-of select="./osgb:polyline/*"/>
      </Geom>
    </TopographicLine>
  </xsl:template>

  <!--  <xsl:template match="osgb:TopographicArea" mode="topographicMember">-->
  <xsl:template match="osgb:TopographicArea" mode="topographicMember">

    <xsl:variable name="descriptiveGroups">
      <xsl:for-each select="./osgb:descriptiveGroup">
        <xsl:value-of select="normalize-space(.)"/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
    </xsl:variable>

    <xsl:variable name="descriptiveTerms">
      <xsl:for-each select="./osgb:descriptiveTerm">
        <xsl:value-of select="normalize-space(.)"/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
    </xsl:variable>

    <xsl:variable name="make">
      <xsl:value-of select="normalize-space(./osgb:make)"/>
    </xsl:variable>

    <xsl:variable name="physicalPresence">
      <xsl:value-of select="normalize-space(./osgb:physicalPresence)"/>
    </xsl:variable>

    <!-- ignore slopes and pylons -->
    <xsl:choose>
      <xsl:when test="contains($descriptiveTerms,'Pylon;') or contains($descriptiveGroups,'Landform;')">
        <xsl:variable name="class">
          <xsl:if test="contains($descriptiveTerms,'Pylon;')">
            structureFill
          </xsl:if>
        </xsl:variable>

        <xsl:variable name="patternFill">
          <xsl:if test="contains($descriptiveGroups,'Landform;')">
            <xsl:choose>
              <xsl:when test="$make = 'Manmade'">manmadeLandformPattern</xsl:when>
              <xsl:when test="$make = 'Natural'">naturalLandformPattern</xsl:when>
            </xsl:choose>
          </xsl:if>
        </xsl:variable>
        <TopDetail>
          <TOID>
            <xsl:value-of select="./@fid"/>
          </TOID>
          <FeatureCode>
            <xsl:value-of select="./osgb:featureCode/."/>
          </FeatureCode>
          <Geom>
            <xsl:copy-of select="./osgb:polygon/*"/>
          </Geom>
          <FillClass>
            <xsl:value-of select="$class"/>
          </FillClass>
          <FillPattern>
            <xsl:value-of select="$patternFill"/>
          </FillPattern>
        </TopDetail>
      </xsl:when>
      <xsl:otherwise>
        <!-- select fill colour based on descriptiveGroup value -->
        <xsl:variable name="class">
          <xsl:choose>
            <xsl:when test="contains($descriptiveGroups,'Building;')">buildingFill</xsl:when>
            <xsl:when test="contains($descriptiveTerms,'Step;')">stepFill</xsl:when>
            <xsl:when test="contains($descriptiveGroups,'Glasshouse;')">glasshouseFill</xsl:when>
            <xsl:when test="contains($descriptiveGroups,'Historic Interest;')">heritageFill</xsl:when>
            <xsl:when test="contains($descriptiveGroups,'Inland Water;')">inlandWaterFill</xsl:when>
            <xsl:when test="contains($descriptiveGroups,'Natural Environment;')">naturalEnvironmentFill</xsl:when>
            <xsl:when test="contains($descriptiveGroups,'Path;')">pathFill</xsl:when>
            <xsl:when test="contains($descriptiveGroups,'Road Or Track;')">roadFill</xsl:when>
            <xsl:when test="contains($descriptiveGroups,'Structure;')">structureFill</xsl:when>
            <xsl:when test="contains($descriptiveGroups,'Tidal Water;')">tidalWaterFill</xsl:when>
            <xsl:when test="contains($descriptiveGroups,'Unclassified;')">unclassifiedFill</xsl:when>
            <xsl:when test="(contains($descriptiveGroups,'Rail;')) and (contains($make,'Manmade'))">railFill</xsl:when>
            <xsl:when test="$make = 'Manmade'">madeSurfaceFill</xsl:when>
            <xsl:when test="$make = 'Natural'">naturalSurfaceFill</xsl:when>
            <xsl:when test="$make = 'Unknown'">madeSurfaceFill</xsl:when>
            <xsl:when test="$make = 'Multiple'">multipleSurfaceFill</xsl:when>
            <xsl:otherwise>unclassifiedFill</xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <!-- select pattern fill if required and draw with an svg use element -->
        <xsl:variable name="nofDTs">
          <xsl:value-of select="count(./osgb:descriptiveTerm)"/>
        </xsl:variable>
        <xsl:variable name="patternFill">
          <xsl:if test="contains($descriptiveGroups,'Natural Environment;') or contains($descriptiveTerms,'Foreshore;')">
            <xsl:choose>
              <xsl:when test="$nofDTs = 3">
                <xsl:choose>
                  <xsl:when test="contains($descriptiveTerms,'Rock;') and 
												(contains($descriptiveTerms,'Rough Grassland;')) and 
												(contains($descriptiveTerms,'Boulders;'))">rocksRoughGrassAndBouldersPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Nonconiferous Trees;')) and 
												(contains($descriptiveTerms,'Coniferous Trees;'))">roughGrassNonconiferousTreesAndConiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Nonconiferous Trees;')) and 
												(contains($descriptiveTerms,'Scrub;'))">roughGrassNonconiferousTreesAndScrubPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Rock (Scattered);')) and 
												(contains($descriptiveTerms,'Boulders;'))">roughGrassScatteredRocksAndBouldersPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Rock (Scattered);')) and 
												(contains($descriptiveTerms,'Heath;'))">roughGrassScatteredRocksAndHeathPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Rock (Scattered);')) and 
												(contains($descriptiveTerms,'Boulders (Scattered);'))">roughGrassScatteredRocksAndScatteredBouldersPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Nonconiferous Trees (Scattered);')) and 
												(contains($descriptiveTerms,'Scrub;'))">roughGrassScatteredNonconiferousTreesAndScrubPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Scrub;') and 
												(contains($descriptiveTerms,'Coniferous Trees;')) and 
												(contains($descriptiveTerms,'Nonconiferous Trees;'))">scrubConiferousTreesAndNonconiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Scrub;') and 
												(contains($descriptiveTerms,'Nonconiferous Trees;')) and 
												(contains($descriptiveTerms,'Coppice Or Osiers;'))">scrubNonconiferousTreesAndCoppicePattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Coniferous Trees (Scattered);') and 
												(contains($descriptiveTerms,'Nonconiferous Trees (Scattered);')) and 
												(contains($descriptiveTerms,'Scrub;'))">scatteredConiferousTreesScatteredNonconiferousTreesAndScrubPattern</xsl:when>
                  <xsl:otherwise>multiVegetationPattern</xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="$nofDTs = 2">
                <xsl:choose>
                  <xsl:when test="contains($descriptiveTerms,'Coniferous Trees;') and 
												(contains($descriptiveTerms,'Rock (Scattered);'))">coniferousTreesAndScatteredRocksPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Coniferous Trees;') and 
												(contains($descriptiveTerms,'Scrub;'))">coniferousTreesAndScrubPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Heath;') and 
												(contains($descriptiveTerms,'Scrub;'))">heathAndScrubPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Heath;') and 
												(contains($descriptiveTerms,'Rock (Scattered);'))">heathAndScatteredRocksPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Nonconiferous Trees;') and 
												(contains($descriptiveTerms,'Coniferous Trees;'))">nonconiferousTreesAndConiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Nonconiferous Trees;') and 
												(contains($descriptiveTerms,'Coppice Or Osiers;'))">nonconiferousTreesAndCoppicePattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Nonconiferous Trees;') and 
												(contains($descriptiveTerms,'Rock (Scattered);'))">nonconiferousTreesAndScatteredRocksPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Nonconiferous Trees;') and 
												(contains($descriptiveTerms,'Scrub;'))">nonconiferousTreesAndScrubPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Boulders;'))">roughGrassAndBouldersPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Coniferous Trees;'))">roughGrassAndConiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and
												(contains($descriptiveTerms,'Heath;'))">roughGrassAndHeathPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Marsh Reeds Or Saltmarsh;'))">roughGrassAndMarshPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Nonconiferous Trees;'))">roughGrassAndNonconiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Rock;'))">roughGrassAndRocksPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Boulders (Scattered);'))">roughGrassAndScatteredBouldersPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Nonconiferous Trees (Scattered);'))">roughGrassAndScatteredNonconiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Rock (Scattered);'))">roughGrassAndScatteredRocksPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;') and 
												(contains($descriptiveTerms,'Scrub;'))">roughGrassAndScrubPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Nonconiferous Trees (Scattered);') and 
											 	(contains($descriptiveTerms,'Coniferous Trees (Scattered);'))">scatteredNonconiferousTreesAndScatteredConiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Scrub;') and 
					   							(contains($descriptiveTerms,'Nonconiferous Trees (Scattered);'))">scrubAndScatteredNonconiferousTreesPattern</xsl:when>
                  <xsl:otherwise>multiVegetationPattern</xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="$nofDTs = 1">
                <xsl:choose>
                  <xsl:when test="contains($descriptiveTerms,'Rough Grassland;')">roughGrassPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Marsh Reeds Or Saltmarsh;')">marshPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Foreshore;')">foreshorePattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Scrub;')">scrubPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Heath;')">heathPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Coppice Or Osiers;')">coppicePattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Orchard;')">orchardPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Coniferous Trees;')">coniferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Coniferous Trees (Scattered);')">scatteredConiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Nonconiferous Trees;')">nonconiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Nonconiferous Trees (Scattered);')">scatteredNonconiferousTreesPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Boulders;')">bouldersPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Boulders (Scattered);')">scatteredBouldersPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rock;')">rocksPattern</xsl:when>
                  <xsl:when test="contains($descriptiveTerms,'Rock (Scattered);')">scatteredRocksPattern</xsl:when>
                  <xsl:otherwise>multiVegetationPattern</xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>multiVegetationPattern</xsl:otherwise>
            </xsl:choose>
          </xsl:if>
          <!-- natural environment -->
        </xsl:variable>
        <TopographicArea>
          <TOID>
            <xsl:value-of select="./@fid"/>
          </TOID>
          <FeatureCode>
            <xsl:value-of select="./osgb:featureCode/."/>
          </FeatureCode>
          <Geom>
            <xsl:copy-of select="./osgb:polygon/*"/>
          </Geom>
          <FillClass>
            <xsl:value-of select="$class"/>
          </FillClass>
          <FillPattern>
            <xsl:value-of select="$patternFill"/>
          </FillPattern>
        </TopographicArea>
      </xsl:otherwise>
    </xsl:choose>
    <!-- landform -->
  </xsl:template>


  <xsl:template match="osgb:CartographicText" mode="cartographicMember">

    <xsl:variable name="descriptiveGroups">
      <xsl:for-each select="./osgb:descriptiveGroup">
        <xsl:value-of select="normalize-space(.)"/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
    </xsl:variable>

    <!-- set colour -->
    <xsl:variable name="textColour">
      <xsl:choose>
        <xsl:when test="contains($descriptiveGroups,'Inland Water;')">0099FF</xsl:when>
        <xsl:when test="contains($descriptiveGroups,'Tidal Water;')">0099FF</xsl:when>
        <xsl:when test="contains($descriptiveGroups,'Political Or Administrative;')">FF00FF</xsl:when>
        <xsl:otherwise>000000</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!-- set colour -->
    <xsl:variable name="textStyle">
      <xsl:choose>
        <xsl:when test="contains($descriptiveGroups,'Historic Interest')">1</xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <CartographicText>
      <TOID>
        <xsl:value-of select="./@fid"/>
      </TOID>
      <FeatureCode>
        <xsl:value-of select="./osgb:featureCode"/>
      </FeatureCode>
      <TextString>
        <xsl:value-of select="./osgb:textString" />
      </TextString>
      <Height>
        <xsl:value-of select="./osgb:textRendering/osgb:height"/>
      </Height>
      <Orientation>
        <xsl:value-of select="./osgb:textRendering/osgb:orientation"/>
      </Orientation>
      <FontNumber>
        <xsl:value-of select="./osgb:textRendering/osgb:font"/>
      </FontNumber>
      <AnchorPosition>
        <xsl:value-of select="./osgb:textRendering/osgb:anchorPosition"/>
      </AnchorPosition>
      <Col>
        <xsl:value-of select="$textColour"/>
      </Col>
      <TextStyle>
        <xsl:value-of select="$textStyle"/>
      </TextStyle>
      <Geom>
        <xsl:copy-of select="./osgb:anchorPoint/*"/>
      </Geom>
    </CartographicText>
  </xsl:template>

  <xsl:template match="osgb:CartographicSymbol" mode="cartographicMember">
    <xsl:variable name="descriptiveTerms">
      <xsl:for-each select="./osgb:descriptiveTerm">
        <xsl:value-of select="normalize-space(.)"/>
        <xsl:text>;</xsl:text>
      </xsl:for-each>
    </xsl:variable>
    <!-- work out what symbol to draw -->
    <xsl:variable name="symbol">
      <xsl:choose>
        <xsl:when test="contains($descriptiveTerms,'Bench Mark;')">
          benchMarkSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Culvert;')">
          culvertSymbol
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Direction Of Flow;')">
          <xsl:text>flowArrowSymbol</xsl:text>
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Boundary Half Mereing;')">
          <xsl:text>boundaryMereingChangeSymbol</xsl:text>
        </xsl:when>
        <xsl:when test="contains($descriptiveTerms,'Road Related Flow;')">
          <xsl:text>roadFlowSymbol</xsl:text>
        </xsl:when>
        <xsl:otherwise>pointSymbol</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <CartographicSymbol>
      <TOID>
        <xsl:value-of select="./@fid"/>
      </TOID>
      <FeatureCode>
        <xsl:value-of select="./osgb:featureCode"/>
      </FeatureCode>
      <Orientation>
        <xsl:value-of select="./osgb:orientation"/>
      </Orientation>
      <SymbolClass>
        <xsl:value-of select="$symbol"/>
      </SymbolClass>
      <Geom>
        <xsl:copy-of select="./osgb:point/*"/>
      </Geom>
    </CartographicSymbol>
  </xsl:template>


  <!--Boundary line data-->


  <xsl:template match="osgb:BoundaryLine" mode="boundaryMember">
    <BoundaryLine>
      <TOID>
        <xsl:value-of select="@fid"/>
      </TOID>
      <FeatureCode>
        <xsl:value-of select="./osgb:featureCode"/>
      </FeatureCode>
      <Geom>
        <xsl:copy-of select="./osgb:polyline/*"/>
      </Geom>
    </BoundaryLine>
  </xsl:template>


  <xsl:template match="osgb:DepartedFeature" mode="departedMember">
    <DepartedFeature>
      <TOID>
        <xsl:value-of select="@fid"/>
      </TOID>
      <ReasonForDeparture>
        <xsl:value-of select="./osgb:reasonForDeparture"/>
      </ReasonForDeparture>
    </DepartedFeature>
  </xsl:template>

  </xsl:stylesheet>