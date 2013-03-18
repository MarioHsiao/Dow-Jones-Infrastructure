<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/">
  <xsl:output method="xml" version="1.0" encoding="utf-8" />
  <xsl:param name ="ClassId"/> 

  <xsl:template match="/">
    <GetAccountGroupItemsRequest>
      <ITEM_CLASS_LIST fcstype="list">
        <ITEM_CLASS>
          <xsl:value-of select="$ClassId"/>
        </ITEM_CLASS>
      </ITEM_CLASS_LIST>     
      <LOAD_BLOB>
        <xsl:choose>
          <xsl:when test="count(soap-env:Envelope/soap-env:Body/GetCompanyListsAccountRequest/includeCompanyCodes) > 0 and /soap-env:Envelope/soap-env:Body/GetCompanyListsAccountRequest/includeCompanyCodes = 'true'">Y</xsl:when>
          <xsl:otherwise>N</xsl:otherwise>
        </xsl:choose>
      </LOAD_BLOB>
      <!--<ALL>Y</ALL>--><!--Option with ITEM_CLASS_LIST-->
      <xsl:choose>
        <xsl:when test="soap-env:Envelope/soap-env:Body/GetCompanyListsAccountRequest/includeCompanyList = 'Assigned'">
          <Flags>G</Flags> 
        </xsl:when>
         <xsl:when test="soap-env:Envelope/soap-env:Body/GetCompanyListsAccountRequest/includeCompanyList = 'Unassigned'">
          <Flags>O</Flags> 
        </xsl:when>        
      </xsl:choose>       
      <!--<CheckExistence>N</CheckExistence>--><!--Required With Flags only-->
    </GetAccountGroupItemsRequest>
  </xsl:template>
</xsl:stylesheet>
