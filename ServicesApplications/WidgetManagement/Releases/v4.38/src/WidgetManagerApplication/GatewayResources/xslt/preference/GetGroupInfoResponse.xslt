<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <GetGroupInfoResponse>
      <GroupName>
        <xsl:value-of select="ResultSet/Result/GROUP_NAME"/>
      </GroupName>
      <ParentGroupName>
        <xsl:value-of select="ResultSet/Result/PARENT_GROUP_NAME"/>
      </ParentGroupName>
      <AdminGroupName>
        <xsl:value-of select="ResultSet/Result/ADMIN_GROUP_NAME"/>
      </AdminGroupName>
      <xsl:for-each select="ResultSet/Result/SUB_GROUP_LIST">
        <SubGroups>
          <xsl:value-of select="."/>
        </SubGroups>
      </xsl:for-each>       
      <xsl:for-each select="ResultSet/Result/GROUP_ITEM_LIST">
        <GroupItems>
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
      <xsl:for-each select="ResultSet/Result/GROUP_USER_LIST">
        <Users>
          <xsl:value-of select="."/>
        </Users>
      </xsl:for-each>
    </GetGroupInfoResponse>
  </xsl:template>
</xsl:stylesheet>

<!--INPUT-->
<!--<GetItemResponse xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <ADMIN_GROUP_NAME>Main Admin</ADMIN_GROUP_NAME>
  <ERROR_CODE>0</ERROR_CODE>
  <ERROR_GENERAL_MSG>Success</ERROR_GENERAL_MSG>
  <GROUP_NAME>Main</GROUP_NAME>
  <PARENT_GROUP_NAME>DJ</PARENT_GROUP_NAME>
</GetItemResponse>-->