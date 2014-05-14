<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <AddGroupsResponse>
        <xsl:apply-templates select="ResultSet/Result/GROUP"/>
        <xsl:apply-templates select="ResultSet/Result/BAD_GROUP_LIST"/>
          <!--<xsl:copy-of select="ResultSet/Result/ERROR_CODE"/>
          <xsl:copy-of select="ResultSet/Result/ERROR_GENERAL_MSG"/>-->
    </AddGroupsResponse>
  </xsl:template>

  <xsl:template match ="GROUP">
    <Group>
      <Name>
        <xsl:value-of select="NAME"/>
      </Name>
      <Id>
        <xsl:value-of select="ID"/>
      </Id>
    </Group>
  </xsl:template>
  <xsl:template match ="BAD_GROUP_LIST">
    <Group>
      <Name>
        <xsl:value-of select="GROUP_NAME"/>
      </Name>
      <Status>
        <xsl:value-of select="ERROR_CODE"/>
      </Status>
    </Group>
  </xsl:template>
</xsl:stylesheet>