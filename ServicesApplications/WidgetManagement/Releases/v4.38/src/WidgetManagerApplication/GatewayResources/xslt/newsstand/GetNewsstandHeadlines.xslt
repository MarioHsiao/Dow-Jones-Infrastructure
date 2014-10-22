<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="/*">
		<xsl:copy>
			    <FolderID><xsl:value-of select="sectionID"/></FolderID>
		        <xsl:apply-templates select="publicationDate"/>
		        <xsl:apply-templates select="maxResultsToReturn"/>
		        <xsl:apply-templates select="bookmark"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="publicationDate">
        <xsl:if test="string-length(normalize-space(.)) = 10">
		<PubDate><xsl:value-of select="concat(substring(.,1,4), substring(.,6,2), substring(.,9,2))" /></PubDate>
	</xsl:if>
	</xsl:template>
	<xsl:template match="maxResultsToReturn">
		<HeadlineCount><xsl:value-of select="."/></HeadlineCount>
	</xsl:template>
	<xsl:template match="bookmark">
			<BookMark><xsl:value-of select="."/></BookMark>
	</xsl:template>
</xsl:stylesheet>