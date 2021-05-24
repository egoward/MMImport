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
                xmlns:xlink='http://www.w3.org/1999/xlink'
                xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
                
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


  <xsl:template match="osgb:departedMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:DepartedFeature" mode="departedMember"/>
    </TransformOutput>
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

  <xsl:template match="osgb:networkMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:RoadNode" mode="networkMember"/>
      <xsl:apply-templates select="./osgb:RoadLink" mode="networkMember"/>
    </TransformOutput>
  </xsl:template>

  <xsl:template match="osgb:roadInformationMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:RoadRouteInformation" mode="roadInformationMember"/>
      <xsl:apply-templates select="./osgb:RoadLinkInformation" mode="roadInformationMember"/>
    </TransformOutput>
  </xsl:template>

  <xsl:template match="osgb:roadMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:Road" mode="roadMember"/>
    </TransformOutput>
  </xsl:template>

  <xsl:template match="osgb:RoadNode" mode="networkMember">
    <RoadNode>
      <TOID>
        <xsl:value-of select="@fid"/>
      </TOID>
      <Version>
        <xsl:value-of select="./osgb:version"/>
      </Version>
      <VersionDate>
        <xsl:value-of select="./osgb:versionDate"/>
      </VersionDate>
      <TopographicAreaTOID>
        <xsl:value-of select="substring(./osgb:referenceToTopographicArea/@xlink:href,2)"/>
      </TopographicAreaTOID>
      <Geom>
        <xsl:copy-of select="./osgb:point/*"/>
      </Geom>
    </RoadNode>
  </xsl:template>

  <xsl:template match="osgb:RoadRouteInformation" mode="roadInformationMember">
    <RoadRouteInformation>
      <TOID>
        <xsl:value-of select="@fid"/>
      </TOID>
      <Version>
        <xsl:value-of select="./osgb:version"/>
      </Version>
      <VersionDate>
        <xsl:value-of select="./osgb:versionDate"/>
      </VersionDate>
      <Summary>Something</Summary>
    </RoadRouteInformation>

    <xsl:for-each select="./osgb:directedLink">
      <RoadRouteInformationToRoadLink>
        <RoadRouteInformationTOID>
          <xsl:value-of select="../@fid"/>
        </RoadRouteInformationTOID>
        <RoadLinkTOID>
          <xsl:value-of select="substring(./@xlink:href,2)"/>
        </RoadLinkTOID>
        <Orientation>
          <xsl:value-of select="./@orientation"/>
        </Orientation>
      </RoadRouteInformationToRoadLink>
    </xsl:for-each>
    
    <xsl:call-template name="OSRoadRoutingInformation"/>
    
  </xsl:template>


  <xsl:template match="osgb:RoadLinkInformation" mode="roadInformationMember">
    <RoadLinkInformation>
      <TOID>
        <xsl:value-of select="@fid"/>
      </TOID>
      <Version>
        <xsl:value-of select="./osgb:version"/>
      </Version>
      <VersionDate>
        <xsl:value-of select="./osgb:versionDate"/>
      </VersionDate>
      <RoadLinkTOID>
        <xsl:value-of select="substring(./osgb:referenceToRoadLink/@xlink:href,2)"/>
      </RoadLinkTOID>
    </RoadLinkInformation>
    <xsl:call-template name="OSRoadRoutingInformation"/>
  </xsl:template>


  <xsl:template match="osgb:Road" mode="roadMember">
    <Road>
      <TOID>
        <xsl:value-of select="@fid"/>
      </TOID>
      <Version>
        <xsl:value-of select="./osgb:version"/>
      </Version>
      <VersionDate>
        <xsl:value-of select="./osgb:versionDate"/>
      </VersionDate>
      <DescriptiveGroup>
        <xsl:value-of select="./osgb:descriptiveGroup"/>
      </DescriptiveGroup>
      <DescriptiveTerms>
        <xsl:value-of select="./osgb:descriptiveTerm"/>
      </DescriptiveTerms>
      <RoadName>
        <xsl:value-of select="./osgb:roadName"/>
      </RoadName>
    </Road>
    <xsl:for-each select="./osgb:networkMember">
      <RoadToRoadLink>
        <RoadTOID>
          <xsl:value-of select="../@fid"/>
        </RoadTOID>
        <RoadLinkTOID>
          <xsl:value-of select="substring(./@xlink:href,2)"/>
        </RoadLinkTOID>
      </RoadToRoadLink>
    </xsl:for-each>
  </xsl:template>


  <xsl:template match="osgb:RoadLink" mode="networkMember">
    <RoadLink>
      <TOID>
        <xsl:value-of select="@fid"/>
      </TOID>
      <Version>
        <xsl:value-of select="./osgb:version"/>
      </Version>
      <VersionDate>
        <xsl:value-of select="./osgb:versionDate"/>
      </VersionDate>
      <DescriptiveTerm>
        <xsl:value-of select="./osgb:descriptiveTerm"/>
      </DescriptiveTerm>
      <NatureOfRoad>
        <xsl:value-of select="./osgb:natureOfRoad"/>
      </NatureOfRoad>
      <Length>
        <xsl:value-of select="./osgb:length"/>
      </Length>
      <RoadNodeFrom>
        <xsl:value-of select="substring(./osgb:directedNode[@orientation='-']/@xlink:href,2)"/>
      </RoadNodeFrom>
      <RoadNodeTo>
        <xsl:value-of select="substring(./osgb:directedNode[@orientation='+']/@xlink:href,2)"/>
      </RoadNodeTo>
      <Geom>
        <xsl:copy-of select="./osgb:polyline/*"/>
      </Geom>
    </RoadLink>
    <xsl:for-each select="./osgb:referenceToTopographicArea">
      <RoadLinkToTopographicArea>
        <RoadLinkTOID>
          <xsl:value-of select="../@fid"/>
        </RoadLinkTOID>
        <TopographicAreaTOID>
          <xsl:value-of select="substring(./@xlink:href,2)"/>
        </TopographicAreaTOID>
      </RoadLinkToTopographicArea>
    </xsl:for-each>
  </xsl:template>

  <!--
  Dump out any qualifiers etc. for a given element
  In line with Ordnance Survey "Road Routing Information"
  -->
  <xsl:template name="OSRoadRoutingInformation">
    <xsl:for-each select="./osgb:dateTimeQualifier">
    </xsl:for-each>
    <xsl:for-each select="./osgb:vehicleQualifier">
      <xsl:for-each select="./*">
        <Qualifier>
          <ItemTOID>
            <xsl:value-of select="../../@fid"/>
          </ItemTOID>
          <Type>
            <xsl:value-of select="name(.)"/>
          </Type>
          <Value>
            <xsl:value-of select="."/>
          </Value>
        </Qualifier>
      </xsl:for-each>
    </xsl:for-each>
    <xsl:for-each select="./osgb:environmentQualifier">
        <xsl:for-each select="./*">
          <Qualifier>
            <ItemTOID>
              <xsl:value-of select="../../@fid"/>
            </ItemTOID>
            <Type>
              <xsl:value-of select="name(.)"/>
            </Type>
            <Value>
              <xsl:value-of select="."/>
            </Value>
          </Qualifier>
        </xsl:for-each>
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>