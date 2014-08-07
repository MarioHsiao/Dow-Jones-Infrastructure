<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no" />
	<xsl:template match="/*">
		<xsl:element name="GetUserOwnedGroupItemsRequest">
			<xsl:for-each select="child::*">
				<xsl:choose>
					<xsl:when test="local-name()= 'ITEM_CLASS_LIST'">
						<xsl:element name="ITEM_CLASS_LIST">
							<xsl:attribute name ="fcstype" >list</xsl:attribute>
							<xsl:copy-of select="child::*"/>
						</xsl:element>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="."/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>
