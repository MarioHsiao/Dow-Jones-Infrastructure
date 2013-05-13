<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

	<xsl:template match="*">
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates select="authorizationCodes"/>
			<xsl:apply-templates select="authorizationCode"/>
			<xsl:apply-templates select="functionCode"/>
			<xsl:apply-templates select="publicationCode"/>
			<xsl:apply-templates select="billingCodes"/>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="authorizationCodes">
			<xsl:apply-templates select="authorizationCode"/>
	</xsl:template>

	<xsl:template match="authorizationCode">
			<AuthCode><xsl:value-of select="."/></AuthCode>
	</xsl:template>

	<xsl:template match="functionCode">
		<FunctionCode><xsl:value-of select="."/></FunctionCode>
		<GenerateBillingRec>1</GenerateBillingRec>
	</xsl:template>


	<xsl:template match="publicationCode">
		<PublicationCode><xsl:value-of select="."/></PublicationCode>
	</xsl:template>
	<xsl:template match="billingCodes">
		<xsl:apply-templates select="billingCode"/>
	</xsl:template>
	<xsl:template match="billingCode">
		<xsl:element name="{@code}"><xsl:value-of select="."/></xsl:element>
	</xsl:template>
</xsl:stylesheet>
