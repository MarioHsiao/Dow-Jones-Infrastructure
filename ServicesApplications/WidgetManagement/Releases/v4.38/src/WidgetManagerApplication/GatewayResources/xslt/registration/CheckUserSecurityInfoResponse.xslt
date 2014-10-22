<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:include href="../common/commonElements.xslt"/>
	<xsl:include href="../common/email_auth_common_elements.xslt"/>
	<xsl:strip-space elements="*"/>
	<xsl:template match="/*">
		<CheckUserSecurityInfoResponse>
			<xsl:apply-templates select="ResultSet/Result"/>
		</CheckUserSecurityInfoResponse>
	</xsl:template>

	<xsl:template match="ResultSet/Result">
		<xsl:apply-templates select="AccountId"/>
		<xsl:apply-templates select="bulkUser"/>
		<xsl:apply-templates select="email"/>
		<xsl:apply-templates select="LanguagePref"/>
		<xsl:apply-templates select="Namespace"/>
		<xsl:apply-templates select="UserId"/>

	</xsl:template>

	<xsl:template match="AccountId">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">accountID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>	

	<xsl:template match="bulkUser">
		<bulkRegistrationInfo>
			<xsl:choose>
				<xsl:when test="normalize-space(string(.))='Y'">BulkRegistration</xsl:when>
				<xsl:when test="normalize-space(string(.))='N'">NoBulkRegistration</xsl:when>
				<xsl:when test="normalize-space(string(.))='U'">BulkRegistrationWithUpdatedPassword</xsl:when>
				<xsl:otherwise/>
			</xsl:choose>
		</bulkRegistrationInfo>
	</xsl:template>

	<xsl:template match="LanguagePref">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">emailLanguage</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="Namespace">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">namespace</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="UserId">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">userID</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

</xsl:stylesheet>