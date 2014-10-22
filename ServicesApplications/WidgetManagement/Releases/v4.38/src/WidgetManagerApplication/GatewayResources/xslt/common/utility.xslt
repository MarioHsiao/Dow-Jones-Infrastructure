<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Strsplit">
		<xsl:param name="string" select="''" />
		<xsl:param name="pattern"/>
		<xsl:param name="elementName"/>
		<xsl:choose>
			<xsl:when test="not($string)" />
			<xsl:when test="not($pattern)">
				<xsl:call-template name="_split-characters">
					<xsl:with-param name="string" select="$string" />
					<xsl:with-param name="elementName" select="$elementName" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="_split-pattern">
					<xsl:with-param name="string" select="$string" />
					<xsl:with-param name="pattern" select="$pattern" />
					<xsl:with-param name="elementName" select="$elementName" />
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="_split-characters">
		<xsl:param name="string" />
		<xsl:param name="elementName"/>
		<xsl:element  name="{$elementName}">
			<xsl:if test="$string">
				<xsl:value-of select="substring($string, 1, 1)" />
				<xsl:call-template name="_split-characters">
					<xsl:with-param name="string" select="substring($string, 2)" />
					<xsl:with-param name="elementName" select="$elementName" />
				</xsl:call-template>
			</xsl:if>
		</xsl:element>
	</xsl:template>

	<xsl:template name="_split-pattern">
		<xsl:param name="string" />
		<xsl:param name="pattern" />
		<xsl:param name="elementName"/>
		<xsl:choose>
			<xsl:when test="contains($string, $pattern)">
				<xsl:if test="not(starts-with($string, $pattern))">
					<xsl:element  name="{$elementName}">
						<xsl:value-of select="substring-before($string, $pattern)"/>
					</xsl:element>
				</xsl:if>
				<xsl:call-template name="_split-pattern">
					<xsl:with-param name="string" select="substring-after($string,$pattern)" />
					<xsl:with-param name="pattern" select="$pattern" />
					<xsl:with-param name="elementName" select="$elementName" />
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:element  name="{$elementName}">
					<xsl:value-of select="$string" />
				</xsl:element>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>