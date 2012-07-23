<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <xsl:element name="GetNewSubscribableItemsResponse">
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
          <xsl:otherwise >
            <xsl:copy-of select ="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>

    </xsl:element>
  </xsl:template>
</xsl:stylesheet>

<!--XML INPUT-->
<!--<Result>
  <ItemList>
    <ItemName>Folder 58</ItemName>
    <ItemId>120037</ItemId>
    <ItemClass>58</ItemClass>
    <ItemBlob>
      ITEM BLOB BLOB</Itemblob>
      <Subscribed>N</Subscribed>
    </ItemList>

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

  <ERROR_CODE>0</ERROR_CODE>
  <ERROR_GENERAL_MSG>
    Success</ ERROR_GENERAL_MSG>
  </Result>-->

<!-- XML OUTPUT -->
<!--<?xml version="1.0" encoding="utf-8"?>
<GetNewSubscribableItemsResponse>
  <Control>
  </Control>
  <Status value="0">
    Success
  </Status>
  <ERROR_CODE>0</ERROR_CODE>
  <ERROR_GENERAL_MSG>Success</ERROR_GENERAL_MSG>
  <ItemList>
    <PreferenceItem>
      <ITEM_INSTANCE_NAME>TestFVS</ITEM_INSTANCE_NAME>
      <ITEM_ID>192248</ITEM_ID>
      <ITEM_CLASS>109</ITEM_CLASS>
      <ITEM_BLOB>
        <ItemBlob>
          <CLASS NAME="selectserviceid">
            <ITEM>
              <VALUE>
                <SELECT_SERVICE TYPE="H" CONFIG="N" />
                <CREATED DATE="20051013" TIME="182009" />
                <LAST_UPDATE DATE="20051013" TIME="182009" />
              </VALUE>
            </ITEM>
          </CLASS>
        </ItemBlob>
      </ITEM_BLOB>
    </PreferenceItem>
  </ItemList>
</GetNewSubscribableItemsResponse>-->
