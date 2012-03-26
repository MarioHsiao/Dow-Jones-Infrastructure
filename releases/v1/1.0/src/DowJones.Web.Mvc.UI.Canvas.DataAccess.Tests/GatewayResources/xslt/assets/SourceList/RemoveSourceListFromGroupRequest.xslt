<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <xsl:output method="xml" version="1.0" encoding="utf-8" />

  <xsl:template match="/">
    <UpdateItem>
      <ITEM_ID>
        <xsl:value-of  select="soap-env:Envelope/soap-env:Body/RemoveSourceListFromGroupRequest/id"/>
      </ITEM_ID>
      <GROUP_LIST fcstype="list">
        <GROUP_INFO>
          <GROUP_NAME>
            <xsl:value-of select="soap-env:Envelope/soap-env:Body/RemoveSourceListFromGroupRequest/groupName"/>
          </GROUP_NAME>
          <ACTION>D</ACTION>
        </GROUP_INFO>
      </GROUP_LIST>
    </UpdateItem>
  </xsl:template>

</xsl:stylesheet>
