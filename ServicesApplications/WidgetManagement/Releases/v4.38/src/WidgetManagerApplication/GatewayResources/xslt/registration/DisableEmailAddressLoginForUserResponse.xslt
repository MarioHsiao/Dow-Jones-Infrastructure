<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:include href="PerformUserRegistrationCommonResponse.xslt"/>
  <xsl:template match="/*">
    <DisableEmailAddressLoginForUserResponse>
      <xsl:copy-of select ="ResultSet/Result/child::*"/>
    </DisableEmailAddressLoginForUserResponse>
  </xsl:template>
</xsl:stylesheet>
