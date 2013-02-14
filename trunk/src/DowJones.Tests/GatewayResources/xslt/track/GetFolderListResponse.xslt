<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:include href ="FolderList.xslt"/>
	<xsl:template match="/*">
		<GetFolderListResponse>
			<folderListResponse>
				<xsl:copy-of select="//Status"/>
				<xsl:apply-templates select="//ResultSet">
				</xsl:apply-templates>
			</folderListResponse>
		</GetFolderListResponse>
	</xsl:template>
	<xsl:template match="//ResultSet">
		<xsl:choose>
			<xsl:when test="position()=1">
				<folderListResultSet>
					<xsl:attribute name="count"><xsl:value-of select="count(//Result[@status='0'])"/></xsl:attribute>
					<xsl:apply-templates select="//Result[@status='0']">
						<xsl:sort data-type ="text" select ="QueryName" case-order ="lower-first" order ="ascending"/>
					</xsl:apply-templates>
				</folderListResultSet>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
