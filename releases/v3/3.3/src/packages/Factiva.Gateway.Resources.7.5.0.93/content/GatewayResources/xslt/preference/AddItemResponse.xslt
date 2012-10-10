<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/*">
		<AddItemResponse>
			<xsl:apply-templates select="/*/Status"/>
			<xsl:apply-templates select="/*/ResultSet"/>
		</AddItemResponse>
	</xsl:template>
	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="/*/ResultSet">
		<ItemID>
			<xsl:value-of select="Result/ITEM_ID"/>
		</ItemID>
	</xsl:template>
</xsl:stylesheet>