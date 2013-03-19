<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="GetDocumentWithDelayedBillingResponse">
    <GetDocumentWithDelayedBillingResponse>
      <xsl:copy-of select="Status"></xsl:copy-of>
      <xsl:apply-templates select="documentResponseSet"></xsl:apply-templates>
      <xsl:apply-templates select="rawData"></xsl:apply-templates>
    </GetDocumentWithDelayedBillingResponse>
  </xsl:template>
  <xsl:template match="documentResponseSet">
    <xsl:element name="documentResponseSet">
      <xsl:attribute name="status">
        <xsl:value-of select="@status"/>
      </xsl:attribute>
      <xsl:copy-of select="count"></xsl:copy-of>
      <xsl:if test="count(articles) > 0">
        <xsl:if test ="count(articles/article) > 0 ">
          <articles xsi:type="Article">
            <xsl:apply-templates select="articles/article"></xsl:apply-templates>
          </articles>
        </xsl:if>
        <xsl:if test ="count(articles/webArticle) > 0 ">
          <articles xsi:type="WebArticle">
            <xsl:apply-templates select="articles/webArticle"></xsl:apply-templates>
          </articles>
        </xsl:if>
        <xsl:if test ="count(articles/multimediaArticle) > 0 ">
          <articles xsi:type="MultimediaArticle">
            <xsl:apply-templates select="articles/multimediaArticle"></xsl:apply-templates>
          </articles>
        </xsl:if>
      </xsl:if>
      <!--<xsl:apply-templates select="articles"></xsl:apply-templates>-->
    </xsl:element>
  </xsl:template>

  <xsl:template match="articles/article">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="articles/webArticle">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="articles/multimediaArticle">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="rawData">
    <xsl:copy-of select="."/>
  </xsl:template>

</xsl:stylesheet>
