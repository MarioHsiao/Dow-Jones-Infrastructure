<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:param name="RediHeaderAction"/>

  <xsl:template match="/*">
    <xsl:call-template name="CreateSoapElements">
      <xsl:with-param name="RediHeaderAction">
        <xsl:value-of select="$RediHeaderAction"/>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

	<xsl:template name="CreateSoapElements">
    <xsl:param name="RediHeaderAction"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:call-template name="Header">
				<xsl:with-param name="RediHeaderAction">
					<xsl:value-of select="$RediHeaderAction"/>
				</xsl:with-param>
			</xsl:call-template>
    <xsl:copy-of select="//SOAP-ENV:Body"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template name="Header">
		<xsl:param name="RediHeaderAction"/>
    <SOAP-ENV:Header>
      <xsl:element name="rediHeader" namespace="">
				<xsl:element name="action" namespace="">
					<xsl:value-of select="$RediHeaderAction"/>
				</xsl:element>
			</xsl:element>
    </SOAP-ENV:Header>
	</xsl:template>
  
</xsl:stylesheet>