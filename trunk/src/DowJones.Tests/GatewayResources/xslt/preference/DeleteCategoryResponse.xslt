<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <xsl:element name="DeleteCategoryResponse">
      <xsl:copy-of select="Control"/>
      <xsl:copy-of select="Status"/>      
      <xsl:element name="BadItemList">
        <xsl:for-each select="ResultSet/Result/child::*">
          <xsl:choose >
            <xsl:when test="local-name() = 'ERROR_CODE' ">
              <xsl:element name="ErrorCode">
                <xsl:value-of select="."/>
              </xsl:element>
            </xsl:when>
            <xsl:otherwise >
              <xsl:copy-of select="."/>
            </xsl:otherwise>
          </xsl:choose>          
        </xsl:for-each>
        
      </xsl:element>
    </xsl:element>
  </xsl:template>

</xsl:stylesheet>