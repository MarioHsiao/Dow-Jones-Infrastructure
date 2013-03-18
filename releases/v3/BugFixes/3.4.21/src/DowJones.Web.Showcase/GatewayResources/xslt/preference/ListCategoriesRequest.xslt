<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />

  <xsl:template match="/*">
    <xsl:element name="ListCategoriesRequest">

      <xsl:element name="ItemClassList">
        <xsl:attribute name ="fcstype" >list</xsl:attribute>
        <xsl:copy-of select="ItemClassList/child::*"/>
      </xsl:element>

      <xsl:call-template name="ChangeTrueFalse" >
        <xsl:with-param name ="ElementName" >IncludeItemList</xsl:with-param>
        <xsl:with-param name ="status" select="IncludeItemList"/>
      </xsl:call-template>
      <xsl:call-template name="ChangeTrueFalse" >
        <xsl:with-param name ="ElementName" >LoadBlob</xsl:with-param>
        <xsl:with-param name ="status" select="LoadBlob"/>
      </xsl:call-template>
      <xsl:call-template name="ChangeTrueFalse" >
        <xsl:with-param name ="ElementName" >IncludeItemCount</xsl:with-param>
        <xsl:with-param name ="status" select="IncludeItemCount"/>
      </xsl:call-template>
      <xsl:call-template name="ChangeTrueFalse" >
        <xsl:with-param name ="ElementName" >SetAccessTime</xsl:with-param>
        <xsl:with-param name ="status" select="SetAccessTime"/>
      </xsl:call-template>
      <xsl:copy-of select ="MaxLevel"/>
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
<!--<?xml version="1.0"?>
<ListCategoriesRequest xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ItemClassList>
    <ItemClass>
      <ITEM_CLASS>407</ITEM_CLASS>
    </ItemClass>
    <ItemClass>
      <ITEM_CLASS>4104</ITEM_CLASS>
    </ItemClass>
  </ItemClassList>
  <IncludeItemList>true</IncludeItemList>
  <LoadBlob>false</LoadBlob>
  <IncludeItemCount>false</IncludeItemCount>
  <MaxLevel>0</MaxLevel>
  <SetAccessTime>false</SetAccessTime>
</ListCategoriesRequest>-->

<!--MESSAGE AFTER APPLYING XSLT:-->
<!--<?xml version="1.0" encoding="utf-8"?>
<ListCategoriesRequest>
  <ItemClassList xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
    <ItemClass>
      <ITEM_CLASS>407</ITEM_CLASS>
    </ItemClass>
    <ItemClass>
      <ITEM_CLASS>4104</ITEM_CLASS>
    </ItemClass>
  </ItemClassList>
  <IncludeItemList>Y</IncludeItemList>
  <LoadBlob>N</LoadBlob>
  <IncludeItemCount>N</IncludeItemCount>
  <SetAccessTime>N</SetAccessTime>
  <MaxLevel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">1</MaxLevel>
</ListCategoriesRequest>-->

