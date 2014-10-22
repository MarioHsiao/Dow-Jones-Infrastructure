<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="/*">
		<UpdateUserDataResponse>
			<xsl:copy-of select="/*/Status"/>
		</UpdateUserDataResponse>
	</xsl:template>
</xsl:stylesheet>
