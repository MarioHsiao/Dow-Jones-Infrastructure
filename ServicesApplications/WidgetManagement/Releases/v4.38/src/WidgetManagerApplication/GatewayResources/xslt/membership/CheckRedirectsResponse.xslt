<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="/*">
		<CheckRedirectsResponse>
			<xsl:call-template name="GetError"/>
			<Redirects>
				<xsl:apply-templates select="//Result"/>
			</Redirects>
		</CheckRedirectsResponse>
	</xsl:template>

	<xsl:template name="GetError">
		<xsl:element name="ERROR_CODE">
			<xsl:value-of select="//ERROR_CODE"/>
		</xsl:element>
		<xsl:element name="ERROR_GENERAL_MSG">
			<xsl:value-of select="//ERROR_GENERAL_MSG"/>
		</xsl:element>
	</xsl:template>

	<xsl:template match="//Result">
		<xsl:call-template name="redirect">
			<xsl:with-param name="var_type" select="'AFR'" />
			<xsl:with-param name="var_value" select="AFR/." />
		</xsl:call-template>
		<xsl:call-template name="redirect">
			<xsl:with-param name="var_type" select="'CCR'" />
			<xsl:with-param name="var_value" select="CCR/." />
		</xsl:call-template>
		<xsl:call-template name="redirect">
			<xsl:with-param name="var_type" select="'NS'" />
			<xsl:with-param name="var_value" select="NS/." />
		</xsl:call-template>
		<xsl:call-template name="redirect">
			<xsl:with-param name="var_type" select="'NDR'" />
			<xsl:with-param name="var_value" select="NDR/." />
		</xsl:call-template>
		<xsl:call-template name="redirect">
			<xsl:with-param name="var_type" select="'TLIR'" />
			<xsl:with-param name="var_value" select="TLIR/." />
		</xsl:call-template>
		<xsl:call-template name="redirect">
			<xsl:with-param name="var_type" select="'WNDR'" />
			<xsl:with-param name="var_value" select="WNDR/." />
		</xsl:call-template>
		<xsl:call-template name="redirect">
			<xsl:with-param name="var_type" select="'WAFR'" />
			<xsl:with-param name="var_value" select="WAFR/." />
		</xsl:call-template>
		<xsl:call-template name="redirect">
			<xsl:with-param name="var_type" select="'WCCR'" />
			<xsl:with-param name="var_value" select="WCCR/." />
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="redirect">
		<xsl:param name="var_type" select="''" />
		<xsl:param name="var_value" select="''" />

		<xsl:choose>
			<xsl:when test="string-length(normalize-space($var_value)) &gt; 0">
				<redirect>
					<xsl:attribute name="type">
						<xsl:value-of select="$var_type" />
					</xsl:attribute>
					<xsl:value-of select="$var_value" />
				</redirect>
			</xsl:when>
			<xsl:otherwise></xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>
