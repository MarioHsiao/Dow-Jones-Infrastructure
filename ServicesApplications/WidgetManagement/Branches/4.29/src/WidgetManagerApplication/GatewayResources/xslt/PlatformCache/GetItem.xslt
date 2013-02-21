<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="GetItem">
		<SharedCacheRequest transaction="false" action="retrieve">
			<RequestList>
				<Request>
					<xsl:apply-templates select="key"/>
					<xsl:apply-templates select="namespace"/>
					<Update_Last_Accessed_Time>Y</Update_Last_Accessed_Time>
				</Request>
			</RequestList>
		</SharedCacheRequest>
	</xsl:template>
	<xsl:template match="key">
		<Storage_Key>
			<xsl:value-of select="."/>
		</Storage_Key>
	</xsl:template>
	<xsl:template match="namespace">
		<NameSpace>
			<xsl:value-of select="."/>
		</NameSpace>
	</xsl:template>
</xsl:stylesheet>
