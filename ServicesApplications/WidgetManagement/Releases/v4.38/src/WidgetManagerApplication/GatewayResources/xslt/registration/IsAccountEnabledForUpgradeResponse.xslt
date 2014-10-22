<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <xsl:element name="IsAccountEnabledForUpgradeResponse">
      <xsl:call-template name="GetError"/>
      <path>
        <xsl:choose>
          <xsl:when test="normalize-space(//path) = '101'">ReaderFlexToIWorks</xsl:when>
          <xsl:otherwise>UnSpecified</xsl:otherwise>
        </xsl:choose>
      </path>
      <accountId>
        <xsl:value-of select="//accountId" />
      </accountId>
      <state>
        <xsl:value-of select="//state" />
      </state>
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

</xsl:stylesheet>