<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:include href="../common/commonElements.xslt"/>
	<xsl:include href="../common/email_auth_common_elements.xslt"/>
	<xsl:strip-space elements="*"/>
	<xsl:template match="/*">
		<PerformTwoPhasePAYGUserRegistrationResponse>
			<paygRegistrationResponse>
				<xsl:apply-templates select="ResultSet/Result"/>
			</paygRegistrationResponse>
		</PerformTwoPhasePAYGUserRegistrationResponse>
	</xsl:template>

	<xsl:template match="ResultSet/Result">
		<xsl:apply-templates select="acpToken"/>
		<xsl:apply-templates select="ACCOUNT_ID"/>
		<xsl:apply-templates select="PRODUCT_ID"/>
		<xsl:apply-templates select="USER_ID"/>
		<xsl:apply-templates select="Namespace"/>
		<xsl:apply-templates select="isoCountryCode"/>
		<xsl:apply-templates select="stateName"/>

	</xsl:template>

	<xsl:template match="acpToken">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">acpToken</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="ACCOUNT_ID">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">accountID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="PRODUCT_ID">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">productID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="USER_ID">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">userID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="Namespace">
		<xsl:call-template name="tagOptional">
			<xsl:with-param name="newNodeName">namespace</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="isoCountryCode">
		<xsl:call-template name="tagOptional">
			<xsl:with-param name="newNodeName">countryCode</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="stateName">
		<xsl:call-template name="tagOptional">
			<xsl:with-param name="newNodeName">stateName</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

</xsl:stylesheet>



