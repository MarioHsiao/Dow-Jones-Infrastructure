<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="GetSessionCacheItemResponse">
		<GetItemResponse>
			<xsl:apply-templates select="//CacheItem"/>
		</GetItemResponse>
	</xsl:template>
	<xsl:template match="CacheItem">
		<xsl:attribute name="status">0</xsl:attribute>
		<item>
			<xsl:apply-templates select="Key"/>
			<xsl:apply-templates select="Scope"/>
			<xsl:apply-templates select="Item"/>
		</item>
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
	<xsl:template match="Item">
		<blob>
			<xsl:value-of select="."/>
		</blob>
	</xsl:template>
</xsl:stylesheet>
