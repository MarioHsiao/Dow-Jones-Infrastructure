<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />

  <xsl:template match="/*">
    <xsl:element name="GetCategoryInfoRequest">

      <xsl:element name="ItemClassList">
        <xsl:attribute name ="fcstype" >list</xsl:attribute>
        <xsl:copy-of select="ItemClassList/child::*"/>
      </xsl:element>
      <xsl:copy-of select="CategoryId"/>

      <xsl:call-template name="ChangeTrueFalse" >
        <xsl:with-param name ="ElementName" >AllSubscribableItems</xsl:with-param>
        <xsl:with-param name ="status" select="AllSubscribableItems"/>
      </xsl:call-template>

      <xsl:call-template name="ChangeTrueFalse" >
        <xsl:with-param name ="ElementName" >SetAccessTime</xsl:with-param>
        <xsl:with-param name ="status" select="SetAccessTime"/>
      </xsl:call-template>

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

<!--INPUT XML-->

<!--MESSAGE AFTER APPLYING XSLT-->
