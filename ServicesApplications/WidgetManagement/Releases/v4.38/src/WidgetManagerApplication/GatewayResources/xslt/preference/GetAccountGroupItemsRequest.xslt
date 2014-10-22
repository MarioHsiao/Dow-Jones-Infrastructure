<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <xsl:element name="GetAccountGroupItemsRequest">
      <xsl:for-each select="child::*">
        <xsl:choose>
          <xsl:when test="local-name()= 'ITEM_CLASS_LIST'">
            <xsl:element name="ITEM_CLASS_LIST">
              <xsl:attribute name ="fcstype" >list</xsl:attribute>
              <xsl:copy-of select="child::*"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="local-name()= 'LOAD_BLOB'">
            <xsl:call-template name="ChangeTrueFalse" >
              <xsl:with-param name ="ElementName" >LOAD_BLOB</xsl:with-param>
              <xsl:with-param name ="status" select="."/>
            </xsl:call-template>
          </xsl:when>
          <xsl:when test="local-name()= 'CheckExistence'">
            <xsl:call-template name="ChangeTrueFalse" >
              <xsl:with-param name ="ElementName" >CheckExistence</xsl:with-param>
              <xsl:with-param name ="status" select="."/>
            </xsl:call-template>
          </xsl:when>	
          <xsl:otherwise>
            <xsl:copy-of select="."/>
          </xsl:otherwise>
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
