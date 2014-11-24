<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <SetRedirectAttributeByKeyResponse>
      <xsl:copy-of select ="ResultSet/Result/child::*"/>
    </SetRedirectAttributeByKeyResponse>
  </xsl:template>
</xsl:stylesheet>