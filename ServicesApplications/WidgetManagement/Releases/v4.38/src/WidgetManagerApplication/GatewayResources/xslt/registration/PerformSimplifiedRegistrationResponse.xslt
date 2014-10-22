<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="../common/commonElements.xslt"/>
	<xsl:include href="../common/email_auth_common_elements.xslt"/>
	<xsl:strip-space elements="*"/>
	<xsl:template match="/*">
		<PerformSimplifiedRegistrationResponse>
			<xsl:call-template name="EmailRegistrationResponse"/>
		</PerformSimplifiedRegistrationResponse>
	</xsl:template>
	<xsl:template match="//accountId">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">AccountID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="//encryptedLogin">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">EncryptedID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="//namespace">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">namespace</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="//userId">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">UserID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="EmailRegistrationResponse">
		<EmailRegistrationResponse>
				<xsl:copy-of select="Control"/>
				<xsl:copy-of select="/*/Status"/>
				<xsl:apply-templates select="//accountId"/>
				<xsl:apply-templates select="//encryptedLogin"/>
				<xsl:apply-templates select="//namespace"/>
				<xsl:apply-templates select="//userId"/>
				<xsl:apply-templates select="//emailAuthToken"/>
				<xsl:apply-templates select="//email"/>
		</EmailRegistrationResponse>
	</xsl:template>
</xsl:stylesheet>
