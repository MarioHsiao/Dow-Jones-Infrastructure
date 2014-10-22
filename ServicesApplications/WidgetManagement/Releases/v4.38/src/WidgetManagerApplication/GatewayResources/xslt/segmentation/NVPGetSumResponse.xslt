<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="*">
		<GetSumResponse>
			<xsl:copy-of select ="//Status"/>
			<xsl:if test="//Result/Sum">
				<xsl:copy-of select="//Result/Sum"/>
				<xsl:copy-of select="//Result/Timestamp"/>
				<xsl:copy-of select="//Result/Hostname"/>
			</xsl:if>
		</GetSumResponse>
	</xsl:template>

</xsl:stylesheet>