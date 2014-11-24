<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:include href="../common/commonElements.xslt"/>
	<xsl:include href="../common/email_auth_common_elements.xslt"/>
	<xsl:strip-space elements="*"/>
	<xsl:template match="/*">
		<GetLWRPermissionByEmailAddressResponse>
			<xsl:apply-templates select="ResultSet/Result"/>
		</GetLWRPermissionByEmailAddressResponse>
	</xsl:template>

	<xsl:template match="ResultSet/Result">
		<xsl:apply-templates select="email"/>
		<xsl:apply-templates select="accountId"/>
		<xsl:apply-templates select="productId"/>
		<xsl:apply-templates select="state"/>
		<xsl:apply-templates select="userId"/>

	</xsl:template>

	<xsl:template match="accountId">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">AccountID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="productId">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">namespace</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="state">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">state</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="userId">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">userID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

</xsl:stylesheet>
