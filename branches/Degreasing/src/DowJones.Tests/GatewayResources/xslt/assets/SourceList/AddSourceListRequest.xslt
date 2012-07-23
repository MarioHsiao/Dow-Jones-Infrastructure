<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <xsl:output method="xml" version="1.0" encoding="utf-8" />
  <xsl:param name ="ClassId"/>
  <xsl:param name ="ClassName"/>

  <xsl:template match="/">
    <xsl:apply-templates select="soap-env:Envelope/soap-env:Body/AddSourceListRequest/sourceList"/>
  </xsl:template>
  <xsl:template match="sourceList">
    <AddItem>
      <ITEM_CLASS>
        <xsl:value-of select="$ClassId"/>
      </ITEM_CLASS>
      <ITEM_INSTANCE_NAME>
        <xsl:value-of select="name"/>
      </ITEM_INSTANCE_NAME>

      <xsl:if test="count(type) > 0">
        <xsl:choose>
          <xsl:when test="type = 'ACCOUNT' ">
            <ITEM_TYPE>GROUP</ITEM_TYPE>
          </xsl:when>
          <xsl:otherwise>
            <ITEM_TYPE>USER</ITEM_TYPE>
            <USER_LIST fcstype="list">
              <USER_INFO>
                <USER_ID>DummyUser</USER_ID>
              </USER_INFO>
            </USER_LIST>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>

      <ITEM_BLOB fcstype="cdata">
        <xsl:element name="CLASS">
          <xsl:attribute name="NAME">
            <xsl:value-of select="$ClassName"/>
          </xsl:attribute>
          <ITEM>
            <NAME>
              <xsl:value-of select="name"/>
            </NAME>
            <VALUE>
              <xsl:for-each select="sourceCode">
                <xsl:value-of select="."/>
                <xsl:if test="position() != last()">,</xsl:if>
              </xsl:for-each>
            </VALUE>
          </ITEM>
        </xsl:element>
      </ITEM_BLOB>
      <MOVE_FLAG>N</MOVE_FLAG>

    </AddItem>
  </xsl:template>

</xsl:stylesheet>

<!--Expected Output-->
<!--<?xml version="1.0"?>
<AddItem>
  <ITEM_CLASS>47</ITEM_CLASS>
  <ITEM_INSTANCE_NAME>TestName1</ITEM_INSTANCE_NAME>
  <ITEM_BLOB fcstype="cdata">
    <CLASS NAME="companyList">
      <ITEM>
        <NAME>TestName1</NAME>
        <VALUE>test1,test2,test3,test4,test5,test6</VALUE>
      </ITEM>
    </CLASS>
  </ITEM_BLOB>
  <USER_LIST fcstype="list">
    <USER_INFO>
      <USER_ID>DummyUserID</USER_ID>
    </USER_INFO>
  </USER_LIST>
</AddItem>-->