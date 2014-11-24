<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" cdata-section-elements="itemBlob"/>
  <xsl:template match="/*">
    <xsl:element name="CSGetItemResponse">
      <xsl:copy-of select="Control"/>
      <xsl:copy-of select="Status"/>
      <items>
        <xsl:apply-templates select="//ResultSet"/>
      </items>
      
    </xsl:element>
  </xsl:template>
  <xsl:template match="ResultSet">
    <xsl:element name="item">

      <xsl:copy-of select="//AccountId"/>
      <xsl:copy-of select="//ERROR_CODE"/>
      <xsl:copy-of select="//ERROR_GENERAL_MSG"/>
      <xsl:if test="//ItemInstanceName">
        <xsl:element name ="ITEM_INSTANCE_NAME">
          <xsl:value-of select="//ItemInstanceName"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="//ItemClass">
        <xsl:element name ="ITEM_CLASS">
          <xsl:value-of select="//ItemClass"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="//ItemBlob">
        <xsl:element name ="ITEM_BLOB">          
          <xsl:copy-of select="//ItemBlob/child::*"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="//ItemId">
        <xsl:element name ="ITEM_ID">
          <xsl:value-of select="//ItemId"/>
        </xsl:element>
      </xsl:if>


      <!--<xsl:copy-of select="Result/child::*"/>-->
    </xsl:element>
  </xsl:template>

</xsl:stylesheet>
