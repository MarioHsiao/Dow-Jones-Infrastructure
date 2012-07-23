<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:template match="/*">
    <SearchByUserResponse>
      <xsl:copy-of select ="ResultSet/Result/child::*"/>
    </SearchByUserResponse>
  </xsl:template>
</xsl:stylesheet>