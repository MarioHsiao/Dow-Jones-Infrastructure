<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no" omit-xml-declaration="yes"/>
  <xsl:template match="/*">
    <GetTokenDataResponse>
      <TokenDataResponse>
        <xsl:apply-templates select="/*/ResultSet/Result"/>
      </TokenDataResponse>
    </GetTokenDataResponse>
  </xsl:template>

  <xsl:template match="ERROR_CODE">
  </xsl:template>

  <xsl:template match="ERROR_GENERAL_MSG">
  </xsl:template>

  <xsl:template match="expirationDate">
    <ExpirationDate>
      <xsl:value-of select="."/>
    </ExpirationDate>
  </xsl:template>

  <xsl:template match="namespace">
    <Namespace>
      <xsl:value-of select="."/>
    </Namespace>
  </xsl:template>

  <xsl:template match="id">
    <Id>
      <xsl:value-of select="."/>
    </Id>
  </xsl:template>

  <xsl:template match="segmentOfCallingUrl">
    <SegmentOfCallingURL>
      <xsl:value-of select="."/>
    </SegmentOfCallingURL>
  </xsl:template>

  <xsl:template match="tokenMode">
    <TokenMode>
      <xsl:value-of select="."/>
    </TokenMode>
  </xsl:template>

  <xsl:template match="domains">
    <ReferringDomains>
      <xsl:variable name="csvString" select="text()"/>
      <xsl:variable name="typeofdata" select="'domain'"/>
      <xsl:call-template name="spliCommaSeparatedString">
      <xsl:with-param name="csvString" select="$csvString"/>
      <xsl:with-param name="position" select="1"/>
      <xsl:with-param name="typeofdata" select="$typeofdata"/>
    </xsl:call-template>
    </ReferringDomains>
  </xsl:template>

  <xsl:template match="authorizations">
    <Authorization>
      <xsl:value-of select="."/>
    </Authorization>
  </xsl:template>


  <xsl:template match="accessPointCodes">
    <AccessPointCodes>
      <xsl:variable name="csvString" select="text()"/>
      <xsl:variable name="typeofdata" select="'accessPointCode'"/>
      <xsl:call-template name="spliCommaSeparatedString">
        <xsl:with-param name="csvString" select="$csvString"/>
        <xsl:with-param name="position" select="1"/>
        <xsl:with-param name="typeofdata" select="$typeofdata"/>
      </xsl:call-template>
    </AccessPointCodes>
  </xsl:template>

  <xsl:template name="spliCommaSeparatedString">
    <xsl:param name="csvString"/>
    <xsl:param name="typeofdata"/>
    <xsl:param name="position"/>
    <xsl:choose>
      <xsl:when test="contains($csvString,',')">
        <!-- Select the first value to process -->
        <xsl:call-template name="writeCommaSeparatedValue">
          <xsl:with-param name="commaSeparatedElement" select="substring-before($csvString,',')"/>
          <xsl:with-param name="position" select="$position"/>
          <xsl:with-param name="typeofdata" select="$typeofdata"/>
        </xsl:call-template>
        <!-- Recurse with remainder of string -->
        <xsl:call-template name="spliCommaSeparatedString">
          <xsl:with-param name="csvString" select="substring-after($csvString,',')"/>
          <xsl:with-param name="position" select="$position + 1"/>
          <xsl:with-param name="typeofdata" select="$typeofdata"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <!-- This is the last value so we don't recurse -->
        <xsl:call-template name="writeCommaSeparatedValue">
          <xsl:with-param name="commaSeparatedElement" select="$csvString"/>
          <xsl:with-param name="position" select="$position"/>
          <xsl:with-param name="typeofdata" select="$typeofdata"/>
        </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Process of individual value here -->
  <xsl:template name="writeCommaSeparatedValue">
    <xsl:param name="commaSeparatedElement"/>
    <xsl:param name="position"/>
    <xsl:param name="typeofdata"/>
    <xsl:if test="$typeofdata = 'domain'">
      <domain>
        <xsl:value-of select="$commaSeparatedElement"/>
      </domain>
    </xsl:if>
    <xsl:if test="$typeofdata = 'accessPointCode'">
      <accessPointCode>
        <xsl:value-of select="$commaSeparatedElement"/>
      </accessPointCode>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>