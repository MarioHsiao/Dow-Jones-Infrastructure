<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="utf-8" /> 

  <xsl:template match="/">
    <UpdateCompanyListResponse>
      <xsl:apply-templates select="/*/Status"/>
    </UpdateCompanyListResponse>
  </xsl:template>
  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>
</xsl:stylesheet>