<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <xsl:element name="AddItemsToCategoryResponse">
      <xsl:copy-of select="Control"/>
      <xsl:copy-of select="Status"/>
      <xsl:copy-of select="ResultSet/Result/BadItemList"/>       
    </xsl:element>
  </xsl:template>
</xsl:stylesheet> 
