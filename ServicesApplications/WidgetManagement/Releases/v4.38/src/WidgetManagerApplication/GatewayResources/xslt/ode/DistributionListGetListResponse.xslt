<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <DistributionListGetListResponse>
      <xsl:for-each select="ResultSet/Result">
      <distributionList>
        <id>
          <xsl:value-of select="ListId"/>
        </id>
        <name>
          <xsl:value-of select="ListName"/>
        </name>
        <!--<emailAddress>-->
          <xsl:call-template name="splitAddrList">
            <xsl:with-param name="addresses" select="AddrList"/>
          </xsl:call-template>
        <!--</emailAddress>-->
      </distributionList>
      </xsl:for-each>
    </DistributionListGetListResponse>
  </xsl:template>

  <xsl:template name="splitAddrList">
    <xsl:param name="addresses"/>
    <xsl:variable name="firstAddress" select="substring-before($addresses,';')"/>
    <xsl:variable name="afterSemiColonAddresses" select="substring-after($addresses,';')"/>

    <xsl:if test="$firstAddress">
      <emailAddress>
        <xsl:value-of select="$firstAddress"/>
      </emailAddress>
    </xsl:if>

    <xsl:if test="$afterSemiColonAddresses">
      <xsl:call-template name="splitAddrList">
        <xsl:with-param name="addresses" select="$afterSemiColonAddresses"/>
      </xsl:call-template>
    </xsl:if>

    <xsl:if test="not($afterSemiColonAddresses)">
      <emailAddress>
        <xsl:value-of select="$addresses"/>
      </emailAddress>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>