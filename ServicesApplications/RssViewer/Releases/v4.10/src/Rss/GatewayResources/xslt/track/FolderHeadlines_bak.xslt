<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:search="http://types.factiva.com/search">
<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
    <xsl:include href="../common/ReplyItem.xslt"/>

    <xsl:template match="//Status">
        <xsl:copy-of select="."/>
    </xsl:template>

	<xsl:template match="//ResultSet">
		<folderHeadlinesResultSet>
		   	<xsl:choose>
				<xsl:when test="string-length(normalize-space(@count)) &gt; 0">
					<xsl:attribute name="count"><xsl:value-of select="@count"/></xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="count">0</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:apply-templates select="Result"/>
		</folderHeadlinesResultSet>
	</xsl:template>
	<xsl:template match="Result">
		<folderHeadline>
			<!--<xsl:attribute name="relevance"><xsl:value-of select="@rl"/></xsl:attribute>-->
			<xsl:apply-templates select="ReplyItem"/>
			<editor>
				<xsl:choose>
					<xsl:when test="normalize-space(@priority)='hot'"><priority>Hot</priority></xsl:when>
					<xsl:when test="normalize-space(@priority)='new'"><priority>New</priority></xsl:when>
					<xsl:when test="normalize-space(@priority)='must read'"><priority>MustRead</priority></xsl:when>
					<xsl:when test="normalize-space(@priority)='none'"><priority>None</priority></xsl:when>
<xsl:otherwise><priority>None</priority></xsl:otherwise>
				</xsl:choose>
				
				<xsl:if test="normalize-space(@comment) !='-1'">
					<comment><xsl:value-of select="normalize-space(CommentText)"/></comment>
				</xsl:if>
			</editor>
		</folderHeadline>
	</xsl:template>
	<xsl:template match="FolderInfo">
	   	<xsl:choose>
	    	<xsl:when test="@status='0'">
		<folder>
			<xsl:if test="string-length(normalize-space(QueryName)) &gt; 0">	
				<folderName><xsl:value-of select="normalize-space(QueryName)"/></folderName>
			</xsl:if>	
			<xsl:choose>
				<xsl:when test="string-length(normalize-space(@folderid)) &gt; 0">
					<folderID><xsl:value-of select="normalize-space(@folderid)"/></folderID>
				</xsl:when>
				<xsl:otherwise>	<folderID/></xsl:otherwise>
			</xsl:choose>
			<xsl:if test="string-length(normalize-space(QueryHighlight)) &gt; 0">
				<highlightString><xsl:value-of select="normalize-space(QueryHighlight)"/></highlightString>
			</xsl:if>	
			<xsl:if test="string-length(normalize-space(@bookmark)) &gt; 0">
				<bookmark><xsl:value-of select="normalize-space(@bookmark)"/></bookmark>
			</xsl:if>	
			<xsl:if test="string-length(normalize-space(./Contact)) &gt; 0">
				<contact><xsl:value-of select="normalize-space(./Contact)"/></contact>
			</xsl:if>	
			<xsl:if test="string-length(normalize-space(@postMethod)) &gt; 0">
				<editorPostMethod>
					<xsl:if test="normalize-space(@postMethod)='auto'">Automatic</xsl:if>
					<xsl:if test="normalize-space(@postMethod)='manual'">Manual</xsl:if>
				</editorPostMethod>
			</xsl:if>
			<xsl:if test="string-length(normalize-space(@queryhitscount)) &gt; 0">
				<queryHitCount><xsl:value-of select="normalize-space(@queryhitscount)"/></queryHitCount>
			</xsl:if>
			<xsl:if test="string-length(normalize-space(@moreheadline)) &gt; 0">
				<moreHeadlines>
					<xsl:if test="normalize-space(@moreheadline)='yes'">true</xsl:if>
					<xsl:if test="normalize-space(@moreheadline)='no'">false</xsl:if>
			    	</moreHeadlines>
		    	</xsl:if>
			<xsl:apply-templates select="parent::ResultSet"/>
			<xsl:apply-templates select="../PerformContentSearchResponse"/>
    </folder>
	     </xsl:when>
	     <xsl:otherwise>
	     	<folder>
			<xsl:attribute name="status"><xsl:value-of select="@status"/></xsl:attribute>	     		
			<xsl:choose>
				<xsl:when test="string-length(normalize-space(@folderid)) &gt; 0">
					<folderID><xsl:value-of select="normalize-space(@folderid)"/></folderID>
				</xsl:when>
				<xsl:otherwise>	<folderID/></xsl:otherwise>
			</xsl:choose>
	     	</folder>
	     </xsl:otherwise>
	    </xsl:choose>
		
	</xsl:template>
	<xsl:template match="//PerformContentSearchResponse">
		<xsl:element name ="performContentSearchResponse">
			<xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="node()" mode="AddNamespace">
		<xsl:choose>
			<xsl:when test="self::*">
				<xsl:element name="{name()}"  namespace="http://types.factiva.com/search">
					<xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
				</xsl:element>
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
