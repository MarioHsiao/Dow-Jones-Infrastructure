<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:param name="category"/>
  <xsl:param name="transVersion"/>


  <xsl:template match="*">
    <xsl:apply-templates select="/*/Status"/>
    <xsl:apply-templates select="GetArchiveObjectResponse"/>
  </xsl:template>

  <xsl:template match="GetArchiveObjectResponse">
    <xsl:choose>
      <xsl:when test="$category='multimedia'">
        <xsl:element name="GetMultimediaArticleUrlResponse">
          <xsl:copy-of select="Status"/>
          <xsl:apply-templates select ="//ResultSet"/>
        </xsl:element>
      </xsl:when>
      <xsl:when test="$category='webpage'">
        <xsl:element name="GetWebArticleUrlResponse">
          <xsl:copy-of select="Status"/>
          <xsl:apply-templates select ="//ResultSet"/>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="GetWebArticleUrlResponse">
          <xsl:copy-of select="Status"/>
          <xsl:apply-templates select ="//ResultSet"/>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match ="/*/ResultSet">
    <xsl:choose>
      <xsl:when test="$category='multimedia'">
        <xsl:element name="multimediaArticleResultSet">
          <xsl:attribute name="count">
            <xsl:copy-of select="number(@count)"/>
          </xsl:attribute>
          <xsl:apply-templates select="Result"/>
        </xsl:element>
      </xsl:when>
      <xsl:when test="$category='webpage'">
        <xsl:element name="webArticleResultSet">
          <xsl:attribute name="count">
            <xsl:copy-of select="number(@count)"/>
          </xsl:attribute>
          <xsl:apply-templates select="Result"/>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="webArticleResultSet">
          <xsl:attribute name="count">
            <xsl:copy-of select="number(@count)"/>
          </xsl:attribute>
          <xsl:apply-templates select="Result"/>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="Result">
    <xsl:choose>
      <xsl:when test="$category='multimedia'">
        <xsl:element name="multimediaArticle">
          <xsl:call-template name="articleProperties"></xsl:call-template>
        </xsl:element>
      </xsl:when>
      <xsl:when test="$category='webpage'">
        <xsl:element name="webArticle">
          <xsl:call-template name="articleProperties"></xsl:call-template>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="webArticle">
          <xsl:call-template name="articleProperties"></xsl:call-template>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>



  <xsl:template name="articleProperties">
    <status>
      <xsl:copy-of select="number(@status)"/>
    </status>
    <xsl:if test="number(@status)!=0">
      <!--<xsl:attribute name="status"><xsl:value-of select="number(@status)"/></xsl:attribute>-->
      <accessionNo>
        <xsl:value-of select="@accessionno"/>
      </accessionNo>
      <reference>
        distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="@accessionno"/>
      </reference>
    </xsl:if>
    <xsl:if test="number(@status)=0">
      <accessionNo>
        <xsl:value-of select="@accessionno"/>
      </accessionNo>
      <reference>
        distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="@accessionno"/>
      </reference>

      <xsl:choose>
        <xsl:when test="$transVersion='1'">
          <!-- Copy only URL to match V1 response -->
          <xsl:apply-templates select=".//Property[@name='url']"/>
        </xsl:when>
        <xsl:when test="$transVersion='2'">
          <xsl:apply-templates select=".//Property"/>
        </xsl:when>
      </xsl:choose>


    </xsl:if>
  </xsl:template>
  <xsl:template match="//Property">
    <xsl:if test="string-length(normalize-space(@value)) > 0">
      <xsl:element name="property">
        <xsl:attribute name="name">
          <xsl:value-of select ="@name"/>
        </xsl:attribute>
        <xsl:attribute name="value">
          <xsl:value-of select ="@value"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>