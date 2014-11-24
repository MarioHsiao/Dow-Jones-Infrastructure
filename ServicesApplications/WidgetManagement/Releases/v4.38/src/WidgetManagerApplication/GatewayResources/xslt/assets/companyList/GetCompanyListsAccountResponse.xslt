<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" />

  <xsl:template match="/*">
    <GetCompanyListsAccountResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </GetCompanyListsAccountResponse>

  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet">
    <xsl:for-each select="Result/ITEM">
      <companyList>
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

      </companyList>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name ="StringSplit">
    <xsl:param name="stringValue"/>
    <xsl:choose>
      <xsl:when test="contains($stringValue,',')">
        <companyCode>
          <xsl:value-of select="substring-before($stringValue,',')"/>
        </companyCode>
        <xsl:call-template name="StringSplit" >
          <xsl:with-param name="stringValue" select="substring-after($stringValue,',')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <companyCode>
          <xsl:value-of select="$stringValue"/>
        </companyCode>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

</xsl:stylesheet>

