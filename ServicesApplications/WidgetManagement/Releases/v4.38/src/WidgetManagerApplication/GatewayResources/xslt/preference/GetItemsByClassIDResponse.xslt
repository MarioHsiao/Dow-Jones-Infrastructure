<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" cdata-section-elements="itemBlob"/>
	<xsl:template match="/*">
		<xsl:element name="GetItemsByClassIDResponse">
			<xsl:copy-of select="Control"/>
			<xsl:copy-of select="Status"/>
			<items>
				<xsl:apply-templates select="//RESPONSE_LIST"/>
			</items>
      <categorizedItems>
        <xsl:apply-templates select="//CategoryItemList"/>
      </categorizedItems>
		</xsl:element>
	</xsl:template>
  <xsl:template match="RESPONSE_LIST">
    <xsl:element name="item">
      <xsl:copy-of select="child::*"/>
    </xsl:element>
  </xsl:template>

    <xsl:template match="CategoryItemList">
		<xsl:element name="CategorizedItem">
			<xsl:copy-of select="child::*"/>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>
