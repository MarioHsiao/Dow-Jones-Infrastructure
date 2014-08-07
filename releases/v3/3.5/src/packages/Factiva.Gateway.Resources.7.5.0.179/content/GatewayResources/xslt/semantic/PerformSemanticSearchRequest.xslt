<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/*">
    <SOAP-ENV:Envelope xmlns:SOAP-ENV="http://schemas.xmlsoap.org/soap/envelope/">
      <SOAP-ENV:Body>
        <xsl:apply-templates select="data"/>
      </SOAP-ENV:Body>
    </SOAP-ENV:Envelope>
  </xsl:template>

  <xsl:template match="data">
    <xsl:value-of select="." disable-output-escaping="yes"/>
  </xsl:template>
</xsl:stylesheet>