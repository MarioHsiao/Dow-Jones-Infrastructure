<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:taxonomy="http://types.factiva.com/symbology/taxonomy">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <xsl:element name="GetTaxonomyListResponse">
      <xsl:apply-templates select="//taxonomy:taxonomyListResult"/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="//taxonomy:taxonomyListResult">
    <xsl:element name="taxonomyListResult">
      <xsl:attribute name="count">
        <xsl:value-of select="@count"/>
      </xsl:attribute>
      <xsl:attribute name="version">
        <xsl:value-of select="@version"/>
      </xsl:attribute>
      <!--<xsl:apply-templates select="taxonomy:taxonomyCode"/>-->
      <xsl:copy-of select="taxonomy:taxonomyCode"/>
    </xsl:element>
  </xsl:template>
  <!--<xsl:template match="taxonomy:taxonomyCode">
    <xsl:element name="TaxonomyCode">
      <xsl:attribute name="code">
        <xsl:value-of select="@code"/>
      </xsl:attribute>
      <xsl:apply-templates select="taxonomy:taxonomyCode"/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="taxonomy:taxonomyCode">
    
  </xsl:template>-->
</xsl:stylesheet>