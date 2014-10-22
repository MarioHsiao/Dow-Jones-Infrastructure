<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
<xsl:include href="GetHistoricalDataResponse.xslt"/>
	<xsl:template match="/*">
		<GetHistoricalDataByDateRangeResponse>
		 	<historicalDataResponse>
				<!--<xsl:apply-templates select="/*/Status"/>-->
				<xsl:apply-templates select="/*/ResultSet"/>
			</historicalDataResponse>
		</GetHistoricalDataByDateRangeResponse>
	</xsl:template>
</xsl:stylesheet>
