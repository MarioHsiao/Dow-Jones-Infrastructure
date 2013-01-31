<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl user" exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:include href="SourceSearchResponse.xslt"/>

  <xsl:template match="/*">
    <PerformSourceSearchPrivateResponse>
      <xsl:call-template name="SourceSearchResponse"/>
    </PerformSourceSearchPrivateResponse>
  </xsl:template>

</xsl:stylesheet>
