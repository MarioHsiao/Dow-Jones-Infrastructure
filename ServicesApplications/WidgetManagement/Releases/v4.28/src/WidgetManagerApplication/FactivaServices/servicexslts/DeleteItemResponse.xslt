<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  version="1.0">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/*">
		<DeleteItemResponse>
				<xsl:apply-templates select="/*/Status"/>
		</DeleteItemResponse>
	</xsl:template>
	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>
</xsl:stylesheet>
