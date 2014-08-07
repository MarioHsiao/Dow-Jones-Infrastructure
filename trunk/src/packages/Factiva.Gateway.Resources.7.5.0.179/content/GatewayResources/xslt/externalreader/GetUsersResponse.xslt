<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="/">
		<xsl:element name="GetUsersResponse">
			<xsl:apply-templates select="/*/ResultSet"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="/*/ResultSet">
		<xsl:apply-templates select="Result"/>
	</xsl:template>
	<xsl:template match="Result">
		<xsl:copy-of select="profileId"/>
		<xsl:copy-of select="accountId"/>
    <xsl:copy-of select="skipCount"/>
    <xsl:copy-of select="totCount"/>
		<xsl:apply-templates select="allocated"/>
		<xsl:copy-of select="user"/>
	</xsl:template>
	<xsl:template match="allocated">
		<maxNumberOfSeats>
			<xsl:value-of select="."/>
		</maxNumberOfSeats>
	</xsl:template>
</xsl:stylesheet>