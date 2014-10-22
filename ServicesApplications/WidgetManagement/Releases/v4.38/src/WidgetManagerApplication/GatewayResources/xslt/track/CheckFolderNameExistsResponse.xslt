<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:include href="common.xslt"/>
  <xsl:template match="/*">
    <CheckFolderNameResponse>
      <xsl:apply-templates select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </CheckFolderNameResponse>
  </xsl:template>

  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet/Result">
    <folderIdResponse>
      <xsl:apply-templates select="@folderName"/>
      <xsl:apply-templates select="@folderId"/>
      <xsl:apply-templates select="@duplicate"/>
      <xsl:apply-templates select="@productType"/>
    </folderIdResponse>
  </xsl:template>

  <xsl:template match="@folderName">
    <folderName>
      <xsl:value-of select="."/>
    </folderName>
  </xsl:template>

  <xsl:template match="@folderId">
    <folderId>
      <xsl:value-of select="."/>
    </folderId>
  </xsl:template>

  <xsl:template match="@duplicate">
    <duplicate>
      <xsl:choose>
        <xsl:when test=". = 'yes'">true</xsl:when>
        <xsl:otherwise> false</xsl:otherwise>
      </xsl:choose>
    </duplicate>
  </xsl:template>

</xsl:stylesheet>
