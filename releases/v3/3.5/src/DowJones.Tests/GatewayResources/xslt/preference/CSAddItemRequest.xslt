<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:include href="CSItemRequest.xslt"></xsl:include>
  <xsl:template match="/*">
    <xsl:element name="CSAddItem">
      <xsl:call-template name="CSItem">        
      </xsl:call-template>
    </xsl:element>
  </xsl:template>   
</xsl:stylesheet>