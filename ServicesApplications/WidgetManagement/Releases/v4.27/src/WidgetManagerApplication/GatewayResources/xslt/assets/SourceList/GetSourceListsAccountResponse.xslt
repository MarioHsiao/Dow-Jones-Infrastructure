<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" />

  <xsl:template match="/*">
    <GetSourceListsAccountResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </GetSourceListsAccountResponse>

  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet">
    <xsl:for-each select="Result/ITEM">
      <sourceList>
        <xsl:for-each select="child::*">
          <xsl:choose>

            <xsl:when test="local-name() = 'ITEM_ID'">
              <id>
                <xsl:value-of select="."/>
              </id>
            </xsl:when>

            <xsl:when test="local-name() = 'ITEM_TYPE'">
              <type>
                <xsl:choose>
                  <xsl:when test=". = 'USER'">USER</xsl:when>
                  <xsl:otherwise >ACCOUNT</xsl:otherwise>
                </xsl:choose>
              </type>
            </xsl:when>
            <xsl:when test="local-name() = 'ITEM_INSTANCE_NAME'">
              <name>
                <xsl:value-of select="."/>
              </name>
            </xsl:when>
            <xsl:when test="local-name() = 'ITEM_BLOB'">

              <xsl:call-template name="StringSplit" >
                <xsl:with-param name="stringValue" select="CLASS/ITEM/VALUE"/>
              </xsl:call-template>
            </xsl:when>

          </xsl:choose>
        </xsl:for-each>

      </sourceList>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name ="StringSplit">
    <xsl:param name="stringValue"/>
    <xsl:choose>
      <xsl:when test="contains($stringValue,',')">
        <sourceCode>
          <xsl:value-of select="substring-before($stringValue,',')"/>
        </sourceCode>
        <xsl:call-template name="StringSplit" >
          <xsl:with-param name="stringValue" select="substring-after($stringValue,',')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <sourceCode>
          <xsl:value-of select="$stringValue"/>
        </sourceCode>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

</xsl:stylesheet>

