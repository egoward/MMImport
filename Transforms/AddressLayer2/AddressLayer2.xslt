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

  <xsl:template match="osgb:boundaryMember">
    <TransformOutput>
      <xsl:apply-templates select="./osgb:BoundaryLine" mode="boundaryMember"/>
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

  <xsl:template match="osgb:AddressPoint" mode="addressPointMember">
    <AddressPoint>
      <!--1 OS Theme (String20) -->
      <OS_THEME>
        <xsl:for-each select="./osgb:theme">
          <xsl:value-of select="normalize-space(.)"/>
        </xsl:for-each>
      </OS_THEME>
      <!--2 OS Address TOID (renamed from OS_A_TOID)  (String20)-->
      <TOID>
        <xsl:value-of select="./@fid"/>        
      </TOID>
      <!--3 OS APR OA  (String20)-->
      <OS_APR>
        <xsl:value-of select="./osgb:OSAPR"/>
      </OS_APR>
      <!--4 RM UDPRN   (xs:int)-->
      <RM_UDPRN>
        <xsl:value-of select="./osgb:addressAuthority/osgb:UDPRN"/>
      </RM_UDPRN>
      <!--5 RM UMRRN  (xs:int)-->
      <RM_UMRRN>
        <xsl:value-of select="./osgb:addressAuthority/osgb:UMRRN"/>
      </RM_UMRRN>
      <!--6 RM Address Key  (xs:int)-->
      <RM_ADD_KEY>
        <xsl:value-of select="./osgb:postalAddress/osgb:addressId/osgb:addressKey"/>
      </RM_ADD_KEY>
      <!--7 RM Organisation Key  (xs:int)-->
      <RM_ORG_KEY>
        <xsl:value-of select="./osgb:postalAddress/osgb:addressId/osgb:organisationKey"/>
      </RM_ORG_KEY>
      <!--8 VO CT UARN  (xs:int)-->
      <VO_C_UARN>
        <xsl:value-of select="./osgb:addressAuthority/osgb:CT_UARN"/>
      </VO_C_UARN>
      <!--9 VO NDR UARN  (xs:int)-->
      <VO_N_UARN>
        <xsl:value-of select="./osgb:addressAuthority/osgb:NDR_UARN"/>
      </VO_N_UARN>
      <!--10 OS Ref To Address TOID (String20)-->
      <OS_RA_TOID>
        <xsl:value-of select="substring(./osgb:referenceToAddress/@xlink:href,2)"/>
      </OS_RA_TOID>
      <!--11 OS Ref To OSAPR  (String20)-->
      <OS_R_OSAPR>
        <xsl:value-of select="./osgb:referenceToOSAPR"/>
      </OS_R_OSAPR>
      <!--12 OS Ref To Topography TOID(String20)  -->
      <!-- We do a SubString to remove an initial # -->
      <OS_RT_TOID>
        <xsl:value-of select="substring(./osgb:referenceToTopographicFeature/@xlink:href,2)"/>
      </OS_RT_TOID>
      <!--13 OS Ref To Carto Text TOID (String20) -->
      <OS_RC_TOID>
        <xsl:value-of select="substring(./osgb:referenceToCartographicText/@xlink:href,2)"/>
      </OS_RC_TOID>
      <!--14 OS Ref To ITN Road Link TOID (String20)  -->
      <OS_RL_TOID>
        <xsl:value-of select="substring(./osgb:referenceToRoadLink/@xlink:href,2)"/>
      </OS_RL_TOID>
      <!--15 OS Ref To ITN Road TOID (String20) -->
      <OS_RR_TOID>
        <xsl:value-of select="substring(./osgb:referenceToRoad/@xlink:href,2)"/>
      </OS_RR_TOID>
      <!--16 RM Ref To UDPRN (xs:int)-->
      <RM_R_UDPRN>
        <xsl:value-of select="./osgb:addressAuthority/osgb:UDPRN"/>
      </RM_R_UDPRN>
      <!--17 RM Ref To Address Key (xs:int)-->
      <RM_RA_KEY>
        <xsl:value-of select="./osgb:referenceToAddressKey"/>
      </RM_RA_KEY>
      <!--18 RM Ref To Organisation Key (xs:int)-->
      <RM_RO_KEY>
        <xsl:value-of select="./osgb:referenceToOrganisationKey"/>
      </RM_RO_KEY>
      <!--19 OS Address Version (xs:int)-->
      <OS_A_TOIDV>
        <xsl:value-of select="./osgb:version"/>
      </OS_A_TOIDV>

      <!-- OS Address Change History -->

      <!--20 OS Reason For Change 1  (String30)-->
      <!--DANGER!-->
      <OS_A_RFC1>
        <xsl:value-of select="./osgb:changeHistory/osgb:reasonForChange"/>
      </OS_A_RFC1>
      <!--21 OS Change Date 1 (DateString)-->
      <!--DANGER!-->
      <OS_A_CD1>
        <xsl:value-of select="./osgb:changeHistory/osgb:changeDate"/>
      </OS_A_CD1>
      <!--22 OS Reason For Change 2 (String30)-->
      <!--DANGER!-->
      <OS_A_RFC2>
      </OS_A_RFC2>
      <!--23 OS Change Date 2 (DateString)-->
      <!--DANGER!-->
      <OS_A_CD2>
      </OS_A_CD2>
      <!--24 OS Address Version Date  (DateString)-->
      <OS_A_TOIDD>
        <xsl:value-of select="./osgb:versionDate"/>
      </OS_A_TOIDD>
      <!--25 OS Topography Version  (xs:int)-->
      <OS_T_TOIDV>
        <xsl:value-of select="./osgb:referenceToTopographicFeature/@version"/>
      </OS_T_TOIDV>
      <!--26 OS Carto Text Version  (int)-->
      <OS_C_TOIDV>
        <xsl:value-of select="./osgb:referenceToCartographicText/@version"/>
      </OS_C_TOIDV>
      <!--27 OS ITN Road Link Version  (int)-->
      <OS_L_TOIDV>
        <xsl:value-of select="./osgb:referenceToRoadLink/@version"/>
      </OS_L_TOIDV>
      <!--28 OS ITN Road Version  (int)-->
      <OS_R_TOIDV>
        <xsl:value-of select="./osgb:referenceToRoad/@version"/>
      </OS_R_TOIDV>
      <!--29 RM Postal Address Date RV (DateString)  -->
      <RM_P_DATE>
        <xsl:value-of select="./osgb:postalAddressDate"/>
      </RM_P_DATE>

      <!-- OS BS7666 Address -->

      <!--30 OS BS7666 Secondary Addressable Object Name   (String110)-->
      <OS_BS_SAON>
        <xsl:value-of select="./osgb:bs7666Address/osgb:saon"/>
      </OS_BS_SAON>
      <!--31 OS BS7666 Primary Addressable Object Name   (String200)-->
      <OS_BS_PAON>
        <xsl:value-of select="./osgb:bs7666Address/osgb:paon"/>
      </OS_BS_PAON>
      <!--32 OS BS7666 Street    (String80)-->
      <OS_BS_STRE>
        <xsl:value-of select="./osgb:bs7666Address/osgb:street"/>
      </OS_BS_STRE>
      <!--33 OS BS7666 Locality    (String80)-->
      <OS_BS_LOCA>
        <xsl:value-of select="./osgb:bs7666Address/osgb:locality"/>
      </OS_BS_LOCA>
      <!--34 OS BS7666 Town    (String60)-->
      <OS_BS_TOWN>
        <xsl:value-of select="./osgb:bs7666Address/osgb:town"/>
      </OS_BS_TOWN>
      <!--35 OS BS7666 Administrative Area   (String100)-->
      <OS_BS_LA>
        <xsl:value-of select="./osgb:bs7666Address/osgb:adminArea"/>
      </OS_BS_LA>
      <!--36 OS BS7666 Postcode  PC  (String8 )-->
      <OS_BS_PC>
        <xsl:value-of select="./osgb:bs7666Address/osgb:postCode"/>
      </OS_BS_PC>

      <!-- OS Alternative Address -->


      <!--37 OS Alt Sub Building Name  SB  (String30)-->
      <OS_AL_SB>
        <xsl:value-of select="./osgb:alternativeAddress/osgb:subBuildingName"/>
      </OS_AL_SB>
      <!--38 OS Alt Building Name  BD  (String20)-->
      <OS_AL_BD>
        <xsl:value-of select="./osgb:alternativeAddress/osgb:buildingName"/>
      </OS_AL_BD>
      <!--39 OS Alt Building Number  BN  (int)-->
      <OS_AL_BN>
        <xsl:value-of select="./osgb:alternativeAddress/osgb:buildingNumber"/>
      </OS_AL_BN>
      <!--40 OS Alt Dependent Thoroughfare Name  DR  (String80)-->
      <OS_AL_DR>
        <xsl:value-of select="./osgb:alternativeAddress/osgb:dependentThoroughfare"/>
      </OS_AL_DR>
      <!--41 OS Alt Thoroughfare Name  TN  (String80)-->
      <OS_AL_TN>
        <xsl:value-of select="./osgb:alternativeAddress/osgb:thoroughfare"/>
      </OS_AL_TN>

      <!-- RM DP Address -->
      <!--42 RM DP Organisation Name ON  (String60)-->
      <RM_DP_ON>
        <xsl:value-of select="./osgb:postalAddress/osgb:organisation"/>
      </RM_DP_ON>
      <!--43 RM DP Department Name DP  (String60)-->
      <RM_DP_DP>
        <xsl:value-of select="./osgb:postalAddress/osgb:department"/>
      </RM_DP_DP>
      <!--44 RM DP PO Box Number  PB  (String6)-->
      <RM_DP_PB>
        <xsl:value-of select="./osgb:postalAddress/osgb:POBox"/>
      </RM_DP_PB>
      <!--45 RM DP Sub Building Name  SB  (String30)-->
      <RM_DP_SB>
        <xsl:value-of select="./osgb:postalAddress/osgb:subBuildingName"/>
      </RM_DP_SB>
      <!--46 RM DP Building Name  BD  (String20)-->
      <RM_DP_BD>
        <xsl:value-of select="./osgb:postalAddress/osgb:buildingName"/>
      </RM_DP_BD>
      <!--47 RM DP Building Number  BN  (int)-->
      <RM_DP_BN>
        <xsl:value-of select="./osgb:postalAddress/osgb:buildingNumber"/>
      </RM_DP_BN>
      <!--48 RM DP Dependent Thoroughfare Name  DR  (String80)-->
      <RM_DP_DR>
        <xsl:value-of select="./osgb:postalAddress/osgb:dependentThoroughfare"/>
      </RM_DP_DR>
      <!--49 RM DP Thoroughfare Name  TN  (String80)-->
      <RM_DP_TN>
        <xsl:value-of select="./osgb:postalAddress/osgb:thoroughfare"/>
      </RM_DP_TN>
      <!--50 RM DP Double Dependent Locality  DD  (String35)-->
      <RM_DP_DD>
        <xsl:value-of select="./osgb:postalAddress/osgb:doubleDependentLocality"/>
      </RM_DP_DD>
      <!--51 RM DP Dependent Locality  DL  (String35)-->
      <RM_DP_DL>
        <xsl:value-of select="./osgb:postalAddress/osgb:dependentLocality"/>
      </RM_DP_DL>
      <!--52 RM DP Post Town  PT  (String30)-->
      <RM_DP_PT>
        <xsl:value-of select="./osgb:postalAddress/osgb:postTown"/>
      </RM_DP_PT>
      <!--53 RM DP Postcode  PC  (String8)-->
      <RM_DP_PC>
        <xsl:value-of select="./osgb:postalAddress/osgb:postCode"/>
      </RM_DP_PC>
      <!--54 RM DP Postcode Type   (String5)-->
      <RM_DP_PCT>
        <xsl:value-of select="./osgb:postalAddress/osgb:postCode/@type"/>
      </RM_DP_PCT>
      <!--55 RM DP Delivery Point Suffix   (String2)-->
      <RM_DP_DPS>
        <xsl:value-of select="./osgb:postalAddress/osgb:deliveryPointSuffix"/>
      </RM_DP_DPS>

      <!-- RM Welsh DP Address  -->
      <!--56 RM Welsh DP Dependent Thoroughfare Name DR  (String80)-->
      <RM_WDP_DR>
        <xsl:value-of select="./osgb:postalAddress/osgb:welshAddress/osgb:dependentThoroughfare"/>
      </RM_WDP_DR>
      <!--57 RM Welsh DP Thoroughfare Name  TN  (String80)-->
      <RM_WDP_TN>
        <xsl:value-of select="./osgb:postalAddress/osgb:welshAddress/osgb:thoroughfare"/>
      </RM_WDP_TN>
      <!--58 RM Welsh DP Double Dependent Locality  DD  (String35)-->
      <RM_WDP_DD>
        <xsl:value-of select="./osgb:postalAddress/osgb:welshAddress/osgb:doubleDependentLocality"/>
      </RM_WDP_DD>
      <!--59 RM Welsh DP Dependent Locality  DL  (String35)-->
      <RM_WDP_DL>
        <xsl:value-of select="./osgb:postalAddress/osgb:welshAddress/osgb:dependentLocality"/>
      </RM_WDP_DL>
      <!--60 RM Welsh DP Post Town  PT  (String30)-->
      <RM_WDP_PT>
        <xsl:value-of select="./osgb:postalAddress/osgb:welshAddress/osgb:postTown"/>
      </RM_WDP_PT>

      <!-- RM Alias DP Address -->
      <!--61 RM Alias DP Also Known As   (String20)-->
      <RM_AL_AKA>
        <xsl:value-of select="./osgb:aliasAddress/osgb:alsoKnownAs"/>
      </RM_AL_AKA>
      <!--62 RM Alias DP Building Name  BD  (String20)-->
      <RM_AL_BD>
        <xsl:value-of select="./osgb:aliasAddress/osgb:buildingName"/>
      </RM_AL_BD>
      <!--63 RM Alias DP Department Name DP  (String20)-->
      <RM_AL_DP>
        <xsl:value-of select="./osgb:aliasAddress/osgb:department"/>
      </RM_AL_DP>
      <!--64 RM Alias DP Organisation Description   (String20)-->
      <RM_AL_OD>
        <xsl:value-of select="./osgb:aliasAddress/osgb:organisationDescription"/>
      </RM_AL_OD>
      <!--65 RM Alias DP Organisation at a Residential   (String20)-->
      <RM_AL_OR>
        <xsl:value-of select="./osgb:aliasAddress/osgb:organisationAtResidential"/>
      </RM_AL_OR>
      <!--66 RM Alias DP Trading Name   (String20)-->
      <RM_AL_TN>
        <xsl:value-of select="./osgb:aliasAddress/osgb:tradingName"/>
      </RM_AL_TN>
      <!--67 RM Alias Welsh Alternative   (String20)-->
      <RM_AL_WA>
        <xsl:value-of select="./osgb:aliasAddress/osgb:welshAlternative"/>
      </RM_AL_WA>

      <!-- RM True PO Box Address -->
      <!--68 RM TPO Organisation Name ON  (String60)-->
      <RM_TPO_ON>
        <xsl:value-of select="./osgb:truePOBoxAddress/osgb:organisation"/>
      </RM_TPO_ON>
      <!--69 RM TPO PO Box Number  PB  (String6)-->
      <RM_TPO_PB>
        <xsl:value-of select="./osgb:truePOBoxAddress/osgb:POBox"/>
      </RM_TPO_PB>
      <!--70 RM TPO Geographical Address   (String325)-->
      <RM_TPO_GA>
        <xsl:value-of select="./osgb:truePOBoxAddress/osgb:geographicAddress"/>
      </RM_TPO_GA>
      <!--71 RM TPO Postcode  PC  (String8)-->
      <RM_TPO_PC>
        <xsl:value-of select="./osgb:truePOBoxAddress/osgb:postCode"/>
      </RM_TPO_PC>

      <!-- RM MR Address  -->
      <!--DANGER!  This relates to royal mail multiple residency information-->

      <!--72 RM MR Organisation Name ON  (String60)-->
      <RM_MR_ON>
      </RM_MR_ON>
      <!--73 RM MR Department Name DP  (String60)-->
      <RM_MR_DP>
      </RM_MR_DP>
      <!--74 RM MR Sub Building Name  SB  (String30)-->
      <RM_MR_SB>
      </RM_MR_SB>
      <!--75 RM MR Building Name  BD  (String20)-->
      <RM_MR_BD>
      </RM_MR_BD>
      <!--76 RM MR Building Number  BN  (int)-->
      <RM_MR_BN>
      </RM_MR_BN>
      <!--77 RM MR Dependent Thoroughfare Name DR  (String80)-->
      <RM_MR_DR>
      </RM_MR_DR>
      <!--78 RM MR Thoroughfare Name  TN  (String80)-->
      <RM_MR_TN>
      </RM_MR_TN>
      <!--79 RM MR Double Dependent Locality  DD  (String35)-->
      <RM_MR_DD>
      </RM_MR_DD>
      <!--80 RM MR Dependent Locality  DL  (String35)-->
      <RM_MR_DL>
      </RM_MR_DL>
      <!--81 RM MR Post Town  PT  (String30)-->
      <RM_MR_PT>
      </RM_MR_PT>
      <!--82 RM MR Postcode  PC  (String8)-->
      <RM_MR_PC>
      </RM_MR_PC>

      <!-- VO NDR Address -->
      <!--83 VO NDR Firm Name   (String20)-->
      <VO_NDR_FN>
        <xsl:value-of select="./osgb:ndrAddress/firm"/>
      </VO_NDR_FN>
      <!--84 OS PO Box Flag   (boolean)-->
      <!--DANGER! -->
      <OS_PB_FLAG>
      </OS_PB_FLAG>
      <!--85 RM Multi-Occ Count   (int)-->
      <RM_MOC>
        <xsl:value-of select="./osgb:multipleOccupancyCount"/>
      </RM_MOC>
      <!--86 LA Code   (int)-->
      <LA_C>
        <xsl:value-of select="./osgb:laCode"/>
      </LA_C>


      <!--We could lose these but we'll keep them for now.-->

      <!-- OS Coordinate  -->
      <!--DANGER! We're leaving this for the GML parsing bits-->
      <!--87 OS Easting XCOORD  (int)-->
      <OS_X>
      </OS_X>
      <!--88 OS Northing YCOORD  (int)-->
      <OS_Y>
      </OS_Y>

      <!-- OS Positional Status Flag SF -->
      <!--89 OS Match Status   (String37)-->
      <OS_SF_MS>
        <xsl:value-of select="./osgb:addressStatus/osgb:matchStatus"/>
      </OS_SF_MS>
      <!--90 OS Physical Status   (String10)-->
      <OS_SF_PS>
        <xsl:value-of select="./osgb:addressStatus/osgb:physicalStatus"/>
      </OS_SF_PS>
      <!--91 OS Position Accuracy   (String20)-->
      <OS_SF_PQA>
        <xsl:value-of select="./osgb:addressStatus/osgb:positionalQuality/@accuracy"/>
      </OS_SF_PQA>
      <!--92 OS Position Status   (String20)-->
      <OS_SF_PQ>
        <xsl:value-of select="./osgb:addressStatus/osgb:positionalQuality"/>
      </OS_SF_PQ>
      <!--93 OS Structure Type   (String18)-->
      <OS_SF_ST>
        <xsl:value-of select="./osgb:addressStatus/osgb:structureType"/>
      </OS_SF_ST>
      <!--94 OS Spatial Referencing System   (String5)-->
      <!--DANGER!-->
      <OS_SRS>
      </OS_SRS>
      <!--95 OS Base Function   (String120)-->
      <OS_CLASS>
        <xsl:value-of select="./osgb:baseFunction"/>
      </OS_CLASS>
      <!--96 NLUD Land Use Group   (String4)-->
      <NLUD_CLASS>
        <xsl:value-of select="./osgb:landUseGroup"/>
      </NLUD_CLASS>
      <!--97 VO NDR PDesc Code   (String5)-->
      <VO_P_CLASS>
        <xsl:value-of select="./osgb:pDescCode"/>
      </VO_P_CLASS>
      <!--98 VO NDR SCat Code   (String4)-->
      <VO_S_CLASS>
        <xsl:value-of select="./osgb:pDescCode"/>
      </VO_S_CLASS>
      <!--99 OS Classification Confidence   (int)-->
      <OS_C_CONF>
        <xsl:value-of select="./osgb:sCatCode"/>
      </OS_C_CONF>
      <Geom>
        <xsl:copy-of select="./osgb:point/*"/>
      </Geom>
    </AddressPoint>
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