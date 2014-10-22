<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:taxonomy="http://types.factiva.com/symbology/taxonomy">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <xsl:element name="GetTaxonomyListInfoResponse" namespace="http://types.factiva.com/symbology/taxonomy">
      <xsl:apply-templates select="//taxonomy:taxonomyListInfoResult"/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="//taxonomy:taxonomyListInfoResult">
    <xsl:element name="taxonomyListInfoResult">
      <xsl:attribute name="count">
        <xsl:value-of select="@count"/>
      </xsl:attribute>
      <!--<xsl:apply-templates select="//taxonomy:taxonomyListInfo"/>-->
      <xsl:copy-of select="//taxonomy:taxonomyListInfo"/>
    </xsl:element>
  </xsl:template>
  <!--<xsl:template match="//taxonomyListInfo">
    <xsl:element name="TaxonomyListInfo"></xsl:element>
  </xsl:template>-->
</xsl:stylesheet>