<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fcp="urn:factiva:fcp:v2_0">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/*">
		<xsl:copy>
			<xsl:apply-templates/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="*">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="fcp:SearchContentSet">
	    <xsl:element name="SearchContentSets" namespace="urn:factiva:fcp:v2_0">
			<xsl:value-of select="."/>
	    </xsl:element>
	</xsl:template>
</xsl:stylesheet>
