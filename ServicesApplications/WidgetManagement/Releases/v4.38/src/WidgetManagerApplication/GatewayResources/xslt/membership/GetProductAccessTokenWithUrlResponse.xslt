<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="/*">
    <GetProductAccessTokenWithUrlResponse>
			<xsl:call-template name="GetError"/>
			<xsl:apply-templates select="//Result"/>
		</GetProductAccessTokenWithUrlResponse>
	</xsl:template>

	<xsl:template name="GetError">
		<xsl:element name="ErrorCode">
			<xsl:value-of select="//ERROR_CODE"/>
		</xsl:element>
		<xsl:element name="ErrorMessage">
			<xsl:value-of select="//ERROR_GENERAL_MSG"/>
		</xsl:element>
	</xsl:template>

	<xsl:template match="//Result">

		<xsl:choose>
			<xsl:when test="string-length(normalize-space(url)) &gt; 0">
				<URL>
					<xsl:value-of select="url" />
				</URL>
			</xsl:when>
			<xsl:otherwise></xsl:otherwise>
		</xsl:choose>

    <xsl:choose>
      <xsl:when test="string-length(normalize-space(token)) &gt; 0">
        <Token>
          <xsl:value-of select="token" />
        </Token>
      </xsl:when>
      <xsl:otherwise></xsl:otherwise>
    </xsl:choose>
    
	</xsl:template>

</xsl:stylesheet>
