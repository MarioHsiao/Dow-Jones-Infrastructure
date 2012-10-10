<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  version="1.0">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:include href="ClipCTPFolder.xslt"/>
	<xsl:template match="/*">
		<CreateFolderResponse>
				<xsl:apply-templates select="/*/Status"/>
				<xsl:apply-templates select="/*/ResultSet"/>
		</CreateFolderResponse>
  </xsl:template>
  <!--<xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="/*/ResultSet">
    <xsl:copy-of select="."/>
  </xsl:template>-->
</xsl:stylesheet>