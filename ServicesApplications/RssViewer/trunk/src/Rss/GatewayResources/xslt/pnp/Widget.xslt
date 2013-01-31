<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="*">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="widget">
		<xsl:copy>
			<xsl:attribute name="ID"><xsl:value-of select="ID"/></xsl:attribute>
			<xsl:attribute name="position"><xsl:value-of select="position"/></xsl:attribute>
			<xsl:attribute name="windowState"><xsl:value-of select="windowState"/></xsl:attribute>
			<xsl:element name="widgetDef">
				<xsl:copy-of select="."/>
			</xsl:element>
			<xsl:element name="type">
				<xsl:value-of select="@xsi:type"/>
			</xsl:element>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>
