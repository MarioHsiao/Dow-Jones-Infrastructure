<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <xsl:output method="xml" version="1.0" encoding="utf-8" />
  <xsl:param name ="ClassId"/>
  <xsl:param name ="ClassName"/>

  <xsl:template match="/">
    <GetItemsByID>
      <ITEM_ID_LIST fcstype="list">
        <xsl:for-each select="soap-env:Envelope/soap-env:Body/GetCompanyListsByIDRequest/id">
        <ITEM_ID>
          <xsl:value-of select="."/>
        </ITEM_ID>
        </xsl:for-each>
      </ITEM_ID_LIST>
    </GetItemsByID>
  </xsl:template>

</xsl:stylesheet>
<!--InPut-->
<!--<soap-env:Envelope xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <soap-env:Body>
    <GetCompanyListsByIDRequest xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
      <id>229026</id>
      <id>221135</id>
    </GetCompanyListsByIDRequest>
  </soap-env:Body>
</soap-env:Envelope>-->

<!--OutPut-->
<!--<?xml version="1.0"?>
<GetItemsByID>
  <ITEM_ID_LIST fcstype="list">
    <ITEM_ID>229026</ITEM_ID>
    <ITEM_ID>221135</ITEM_ID>
  </ITEM_ID_LIST>
</GetItemsByID>-->