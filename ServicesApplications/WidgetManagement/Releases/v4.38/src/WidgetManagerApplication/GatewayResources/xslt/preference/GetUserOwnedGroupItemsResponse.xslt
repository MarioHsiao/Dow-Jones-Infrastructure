<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
	<xsl:template match="/*">
		<xsl:element name="GetUserOwnedGroupItemsResponse">
			<xsl:copy-of select="Control"/>
			<xsl:copy-of select="Status"/>
			<xsl:for-each select="ResultSet/Result/child::*">
				<xsl:choose >
					<xsl:when test="local-name() = 'ITEM'">
						<xsl:element name="item">
							<xsl:element name ="ITEM_INSTANCE_NAME">
								<xsl:value-of select="ITEM_INSTANCE_NAME"/>
							</xsl:element>
							<xsl:element name ="ITEM_ID">
								<xsl:value-of select="ITEM_ID"/>
							</xsl:element>
							<xsl:element name ="ITEM_CLASS">
								<xsl:value-of select="ITEM_CLASS"/>
							</xsl:element>
							<xsl:element name ="ITEM_BLOB">
								<xsl:copy-of select="ITEM_BLOB/child::*"/>								
							</xsl:element>
						</xsl:element>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select ="."/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>

		</xsl:element>
	</xsl:template>
</xsl:stylesheet>