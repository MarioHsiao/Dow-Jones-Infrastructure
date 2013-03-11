<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="/*">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:copy-of select="/*/Control"/>
			<xsl:apply-templates select="/*/Request"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="/*/Request">
		<Request>
		        <FolderID><xsl:value-of select="sectionID"/></FolderID>
		        <xsl:apply-templates select="publicationDate"/>
		</Request>
	</xsl:template>
	<xsl:template match="//publicationDate">
        <xsl:if test="//publicationDate">
		<PubDate><xsl:value-of select="concat(substring(.,1,4), substring(.,6,2), substring(.,9,2))" /></PubDate>
	</xsl:if>
	</xsl:template>
</xsl:stylesheet>