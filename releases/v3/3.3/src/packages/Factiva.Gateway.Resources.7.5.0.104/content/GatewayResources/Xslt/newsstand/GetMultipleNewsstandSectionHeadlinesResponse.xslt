<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
    <xsl:include href="../common/ReplyItem.xslt"/>
	<xsl:template match="/*">
    <xsl:apply-templates select ="//ResultSetList"></xsl:apply-templates>
	</xsl:template>
  <xsl:template match="//ResultSetList">
    <GetMultipleNewsstandSectionHeadlinesResponse>
      <newsstandSectionHeadlinesResultSet>
        <xsl:attribute name="count">
          <xsl:value-of select="count(//ResultSet)"/>
        </xsl:attribute>
        <xsl:apply-templates select="//Status"/>
        <xsl:call-template name="SectionInstance"/>
      </newsstandSectionHeadlinesResultSet>
    </GetMultipleNewsstandSectionHeadlinesResponse>
  </xsl:template>
	<xsl:template match="//Status">
    <xsl:attribute name="status">
      <xsl:value-of select="."/>
    </xsl:attribute>
	</xsl:template>
	<xsl:template name="SectionInstance">
		<xsl:for-each select="//ResultSet">
      <newsstandSectionHeadlinesResult>
        <xsl:apply-templates select="Folders"/>
        <xsl:apply-templates select="*[local-name()='PerformContentSearchResponse']"/>
      </newsstandSectionHeadlinesResult>
		</xsl:for-each>		
	</xsl:template>
  
  <xsl:template match="//*[local-name()='PerformContentSearchResponse']">
    <searchResponse>
      <xsl:apply-templates select="*[local-name()='contentSearchResult']"/>
    </searchResponse>
  </xsl:template>
  
  <xsl:template match="//*[local-name()='contentSearchResult']">
    <contentSearchResult hitCount="{@hitCount}" xmlns="http://types.factiva.com/search">
      <xsl:copy-of select="child::*"/>
    </contentSearchResult>
  </xsl:template>
  <xsl:template match="Folders">
    <xsl:attribute name="bookmark">
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@bookmark)) > 0">
          <xsl:value-of select="@bookmark"/>
        </xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
    <xsl:attribute name="status">
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@status)) > 0">
          <xsl:value-of select="@status"/>
        </xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
    <xsl:if test="count(FolderInfo)=0">
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@folderid)) > 0">
          <newsstandSectionInfo>
            <sectionID>
              <xsl:value-of select="@folderid"/>
            </sectionID>
          </newsstandSectionInfo>
        </xsl:when>
      </xsl:choose>
    </xsl:if>
    <xsl:apply-templates select="FolderInfo"/>
  </xsl:template>
	<xsl:template match="FolderInfo">
    <newsstandSectionInfo>
		<sectionID><xsl:value-of select="@folderid"/></sectionID>
    <sectionCode><xsl:value-of select="normalize-space(SectionCode)"/></sectionCode>
		<sectionName><xsl:value-of select="normalize-space(substring-after(SectionName, '|'))"/></sectionName> 
		<sourceCode><xsl:value-of select="normalize-space(SourceCode)"/></sourceCode> 
		<sourceName><xsl:value-of select="normalize-space(substring-after(SourceName, '|'))"/></sourceName>
    </newsstandSectionInfo>
  </xsl:template>
</xsl:stylesheet>

