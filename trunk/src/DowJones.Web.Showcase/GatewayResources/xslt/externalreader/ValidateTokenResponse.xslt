<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="xsl xsi user">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="/">
		<xsl:element name="ValidateTokenResponse">
			<xsl:call-template name="GetError"/>
      <xsl:copy-of select="//userActionData"/>
      <xsl:copy-of select="//email"/>
      <xsl:copy-of select="//profileId"/>
      <xsl:copy-of select="//firstName"/>
      <xsl:copy-of select="//lastName"/>
      <xsl:copy-of select="//accountId"/>
      <xsl:copy-of select="//companyName"/>
      <xsl:if test="//isoCountryCode != 'ZZ'">
        <xsl:copy-of select="//isoCountryCode"/>
      </xsl:if>
      <xsl:if test="//state != 'ZZ'">
      <xsl:copy-of select="//state"/>
      </xsl:if>
    </xsl:element>
	</xsl:template>
	<xsl:template name="GetError">
		<xsl:element name="ERROR_CODE">
			<xsl:value-of select="//ERROR_CODE"/>
		</xsl:element>
		<xsl:element name="ERROR_GENERAL_MSG">
			<xsl:value-of select="//ERROR_GENERAL_MSG"/>
		</xsl:element>
	</xsl:template>

</xsl:stylesheet>
