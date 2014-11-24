<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" />

  <xsl:template match="/*">
    <GetListByIDResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet/Result/RESPONSE_LIST"/>
    </GetListByIDResponse>
  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet/Result/RESPONSE_LIST">
    <list>
      <xsl:apply-templates select="ITEM_ID"/>
      <xsl:apply-templates select="ITEM_TYPE"/>
      <xsl:apply-templates select="ITEM_BLOB"/>

    </list>
  </xsl:template>

  <xsl:template match="ITEM_ID">
    <id>
      <xsl:value-of select="."/>
    </id>
  </xsl:template>

  <xsl:template match="ITEM_TYPE">
    <scope>
      <xsl:choose>
        <xsl:when test=". = 'USER'">Personal</xsl:when>
        <xsl:otherwise >Shareable</xsl:otherwise>
      </xsl:choose>
    </scope>
  </xsl:template>

  <xsl:template match="ITEM_BLOB">
    <properties>
      <name>
        <xsl:value-of select="CLASS/ITEM/NAME"/>
      </name>
    </properties>
    <xsl:call-template name="StringSplit" >
      <xsl:with-param name="stringValue" select="CLASS/ITEM/VALUE"/>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="ERROR_CODE">
    <status>
      <xsl:value-of select="."/>
    </status>
  </xsl:template>

  <xsl:template name ="StringSplit">
    <xsl:param name="stringValue"/>
    <xsl:choose>
      <xsl:when test="contains($stringValue,',')">
        <item>
          <itemCode>
            <xsl:value-of select="substring-before($stringValue,',')"/>
          </itemCode>
        </item>
        <xsl:call-template name="StringSplit" >
          <xsl:with-param name="stringValue" select="substring-after($stringValue,',')"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <item>
          <itemCode>
            <xsl:value-of select="$stringValue"/>
          </itemCode>
        </item>
      </xsl:otherwise>
    </xsl:choose>

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
          <CLASS NAME="companyList">
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
          <CLASS NAME="companylist">
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