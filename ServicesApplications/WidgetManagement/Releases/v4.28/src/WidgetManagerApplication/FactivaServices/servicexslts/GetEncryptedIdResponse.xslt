<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/*">
    <GetEncryptedIdResponse>
      <xsl:copy-of select="/*/Control"/>
      <xsl:copy-of select="/*/Status"/>
      <xsl:apply-templates select="/*/ResultSet"/>
    </GetEncryptedIdResponse>
  </xsl:template>

  <xsl:template match="/*/ResultSet">
    <EncryptedId>
      <xsl:value-of select="//ENCRYPTED_STRING"/>
    </EncryptedId>
  </xsl:template>
</xsl:stylesheet>
