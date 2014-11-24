<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:template match="/">
    <xsl:element name="GetWhatsNewSettingsResponse">
      <xsl:apply-templates select="/*/ResultSet"/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="/*/ResultSet">
    <xsl:apply-templates select="Result"/>
  </xsl:template>
  <xsl:template match="Result">
    <xsl:element name="setupCode">
      <xsl:value-of select="@setupcode"/>
    </xsl:element>
    <xsl:element name="deliverySettings">
      <xsl:element name="DeliveryTime">
        <xsl:value-of select="deliverytime"/>
      </xsl:element>
      <xsl:element name="EmailAddress">
        <xsl:value-of select="emailaddress"/>
      </xsl:element>
      <xsl:element name="EmailFormat">
        <xsl:choose>
          <xsl:when test="string-length(normalize-space(emailformat)) &gt; 0"><xsl:value-of select="emailformat"/></xsl:when>
          <xsl:otherwise>UnSpecified</xsl:otherwise>
        </xsl:choose>
      </xsl:element>
      <xsl:element name="Language">
        <xsl:value-of select="language"/>
      </xsl:element>
      <xsl:element name="EmailLayout">
        <xsl:choose>
          <xsl:when test="string-length(normalize-space(resultsdisplay)) &gt; 0"><xsl:value-of select="resultsdisplay"/></xsl:when>
          <xsl:otherwise>UnSpecified</xsl:otherwise>
        </xsl:choose>
      </xsl:element>
      <xsl:element name="Subject">
        <xsl:value-of select="subject"/>
      </xsl:element>
      <xsl:element name="TimeZone">
        <xsl:value-of select="timezone"/>
      </xsl:element>
      <xsl:element name="IsWirelessFriendly">
        <xsl:choose>
          <xsl:when test="wirelessfriendly='y'">true</xsl:when>
          <xsl:otherwise>false</xsl:otherwise>
        </xsl:choose>
      </xsl:element>
    </xsl:element>
  </xsl:template>
</xsl:stylesheet>