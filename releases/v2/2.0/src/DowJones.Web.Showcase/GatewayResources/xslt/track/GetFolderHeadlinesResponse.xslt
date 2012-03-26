<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:include href="FolderHeadlines.xslt"/>
  <xsl:template match="/*">
    <GetFolderHeadlinesResponse>
      <folderHeadlinesResponse>
        <xsl:copy-of select="//Status"/>
        <folderHeadlinesResult>
          <xsl:attribute name="count">
            <xsl:value-of select="count(//ResultSet)"/>
          </xsl:attribute>
          <xsl:apply-templates select="//FolderInfo"/>
        </folderHeadlinesResult>
      </folderHeadlinesResponse>
    </GetFolderHeadlinesResponse>
  </xsl:template>
</xsl:stylesheet>
<!--attribute-->