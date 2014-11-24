<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="/*/ResultSet">
    <folderIDResponse>
      <folderID>
        <xsl:value-of select="Result/@folderId"/>
      </folderID>
      <xsl:if test="string-length(normalize-space(Result/@folderName)) &gt; 0">
        <folderName>
          <xsl:value-of select="Result/@folderName"/>
        </folderName>
      </xsl:if>
    </folderIDResponse>
  </xsl:template>
</xsl:stylesheet>