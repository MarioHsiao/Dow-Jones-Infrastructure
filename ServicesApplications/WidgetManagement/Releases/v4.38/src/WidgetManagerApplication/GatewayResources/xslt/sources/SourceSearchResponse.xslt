<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl user" exclude-result-prefixes="user" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="CDBSearchResponse.xslt"/>
  <xsl:template name="SourceSearchResponse">
    <sourceSearchResponse>
      <sourceSearchResult>
        <xsl:apply-templates select="//Status[@value]"/>
        <xsl:apply-templates select="//Control"/>
        <xsl:apply-templates select="//ContContextString"/>
        <xsl:call-template name="ResultSet"/>
      </sourceSearchResult>
    </sourceSearchResponse>
  </xsl:template>
	<xsl:template match="//Control">
		<xsl:if test="position()=1">
			<xsl:copy-of select="."/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="//ContContextString">
		<xsl:if test="position()=1">
			<xsl:choose>
				<xsl:when test="string-length(normalize-space(.)) &gt; 0">
					<searchContext>
						<xsl:value-of select="normalize-space(.)"/>
					</searchContext>
				</xsl:when>
				<xsl:otherwise>
					<searchContext/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<xsl:template name="ResultSet">
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(//ResultSet/@total)) &gt; 0">
				<queryHitCount>
					<xsl:value-of select="number(normalize-space(//ResultSet/@total))"/>
				</queryHitCount>
			</xsl:when>
			<xsl:otherwise>
				<queryHitCount>0</queryHitCount>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(//ResultSet/@first)) &gt; 0">
				<indexOfFirstHeadline>
					<xsl:value-of select="number(normalize-space(//ResultSet/@first))"/>
				</indexOfFirstHeadline>
			</xsl:when>
			<xsl:otherwise>
				<indexOfFirstHeadline>0</indexOfFirstHeadline>
			</xsl:otherwise>
		</xsl:choose>
		<sourcesResultSet>
			<xsl:attribute name="count"><xsl:value-of select="count(//ResultSet/Result)"/></xsl:attribute>
			<xsl:apply-templates select="//Result"/>
		</sourcesResultSet>
	</xsl:template>
	<xsl:template match="Result">
    <source>
      <xsl:attribute name="xsi:type">
        <xsl:choose>
          <xsl:when test="GroupDoc">GroupDoc</xsl:when>
          <xsl:when test="SourceDoc">SourceDoc</xsl:when>
        </xsl:choose>
      </xsl:attribute>
      
      <xsl:if test=".//SourceType">
			  <xsl:attribute name="sourceType"><xsl:value-of select="normalize-space(.//SourceType/@v)"/></xsl:attribute>
      </xsl:if>
      <xsl:if test=".//GroupType">
        <xsl:attribute name="groupType"><xsl:value-of select="normalize-space(.//GroupType/@v)"/></xsl:attribute>
      </xsl:if>
			<xsl:attribute name="lang"><xsl:choose><xsl:when test="count(child::DocData/InterfaceLang) &gt; 0">en</xsl:when><xsl:otherwise><xsl:value-of select="user:toLowerCase(string(.//DocData/InterfaceLang/@v))"/></xsl:otherwise></xsl:choose></xsl:attribute>
      <!--<xsl:apply-templates select="SourceDoc/ReplyItem"/>-->
      <xsl:apply-templates select="*/ReplyItem"/>        <!--include SourceDoc/ReplyItem and GroupDoc/ReplyItem-->
			<xsl:if test="string-length(normalize-space(LatestIssueDate/@v)) &gt; 0">
				<mostRecentIssue>
					<xsl:value-of select="user:ChangeDateFormat(string(normalize-space(LatestIssueDate/@v)))"/>
				</mostRecentIssue>
			</xsl:if>
		</source>
	</xsl:template>
	<!--<xsl:template match="SourceDoc/ReplyItem">-->
  <xsl:template match="*/ReplyItem">
		<xsl:apply-templates select="Verbose/*" mode="ReplyItem"/>
		<xsl:apply-templates select="Brief/*" mode="ReplyItem"/>
	</xsl:template>
</xsl:stylesheet>
