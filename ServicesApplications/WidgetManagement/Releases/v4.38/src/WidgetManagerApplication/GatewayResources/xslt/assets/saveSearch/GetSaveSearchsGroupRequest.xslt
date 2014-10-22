<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <xsl:output method="xml" version="1.0" encoding="utf-8" indent="yes"/>
  <xsl:param name ="ClassId"/>

  <xsl:template match="/">
    <GetSaveSearchsGroupRequest>
      <GROUP_ITEM_LIST fcstype="list">
        <VALUE>
          <xsl:value-of select="$ClassId"/>
        </VALUE>
      </GROUP_ITEM_LIST>
      <GROUP_NAME>
        <xsl:value-of  select="soap-env:Envelope/soap-env:Body/GetSaveSearchsGroupRequest/groupName"/>
      </GROUP_NAME>
      <GROUP_USERS>N</GROUP_USERS>
      <SUB_GROUPS>N</SUB_GROUPS>
    </GetSaveSearchsGroupRequest>
  </xsl:template>

</xsl:stylesheet>