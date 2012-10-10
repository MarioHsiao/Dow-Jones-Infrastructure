<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" />

  <xsl:template match="/*">
    <GetCompanyListsByGroupResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </GetCompanyListsByGroupResponse>

  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet">
    <xsl:for-each select="Result/GROUP_ITEM_LIST">
      <companyList>
        <id>
          <xsl:value-of select="ITEM_ID"/>
        </id>
        <name>
          <xsl:value-of select="ITEM_INSTANCE_NAME"/>
        </name>
        <type>ACCOUNT</type>

      </companyList>
    </xsl:for-each>
  </xsl:template>  

</xsl:stylesheet>