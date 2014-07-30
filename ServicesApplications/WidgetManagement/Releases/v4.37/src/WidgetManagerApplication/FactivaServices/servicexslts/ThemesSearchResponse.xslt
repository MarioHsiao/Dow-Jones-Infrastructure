<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:srch="http://types.factiva.com/search">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:param name="ClusterNdx"></xsl:param>
	<xsl:template match="/*">
		<xsl:element name="PerformContentSearchResponse" namespace="http://types.factiva.com/search">
			<xsl:apply-templates select="//srch:contentSearchResult"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//srch:contentSearchResult">
		<xsl:element name="contentSearchResult" namespace="http://types.factiva.com/search">
			<xsl:attribute name="hitCount"><xsl:value-of select="@hitCount"/></xsl:attribute>
			<xsl:attribute name="indexOfFirstHeadline"><xsl:value-of select="@indexOfFirstHeadline"/></xsl:attribute>
			<xsl:apply-templates select="//srch:canonicalQueryString"/>
			<xsl:apply-templates select="//srch:queryTransformSet"/>
			<xsl:apply-templates select="//srch:collectionCountSet"/>
			<xsl:apply-templates select="//srch:codeNavigatorSet"/>
			<xsl:apply-templates select="//srch:timeNavigatorSet"/>
			<xsl:apply-templates select="//srch:clusterSet"/>
			<xsl:element name="contentHeadlinesResultSet" namespace="http://types.factiva.com/search">
				<xsl:attribute name="count"><xsl:value-of select="count(//srch:clusterSet/srch:cluster[position()=$ClusterNdx+1]/srch:index)"/></xsl:attribute>
				<xsl:for-each select="//srch:clusterSet/srch:cluster[position()=$ClusterNdx+1]/srch:index">
					<xsl:variable name="Ndx">
						<xsl:value-of select="."/>
					</xsl:variable>
					<xsl:apply-templates select="//srch:contentHeadline[position()=$Ndx+1]"/>
				</xsl:for-each>
			</xsl:element>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//srch:canonicalQueryString">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="//srch:queryTransformSet">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="//srch:collectionCountSet">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="//srch:codeNavigatorSet">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="//srch:timeNavigatorSet">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="//srch:clusterSet">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="//srch:contentHeadline">
		<xsl:copy-of select="."/>
	</xsl:template>
</xsl:stylesheet>
