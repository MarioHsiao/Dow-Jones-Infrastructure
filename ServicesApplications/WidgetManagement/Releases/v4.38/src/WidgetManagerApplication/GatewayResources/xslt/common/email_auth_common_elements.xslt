<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl">
	<xsl:import href="commonElements.xslt"/>
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <msxsl:script language="JScript" implements-prefix="user"><![CDATA[
	function utf8Unescape(str){
		return unescape(str);
	}
]]></msxsl:script>
	<xsl:template match="emailAddress">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">EmailAddress</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="securityQuestionIndex">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">SecurityQuestion</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="securityAnswer">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">SecurityAnswer</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="confirmationPageURL">
		<xsl:call-template name="tagOptional">
			<xsl:with-param name="newNodeName">SecondaryURL</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="getEmailFromFactiva">
		<!--Do nothing-->
	</xsl:template>
	<xsl:template match="emailLanguage">
		<xsl:call-template name="tagOptional">
			<xsl:with-param name="newNodeName">LanguagePref</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="LoginPageURL">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">LoginPageURL</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template match="//emailAuthToken">
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(.)) &gt; 0">
				<xsl:element name="emailConfirmationToken">
					<xsl:value-of select="user:utf8Unescape(normalize-space(string(.)))"/>
				</xsl:element>
			</xsl:when>
			<xsl:otherwise>
				<xsl:element name="emailConfirmationToken"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="//email">
		<xsl:call-template name="tagRequired">
			<xsl:with-param name="newNodeName">EmailAddress</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="EmailTokenResponse">
		<emailTokenResponse>
			<emailTokenResult>
				<xsl:copy-of select="Control"/>
				<xsl:copy-of select="/*/Status"/>
				<xsl:apply-templates select="//emailAuthToken"/>
			</emailTokenResult>
		</emailTokenResponse>
	</xsl:template>
</xsl:stylesheet>
