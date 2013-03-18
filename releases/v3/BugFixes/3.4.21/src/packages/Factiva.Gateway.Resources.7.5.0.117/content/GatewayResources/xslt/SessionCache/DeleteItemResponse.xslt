<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="ReleaseSessionCacheItemResponse">
		<DeleteItemResponse>
			<xsl:apply-templates select="//CacheItem"/>
		</DeleteItemResponse>
	</xsl:template>
	<xsl:template match="CacheItem">
		<xsl:attribute name="status">0</xsl:attribute>
		<xsl:apply-templates select="Key"/>
		<xsl:apply-templates select="Scope"/>
	</xsl:template>
	<xsl:template match="Key">
		<key>
			<xsl:value-of select="."/>
		</key>
	</xsl:template>
	<xsl:template match="Scope">
		<scope>
			<xsl:value-of select="."/>
		</scope>
	</xsl:template>
</xsl:stylesheet>
