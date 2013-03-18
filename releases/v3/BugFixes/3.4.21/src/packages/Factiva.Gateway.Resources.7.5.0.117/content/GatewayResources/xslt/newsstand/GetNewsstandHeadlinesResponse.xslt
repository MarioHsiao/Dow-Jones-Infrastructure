<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
    <xsl:include href="../common/ReplyItem.xslt"/>
	<xsl:template match="/*">
		<GetNewsstandHeadlinesResponse>
			<newsstandHeadlinesResponse>
				<newsstandHeadlinesResultSet>
					<xsl:attribute name="count">
						<xsl:value-of select="count(//ResultSet)"/>
					</xsl:attribute>
					<xsl:apply-templates select="//Status"/>
					<xsl:call-template name="SectionInstance"/>
				</newsstandHeadlinesResultSet>
			</newsstandHeadlinesResponse>
		</GetNewsstandHeadlinesResponse>
	</xsl:template>

	<xsl:template match="//Status">
		<xsl:copy-of select="."/>
	</xsl:template>
	
	<xsl:template name="SectionInstance">
		<xsl:for-each select="//ResultSet">
			<section>
				<xsl:apply-templates select="//FolderInfo"/>
				<xsl:apply-templates select="//ResultSet"/>
			</section>		
		</xsl:for-each>		
	</xsl:template>
	
	<xsl:template match="//FolderInfo">
		<sectionID><xsl:value-of select="@folderid"/></sectionID>
		<sectionName><xsl:value-of select="substring-after(SectionName, '|')"/></sectionName> 
		<sourceCode><xsl:value-of select="SourceCode"/></sourceCode> 
		<sourceName><xsl:value-of select="substring-after(SourceName, '|')"/></sourceName>
    <bookmark>
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@bookmark)) > 0"><xsl:value-of select="@bookmark"/></xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </bookmark>
    <hitCount>
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@totalhits)) > 0"><xsl:value-of select="@totalhits"/></xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>      
    </hitCount>
  </xsl:template>
	
	<xsl:template match="//ResultSet">
		<sectionHeadlinesResultSet>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates select="Result"/>
		</sectionHeadlinesResultSet>
	</xsl:template>

	<xsl:template match="Result">
		<newsstandHeadline >
			<xsl:apply-templates select="ReplyItem">
				<xsl:with-param name="type">newsstand</xsl:with-param>
			</xsl:apply-templates>
		</newsstandHeadline >
	</xsl:template>

</xsl:stylesheet>

