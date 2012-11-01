<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:param name="subscribeOrunsubscribe"/>
  <xsl:template match="/*">
    <xsl:choose>
      <xsl:when test="$subscribeOrunsubscribe='subscribe' or $subscribeOrunsubscribe='revise' ">
        <xsl:element name="SubscribeFolderResponse">
          <xsl:element name="folderId">
            <xsl:value-of select="//Result/@subscriptionFolderId"/>
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:when test="$subscribeOrunsubscribe='unsubscribe'">
        <xsl:element name="UnSubscribeFolderResponse">
          <xsl:element name="folderId">
            <xsl:value-of select="//Result/@subscriptionFolderId"/>
          </xsl:element>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise></xsl:otherwise>
    </xsl:choose>
    
  </xsl:template>
  
</xsl:stylesheet>
