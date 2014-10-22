<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" cdata-section-elements="itemBlob"/>
  <xsl:template match="/*">
    <xsl:element name="AddCategoryResponse">
      <xsl:copy-of select="Control"/>
      <xsl:copy-of select="Status"/>
      <xsl:copy-of select="ResultSet/Result/CategoryId"/>
    </xsl:element>
  </xsl:template>
   
</xsl:stylesheet>