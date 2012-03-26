<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no" omit-xml-declaration="yes"/>
  <xsl:template match="@productType">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <productType>
        <xsl:choose>
          <xsl:when test=". = 'global'">Global</xsl:when>
          <xsl:when test=". = 'select-headline'">SelectHeadlines</xsl:when>
          <xsl:when test=". = 'select-fulltext'">SelectFullText</xsl:when>
          <xsl:when test=". = 'fast-track'">FastTrack</xsl:when>
          <xsl:when test=". = 'fcp-industry'">FCPIndustry</xsl:when>
          <xsl:when test=". = 'fcp-company'">FCPCompany</xsl:when>
          <xsl:when test=". = 'fcp-executive'">FCPExecutive</xsl:when>
          <xsl:when test=". = 'lexis'">Lexis</xsl:when>
          <xsl:when test=". = 'iff'">Iff</xsl:when>
          <xsl:when test=". = 'iwe'">IWE</xsl:when>
          <xsl:when test=". = 'fs-alert-wm'">WealthManagementAlerts</xsl:when>
          <xsl:when test=". = 'fs-alert-ib'">InvestmentBankingAlerts</xsl:when>
          <xsl:when test=". = 'trigger-wm'">WealthManagementTriggers</xsl:when>
          <xsl:when test=". = 'trigger-ib'">InvestmentBankingTriggers</xsl:when>
          <xsl:when test=". = 'trigger-bri'">BRITriggers</xsl:when>
          <xsl:when test=". = 'bri'">BRI</xsl:when>
          <xsl:when test=". = 'trigger-glob'">GlobalTrigger</xsl:when>
          <xsl:when test=". = 'wsj-pro'">WsjProfessional</xsl:when>
          <xsl:when test=". = 'djc'">DjConsultant</xsl:when>
          <xsl:when test=". = 'author'">Author</xsl:when>
          <xsl:when test=". = 'new-author'">NewAuthor</xsl:when>
          <xsl:when test=". = 'd2c'">DirectToClient</xsl:when>
          <xsl:otherwise>Iff</xsl:otherwise>
        </xsl:choose>
      </productType>
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>