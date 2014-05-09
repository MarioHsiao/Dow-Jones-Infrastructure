<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
   
  <xsl:template name="CSItem" match="//CSAddItem|//CSUpdateItem|//CSGetItem|//CSDeleteItem">   
    <xsl:copy-of select="//AccountId"/>
    <xsl:copy-of select="//Action"/>
    <xsl:if test="//ITEM_INSTANCE_NAME">
    <xsl:element name ="ItemInstanceName">
      <xsl:value-of select="//ITEM_INSTANCE_NAME"/>
    </xsl:element>
    </xsl:if>
    <xsl:if test="//ITEM_CLASS">
      <xsl:element name ="ItemClass">
        <xsl:value-of select="//ITEM_CLASS"/>
      </xsl:element>
    </xsl:if>
    <xsl:if test="//ITEM_BLOB">
      <xsl:element name ="ItemBlob">
        <xsl:attribute name="fcstype">
          <xsl:value-of select="'cdata'"/>
        </xsl:attribute>
        <xsl:copy-of select="//ITEM_BLOB/child::*"/>
      </xsl:element>
    </xsl:if>    
  </xsl:template>
</xsl:stylesheet>