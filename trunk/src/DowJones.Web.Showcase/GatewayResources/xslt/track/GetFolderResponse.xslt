<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:include href ="GetFolder.xslt"/>

  <xsl:template match="/*">
    <GetFolderResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </GetFolderResponse>
  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="/*/ResultSet">
    <folderResponse>
      <xsl:apply-templates select="//Folder"/>
    </folderResponse>
  </xsl:template>

</xsl:stylesheet>
