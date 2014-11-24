<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <xsl:output method="xml" version="1.0" encoding="utf-8" />
  <xsl:param name ="ClassId"/>

  <xsl:template match="/">
    <xsl:variable  name="loadBlob" select="//includeCompanyCodes"/>
    <GetItemsByClassID>
      <ITEM_CLASS_LIST fcstype="list">
        <CLASS_INFO>
          <ITEM_CLASS>
            <xsl:value-of select="$ClassId"/>
          </ITEM_CLASS>
          <LOAD_BLOB>
            <xsl:choose>
              <xsl:when test="count(soap-env:Envelope/soap-env:Body/GetCompanyListsUserRequest/includeCompanyCodes) > 0 and /soap-env:Envelope/soap-env:Body/GetCompanyListsUserRequest/includeCompanyCodes = 'true'">Y</xsl:when>
              <xsl:otherwise>N</xsl:otherwise>
            </xsl:choose>
          </LOAD_BLOB>
          <REVERSE>N</REVERSE>
          <CategorizedItems>N</CategorizedItems>
          <loadLcDate>N</loadLcDate>
        </CLASS_INFO>
      </ITEM_CLASS_LIST>
    </GetItemsByClassID>
  </xsl:template>

</xsl:stylesheet>
