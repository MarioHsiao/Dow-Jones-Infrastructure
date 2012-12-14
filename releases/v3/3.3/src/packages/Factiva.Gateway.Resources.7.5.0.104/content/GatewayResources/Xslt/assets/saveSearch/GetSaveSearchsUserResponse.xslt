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
        <xsl:for-each select="child::*">
          <xsl:choose>
            <xsl:when test="local-name() = 'ITEM_ID'">
              <id>
                <xsl:value-of select="."/>
              </id>
            </xsl:when>
            <xsl:when test="local-name() = 'ITEM_INSTANCE_NAME'">
              <name>
                <xsl:value-of select="."/>
              </name>
            </xsl:when>
            <xsl:when test="local-name() = 'ITEM_BLOB'">
              <xsl:value-of select="CLASS/ITEM/VALUE"/>
            </xsl:when>
          </xsl:choose>
        </xsl:for-each>
        <type>USER</type>
        <!--<xsl:choose>
            <xsl:when test="count(GROUP_NAME) > 0">ACCOUNT</xsl:when>
            <xsl:otherwise >USER</xsl:otherwise>
          </xsl:choose>-->
      </itemInfo>
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>