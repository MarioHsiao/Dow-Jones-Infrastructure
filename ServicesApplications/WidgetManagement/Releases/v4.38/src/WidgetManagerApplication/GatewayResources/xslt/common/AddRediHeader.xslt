<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:param name="rediAction"/>
	<xsl:variable name="SOAPNS">http://schemas.xmlsoap.org/soap/envelope/</xsl:variable>
	<xsl:template match="/*">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:call-template name="AddRediHeader"/>
			<xsl:copy-of select="soap-env:Body"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template name="AddRediHeader">
		<xsl:element name="soap-env:Header" namespace="{$SOAPNS}">
			<xsl:element name="rediHeader" namespace="">
				<xsl:element name="action" namespace="">
					<xsl:value-of select="$rediAction"/>
				</xsl:element>
			</xsl:element>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>
