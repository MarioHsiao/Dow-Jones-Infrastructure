<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="xsl xsi user">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="/">
		<xsl:element name="ValidateUserResponse">
			<xsl:call-template name="GetError"/>
			<AccountId>
				<xsl:value-of select="//accountId" />
			</AccountId>
			<CompanyName>
				<xsl:value-of select="//companyName" />
			</CompanyName>
			<Email>
				<xsl:value-of select="//email" />
			</Email>
			<FirstName>	
				<xsl:value-of select="//firstName" />
			</FirstName>
			<LastName>
				<xsl:value-of select="//lastName" />
			</LastName>
			<ProfileId>
				<xsl:value-of select="//profileId" />
			</ProfileId>
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
