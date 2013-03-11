<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="ContentSearchResponse.xslt"/>

	<xsl:template match="/*">
		<PerformContentSearchResponse>
			<xsl:call-template name="contentSearch"/>
		</PerformContentSearchResponse>
	</xsl:template>
</xsl:stylesheet>
