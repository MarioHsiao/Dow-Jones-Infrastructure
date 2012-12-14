<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template name="tagRequired">
		<xsl:param name="newNodeName"/>
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(.)) &gt; 0">
				<xsl:element name="{$newNodeName}">
					<xsl:value-of select="normalize-space(string(.))"/>
				</xsl:element>
			</xsl:when>
			<xsl:otherwise>
				<xsl:element name="{$newNodeName}"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="tagOptional">
		<xsl:param name="newNodeName"/>
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<xsl:element name="{$newNodeName}">
				<xsl:value-of select="normalize-space(string(.))"/>
			</xsl:element>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
