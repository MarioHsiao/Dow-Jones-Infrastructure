<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="xsl xsi user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:template match="/">
    <xsl:element name="BaseFlexLicensedAccountsResponse">
      <xsl:call-template name="GetError"/>
      <xsl:apply-templates select="//account"/>
      <xsl:apply-templates select="//BAD_ACCOUNT_LIST"/>
    </xsl:element>
  </xsl:template>

  <xsl:template name="GetError">
    <xsl:element name="ERROR_CODE">
      <xsl:value-of select="//ERROR_CODE"/>
    </xsl:element>
    <xsl:element name="ERROR_GENERAL_MSG">
      <xsl:value-of select="//ERROR_GENERAL_MSG"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//account">
    <account>
      <accountId><xsl:value-of select="accountId" /></accountId>
      <maxSeats><xsl:value-of select="maxSeats" /></maxSeats>
      <ssEmail><xsl:value-of select="ssEmail" /></ssEmail>
      <upgraded><xsl:value-of select="upgraded" /></upgraded>
      <path>
        <xsl:choose>
          <xsl:when test="normalize-space(path) = '101'">ReaderFlexToIWorks</xsl:when>
          <xsl:otherwise>UnSpecified</xsl:otherwise>           
        </xsl:choose>
      </path>
      <state>
        <xsl:choose>
          <xsl:when test="string-length(normalize-space(state)) &gt; 0">
            <xsl:value-of select="state"/>
          </xsl:when>
          <xsl:otherwise>D</xsl:otherwise>
        </xsl:choose>
      </state>
    </account>
  </xsl:template>

  <xsl:template match="//BAD_ACCOUNT_LIST">
    <badAccount>
      <accountId>
        <xsl:value-of select="accountId" />
      </accountId>
      <errorCode>
        <xsl:value-of select="ERROR_CODE" />
      </errorCode>
      <errorMsg>
        <xsl:value-of select="ERROR_GENERAL_MSG" />
      </errorMsg>
    </badAccount>
  </xsl:template>
  
</xsl:stylesheet>