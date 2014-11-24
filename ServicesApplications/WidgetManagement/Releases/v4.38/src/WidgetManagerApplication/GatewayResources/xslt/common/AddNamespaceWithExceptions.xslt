<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:param name="addnamespace"/>
	<xsl:param name="exception_namespace"/>
	<xsl:variable name="SoapNS">http://schemas.xmlsoap.org/soap/envelope/</xsl:variable>
	
	<xsl:template match="/">
	<xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
	</xsl:template>
	
	<xsl:template match="node()" mode="AddNamespace">
		<xsl:choose>
			<xsl:when test="namespace-uri()=$SoapNS">
				<xsl:element name="{concat('SOAP-ENV:', local-name())}" namespace="{$SoapNS}">
					<xsl:choose>
						<xsl:when test="local-name()='Fault'">
							<xsl:copy-of select="node()"	/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
						</xsl:otherwise>	
					</xsl:choose>	
				</xsl:element>
			</xsl:when>
			<xsl:when test="self::*">
				<xsl:choose>
					<xsl:when test="namespace-uri()=$exception_namespace">
						<xsl:element name="{name()}" namespace="{$exception_namespace}">
							<xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
						</xsl:element>
					</xsl:when>
					<xsl:otherwise>
						<xsl:element name="{name()}" namespace="{$addnamespace}">
							<xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
						</xsl:element>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="@*" mode="AddNamespace">
		<xsl:copy>
			<xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>
