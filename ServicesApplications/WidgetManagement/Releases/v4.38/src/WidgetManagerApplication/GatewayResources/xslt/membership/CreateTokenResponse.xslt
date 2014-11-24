<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no" omit-xml-declaration="yes"/>
  <xsl:template match="Result">
    <CreateTokenResponse>
      <TokenResponse>
        <Token>
          <xsl:value-of select="token" />
        </Token>
      </TokenResponse>
    </CreateTokenResponse>
  </xsl:template>
</xsl:stylesheet>