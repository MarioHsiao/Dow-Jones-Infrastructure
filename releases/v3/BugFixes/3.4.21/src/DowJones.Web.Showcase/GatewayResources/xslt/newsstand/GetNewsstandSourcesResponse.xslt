<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl user">
    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

	<xsl:template match="/*">
		<GetNewsstandSourcesResponse>
			<newsstandSourcesResponse>
			<xsl:apply-templates select="//Status"/>
			<xsl:apply-templates select="//SourceList"/>
			</newsstandSourcesResponse>
		</GetNewsstandSourcesResponse>
	</xsl:template>
	
	<xsl:template match="//Status">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="//SourceList">
		<newsstandSourcesResultSet>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates select="Source"/>
		</newsstandSourcesResultSet>
	</xsl:template>
	
	 <xsl:template match="Source">
		 	<sourceInfo>
		 		<xsl:choose>
					<xsl:when test="string-length(normalize-space(DaysToKeep/@value)) &gt; 0">
					<daysKept><xsl:value-of select="normalize-space(DaysToKeep/@value)"/></daysKept>	
					</xsl:when>
					<xsl:otherwise><daysKept>0</daysKept></xsl:otherwise>
				</xsl:choose>
		 		<xsl:choose>
					<xsl:when test="string-length(normalize-space(@sectioncount)) &gt; 0">
					<sectionCount><xsl:value-of select="normalize-space(@sectioncount)"/></sectionCount>
					</xsl:when>
					<xsl:otherwise><sectionCount>0</sectionCount></xsl:otherwise>
				</xsl:choose>
		 		<xsl:choose>
					<xsl:when test="string-length(normalize-space(@sourcecode)) &gt; 0">
					<sourceCode><xsl:value-of select="normalize-space(@sourcecode)"/></sourceCode>
					</xsl:when>
					<xsl:otherwise><sourceCode/></xsl:otherwise>
				</xsl:choose>
		 		<xsl:choose>
					<xsl:when test="string-length(normalize-space(substring-after(SourceName,'|'))) &gt; 0">
					<sourceName><xsl:value-of select="normalize-space(substring-after(SourceName,'|'))"/></sourceName>
					</xsl:when>
					<xsl:otherwise><sourceName/></xsl:otherwise>
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="string-length(normalize-space(@sourcetype)) &gt; 0">
					<sourceCategory><xsl:value-of select="normalize-space(@sourcetype)"/></sourceCategory>
					</xsl:when>
					<xsl:otherwise><sourceCategory>publication</sourceCategory></xsl:otherwise>
				</xsl:choose>
		 	</sourceInfo>
	 </xsl:template>
	
</xsl:stylesheet>

