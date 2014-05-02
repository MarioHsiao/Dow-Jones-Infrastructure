<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/*">
    <xsl:element name="PerformConceptSearchResponse">
      <data>
        <xsl:text disable-output-escaping="yes">&lt;![CDATA[</xsl:text>
        <xsl:copy-of select="//ConceptSearchResult"/>
        <xsl:text disable-output-escaping="yes">]]&gt;</xsl:text>
      </data>
    </xsl:element>
  </xsl:template>
</xsl:stylesheet>