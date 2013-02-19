<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <xsl:element name="DowngradeUsersResponse">
      <xsl:call-template name="GetError"/>
      <xsl:apply-templates select="//BAD_USER_LIST"/>
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

  <xsl:template match="//BAD_USER_LIST">
    <badUser>
      <userId>
        <xsl:value-of select="userId" />
      </userId>
      <prodId>
        <xsl:value-of select="prodId" />
      </prodId>
      <errorCode>
        <xsl:value-of select="ERROR_CODE" />
      </errorCode>
      <errorMsg>
        <xsl:value-of select="ERROR_GENERAL_MSG" />
      </errorMsg>
    </badUser>
  </xsl:template>
</xsl:stylesheet>