<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

	<xsl:template match="/*">
		<HashURLResponse>

			<xsl:apply-templates select="/*/Status"/>
			<xsl:apply-templates select="/*/ResultSet"/>

		</HashURLResponse>
	</xsl:template>

	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="/*/ResultSet">
		<ResultSet>
			<xsl:for-each select="./Result">
				<Result>
					<xsl:attribute name="status">
						<xsl:value-of select="@status"/>
					</xsl:attribute>
					<uid>
						<xsl:value-of select="substring(@id,4)"/>
					</uid>
					<Value>
						<xsl:value-of select="."/>
					</Value>
				</Result>
			</xsl:for-each>
		</ResultSet>
	</xsl:template>

</xsl:stylesheet>
