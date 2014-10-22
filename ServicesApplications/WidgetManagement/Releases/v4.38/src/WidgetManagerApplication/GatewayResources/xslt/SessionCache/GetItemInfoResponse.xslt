<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="GetSessionCacheItemInfoResponse">
		<GetItemInfoResponse>
			<xsl:apply-templates select="//CacheItem"/>
		</GetItemInfoResponse>
	</xsl:template>
	<xsl:template match="CacheItem">	
		<xsl:attribute name="status">0</xsl:attribute>
			<itemInfo>
			<xsl:apply-templates select="Key"/>
			<xsl:apply-templates select="Scope"/>
			<xsl:apply-templates select="ExpirationPolicy"/>
			<xsl:apply-templates select="ExpirationTime	"/>
			<xsl:apply-templates select="TimeCreated"/>
			<xsl:apply-templates select="TimeLastAccessed"/>
			<!--<xsl:apply-templates select="TimeLastRefreshed"/>-->
		</itemInfo>
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
	<xsl:template match="ExpirationPolicy">
		<expirationPolicy>
			<xsl:value-of select="."/>
		</expirationPolicy>
	</xsl:template>
	<xsl:template match="ExpirationTime">	
		<expiryInterval>
			<xsl:value-of select="."/>
		</expiryInterval>
	</xsl:template>
	<xsl:template match="TimeCreated">
		<createTime>
			<xsl:value-of select="."/>
		</createTime>
	</xsl:template>
	<xsl:template match="TimeLastAccessed">
		<lastAccessTime>
			<xsl:value-of select="."/>
		</lastAccessTime>
	</xsl:template>
	<!--<xsl:template match="TimeLastRefreshed">
		<lastRefreshTime>	
			<xsl:value-of select="."/>
		</lastRefreshTime>
	</xsl:template>-->
	
</xsl:stylesheet>
