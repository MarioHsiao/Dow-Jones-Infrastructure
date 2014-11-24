<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/*" xmlns="http://types.dowjones.net/foldermanager" xmlns:rta="http://types.dowjones.net/foldermanager">

    <xsl:element name="ListFolderResponse">
      <xsl:element name="folderInfoList">
        <xsl:attribute name="count">
          <xsl:value-of select="//rta:ListFolderResponse/rta:folderInfoList/@count" />
        </xsl:attribute>

        <xsl:for-each select="//rta:ListFolderResponse/rta:folderInfoList/rta:folderInfo" >
          <xsl:element name="folderInfo">
            <xsl:for-each select="./child::*">
              <xsl:choose>
                <xsl:when test="local-name() = 'userQuery'">
                  <xsl:copy-of select="./child::*"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:copy-of select="."/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:for-each>
          </xsl:element>
        </xsl:for-each>
      </xsl:element>
    </xsl:element>
  </xsl:template>

</xsl:stylesheet>
