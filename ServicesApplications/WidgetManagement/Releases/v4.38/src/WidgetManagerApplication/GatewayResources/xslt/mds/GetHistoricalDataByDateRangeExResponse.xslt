<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
<xsl:include href="GetHistoricalDataExResponse.xslt"/>
	<xsl:template match="/*">
		<GetHistoricalDataByDateRangeExResponse>
		 	<historicalDataResponse>
				<!--<xsl:apply-templates select="/*/Status"/>-->
				<xsl:apply-templates select="/*/ResultSet"/>
			</historicalDataResponse>
		</GetHistoricalDataByDateRangeExResponse>
	</xsl:template>
</xsl:stylesheet>
