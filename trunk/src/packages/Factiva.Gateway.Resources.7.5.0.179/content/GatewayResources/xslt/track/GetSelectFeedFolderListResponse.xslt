<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no" />
  <xsl:include href="common.xslt" />
  <xsl:template match="/*">
    <GetSelectFeedFolderListResponse>
      <xsl:copy-of select="//Status" />
      <xsl:apply-templates select="//ResultSet"></xsl:apply-templates>
    </GetSelectFeedFolderListResponse>
  </xsl:template>
  <xsl:template match="//ResultSet">
  <selectFeedFolderList>
    <xsl:if test="string-length(normalize-space(@selectid)) &gt; 0">
      <xsl:attribute name="selectId"><xsl:value-of select="@selectid"/></xsl:attribute>
    </xsl:if>
    <xsl:apply-templates select="./Result">
      <xsl:sort data-type="text" select="QueryName" case-order="lower-first" order="ascending" />
    </xsl:apply-templates>
  </selectFeedFolderList>
  </xsl:template>
  <xsl:template match="Result">
    <folderList>
      <xsl:if test="string-length(normalize-space(@folderid)) &gt; 0">
        <folderID><xsl:value-of select="@folderid" /></folderID>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(QueryName)) &gt; 0">
        <folderName><xsl:value-of select="normalize-space(QueryName)" /></folderName>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(QueryHighlight)) &gt; 0">
        <highlightString><xsl:value-of select="normalize-space(QueryHighlight)" /></highlightString>
      </xsl:if>
      <xsl:apply-templates select="@producttype" />
      <xsl:if test="string-length(normalize-space(@deliverymethod)) &gt; 0">
        <xsl:if test="@deliverymethod='online'">
          <deliveryMethod>Online</deliveryMethod>
        </xsl:if>
        <xsl:if test="@deliverymethod='continuous'">
          <deliveryMethod>Continuous</deliveryMethod>
        </xsl:if>
        <xsl:if test="@deliverymethod='batch'">
          <deliveryMethod>Batch</deliveryMethod>
        </xsl:if>
        <xsl:if test="@deliverymethod='feed'">
          <deliveryMethod>Feed</deliveryMethod>
        </xsl:if>
      </xsl:if>
      
      <xsl:if test="boolean(owner)">
        <owner>
          <xsl:attribute name="namespace"><xsl:value-of select="owner/@prod_id"/></xsl:attribute>
          <xsl:attribute name="userId"><xsl:value-of select="owner/@user_id"/></xsl:attribute>
        </owner>
      </xsl:if>

      <revisionPrivileges>
        <xsl:call-template name="TrackReviseTypeMapper">
          <xsl:with-param name="type">
            <xsl:value-of select="normalize-space(@revise)" />
          </xsl:with-param>
        </xsl:call-template>
      </revisionPrivileges>
      
    </folderList>
  </xsl:template>
</xsl:stylesheet>