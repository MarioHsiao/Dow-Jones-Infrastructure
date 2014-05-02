<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:template match="/GetArchiveObjectResponse">
    <GetRawArticleResponse>
      <xsl:apply-templates select="Status"/>
      <RawResponse>
        <xsl:copy-of select="ResultSet"/>
      </RawResponse>
    </GetRawArticleResponse>
  </xsl:template>
</xsl:stylesheet>