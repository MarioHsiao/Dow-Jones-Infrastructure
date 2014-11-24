<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:include href="../common/commonElements.xslt"/>
	<xsl:include href="../common/email_auth_common_elements.xslt"/>
	<xsl:strip-space elements="*"/>
	<xsl:template match="/*">
		<GetLWRPermissionByAccountIDResponse>
			<xsl:apply-templates select="ResultSet/Result"/>
		</GetLWRPermissionByAccountIDResponse>
	</xsl:template>

	<xsl:template match="ResultSet/Result">
		<xsl:apply-templates select="lwrFlag"/>

	</xsl:template>

	<xsl:template match="lwrFlag">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">lwrFlag</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

</xsl:stylesheet>
