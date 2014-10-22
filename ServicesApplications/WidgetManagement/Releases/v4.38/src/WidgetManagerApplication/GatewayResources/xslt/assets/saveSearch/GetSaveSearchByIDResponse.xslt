<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" indent="yes"/>

  <xsl:template match="/*">
    <GetSaveSearchByIdResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </GetSaveSearchByIdResponse>
  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet">
    <ReturnCode>
      <xsl:value-of select="ERROR_CODE"/>
    </ReturnCode>
    <SaveSearch>
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

          <xsl:when test="local-name() = 'ITEM_BLOB'">
            <name>
              <xsl:value-of select="CLASS/ITEM/NAME"/>
            </name>
            <value>
              <xsl:value-of select="CLASS/ITEM/VALUE"/>
            </value>
          </xsl:when>

        </xsl:choose>
      </xsl:for-each>

    </SaveSearch>
  </xsl:template>


</xsl:stylesheet>

<!--Input-->
<!--<?xml version="1.0"?>
<MEMBER_GET_ITEM_BY_ID_RESP version="1.1" nvpvers="1.3">
  <Control>
  </Control>
  <Status value="0">
    Success
  </Status>
  <ResultSet count="1">
    <Result>
      <ERROR_CODE>0</ERROR_CODE>
      <ERROR_GENERAL_MSG>Success</ERROR_GENERAL_MSG>
      <RESPONSE_LIST>
        <ERROR_CODE>0</ERROR_CODE>
        <ITEM_ACCESS>0</ITEM_ACCESS>
        <ITEM_BLOB>
          <CLASS NAME="SaveSearch">
            <ITEM>
              <NAME>test1</NAME>
              <VALUE>cl1,cl2,cl3,cl4</VALUE>
            </ITEM>
          </CLASS>
        </ITEM_BLOB>
        <ITEM_CLASS>47</ITEM_CLASS>
        <ITEM_ID>229026</ITEM_ID>
        <ITEM_INSTANCE_NAME>test1</ITEM_INSTANCE_NAME>
        <ITEM_MODIFIER>0</ITEM_MODIFIER>
        <ITEM_SUBSCRIBE>0</ITEM_SUBSCRIBE>
        <ITEM_TYPE>USER</ITEM_TYPE>
      </RESPONSE_LIST>
      <RESPONSE_LIST>
        <ERROR_CODE>0</ERROR_CODE>
        <ITEM_ACCESS>0</ITEM_ACCESS>
        <ITEM_BLOB>
          <CLASS NAME="SaveSearch">
            <ITEM>
              <NAME>MSGs list 27.6.0 RN</NAME>
              <VALUE>gzprm,dwitd,uhelc,dowjon,isptnl,crdto,acocl,ubs</VALUE>
            </ITEM>
          </CLASS>
        </ITEM_BLOB>
        <ITEM_CLASS>47</ITEM_CLASS>
        <ITEM_ID>219910</ITEM_ID>
        <ITEM_INSTANCE_NAME>MSGs list 27.6.0 RN</ITEM_INSTANCE_NAME>
        <ITEM_MODIFIER>0</ITEM_MODIFIER>
        <ITEM_SUBSCRIBE>0</ITEM_SUBSCRIBE>
        <ITEM_TYPE>GROUP</ITEM_TYPE>
      </RESPONSE_LIST>
    </Result>
  </ResultSet>
</MEMBER_GET_ITEM_BY_ID_RESP>-->