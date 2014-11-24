<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/*">
    <GetWSJIETokenUrlResponse>
      <xsl:copy-of select="/*/Control"/>
      <xsl:copy-of select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </GetWSJIETokenUrlResponse>
  </xsl:template>

  <xsl:template match="/*/ResultSet">
      <WSJIEUrl>
        <xsl:value-of select="//WSJ_URL"/>
      </WSJIEUrl>
  </xsl:template>
</xsl:stylesheet>
