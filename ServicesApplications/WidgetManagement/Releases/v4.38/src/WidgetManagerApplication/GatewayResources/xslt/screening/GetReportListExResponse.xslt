<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"
                xmlns:fcp ="urn:factiva:fcp:v2_0">

  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:variable name="ns" >urn:factiva:fcp:v2_0</xsl:variable>
  <xsl:template match="/">
    <xsl:apply-templates select="soap:Envelope/soap:Body/fcp:GetReportListExXMLResponse" />
  </xsl:template>

  <xsl:template match="fcp:GetReportListExXMLResponse" >
    <xsl:element name ="GetReportListExXMLResponse" namespace="{$ns}">
      <xsl:element name ="GetReportListExXMLResult" namespace="{$ns}">
        <xsl:element name ="reportListResult" namespace="{$ns}">
          <xsl:for-each select="fcp:GetReportListExXMLResult/fcp:reportListResult/child::*">
            <xsl:choose>
              <xsl:when test="local-name()= 'reportCategory'">
                <xsl:apply-templates select="."/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:copy-of select="."/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each>
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="fcp:reportCategory">
    <xsl:element name="reportCategory" namespace="{$ns}">
      <xsl:for-each select="@*">
        <xsl:copy-of select="."/>
      </xsl:for-each>
      <xsl:if test="count(//fcp:descriptor) > 0">
        <xsl:element name="descriptor" namespace="{$ns}">
          <xsl:for-each select="fcp:descriptor">
            <xsl:copy-of select="."/>
          </xsl:for-each>
        </xsl:element>
      </xsl:if>
      <xsl:for-each select="child::*">
        <xsl:choose>
          <xsl:when test="local-name()='descriptor'">
            <!--Do Nothing. Already Handled-->
          </xsl:when>
          <xsl:when test="local-name()='marketIndices'">
            <xsl:apply-templates select="."/>
          </xsl:when>
          <xsl:when test="local-name()='primaryIndustries'">
            <xsl:apply-templates select="."/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>

  <xsl:template match="fcp:marketIndices">
    <xsl:element name="marketIndices" namespace="{$ns}">
      <xsl:element name="MarketIndices" namespace="{$ns}">
        <xsl:for-each select="fcp:index">
          <xsl:copy-of select="."/>
        </xsl:for-each>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="fcp:primaryIndustries">
    <xsl:element name="primaryIndustries" namespace="{$ns}">
      <xsl:for-each select="fcp:industry">
        <xsl:element name="PrimaryIndustryDescriptor" namespace="{$ns}">
          <xsl:for-each select="@*">
            <xsl:copy-of select="."/>
          </xsl:for-each>
          <xsl:value-of select="."/>
        </xsl:element>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>

</xsl:stylesheet>