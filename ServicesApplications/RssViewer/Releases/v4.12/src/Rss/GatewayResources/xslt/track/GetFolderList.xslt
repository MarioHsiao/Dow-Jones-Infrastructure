<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions" >
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"  omit-xml-declaration="yes"/>
  <xsl:template match="*">
    <Request>
      <xsl:apply-templates select="//productType"/>
      <xsl:if test="groupFolderIds/int">
        <GroupFolderID>
          <xsl:for-each select="groupFolderIds/int">
            <xsl:choose>
              <xsl:when test="position()&gt;1">
                ,<xsl:value-of select="."/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="."/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
        </GroupFolderID>
      </xsl:if>
      <xsl:if test="excludeInactiveFolders">
        <xsl:choose>
          <xsl:when test="excludeInactiveFolders='true'">
            <InactiveFolder>exclude</InactiveFolder>
          </xsl:when>
          <xsl:when test="excludeInactiveFolders='false'">
            <InactiveFolder>include</InactiveFolder>
          </xsl:when>
        </xsl:choose>
      </xsl:if>
      <xsl:if test="returnGroupFolderHitCounts">
        <xsl:choose>
          <xsl:when test="returnGroupFolderHitCounts='true'">
            <Hitscount>True</Hitscount>
          </xsl:when>
          <xsl:when test="returnGroupFolderHitCounts='false'">
            <Hitscount>False</Hitscount>
          </xsl:when>
        </xsl:choose>
      </xsl:if>
      <xsl:apply-templates select="//folderAssetType"/>
    </Request>
  </xsl:template>
  <xsl:template match="//folderAssetType">
    <ListFolderType>
      <xsl:for-each select="./RequestAssetType">
        <xsl:choose>
          <xsl:when test="position()&gt;1">
            <xsl:text>,</xsl:text> 
            <xsl:call-template name="getassettype">
              <xsl:with-param name="at">
                <xsl:value-of select="normalize-space(.)"/>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="getassettype">
              <xsl:with-param name="at">
                <xsl:value-of select="normalize-space(.)"/>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ListFolderType>
  </xsl:template>
  <xsl:template name="getassettype">
    <xsl:param name="at"/>
    <xsl:choose>
      <xsl:when test="$at='Personal'">personal</xsl:when>
      <xsl:when test="$at='Subscribed'">subscribed-personal</xsl:when>
      <xsl:when test="$at='Assigned'">assigned-personal</xsl:when>
      <xsl:when test="$at='All'">all</xsl:when>
      <xsl:otherwise>default</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="//productType">
    <ProductType>
      <xsl:for-each select="./ProductType">
        <xsl:choose>
          <xsl:when test="position()&gt;1">
            <xsl:text>,</xsl:text>
            <xsl:call-template name="getproducttype">
              <xsl:with-param name="pt">
                <xsl:value-of select="normalize-space(.)"/>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:when>
          <xsl:otherwise>
            <xsl:call-template name="getproducttype">
              <xsl:with-param name="pt">
                <xsl:value-of select="normalize-space(.)"/>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </ProductType>
  </xsl:template>
  <xsl:template name="getproducttype">
    <xsl:param name="pt"/>
    <xsl:choose>
      <xsl:when test="$pt='Global'">global</xsl:when>
      <xsl:when test="$pt='Iff'">iff</xsl:when>
      <xsl:when test="$pt='FastTrack'">fast-track</xsl:when>
      <xsl:when test="$pt='SelectHeadlines'">select-headline</xsl:when>
      <xsl:when test="$pt='SelectFullText'">select-fulltext</xsl:when>
      <xsl:when test="$pt='FCPCompany'">fcp-company</xsl:when>
      <xsl:when test="$pt='FCPIndustry'">fcp-industry</xsl:when>
      <xsl:when test="$pt='FCPExecutive'">fcp-executive</xsl:when>
      <xsl:when test="$pt='IWE'">iwe</xsl:when>
      <xsl:when test="$pt='Lexis'">lexis</xsl:when>
      <xsl:when test="$pt='WealthManagementAlerts'">fs-alert-wm</xsl:when>
      <xsl:when test="$pt='InvestmentBankingAlerts'">fs-alert-ib</xsl:when>
      <xsl:when test="$pt='WealthManagementTriggers'">trigger-wm</xsl:when>
      <xsl:when test="$pt='InvestmentBankingTriggers'">trigger-ib</xsl:when>
      <xsl:when test="$pt='BRITriggers'">trigger-bri</xsl:when>
      <xsl:when test="$pt='BRI'">bri</xsl:when>
      <xsl:when test="$pt='GlobalTrigger'">trigger-glob</xsl:when>
      <xsl:when test="$pt='All'">all</xsl:when>
      <xsl:when test="$pt='WsjProfessional'">wsj-pro</xsl:when>
      <xsl:when test="$pt='DjConsultant'">djc</xsl:when>
      <xsl:when test="$pt='Author'">author</xsl:when>
      <xsl:when test="$pt='NewAuthor'">new-author</xsl:when>
      <xsl:when test="$pt='DirectToClient'">d2c</xsl:when>
      <xsl:when test="$pt='Made_News'">made-news</xsl:when>
      <xsl:when test="$pt='Made_Author'">made-author</xsl:when>
      <xsl:when test="$pt='Made_New_Author'">made-new-author</xsl:when>
      <xsl:when test="$pt='Made_Topic'">made-topic</xsl:when>
      <xsl:when test="$pt='Made_Topic_Author'">made-topic-author</xsl:when>
      <xsl:when test="$pt='Made_Topic_New_Author'">made-topic-new-author</xsl:when>
      <xsl:otherwise>Iff</xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
