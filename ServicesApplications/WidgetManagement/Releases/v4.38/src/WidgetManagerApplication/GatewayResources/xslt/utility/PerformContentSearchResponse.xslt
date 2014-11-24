<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:variable name="lcletters">abcdefghijklmnopqrstuvwxyz</xsl:variable>
  <xsl:variable name="ucletters">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
  <xsl:variable name="searchNS">http://types.factiva.com/search</xsl:variable>
  <xsl:template match="/*">
    <xsl:element name="PerformContentSearchResponse" namespace="{$searchNS}">
    <xsl:apply-templates select="//contentSearchResult"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//contentSearchResult">
    <xsl:element name="contentSearchResult" namespace="{$searchNS}">
      <xsl:attribute name="hitCount">
        <xsl:value-of select="//queryHitCount"/>
      </xsl:attribute>
      <xsl:element name="canonicalQueryString" namespace="{$searchNS}">
        <xsl:value-of select="searchContext"/>
      </xsl:element>
      <xsl:element name="highlightString" namespace="{$searchNS}">
        <xsl:value-of select="highlightString"/>
      </xsl:element>
      <xsl:element name="queryTransformSet" namespace="{$searchNS}">
      </xsl:element>
      <xsl:element name="codeNavigatorSet" namespace="{$searchNS}">
      </xsl:element>
      <xsl:element name="timeNavigatorSet" namespace="{$searchNS}">
      </xsl:element>
      <xsl:element name="contextualNavigatorSet" namespace="{$searchNS}">
      </xsl:element>
      <xsl:apply-templates select="contentHeadlinesResultSet"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="contentHeadlinesResultSet">
    <xsl:element name="contentHeadlineResultSet" namespace="{$searchNS}">
      <xsl:copy-of select="@count"/>
      <xsl:attribute name="first">
        <xsl:value-of select="../indexOfFirstHeadline"/>
      </xsl:attribute>
      <xsl:apply-templates select="contentHeadline"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="contentHeadline">
    <xsl:element name="contentHeadline" namespace="{$searchNS}">
      <xsl:element name="rank" namespace="{$searchNS}"/>
      <!--Empty elment bacuse it's required but not from Index-->
      <xsl:element name="documentVector" namespace="{$searchNS}"/>      <!--Empty elment bacuse it's required but not from Index-->
      <xsl:element name="accessionNo" namespace="{$searchNS}">
        <xsl:value-of select="accessionNo"/>
      </xsl:element>
      <xsl:element name="ipDocumentID" namespace="{$searchNS}">
        <xsl:value-of select="ipDocumentID"/>
      </xsl:element>
      <xsl:element name="attributionCode" namespace="{$searchNS}">
        <xsl:value-of select="attributionCode"/>
      </xsl:element>
      <xsl:element name="wordCount" namespace="{$searchNS}">
        <xsl:value-of select="wordCount"/>
      </xsl:element>
      <xsl:element name="publicationDate" namespace="{$searchNS}">
        <xsl:value-of select="publicationDate"/>
      </xsl:element>
      <xsl:apply-templates select="publicationTime"/>
      <xsl:element name="baseLanguage" namespace="{$searchNS}">
        <xsl:value-of select="baseLanguage"/>
      </xsl:element>
      <xsl:element name="sourceCode" namespace="{$searchNS}">
        <xsl:value-of select="sourceCode"/>
      </xsl:element>
      <xsl:element name="sourceName" namespace="{$searchNS}">
        <xsl:value-of select="sourceName"/>
      </xsl:element>
      <xsl:apply-templates select="sectionName"/>
      <xsl:apply-templates select="headline"/>
      <xsl:apply-templates select="snippet"/>
      <xsl:apply-templates select="byline"/>
      <xsl:apply-templates select="credit"/>
      <xsl:apply-templates select="copyright"/>
      <xsl:apply-templates select="contentParts"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="publicationTime">
    <xsl:element name="publicationTime" namespace="{$searchNS}">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="sectionName">
    <xsl:element name="sectionName" namespace="{$searchNS}">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
	<xsl:template match="headline">
    <xsl:element name="headline" namespace="{$searchNS}">
      <xsl:apply-templates select="paragraph"/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="snippet">
    <xsl:element name="snippet" namespace="{$searchNS}">
      <xsl:apply-templates select="paragraph"/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="paragraph">
    <xsl:element name="para" namespace="{$searchNS}">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="byline">
    <xsl:element name="byline" namespace="{$searchNS}">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="credit">
    <xsl:element name="credit" namespace="{$searchNS}">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="copyright">
    <xsl:element name="copyright" namespace="{$searchNS}">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="contentParts">
    <xsl:element name="contentItems" namespace="{$searchNS}">
      <xsl:element name="contentType">
        <xsl:value-of select="translate(@contentType,$ucletters,$lcletters)"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

</xsl:stylesheet>