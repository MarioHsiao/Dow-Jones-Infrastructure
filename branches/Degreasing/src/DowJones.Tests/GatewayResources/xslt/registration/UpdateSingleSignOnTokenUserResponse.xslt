<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:include href="..\membership\CommonInfo.xslt"/>

  <xsl:template match="/*">
    <UpdateSingleSignOnTokenUserResponse>
      <xsl:call-template name="GetError"/>
    </UpdateSingleSignOnTokenUserResponse>
  </xsl:template>

</xsl:stylesheet>
