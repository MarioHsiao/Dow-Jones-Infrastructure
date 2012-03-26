<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" />

  <xsl:template match="/*">
    <GetListsByGroupResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet/Result"/>
    </GetListsByGroupResponse>

  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet/Result">     
      <xsl:for-each select="GROUP_ITEM_LIST">
        <list>
          <id>
          <xsl:value-of select="ITEM_ID"/>
        </id>
        <properties>
          <name>
            <xsl:value-of select="ITEM_INSTANCE_NAME"/>
          </name>
        </properties>
      </list>
      </xsl:for-each> 
  </xsl:template>  

</xsl:stylesheet>
