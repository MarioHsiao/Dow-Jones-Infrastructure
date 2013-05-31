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
		        <SourceCode><xsl:value-of select="sourceCode"/></SourceCode>
	        <DateInfo>
	        	<xsl:if test="includePublicationDates='true'">Yes</xsl:if>
	        	<xsl:if test="includePublicationDates='false'">No</xsl:if>
	        	<xsl:if test="includePublicationDates='1'">Yes</xsl:if>
	        	<xsl:if test="includePublicationDates='0'">No</xsl:if>
	        	
	        </DateInfo>
			        
		</Request>
	</xsl:template>
</xsl:stylesheet>