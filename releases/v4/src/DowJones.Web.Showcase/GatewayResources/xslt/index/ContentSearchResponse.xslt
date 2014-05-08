<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="../common/ReplyItem.xslt"/>

	<xsl:template name="contentSearch">
		<contentSearchResponse>
			<contentSearchResult>
				<xsl:apply-templates select="//Status"/>
				<xsl:apply-templates select="//Control"/>
				<xsl:apply-templates select="//CanonicalString"/>
				<xsl:apply-templates select="//ContContextString"/>
				<xsl:call-template name="ResultSet"/>
			</contentSearchResult>
		</contentSearchResponse>
	</xsl:template>

	<xsl:template match="//Control">
		<xsl:if test="position()=1">
			<xsl:copy-of select="."/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="//Status">
		<xsl:if test="position()=1">
			<xsl:copy-of select="."/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="//CanonicalString">
		<xsl:if test="position()=1">
			<xsl:choose>
				<xsl:when test="string-length(normalize-space(.)) &gt; 0">
					<highlightString>
						<xsl:value-of select="normalize-space(string(.))"/>
					</highlightString>
				</xsl:when>
				<xsl:otherwise>
					<highlightString />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template match="//ContContextString" >
		<xsl:if test="position()=1">
			<xsl:choose>
				<xsl:when test="string-length(normalize-space(.)) &gt; 0">
					<searchContext>
						<xsl:value-of select="normalize-space(string(.))"/>
					</searchContext>
				</xsl:when>
				<xsl:otherwise>
					<searchContext/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="ResultSet">
		<xsl:param name="isFCP"/>
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(//ResultSet/@total)) &gt; 0">
				<queryHitCount>
					<xsl:value-of select="normalize-space(string(number(//ResultSet/@total)))"/>
				</queryHitCount>
			</xsl:when>
			<xsl:otherwise>
				<queryHitCount/>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(//ResultSet/@first)) &gt; 0">
				<indexOfFirstHeadline>
					<xsl:value-of select="normalize-space(string(number(//ResultSet/@first)))"/>
				</indexOfFirstHeadline>
			</xsl:when>
			<xsl:otherwise>
				<indexOfFirstHeadline/>
			</xsl:otherwise>
		</xsl:choose>
		<contentHeadlinesResultSet>
			
			<xsl:attribute name="count"><xsl:value-of select="count(//ResultSet/Result)"/></xsl:attribute>
			<xsl:apply-templates select="//Result"/>
			
		</contentHeadlinesResultSet>
	</xsl:template>
	
	<xsl:template match="//Result">
		<contentHeadline>
			<!--<xsl:attribute name="relevance"><xsl:value-of select="number(@relscore)"/></xsl:attribute>-->
			<xsl:apply-templates select="ReplyItem"/>
		</contentHeadline>
	</xsl:template>
</xsl:stylesheet>
