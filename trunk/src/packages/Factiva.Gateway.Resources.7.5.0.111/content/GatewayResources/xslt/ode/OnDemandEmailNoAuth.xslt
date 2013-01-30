<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions" >
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"  omit-xml-declaration="yes"/>
  <xsl:template match="*">
    <Request>
      <xsl:apply-templates select="//recipientEmail"/>
      <xsl:apply-templates select="//replyToEmail"/>
      <xsl:apply-templates select="//subject"/>
      <xsl:apply-templates select="//formatType"/>
      <xsl:apply-templates select="//freeText"/>
      <xsl:apply-templates select="//language"/>

      <xsl:if test="//reference">

      </xsl:if>
      <xsl:apply-templates select="//contentHeadline"/>



      <xsl:choose>
        <xsl:when test="//reference">
          <!-- for the source content categories.-->
          <!-- if not refenrece set this to 1 as per ode requirements-->
          <xsl:element name="TotalRef">
            <xsl:value-of select="count(//reference)"/>
          </xsl:element>
          <xsl:call-template name="GetContentTypes"/>
          <!-- for the contentIds content categories.-->
          <xsl:call-template name="GetContentIds"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:element name="TotalRef">1</xsl:element>
          <xsl:call-template name="AddSource">
            <xsl:with-param name="node">
              <xsl:value-of select="//emailSource"/>
            </xsl:with-param>
            <xsl:with-param name="pos">1</xsl:with-param>
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:apply-templates select="//emailDevice"/>


      <xsl:apply-templates select="//FromName"/>
      <xsl:apply-templates select="//emailType"/>
      <!--<xsl:apply-templates select="//emailSource"/>-->
      <xsl:call-template name="AppendMessages"/>
      <xsl:apply-templates select="//productType"/>
      <xsl:apply-templates select="//adoctocList"/>
      <xsl:apply-templates select="//ImgType"/>
      <xsl:apply-templates select="//HdLines"/>
      <xsl:apply-templates select="//CanonicStr"/>
      <xsl:apply-templates select="//AddCC"/>
      <xsl:apply-templates select="//costCodeInfo"/>
      <xsl:apply-templates select="//ObjType"/>
      <xsl:apply-templates select="//PostData"/>
      <xsl:apply-templates select="//ScreeningCounts"/>
      <xsl:apply-templates select="//ScreeningQueryString"/>
      <xsl:apply-templates select="//ScreeningWhereClause"/>
      <xsl:apply-templates select="//type"/>
      <!--<Source1>E</Source1>-->
    </Request>
  </xsl:template>

  <xsl:template match="//emailType">
    <xsl:if test=".='ShareEmail'">
      <xsl:element name="TransSubType">Share</xsl:element>
    </xsl:if>
  </xsl:template>

  <xsl:template name="AppendMessages">
    <xsl:element name="Message">
      <xsl:value-of select="MessageData_1"/>|<xsl:value-of select="MessageData_2"/>|<xsl:value-of select="MessageData_3"/>|<xsl:value-of select="MessageData_4"/>|<xsl:value-of select="MessageData_5"/>|<xsl:value-of select="MessageData_6"/>
    </xsl:element>
  </xsl:template>

  <!--<xsl:template match="//emailSource">
    <xsl:element name="Source1">
      <xsl:choose>
        <xsl:when test=".='PUBLICATION'">P</xsl:when>
        <xsl:when test=".='WEB'">W</xsl:when>
        <xsl:when test=".='IMAGE'">I</xsl:when>
        <xsl:when test=".='REPORT'">R</xsl:when>
        <xsl:when test=".='SCREENING'">S</xsl:when>
        <xsl:when test=".='HTTP'">H</xsl:when>
        <xsl:when test=".='EMBEDDEDMESSAGE'">E</xsl:when>
        <xsl:otherwise>E</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>-->

  <xsl:template match="//emailDevice">
    <xsl:element name="Mobile">
      <xsl:choose>
        <xsl:when test=".='1'">1</xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//FromName">
    <xsl:element name="FromName">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template name="GetContentTypes">

    <xsl:for-each select=".//reference">
      <xsl:variable name="pos">
        <!-- 2/26/09:HD:Remove +1 to fix email attachement issue 
		  <xsl:value-of select="position()+1"/>-->
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:call-template name="AddSource">
        <xsl:with-param name="node">
          <xsl:value-of select="//emailSource"/>
        </xsl:with-param>
        <xsl:with-param name="pos">
          <xsl:value-of select="position()"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="GetContentIds">

    <xsl:for-each select=".//reference">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:if test="string-length(normalize-space(.)) &gt; 0">
        <xsl:element name="ContentID{$pos}">
          <xsl:value-of select="."/>
        </xsl:element>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name ="AddSource">
    <xsl:param name ="node"/>
    <xsl:param name ="pos"/>

    <xsl:if test="string-length(normalize-space($node)) &gt; 0">
      <xsl:element name="Source{$pos}">
        <xsl:choose>
          <xsl:when test="$node ='PUBLICATION'">P</xsl:when>
          <xsl:when test="$node ='WEB'">W</xsl:when>
          <xsl:when test="$node ='IMAGE'">I</xsl:when>
          <xsl:when test="$node ='REPORT'">R</xsl:when>
          <xsl:when test="$node ='SCREENING'">S</xsl:when>
          <xsl:when test="$node ='HTTP'">H</xsl:when>
          <xsl:when test="$node ='EMBEDDEDMESSAGE'">E</xsl:when>
          <xsl:otherwise>E</xsl:otherwise>
        </xsl:choose>
      </xsl:element>
    </xsl:if>
  </xsl:template>

  <xsl:template match="//contentHeadline">
    <xsl:element name="ContentHeadline">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="//recipientEmail">
    <xsl:element name="SmtpAddress">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="//replyToEmail">
    <xsl:element name="ReplyToAddress">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="//subject">
    <xsl:element name="Subject">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="//formatType">
    <xsl:element name="FmtType">
      <xsl:choose>
        <xsl:when test=".='ASCII'">ASCII</xsl:when>
        <xsl:when test=".='HTML'">HTML</xsl:when>
        <xsl:otherwise>ASCII</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <xsl:template match="//freeText">
    <xsl:element name="FreeText">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="//language">
    <xsl:element name="Language">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//productType">
    <xsl:element name="ProductType">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//adoctocList">
    <xsl:for-each select="//adoctoc">
      <xsl:element name="adoctoc{position()}">
        <xsl:value-of select="."/>
      </xsl:element>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="//ImgType">
    <xsl:element name="ImgType">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//HdLines">
    <xsl:element name="HdLines">
      <xsl:choose>
        <xsl:when test=".= 'true'">1</xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//CanonicStr">
    <xsl:element name="CanonicStr">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//AddCC">
    <xsl:element name="AddCC">
      <xsl:choose >
        <xsl:when test=". = 'true'">1</xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//costCodeInfo">
    <xsl:for-each select="//costCode">
      <xsl:element name="CC_{position()}">
        <xsl:value-of select="."/>
      </xsl:element>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="//ObjType">
    <xsl:element name="ObjType">
      <xsl:choose>
        <xsl:when test=". = 'Custom'">
          <xsl:apply-templates select="//fids"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="."/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//fids">
    fid:<xsl:for-each select="./DistDocField">
      <xsl:value-of select="translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')"/>
      <xsl:if test="position() != count(parent::*/DistDocField)">,</xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="//PostData">
    <xsl:element name="PostData">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//ScreeningCounts">
    <xsl:element name="ScreeningCounts">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//ScreeningQueryString">
    <xsl:element name="ScreeningQueryString">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//ScreeningWhereClause">
    <xsl:element name="ScreeningWhereClause">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//type">
    <xsl:element name="type">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

</xsl:stylesheet>
