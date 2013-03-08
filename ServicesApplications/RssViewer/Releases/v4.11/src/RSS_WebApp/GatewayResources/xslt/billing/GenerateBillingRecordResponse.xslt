<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="*">
		<xsl:apply-templates select="/*/Status"/>
		<xsl:apply-templates select="UERAuthorizeAndBillResponse"/>
	</xsl:template>

<xsl:template match="UERAuthorizeAndBillResponse">
		<GenerateBillingRecordResponse>
			<xsl:copy-of select="Status"/>
		</GenerateBillingRecordResponse>
	</xsl:template>
	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>
</xsl:stylesheet>
