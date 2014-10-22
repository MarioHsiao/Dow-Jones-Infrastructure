<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="SharedCacheResponse">
		<StoreItemResponse>
			<xsl:apply-templates select="//Response"/>
		</StoreItemResponse>
	</xsl:template>
	<xsl:template match="Response">
		<xsl:attribute name="status"><xsl:value-of select="@Status"/></xsl:attribute>
		<xsl:apply-templates select="Storage_Key"/>
		<xsl:apply-templates select="NameSpace"/>
		<xsl:apply-templates select="Info"/>
	</xsl:template>
	<xsl:template match="Storage_Key">
		<key>
			<xsl:value-of select="."/>
		</key>
	</xsl:template>
	<xsl:template match="NameSpace">
		<namespace>
			<xsl:value-of select="."/>
		</namespace>
	</xsl:template>
	<xsl:template match="Info">
		<message>
			<xsl:value-of select="."/>
		</message>
	</xsl:template>
</xsl:stylesheet>
