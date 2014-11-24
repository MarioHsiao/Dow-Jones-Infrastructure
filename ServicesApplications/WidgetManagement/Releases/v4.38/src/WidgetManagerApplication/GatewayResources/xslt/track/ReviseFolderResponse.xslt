<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  version="1.0">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/*">
    <ReviseFolderResponse>
      <xsl:apply-templates select="Status"/>
      <xsl:apply-templates select="ResultSet"/>
    </ReviseFolderResponse>
  </xsl:template>
  <xsl:template match="Status">
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="ResultSet">
    <xsl:apply-templates select="Result"/>
  </xsl:template>
  <xsl:template match="Result">
    <folderIDResponse>
      <folderID>
        <xsl:value-of select="@folderId"/>
      </folderID>
    </folderIDResponse>
  </xsl:template>
</xsl:stylesheet>
