<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="GetUserDataCommonResponse.xslt"/>
	<xsl:template match="/*">
		<GetUserDataResponse>
			<xsl:call-template name="UserDataResponse"/>
		</GetUserDataResponse>
	</xsl:template>
	<xsl:template match="isoCountryCode">
		<!--Since Yugoslavia (YU) is seperated to Serbia (RS) and Montenegro (ME)-->
		<!--for the backward compatible, we response YU as the countryCode when we get RS or ME-->
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(.)) &gt; 0">
				<xsl:element name="CountryCode">
					<xsl:choose>
						<xsl:when test="normalize-space(.)='RS' or normalize-space(.)='ME'">YU</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="normalize-space(string(.))"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:element>
			</xsl:when>
			<xsl:otherwise>
				<xsl:element name="CountryCode"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>