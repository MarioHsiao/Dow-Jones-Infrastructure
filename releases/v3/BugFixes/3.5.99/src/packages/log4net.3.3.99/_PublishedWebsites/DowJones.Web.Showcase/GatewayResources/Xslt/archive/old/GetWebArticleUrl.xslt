<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="*">
		<Request>
			<typ>1</typ> <!-- not sure what this means but archive needs it-->
			<xsl:element name="nrq">
				<xsl:value-of select ="count (.//accessionNumbers/string)"/>
			</xsl:element>
			<xsl:apply-templates select ="//accessionNumbers/string"/>
		</Request>
	</xsl:template>

	<xsl:template match="//accessionNumbers/string">
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:element name="acc{$pos}">
			<xsl:value-of select="."/>
		</xsl:element>
		<xsl:element name="oty{$pos}">URLR</xsl:element>		
	</xsl:template>
</xsl:stylesheet>
