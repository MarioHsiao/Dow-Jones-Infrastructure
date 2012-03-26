<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" extension-element-prefixes="msxsl">
	<xsl:output method="xml" omit-xml-declaration="yes" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:param name="marketingMessage"/>
	<xsl:template match="/GetArticleResponse/articleResponseSet">
		<html>
			<body>
				<xsl:apply-templates select="article"/>
			</body>
		</html>
	</xsl:template>
	<xsl:template match="article">
    <!-- Removed for POC -->
    <xsl:if test="string-length(normalize-space($marketingMessage)) > 0">
      <div>
        <h1><xsl:value-of select="normalize-space($marketingMessage)"/></h1>
        <br class="ISI_PAUSE"/>
        <br class="ISI_PAUSE"/>
        <br class="ISI_PAUSE"/>
        <br class="ISI_PAUSE"/>
      </div>
    </xsl:if>
    
    <div><xsl:apply-templates select="headline"/></div>
    <div><xsl:apply-templates select="publicationDate"/></div>
    <div><xsl:apply-templates select="sourceName"/></div>
    <div><xsl:apply-templates select="corrections"/></div>
    <div><xsl:apply-templates select="leadParagraph"/></div>
    <div><xsl:apply-templates select="tailParagraphs"/></div>
	</xsl:template>

	<xsl:template match="paragraph"><h1><xsl:apply-templates select =".//paragraph"/></h1></xsl:template>
	<xsl:template match="paragraph">
		<p>
			<xsl:for-each select="child::node()">
				<xsl:value-of select="normalize-space(.)"/>
			</xsl:for-each>
		</p>
	</xsl:template>
	<xsl:template match="headline"><h1><xsl:apply-templates select =".//paragraph"/></h1></xsl:template>
	<xsl:template match="publicationDate"><p><xsl:value-of select="normalize-space(.)"/></p></xsl:template>
	<xsl:template match="sourceName"><p><xsl:value-of select="normalize-space(.)"/></p></xsl:template>
	<xsl:template match="leadParagraph"><p><xsl:apply-templates select =".//paragraph"/></p></xsl:template>
	<xsl:template match="tailParagraphs"><p><xsl:apply-templates select =".//paragraph"/></p></xsl:template>	
</xsl:stylesheet>
