<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <xsl:element name="ListCategoriesResponse">
      <xsl:copy-of select="Control"/>
      <xsl:copy-of select="Status"/>
      <xsl:for-each select="ResultSet/Result/child::*">
        <xsl:choose >
          <xsl:when test="local-name() = 'CategoryList'">
            <xsl:apply-templates select="."/>
          </xsl:when>
          <xsl:otherwise >
            <xsl:copy-of select ="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>

  <xsl:template match="CategoryList">
    <xsl:element name="CategoryList">
      <xsl:for-each select="child::*">
        <xsl:choose >
          <xsl:when test="local-name() = 'ItemList'">
            <xsl:element name="ItemList">
              <xsl:element name="PreferenceItem">
                <xsl:element name ="ITEM_INSTANCE_NAME">
                  <xsl:value-of select="ItemName"/>
                </xsl:element>
                <xsl:element name ="ITEM_ID">
                  <xsl:value-of select="ItemId"/>
                </xsl:element>
                <xsl:element name ="ITEM_CLASS">
                  <xsl:value-of select="ItemClass"/>
                </xsl:element>
                <xsl:element name ="ITEM_BLOB">
                  <xsl:copy-of select="ItemBlob/child::*"/>
                </xsl:element>
              </xsl:element>
              <xsl:element name="Subscribed">
                <xsl:choose>
                  <xsl:when test ="Subscribed = 'Y'">true</xsl:when>
                  <xsl:otherwise>false</xsl:otherwise>
                </xsl:choose>
              </xsl:element>
            </xsl:element>
          </xsl:when>
          <xsl:when test="local-name() = 'CategoryList'">
            <xsl:apply-templates select="."/>
          </xsl:when>
          <xsl:otherwise >
            <xsl:copy-of select="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </xsl:element>
  </xsl:template>
</xsl:stylesheet>

<!--INPUT XML-->
<!--
<Result>
      <CategoryList>
        <CategoryName>Computers</ CategoryName>
          <CategoryId>10</CategoryId>
          <CategoryList>
            <CategoryName>
              Computers Advanced</ CategoryName>
              <CategoryId>100</CategoryId>
              <ItemCount>3</ ItemCount>
                <ItemList>
                  <ItemName>Folder A</ItemName>
                  <ItemId>120044</ItemId>
                  <ItemClass>76</ItemClass>
                  <Subscribed>Y</Subscribed>
                </ItemList>
                <ItemList>
                  <ItemName>Folder B</ItemName>
                  <ItemId>120045</ItemId>
                  <ItemClass>109</ItemClass>
                  <Subscribed>N</Subscribed>
                </ItemList>
                <ItemList>
                  <ItemName>Folder C</ItemName>
                  <ItemId>120046</ItemId>
                  <ItemClass>76</ItemClass>
                  <Subscribed>N</Subscribed>
                </ItemList>              
          </CategoryList>
          <ItemCount>3</ ItemCount>
            <ItemList>
              <ItemName>Folder One</ItemName>
              <ItemId>120034</ItemId>
              <ItemClass>76</ItemClass>
              <Subscribed>Y</Subscribed>
            </ItemList>
            <ItemList>
              <ItemName>Folder Two</ItemName>
              <ItemId>120035</ItemId>
              <ItemClass>76</ItemClass>
              <Subscribed>N</Subscribed>
            </ItemList>
            <ItemList>
              <ItemName>Folder Three</ItemName>
              <ItemId>120036</ItemId>
              <ItemClass>76</ItemClass>
              <Subscribed>N</Subscribed>
            </ItemList>          
      </CategoryList>
      <CategoryList>
        <CategoryName>tractors</ CategoryName>
          <CategoryId>12</CategoryId>
          <ItemCount>
            1</ ItemCount>
            <ItemList>
              <ItemName>Folder 12</ItemName>
              <ItemId>120</ItemId>
              <ItemClass>76</ItemClass>
              <Subscribed>Y</Subscribed>
            </ItemList>          
      </CategoryList>
      <ERROR_CODE>0</ERROR_CODE>
      <ERROR_GENERAL_MSG>Success</ ERROR_GENERAL_MSG>
      </Result>
      -->