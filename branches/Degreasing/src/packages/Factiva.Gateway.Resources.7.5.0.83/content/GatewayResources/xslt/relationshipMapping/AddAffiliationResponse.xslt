<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  
  <xsl:template match="/">
    <xsl:element name="AddAffiliationResponse">    
        <xsl:copy-of select="AddAffiliationResponse/AddAffiliationResult/affiliationAddResultSet/child::*"/>
      </xsl:element>    
  </xsl:template>  

</xsl:stylesheet>