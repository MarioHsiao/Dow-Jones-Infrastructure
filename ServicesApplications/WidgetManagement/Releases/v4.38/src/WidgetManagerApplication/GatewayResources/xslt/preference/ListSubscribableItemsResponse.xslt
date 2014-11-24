<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <xsl:element name="ListSubscribableItemsResponse">
      <xsl:copy-of select="Control"/>
      <xsl:copy-of select="Status"/>
      <xsl:for-each select="ResultSet/Result/child::*">
        <xsl:choose >
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
          <xsl:when test="local-name() = 'Exists'">
            <xsl:element name="Exists">
              <xsl:choose>
                <xsl:when test ="Exists = 'Y'">true</xsl:when>
                <xsl:otherwise>false</xsl:otherwise>
              </xsl:choose>
            </xsl:element>
          </xsl:when>
          <xsl:otherwise >
            <xsl:copy-of select ="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>

    </xsl:element>
  </xsl:template>
</xsl:stylesheet>


<!--INPUT XML-->
<!--<Result>
      <ItemCount>3</ItemCount>
      <SubscribedItemList>
        <ItemName>Folder One</ItemName>
        <ItemId>120034</ItemId>
        <ItemClass>76</ItemClass>
        <ItemBlob>ITEM BLOB BLOB</ItemBlob>
        <CategoryList>
          <CategoryName>Cat 1</CategoryName>
          <CategoryId>19</CategoryId>
        </CategoryList>
        <CategoryList>
          <CategoryName>Cat 3</CategoryName>
          <CategoryId>15</CategoryId>
        </CategoryList>
        <CategoryList>
          <CategoryName>Cat 18</CategoryName>
          <CategoryId>32</CategoryId>
        </CategoryList>
      </SubscribedItemList>
      <SubscribedItemList>
        <ItemName>Folder Two</ItemName>
        <ItemId>120035</ItemId>
        <ItemClass>76</ItemClass>
        <ItemBlob>ITEM BLOB BLOB</ItemBlob>
      </SubscribedItemList>
      <SubscribedItemList>
        <ItemName>Folder Three</ItemName>
        <ItemId>120036</ItemId>
        <ItemClass>58</ItemClass>
        <ItemBlob>ITEM BLOB BLOB</ItemBlob>
      </SubscribedItemList>
    <ERROR_CODE>0</ERROR_CODE>
    <ERROR_GENERAL_MSG>Success</ ERROR_GENERAL_MSG>    
</Result>-->

<!--OUTPUT XML-->
<!--<?xml version="1.0" encoding="utf-8"?>
<ListSubscribableItemsResponse>
  <Control>
  </Control>
  <Status value="0">
    Success
  </Status>
  <Result>
    <ERROR_CODE>0</ERROR_CODE>
    <ERROR_GENERAL_MSG>Success</ERROR_GENERAL_MSG>
    <ItemCount>1</ItemCount>
    <SubscribableItemList>
      <CategoryList>
        <CategoryId>4136</CategoryId>
        <CategoryName>IT6</CategoryName>
      </CategoryList>
      <ItemBlob>
        <CLASS NAME="savesearchformat">
          <ITEM>
            <NAME>MSGs test 29.12.1</NAME>
            <VALUE>td, lp, cx, sn, clm, de, ngc, la, by, gc, hl, hlp, pg, hd, vol, art, ct, rf, ed, se, pub, cr|MSGs test 29.12.1</VALUE>
          </ITEM>
        </CLASS>
      </ItemBlob>
      <ItemClass>79</ItemClass>
      <ItemId>228851</ItemId>
      <ItemName>MSGs test 29.12.1</ItemName>
    </SubscribableItemList>
  </Result>
</ListSubscribableItemsResponse>-->
