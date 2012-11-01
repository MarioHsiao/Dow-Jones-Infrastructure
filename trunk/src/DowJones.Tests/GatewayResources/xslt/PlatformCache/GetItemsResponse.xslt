<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="SharedCacheResponse">
		<GetItemsResponse>
			<xsl:apply-templates select="//Response"/>
		</GetItemsResponse>
	</xsl:template>
	<xsl:template match="Response">
    <itemResponse>
      <xsl:element name="status">
        <xsl:value-of select="@Status"/>
      </xsl:element>
		<xsl:apply-templates select="Info"/>
		<item>
			<xsl:apply-templates select="Storage_Key"/>
			<xsl:apply-templates select="NameSpace"/>
			<xsl:apply-templates select="Blob"/>
		</item>
    </itemResponse>
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
	<xsl:template match="Blob">
		<blob>
			<xsl:value-of select="."/>
		</blob>
	</xsl:template>
	<xsl:template match="Info">
		<message>
			<xsl:value-of select="."/>
		</message>
	</xsl:template>
</xsl:stylesheet>
