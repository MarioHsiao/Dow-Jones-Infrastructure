<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

	<xsl:template match="/*">
		<UnHashURLResponse>

			<xsl:apply-templates select="/*/Status"/>
			<xsl:apply-templates select="/*/ResultSet"/>

		</UnHashURLResponse>
	</xsl:template>

	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="/*/ResultSet">
		<xsl:for-each select="./Result">
			<unhashedURLs>
				<status>
					<xsl:value-of select="@status"/>
				</status>
				<uid>
					<xsl:value-of select="@uid"/>
				</uid>
				<url>
					<xsl:value-of select="."/>
				</url>
			</unhashedURLs>
		</xsl:for-each>
	</xsl:template>

</xsl:stylesheet>