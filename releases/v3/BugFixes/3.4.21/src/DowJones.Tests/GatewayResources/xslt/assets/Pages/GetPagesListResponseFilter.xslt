<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:soap-env="http://schemas.xmlsoap.org/soap/envelope/" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template  match="/">
    <xsl:apply-templates select="@* | node()"/>
  </xsl:template>

  <!--elements-->
  <xsl:template match="node()">
    <xsl:choose>
      <xsl:when test="name()='lastModifiedBy' ">
      </xsl:when>
      <xsl:when test="name()='soap-env:Envelope'">
        <soap-env:Envelope>
          <xsl:apply-templates select="@* | node()"/>
        </soap-env:Envelope>
      </xsl:when>
      <xsl:when test="name()='soap-env:Body'">
        <soap-env:Body>
          <xsl:apply-templates select="@* | node()"/>
        </soap-env:Body>
      </xsl:when>
      <xsl:when test="self::*">
        <xsl:element name="{name()}" namespace="">
          <xsl:apply-templates select="@* | node()"/>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="@*">
    <xsl:choose>
      <xsl:when test="name()='lastModifiedBy' ">
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
</xsl:stylesheet>
