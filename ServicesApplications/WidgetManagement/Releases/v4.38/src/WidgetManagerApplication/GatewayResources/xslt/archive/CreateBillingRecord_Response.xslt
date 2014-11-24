<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:template match="/FulfillBillingResponse">
    <CreateBillingRecordsResponse>
      <xsl:apply-templates select="Status"/>
      <xsl:apply-templates select="ResultSet"/>
    </CreateBillingRecordsResponse>
  </xsl:template>

  <xsl:template match="Status">
    <Status>
    <xsl:value-of select="@value"/>
    </Status>
  </xsl:template>

  <xsl:template match="ResultSet">
    <billingResponseSet>
      <xsl:attribute name="count">
        <xsl:copy-of select="number(@count)"/>
      </xsl:attribute>
      <xsl:apply-templates select="./Result"/>
    </billingResponseSet>
  </xsl:template>

  <xsl:template match="//Result">
    <billingResult>
      <accessionNumber>
        <xsl:value-of select="@accessionno"/>
      </accessionNumber>
      <status>
        <xsl:value-of select="@status"/>
      </status>
    </billingResult>
  </xsl:template>
</xsl:stylesheet>

