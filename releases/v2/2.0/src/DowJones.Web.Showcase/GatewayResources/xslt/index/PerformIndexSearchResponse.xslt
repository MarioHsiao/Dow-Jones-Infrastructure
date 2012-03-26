<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="ContentSearchResponse.xslt"/>

	<xsl:template match="/*">
    <PerformIndexSearchResponse>
			<xsl:call-template name="contentSearch"/>
		</PerformIndexSearchResponse>
	</xsl:template>
</xsl:stylesheet>
