<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="utf-8" />

  <xsl:template match="/*">
    <GetSourceListsUserResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </GetSourceListsUserResponse>

  </xsl:template>

  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet">
    <xsl:for-each select="Result/RESPONSE_LIST">
      <sourceList>
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
              <xsl:call-template name="StringSplit" >
                <xsl:with-param name="stringValue" select="CLASS/ITEM/VALUE"/>
              </xsl:call-template>
            </xsl:when>
          </xsl:choose>

        </xsl:for-each>
        <type>
          <xsl:choose>
            <xsl:when test="count(GROUP_NAME) > 0">ACCOUNT</xsl:when>
            <xsl:otherwise >USER</xsl:otherwise>
          </xsl:choose>
        </type>
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

<!--Input-->
<!--
<?xml version="1.0"?>
<MEMBER_GET_ITEM_BY_CLASS_RESP version="1.1" nvpvers="1.3">
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
        <ITEM_ACCESS>0</ITEM_ACCESS>
        <ITEM_BLOB>
          <CLASS NAME="sourceList">
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
      </RESPONSE_LIST>
      <RESPONSE_LIST>
        <ITEM_ACCESS>0</ITEM_ACCESS>
        <ITEM_BLOB>
          <CLASS NAME="sourcelist">
            <ITEM>
              <NAME>agBanksBased</NAME>
              <VALUE>agrec,toyta,aanzb,vympek,gnmoc,slavny</VALUE>
            </ITEM>
          </CLASS>
        </ITEM_BLOB>
        <ITEM_CLASS>47</ITEM_CLASS>
        <ITEM_ID>221135</ITEM_ID>
        <ITEM_INSTANCE_NAME>agBanksBased</ITEM_INSTANCE_NAME>
        <ITEM_MODIFIER>0</ITEM_MODIFIER>
        <ITEM_SUBSCRIBE>0</ITEM_SUBSCRIBE>
      </RESPONSE_LIST>     
    </Result>
  </ResultSet>
</MEMBER_GET_ITEM_BY_CLASS_RESP>
-->