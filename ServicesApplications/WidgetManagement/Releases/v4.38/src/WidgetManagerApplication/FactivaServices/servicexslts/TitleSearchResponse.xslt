<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl user" exclude-result-prefixes="user">
<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
<xsl:include href="CDBSearchResponse.xslt"/>

<xsl:template match="//ContContextString">
	<xsl:if test="position()=1">
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(.)) &gt; 0"><searchContext><xsl:value-of select="normalize-space(.)"/></searchContext></xsl:when>
			<xsl:otherwise><searchContext/></xsl:otherwise>
		</xsl:choose>
	</xsl:if>
</xsl:template>

<xsl:template name="ResultSet">
	<sourcesResultSet>
		<xsl:attribute name="count"><xsl:value-of select="count(//ResultSet/Result)"/></xsl:attribute>
		<xsl:apply-templates select="//Result"/>
	</sourcesResultSet>
</xsl:template>

<xsl:template match="Result">
	<source>
		<xsl:attribute name="sourceType">
			<xsl:value-of select="normalize-space(//SourceType/@v)"/>
		</xsl:attribute>
		<xsl:attribute name="lang">
			<xsl:choose>
				<xsl:when test="count(child::DocData/InterfaceLang) &gt; 0">en</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="normalize-space(user:toLowerCase(string(.//DocData/InterfaceLang/@v)))"/>	
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:apply-templates select="SourceDoc/ReplyItem"/>
	</source>
</xsl:template>
<xsl:template match="SourceDoc/ReplyItem">
	<xsl:call-template name="applicableContentCategories"/>
	<xsl:apply-templates select="Verbose/*" mode="ReplyItem"/>
	<xsl:apply-templates select="Brief/*" mode="ReplyItem"/>
</xsl:template>


</xsl:stylesheet>
