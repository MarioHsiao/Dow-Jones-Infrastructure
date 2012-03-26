<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="SharedCacheResponse">
		<GetItemInfoResponse>
			<xsl:apply-templates select="//Response"/>
		</GetItemInfoResponse>
	</xsl:template>
	<xsl:template match="Response">
		<xsl:attribute name="status"><xsl:value-of select="@Status"/></xsl:attribute>
		<xsl:apply-templates select="Info"/>
		<itemInfo>
			<xsl:apply-templates select="Storage_Key"/>
			<xsl:apply-templates select="NameSpace"/>
			<xsl:apply-templates select="Expiration_Policy"/>
			<xsl:apply-templates select="Expiration_Time"/>
			<xsl:apply-templates select="Time_Created"/>
			<xsl:apply-templates select="Last_Accessed_Time"/>
			<xsl:apply-templates select="Last_Refreshed_Time"/>
		</itemInfo>
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
	<xsl:template match="Expiration_Policy">
		<expirationPolicy>
			<xsl:value-of select="."/>
		</expirationPolicy>
	</xsl:template>
	<xsl:template match="Expiration_Time">
		<expiryInterval>
			<xsl:value-of select="."/>
		</expiryInterval>
	</xsl:template>
	<xsl:template match="Time_Created">
		<createTime>
			<xsl:value-of select="."/>
		</createTime>
	</xsl:template>
	<xsl:template match="Last_Accessed_Time">
		<lastAccessTime>
			<xsl:value-of select="."/>
		</lastAccessTime>
	</xsl:template>
	<xsl:template match="Last_Refreshed_Time">
		<lastRefreshTime>
			<xsl:value-of select="."/>
		</lastRefreshTime>
	</xsl:template>
	
	<xsl:template match="Info">
		<message>
			<xsl:value-of select="."/>
		</message>
	</xsl:template>
</xsl:stylesheet>
