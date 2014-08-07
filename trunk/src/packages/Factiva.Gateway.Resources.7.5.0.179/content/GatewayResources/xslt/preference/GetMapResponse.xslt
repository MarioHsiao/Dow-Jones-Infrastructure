<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <GetMapResponse>
      <xsl:for-each select="ResultSet/Result/MAP_VALUES">
        <Items><Name><xsl:value-of select="KEY"/></Name></Items>
      </xsl:for-each>
    </GetMapResponse>
  </xsl:template>
</xsl:stylesheet>