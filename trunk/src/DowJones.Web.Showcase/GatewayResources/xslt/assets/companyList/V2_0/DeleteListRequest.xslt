<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <xsl:output method="xml" version="1.0" encoding="utf-8" />
  <xsl:param name ="ClassId"/>
  <xsl:param name ="ClassName"/>

  <xsl:template match="/">
    <DeleteItem>
      <ITEM_ID_LIST fcstype="list">

        <ITEM_ID>
          <xsl:value-of select="soap-env:Envelope/soap-env:Body/DeleteListRequest/id"/>
        </ITEM_ID>

      </ITEM_ID_LIST>
    </DeleteItem>
  </xsl:template>


</xsl:stylesheet>

<!--Input-->
<!--<soap-env:Envelope xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <soap-env:Body>
    <DeleteCompanyListsRequest xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
      <id>233949</id>
    </DeleteCompanyListsRequest>
  </soap-env:Body>
</soap-env:Envelope>-->


<!--Output-->
<!--<?xml version="1.0"?>
<DeleteItem>
  <ITEM_ID_LIST fcstype="list">
    <ITEM_ID>233948</ITEM_ID>
  </ITEM_ID_LIST>
</DeleteItem>-->
