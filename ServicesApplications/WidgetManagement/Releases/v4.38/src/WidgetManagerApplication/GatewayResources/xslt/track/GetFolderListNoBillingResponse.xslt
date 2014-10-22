<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:include href ="GetFolder.xslt"/>

  <xsl:template match="/*">
    <GetFolderListNoBillingResponse>
      <folderListResponse>
        <xsl:apply-templates select="/*/Status"/>
        <xsl:apply-templates select="/*/ResultSet"/>
      </folderListResponse>
    </GetFolderListNoBillingResponse>
  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="/*/ResultSet">
    <folderListResultSet>
      <xsl:apply-templates select="//Folder"/>
    </folderListResultSet>
  </xsl:template>

</xsl:stylesheet>
