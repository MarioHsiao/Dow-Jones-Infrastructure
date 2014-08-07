<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions" >
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"  omit-xml-declaration="yes"/>
  <xsl:template match="*">
    <Request>
      <xsl:if test="selectId">
        <SelectID><xsl:for-each select="selectId">
            <xsl:choose>
              <xsl:when test="position()&gt;1">,<xsl:value-of select="."/></xsl:when>
              <xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
            </xsl:choose>
          </xsl:for-each></SelectID>
      </xsl:if>
    </Request>
  </xsl:template>
</xsl:stylesheet>
