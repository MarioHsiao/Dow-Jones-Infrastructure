<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />

  <xsl:template match="/*">
    <GetNestedGroupInfoResponse>
      <Group>
        <xsl:apply-templates select="ResultSet/Result/GROUP"/>
      </Group>
    </GetNestedGroupInfoResponse>
  </xsl:template>

  <xsl:template match ="GROUP">
    <Name>
      <xsl:value-of select="NAME"/>
    </Name>
    <Id>
      <xsl:value-of select="ID"/>
    </Id>

    <ParentGroupName>
      <xsl:value-of select="PARENT_GROUP_NAME"/>
    </ParentGroupName>

    <AdminGroupName>
      <xsl:value-of select="ADMIN_GROUP_NAME"/>
    </AdminGroupName>
    <AdminRights>
      <xsl:choose>
        <xsl:when test="ADMIN_RIGHTS='Y'">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </AdminRights>

    <xsl:for-each select ="GROUP_USER_LIST">
      <GroupUsers>
        <xsl:value-of select="."/>
      </GroupUsers>
    </xsl:for-each>

    <xsl:for-each select ="GROUP_ITEM_LIST">
      <GroupItems>
        <xsl:apply-templates select="ITEM_BLOB"/>

        <ITEM_ID>
          <xsl:value-of select="ITEM_ID"/>
        </ITEM_ID>
        <ITEM_CLASS>
          <xsl:value-of select="ITEM_CLASS"/>
        </ITEM_CLASS>
        <ITEM_INSTANCE_NAME>
          <xsl:value-of select="ITEM_INSTANCE_NAME"/>
        </ITEM_INSTANCE_NAME>
      </GroupItems>
    </xsl:for-each>
    <xsl:for-each select ="GROUP">
      <NestedGroup>
        <xsl:apply-templates select ="."/>
      </NestedGroup>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="ITEM_BLOB">
    <xsl:copy>
      <xsl:copy-of select="@*"/>
      <xsl:apply-templates mode="itemblob"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="*" mode="itemblob">
    <xsl:copy>
      <xsl:copy-of select="@*"/>
      <xsl:apply-templates mode="itemblob"/>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>


<!--InPut-->
<!--<GROUP>
  <ADMIN_GROUP_NAME>Main Admin</ADMIN_GROUP_NAME>
  <ADMIN_RIGHTS>Y</ADMIN_RIGHTS>
  <GROUP_USER_LIST>joyful</GROUP_USER_LIST>
  <GROUP_USER_LIST>dacostad</GROUP_USER_LIST>
  <NAME>Main</NAME>
  <PARENT_GROUP_NAME>DJ</PARENT_GROUP_NAME>
</GROUP>-->

<!--OUTPUT-->
<!--<GetNestedGroupInfoResponse>
  <Groups>
    <Name>N1</Name>
    <ParentGroupName>P1</ParentGroupName>
    <AdminGroupName>A1</AdminGroupName>
    <GroupUsers>U1</GroupUsers>
    <GroupUsers>U2</GroupUsers>
    <GroupItems>
      <itemID>111111111</itemID>
      <itemClassID>itemClassID</itemClassID>
      <itemName>itemName</itemName>
      <itemBlob>BLOB</itemBlob>
    </GroupItems>
    <GroupItems>
      <itemID>111111111</itemID>
      <itemClassID>itemClassID</itemClassID>
      <itemName>itemName</itemName>
      <itemBlob>BLOB</itemBlob>
    </GroupItems>
    <Groups>
      <Name>N1</Name>
      <ParentGroupName>P1</ParentGroupName>
      <AdminGroupName>A1</AdminGroupName>
      <GroupUsers>U1</GroupUsers>
      <GroupUsers>U2</GroupUsers>
      <GroupItems>
        <itemID>111111111</itemID>
        <itemClassID>itemClassID</itemClassID>
        <itemName>itemName</itemName>
        <itemBlob>BLOB</itemBlob>
      </GroupItems>
      <GroupItems>
        <itemID>111111111</itemID>
        <itemClassID>itemClassID</itemClassID>
        <itemName>itemName</itemName>
        <itemBlob>BLOB</itemBlob>
      </GroupItems>
    </Groups>
    <Groups>
      <Name>N1</Name>
      <ParentGroupName>P1</ParentGroupName>
      <AdminGroupName>A1</AdminGroupName>
      <GroupUsers>U1</GroupUsers>
      <GroupUsers>U2</GroupUsers>
      <GroupItems>
        <itemID>111111111</itemID>
        <itemClassID>itemClassID</itemClassID>
        <itemName>itemName</itemName>
        <itemBlob>BLOB</itemBlob>
      </GroupItems>
      <GroupItems>
        <itemID>111111111</itemID>
        <itemClassID>itemClassID</itemClassID>
        <itemName>itemName</itemName>
        <itemBlob>BLOB</itemBlob>
      </GroupItems>
    </Groups>
  </Groups>
  <Groups>
    <Name>N1</Name>
    <ParentGroupName>P1</ParentGroupName>
    <AdminGroupName>A1</AdminGroupName>
    <GroupUsers>U1</GroupUsers>
    <GroupUsers>U2</GroupUsers>
    <GroupItems>
      <itemID>111111111</itemID>
      <itemClassID>itemClassID</itemClassID>
      <itemName>itemName</itemName>
      <itemBlob>BLOB</itemBlob>
    </GroupItems>
    <GroupItems>
      <itemID>111111111</itemID>
      <itemClassID>itemClassID</itemClassID>
      <itemName>itemName</itemName>
      <itemBlob>BLOB</itemBlob>
    </GroupItems>
  </Groups>
</GetNestedGroupInfoResponse>-->