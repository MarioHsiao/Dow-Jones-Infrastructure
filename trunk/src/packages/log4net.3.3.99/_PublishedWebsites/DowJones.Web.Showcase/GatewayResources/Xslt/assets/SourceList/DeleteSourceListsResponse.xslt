<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" />

  <xsl:template match="/*">
    <DeleteSourceListsResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </DeleteSourceListsResponse>

  </xsl:template>
  
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>
  
  <xsl:template match="/*/ResultSet">
    <resultSet>
      <xsl:for-each select="child::node()">
        <xsl:if test="string-length(normalize-space(.)) &gt; 0">
          <result>
            <errorCode>
              <xsl:value-of select="ERROR_CODE"/>
            </errorCode>
            <message>
              <xsl:value-of select="ERROR_GENERAL_MSG"/>
            </message>
          </result>
        </xsl:if>
      </xsl:for-each>
    </resultSet>
  </xsl:template>
</xsl:stylesheet>

<!--Input-->
<!--<?xml version="1.0"?>
<MEMBER_DEL_ITEM_RESP version="1.1" nvpvers="1.3">
  <Control>
  </Control>
  <Status value="0">
    Success
  </Status>
  <ResultSet count="1">
    <Result>
      <ERROR_CODE>0</ERROR_CODE>
      <ERROR_GENERAL_MSG>Success</ERROR_GENERAL_MSG>
    </Result>
  </ResultSet>
</MEMBER_DEL_ITEM_RESP>-->