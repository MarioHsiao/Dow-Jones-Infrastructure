<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <xsl:element name="GetUpgradedUsersResponse">
      <xsl:call-template name="GetError"/>
      <xsl:apply-templates select="//accountId"/>
      <xsl:apply-templates select="//allocated"/>
      <xsl:apply-templates select="//path"/>
      <xsl:apply-templates select="//upgraded"/>
      <xsl:apply-templates select="//user"/>
    </xsl:element>
  </xsl:template>

  <xsl:template name="GetError">
    <xsl:element name="ERROR_CODE">
      <xsl:value-of select="//ERROR_CODE"/>
    </xsl:element>
    <xsl:element name="ERROR_GENERAL_MSG">
      <xsl:value-of select="//ERROR_GENERAL_MSG"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//accountId">
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="//allocated">
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="//path">
    <path>
      <xsl:choose>
        <xsl:when test="normalize-space(path) = '101'">ReaderFlexToIWorks</xsl:when>
        <xsl:otherwise>UnSpecified</xsl:otherwise>
      </xsl:choose>
    </path>
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="//upgraded">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="//user">
    <userDetail>
      <userId>
        <xsl:value-of select="userId" />
      </userId>
      <prodId>
        <xsl:value-of select="prodId" />
      </prodId>
      <firstName>
        <xsl:value-of select="firstName" />
      </firstName>
      <lastName>
        <xsl:value-of select="lastName" />
      </lastName>
      <companyName>
        <xsl:value-of select="companyName" />
      </companyName>
      <email>
        <xsl:value-of select="email" />
      </email>
    </userDetail>
  </xsl:template>

</xsl:stylesheet>