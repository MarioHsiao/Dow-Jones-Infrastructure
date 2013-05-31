<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <xsl:output method="xml" version="1.0" encoding="utf-8" />
  <xsl:param name ="ClassId"/>
  <xsl:param name ="ClassName"/>

  <xsl:template match="/">
    <xsl:apply-templates select="soap-env:Envelope/soap-env:Body/UpdateCompanyListRequest/companyList"/>     
  </xsl:template>
  
  <xsl:template match="companyList">
    <UpdateItem>
      <ITEM_ID>
        <xsl:value-of select="id"/>
      </ITEM_ID>

      <ITEM_CLASS>
        <xsl:value-of select="$ClassId"/>
      </ITEM_CLASS>
      <ITEM_INSTANCE_NAME>
        <xsl:value-of select="name"/>
      </ITEM_INSTANCE_NAME>
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
              <xsl:for-each select="companyCode">
                <xsl:value-of select="."/>
                <xsl:if test="position() != last()">,</xsl:if>
              </xsl:for-each>
            </VALUE>
          </ITEM>
        </xsl:element>
      </ITEM_BLOB>
      <MOVE_FLAG>N</MOVE_FLAG>

    </UpdateItem>
  </xsl:template>
</xsl:stylesheet>

<!--Expected Output-->
<!--<?xml version="1.0"?>
<UpdateItem>
  <ITEM_ID>229026</ITEM_ID>
  <ITEM_CLASS>47</ITEM_CLASS>
  <ITEM_INSTANCE_NAME>test1</ITEM_INSTANCE_NAME>
  <ITEM_BLOB fcstype="cdata">
    <CLASS NAME="companyList">
      <ITEM>
        <NAME>test1</NAME>
        <VALUE>newnametest1</VALUE>
      </ITEM>
    </CLASS>
  </ITEM_BLOB>
  <MOVE_FLAG>N</MOVE_FLAG>
</UpdateItem>-->