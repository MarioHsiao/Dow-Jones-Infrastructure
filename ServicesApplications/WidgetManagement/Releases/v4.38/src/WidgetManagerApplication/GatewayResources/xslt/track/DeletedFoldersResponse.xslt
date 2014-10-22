<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"  omit-xml-declaration="yes"/>

  <xsl:template match="//ResultSet">
    <DeletedFoldersResponse>
      
      <xsl:copy-of select="//Status"/>
      
      <count>
        <xsl:value-of select="@count"/>
      </count>

      <xsl:if test="number(@count) &gt; 0">
        <xsl:for-each select="//FolderId">
          <folderID>
            <xsl:value-of select="normalize-space(.)"/>
          </folderID>
        </xsl:for-each>
      </xsl:if>

    </DeletedFoldersResponse>
  </xsl:template>
  
</xsl:stylesheet>