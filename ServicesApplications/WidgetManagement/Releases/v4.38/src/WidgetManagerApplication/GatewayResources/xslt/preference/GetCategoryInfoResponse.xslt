<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <xsl:element name="GetCategoryInfoResponse">
      <xsl:copy-of select="Control"/>
      <xsl:copy-of select="Status"/>
      <xsl:for-each select="ResultSet/Result/child::*">
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
          <xsl:when test="local-name() = 'SubscribableItemList'">
            <xsl:element name="SubscribableItemList">
              <xsl:for-each select="CategoryList">
                <xsl:copy-of select="."/>
              </xsl:for-each>
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
            </xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select ="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>

    </xsl:element>
  </xsl:template>
</xsl:stylesheet>
<!--INPUT MESSAGE-->
<!--<Result>
  <CategoryName>
    Computers</ CategoryName>
    <CategoryId>10</CategoryId>
    <ItemCount>
      3</ ItemCount>
      <ItemList>
        <ItemName>Folder One</ItemName>
        <ItemId>120034</ItemId>
        <ItemClass>76</ItemClass>
        <ItemBlob>
          ITEM BLOB BLOB</Itemblob>
          <Subscribed>N</Subscribed>
        </ItemList>
      <ItemList>
        <ItemName>Folder Two</ItemName>
        <ItemId>120035</ItemId>
        <ItemClass>76</ItemClass>
        <ItemBlob>
          ITEM BLOB BLOB</Itemblob>
          <Subscribed>N</Subscribed>
        </ItemList>
      <ItemList>
        <ItemName>Folder Three</ItemName>
        <ItemId>120036</ItemId>
        <ItemClass>76</ItemClass>
        <ItemBlob>
          ITEM BLOB BLOB</Itemblob>
          <Subscribed>N</Subscribed>
        </ItemList>
      <SubCategoryList>
        <CategoryName>
          Computers Advanced</ CategoryName>
          <CategoryId>100</CategoryId>
        </SubCategoryList>

      <ERROR_CODE>0</ERROR_CODE>
      <ERROR_GENERAL_MSG>
        Success</ ERROR_GENERAL_MSG>
      
</Result>-->
