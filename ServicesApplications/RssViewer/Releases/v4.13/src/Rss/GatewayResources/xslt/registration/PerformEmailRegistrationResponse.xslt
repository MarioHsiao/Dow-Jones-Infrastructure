<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="../common/commonElements.xslt"/>
	<xsl:include href="../common/email_auth_common_elements.xslt"/>
	<xsl:template match="/*">
		<PerformEmailRegistrationResponse>
			<xsl:call-template name="EmailRegistrationResponse"/>
		</PerformEmailRegistrationResponse>
	</xsl:template>
	

	<xsl:strip-space elements="*"/>
	<xsl:template match="//ACCOUNT_ID">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">AccountID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="//ENCRYPTED_LOGIN">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">EncryptedID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="//PRODUCT_ID">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">namespace</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="//USER_ID">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">UserID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="EmailRegistrationResponse">
		<EmailRegistrationResponse>
				<xsl:copy-of select="Control"/>
				<xsl:copy-of select="/*/Status"/>
				<xsl:apply-templates select="//ACCOUNT_ID"/>
				<xsl:apply-templates select="//ENCRYPTED_LOGIN"/>
				<xsl:apply-templates select="//PRODUCT_ID"/>
				<xsl:apply-templates select="//USER_ID"/>
				<xsl:apply-templates select="//email"/>
				<xsl:apply-templates select="//emailAuthToken"/>
		</EmailRegistrationResponse>
	</xsl:template>	
</xsl:stylesheet>
