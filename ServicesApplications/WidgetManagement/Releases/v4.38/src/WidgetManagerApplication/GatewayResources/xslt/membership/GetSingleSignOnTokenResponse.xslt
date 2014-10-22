<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:include href="CommonInfo.xslt"/>

  <xsl:template match="/*">
    <GetSingleSignOnTokenResponse>
      <xsl:apply-templates select="//ResultSet/Result/sso"/>
      <xsl:call-template name="GetError"/>
    </GetSingleSignOnTokenResponse>
  </xsl:template>

  <xsl:template match="sso">
    <token>
      <xsl:value-of select="."/>
    </token>
  </xsl:template>
  
</xsl:stylesheet>