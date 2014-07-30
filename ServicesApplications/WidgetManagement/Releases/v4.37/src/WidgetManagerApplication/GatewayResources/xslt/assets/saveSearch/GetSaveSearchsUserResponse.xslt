<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" indent="yes"/>

  <xsl:template match="/*">
    <GetSaveSearchsUserResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </GetSaveSearchsUserResponse>
  </xsl:template>

  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet">
    <xsl:for-each select="Result/RESPONSE_LIST">
      <itemInfo>
           <id><xsl:value-of select="ITEM_ID"/></id>
           <name><xsl:value-of select="ITEM_INSTANCE_NAME"/></name>
           <type>
            <xsl:choose>
              <xsl:when test="string-length(normalize-space(GROUP_NAME)) &gt; 0">ACCOUNT</xsl:when>
              <xsl:otherwise>USER</xsl:otherwise>
            </xsl:choose>
           </type>
        
      </itemInfo>
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>