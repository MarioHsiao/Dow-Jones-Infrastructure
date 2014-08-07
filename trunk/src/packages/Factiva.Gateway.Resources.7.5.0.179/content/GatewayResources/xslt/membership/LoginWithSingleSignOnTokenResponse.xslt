<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:include href="CommonAuth.xslt"/>

  <xsl:template match="/*">
    <LoginWithSingleSignOnTokenResponse>
      <xsl:apply-templates select="/*/ResultSet/Result"/>
      <xsl:call-template name="GetError"/>
    </LoginWithSingleSignOnTokenResponse>
  </xsl:template>

</xsl:stylesheet>