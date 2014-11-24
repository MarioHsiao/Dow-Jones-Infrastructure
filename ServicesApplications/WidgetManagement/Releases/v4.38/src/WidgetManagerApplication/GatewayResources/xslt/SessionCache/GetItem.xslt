<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="GetItem">
		<GetSessionCacheItem>
			<CacheItems>
				<CacheItem>
					<xsl:apply-templates select="key"/>
					<xsl:apply-templates select="scope"/>
					<Format>CDATA</Format>
				</CacheItem>
			</CacheItems>
		</GetSessionCacheItem>
	</xsl:template>
	<xsl:template match="key">
		<Key>
			<xsl:value-of select="."/>
		</Key>
	</xsl:template>
	<xsl:template match="scope">
		<Scope>
			<xsl:value-of select="."/>
		</Scope>
	</xsl:template>
</xsl:stylesheet>
