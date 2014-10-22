<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <xsl:element name="AddItemsToCategoryRequest">
      <xsl:for-each select ="child::*">
        <xsl:choose >
          <xsl:when test="local-name() = 'CategoryIDList'">
            <xsl:element name="CategoryIdList">
              <xsl:attribute name ="fcstype" >list</xsl:attribute>
              <xsl:copy-of select="child::*"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="local-name() = 'ItemIDList'">
            <xsl:element name="ItemIdList">
              <xsl:attribute name ="fcstype" >list</xsl:attribute>
              <xsl:copy-of select="child::*"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="local-name() = 'ReplaceAll'">
            <xsl:call-template name="ChangeTrueFalse" >
              <xsl:with-param name ="ElementName" >ReplaceAll</xsl:with-param>
              <xsl:with-param name ="status" select="ReplaceAll"/>
            </xsl:call-template>
          </xsl:when>
          <xsl:when test="local-name() = 'RemoveItemFromAllCats'">
            <xsl:call-template name="ChangeTrueFalse" >
              <xsl:with-param name ="ElementName" >RemoveItemFromAllCats</xsl:with-param>
              <xsl:with-param name ="status" select="RemoveItemFromAllCats"/>
            </xsl:call-template>
          </xsl:when>
        </xsl:choose>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>
  <xsl:template name="ChangeTrueFalse">
    <xsl:param name ="ElementName"/>
    <xsl:param name ="status"/>
    <xsl:element name="{$ElementName}">
      <xsl:choose>
        <xsl:when test="$status = 'true'">Y</xsl:when>
        <xsl:otherwise >N</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>

</xsl:stylesheet>
