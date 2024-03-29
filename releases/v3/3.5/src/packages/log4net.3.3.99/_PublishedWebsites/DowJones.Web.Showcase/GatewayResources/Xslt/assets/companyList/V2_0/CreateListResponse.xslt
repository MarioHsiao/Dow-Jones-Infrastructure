<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" />

  <xsl:template match="/*">
    <CreateListResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </CreateListResponse>

  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>
  
  <xsl:template match="/*/ResultSet">
    <id>
      <xsl:value-of select="Result/ITEM_ID"/>
    </id>
  </xsl:template>
  
</xsl:stylesheet>

<!--Input-->
<!--<?xml version="1.0"?>
<MEMBER_COMP_ADD_ITEM_RESP version="1.1" nvpvers="1.3">
  <Control>
  </Control>
  <Status value="0">
    Success
  </Status>
  <ResultSet count="1">
    <Result>
      <ERROR_CODE>0</ERROR_CODE>
      <ERROR_GENERAL_MSG>Success</ERROR_GENERAL_MSG>
      <ITEM_ID>233862</ITEM_ID>
    </Result>
  </ResultSet>
</MEMBER_COMP_ADD_ITEM_RESP>-->