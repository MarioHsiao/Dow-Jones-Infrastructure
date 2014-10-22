<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:param name="RegOrNoAuth"/>
	<xsl:template match="/*">
		<xsl:choose>
			<xsl:when test="$RegOrNoAuth='Reg'">
				<xsl:element name="OnDemandEmailResponse">
					<xsl:element name="strDeliveryNumber">
						<xsl:value-of select="//DeliveryNum"/>
					</xsl:element>
				</xsl:element>
			</xsl:when>
			<xsl:when test="$RegOrNoAuth='NoAuth'">
				<xsl:element name="OnDemandEmailNoAuthResponse">
					<xsl:element name="strDeliveryNumber">
						<xsl:value-of select="//DeliveryNum"/>
					</xsl:element>
				</xsl:element>
			</xsl:when>
			<xsl:otherwise></xsl:otherwise>
		</xsl:choose>

	</xsl:template>

</xsl:stylesheet>
