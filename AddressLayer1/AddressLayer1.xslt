<?xml version="1.0" encoding="utf-8" ?>
<!-- 
  Copyright Edonica
  
  Based on a best effort reproduction of the Ordnance Survey XSL style guide.
  Style guide is Ordnance Survey (c) Crown Copyright, All rights reserved


  Code for address layer, Zaki Ahsan October 2007

-->

<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:osgb="http://www.ordnancesurvey.co.uk/xml/namespaces/osgb"
                xmlns:gml="http://www.opengis.net/gml"
                xmlns:xmlin="http://www.edonica.com/xmlin1"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
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

  <xsl:template match="osgb:addressPointMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:AddressPoint" mode="addressPointMember"/>
    </TransformOutput>
  </xsl:template>

  <xsl:template match="osgb:departedMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:DepartedFeature" mode="departedMember"/>
    </TransformOutput>
  </xsl:template>

  <!-- start of added code for addresslayer -->
  <xsl:template match="osgb:AddressPoint" mode="addressPointMember">
    <AddressPoint>
      <TOID>
        <xsl:value-of select="@fid"/>
      </TOID>
      <StructureType>
        <xsl:value-of select="./osgb:addressStatus/osgb:structureType"/>
      </StructureType>
      <Organisation>
        <xsl:value-of select="./osgb:postalAddress/osgb:organisation"/>
      </Organisation>
      <Department>
        <xsl:value-of select="./osgb:postalAddress/osgb:department"/>
      </Department>
      <POBox>
        <xsl:value-of select="./osgb:postalAddress/osgb:POBox"/>
      </POBox>
      <SubBuildingName>
        <xsl:value-of select="./osgb:postalAddress/osgb:subBuildingName"/>
      </SubBuildingName>
      <BuildingName>
        <xsl:value-of select="./osgb:postalAddress/osgb:buildingName"/>
      </BuildingName>
      <BuildingNumber>
        <xsl:value-of select="./osgb:postalAddress/osgb:buildingNumber"/>
      </BuildingNumber>
      <DependentThoroughfare>
        <xsl:value-of select="./osgb:postalAddress/osgb:dependentThoroughfare"/>
      </DependentThoroughfare>
      <Thoroughfare>
        <xsl:value-of select="./osgb:postalAddress/osgb:thoroughfare"/>
      </Thoroughfare>
      <DoubleDependentLocality>
        <xsl:value-of select="./osgb:postalAddress/osgb:doubleDependentLocality"/>
      </DoubleDependentLocality>
      <DependentLocality>
        <xsl:value-of select="./osgb:postalAddress/osgb:dependentLocality"/>
      </DependentLocality>
      <PostTown>
        <xsl:value-of select="./osgb:postalAddress/osgb:postTown"/>
      </PostTown>
      <PostCode>
        <xsl:value-of select="./osgb:postalAddress/osgb:postCode"/>
      </PostCode>
      <ReferenceToTopographicArea>
        <xsl:value-of select="substring(./osgb:referenceToTopographicArea/@xlink:href,2)"/>
      </ReferenceToTopographicArea>
      <Geom>
        <xsl:copy-of select="./osgb:point/*"/>
      </Geom>
    </AddressPoint>
  </xsl:template>
  <!-- End of added code for addresslayer -->

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