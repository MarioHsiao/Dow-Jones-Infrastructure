<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />

  <xsl:template match="/*">
    <xsl:element name="GetNewSubscribableItemsRequest">

      <xsl:element name="ItemClassList">
        <xsl:attribute name ="fcstype" >list</xsl:attribute>
        <xsl:copy-of select="ItemClassList/child::*"/>
      </xsl:element>
      
      <xsl:copy-of select="NumberOfDays"/>
      
    </xsl:element>
  </xsl:template>
  
</xsl:stylesheet>

<!--INPUT XML-->

<!--MESSAGE AFTER APPLYING XSLT:-->
