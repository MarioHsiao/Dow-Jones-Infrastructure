<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="StoreItem">
		<SharedCacheRequest transaction="false" action="store">
		  <RequestList>
				<xsl:apply-templates select="item"/>
			</RequestList>
		</SharedCacheRequest>	
	</xsl:template>
	
	<xsl:template match="item">
		<Request>
			<xsl:apply-templates select="key"/>
			<xsl:apply-templates select="namespace"/>
			<xsl:apply-templates select="blob"/>
			<xsl:apply-templates select="expirationPolicy"/>
			<xsl:apply-templates select="expiryInterval"/>
		</Request>
	</xsl:template>
	<xsl:template match="key">
		<Storage_Key><xsl:value-of select="."/></Storage_Key>
	</xsl:template>
	<xsl:template match="namespace">
	<NameSpace><xsl:value-of select="."/></NameSpace>
	</xsl:template>
	<xsl:template match="blob">
	<Blob><xsl:value-of select="."/></Blob>
	</xsl:template>
	<xsl:template match="expirationPolicy">
	<Expiration_Policy><xsl:value-of select="."/></Expiration_Policy>
	</xsl:template>
	<xsl:template match="expiryInterval">
	<Expiration_Time><xsl:value-of select="."/></Expiration_Time>
	<Refresh_Interval>0</Refresh_Interval>
	</xsl:template>
	
</xsl:stylesheet>
