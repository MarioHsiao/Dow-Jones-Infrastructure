<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <xsl:output method="xml" version="1.0" encoding="utf-8" />
  <xsl:param name ="ClassId"/>

  <xsl:template match="/">
    <xsl:variable  name="loadBlob" select="//includeCompanyCodes"/>
    <GetListsPropertiesRequest>
      <ITEM_CLASS_LIST fcstype="list">
        <CLASS_INFO>
          <ITEM_CLASS>
            <xsl:value-of select="$ClassId"/>
          </ITEM_CLASS>
          <LOAD_BLOB>N</LOAD_BLOB>
          <REVERSE>N</REVERSE>
          <CategorizedItems>N</CategorizedItems>
          <loadLcDate>N</loadLcDate>
        </CLASS_INFO>
      </ITEM_CLASS_LIST>
    </GetListsPropertiesRequest>
  </xsl:template>

</xsl:stylesheet>
