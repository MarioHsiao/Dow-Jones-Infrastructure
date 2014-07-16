<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/*">
    <xsl:element name="GetItemResponse">
      <xsl:apply-templates select="//ResultSet"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="//ResultSet">
    <xsl:element name="itemList">
      <xsl:apply-templates select="Result/RESPONSE_LIST"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Result/RESPONSE_LIST">
    <xsl:element name="item">
      <xsl:element name="groupName">
        <xsl:value-of select="GROUP_NAME"/>
      </xsl:element>
      <xsl:element name="userName">
        <xsl:value-of select="USER_ID"/>
      </xsl:element>
      <xsl:element name="itemID">
        <xsl:value-of select="ITEM_ID"/>
      </xsl:element>
      <xsl:element name="classID">
        <xsl:value-of select="ITEM_CLASS"/>
      </xsl:element>
      <xsl:element name="instanceName">
        <xsl:value-of select="ITEM_INSTANCE_NAME"/>
      </xsl:element>
      <xsl:element name="itemBlob">
        <xsl:apply-templates select="ITEM_BLOB"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="ITEM_BLOB">
    <xsl:choose>
      <xsl:when test=".//personalContent">
        <xsl:apply-templates select="personalContent"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy-of select="child::node()"/>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>
  <xsl:template match="personalContent">
    <xsl:choose>
      <xsl:when test="@version">
        <xsl:choose>
          <xsl:when test="@version='1.0'">
            <xsl:call-template name="RemoveAlertHeadlinesWidget"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="RemoveAlertHeadlinesWidget"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="RemoveAlertHeadlinesWidget">

    <xsl:element name="personalContent">
      <xsl:copy-of select="@*"/>
      <xsl:for-each select="*">
        <xsl:choose>
          <xsl:when test="name(.)='WidgetList'">
            <xsl:call-template name="WidgetLists"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>

  <xsl:template name="WidgetLists">
    <WidgetList>
      <xsl:copy-of select="@*"/>
      <xsl:for-each select="*">
        <xsl:choose>
          <xsl:when test="name(.)='widget'">
            <xsl:choose>
              <xsl:when test="./@xsi:type='AlertHeadlinesWidget'">
                <!--drop-->
              </xsl:when>
              <xsl:otherwise>
                <xsl:copy-of select="."/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </WidgetList>
  </xsl:template>
</xsl:stylesheet>
