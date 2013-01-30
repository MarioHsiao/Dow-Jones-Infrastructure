<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:variable name="SoapNS">http://schemas.xmlsoap.org/soap/envelope/</xsl:variable>
	<xsl:template match="/">
		<xsl:apply-templates select="@* | node()" mode="RemoveSoapEnv"/>
	</xsl:template>
	<xsl:template match="node()" mode="RemoveSoapEnv">
    <xsl:variable name="LocalNS">
      <xsl:value-of select="namespace-uri()"/>
    </xsl:variable>
		<xsl:choose>
			<xsl:when test="namespace-uri()=$SoapNS">
				<xsl:apply-templates select="@* | node()" mode="RemoveSoapEnv"/>
			</xsl:when>
			<xsl:when test="self::*">
				<xsl:element name="{local-name()}" namespace="{$LocalNS}">
					<xsl:apply-templates select="@* | node()" mode="RemoveSoapEnv"/>
				</xsl:element>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="@*" mode="RemoveSoapEnv">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="RemoveSoapEnv"/>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>
