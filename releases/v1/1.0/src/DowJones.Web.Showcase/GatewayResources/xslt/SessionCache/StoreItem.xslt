<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output omit-xml-declaration="yes" indent="yes"/>
	<xsl:template match="StoreItem">
		<xsl:apply-templates select="item"/>
	</xsl:template>
	
	<xsl:template match="item">
			<xsl:apply-templates select="blob"/>
	</xsl:template>
	<xsl:template match="blob">
		<xsl:value-of select="."/>
	</xsl:template>	
</xsl:stylesheet>
