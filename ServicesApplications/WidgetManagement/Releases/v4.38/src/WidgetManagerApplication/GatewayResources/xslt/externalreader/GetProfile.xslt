<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:include href="../common/utility.xslt"/>
	<xsl:template match="//profile">
		<profile>
			<xsl:apply-templates select="profileId"/>
			<properties>
				<xsl:apply-templates select="allocated"/>
				<xsl:apply-templates select="checkDomains"/>
				<xsl:call-template name="clientLogo"/>
				<xsl:apply-templates select="clientMsg"/>
				<xsl:apply-templates select="profileName"/>
				<xsl:apply-templates select="registered"/>
				<xsl:call-template name="accountLogo"/>
				<xsl:call-template name="domains"/>
			</properties>
		</profile>
	</xsl:template>
	<xsl:template match="profileId">
		<id>
			<xsl:value-of select="."/>
		</id>
	</xsl:template>
	<xsl:template match="allocated">
		<maxNumberOfSeats>
			<xsl:value-of select="."/>
		</maxNumberOfSeats>
	</xsl:template>
	<xsl:template match="checkDomains">
		<checkAgainstWhitelistedDomains>
			<xsl:choose>
				<xsl:when test=".='Y'">true</xsl:when>
				<xsl:when test=".='N'">false</xsl:when>
				<xsl:otherwise>false</xsl:otherwise>
			</xsl:choose>
		</checkAgainstWhitelistedDomains>
	</xsl:template>
	<xsl:template name="clientLogo">
		<clientLogo>
			<url>
				<xsl:value-of select="clientLogoUrl"/>
			</url>
			<text>
				<xsl:value-of select="clientLogoText"/>
			</text>
			<alignment>
				<xsl:value-of select="clientLogoAlign"/>
			</alignment>
		</clientLogo>
	</xsl:template>
	<xsl:template match="clientMsg">
		<clientMessage>
			<xsl:value-of select="."/>
		</clientMessage>
	</xsl:template>
	<xsl:template match="profileName">
		<name>
			<xsl:value-of select="."/>
		</name>
	</xsl:template>
	<xsl:template match="registered">
		<registeredNumberOfSeats>
			<xsl:value-of select="."/>
		</registeredNumberOfSeats>
	</xsl:template>
	<xsl:template name="accountLogo">
		<accountLogo>
			<url>
				<xsl:value-of select="selfLogoUrl"/>
			</url>
			<text>
				<xsl:value-of select="selfLogoText"/>
			</text>
			<alignment>
				<xsl:value-of select="selfLogoAlign"/>
			</alignment>
		</accountLogo>
	</xsl:template>
	<xsl:template name="domains">
		<xsl:if test="string-length(normalize-space(domains)) &gt; 0">
			<xsl:call-template name="Strsplit">
				<xsl:with-param name="string">
					<xsl:value-of select="domains"/>
				</xsl:with-param>
				<xsl:with-param name="pattern">,</xsl:with-param>
				<xsl:with-param name="elementName">whitelistedDomain</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>