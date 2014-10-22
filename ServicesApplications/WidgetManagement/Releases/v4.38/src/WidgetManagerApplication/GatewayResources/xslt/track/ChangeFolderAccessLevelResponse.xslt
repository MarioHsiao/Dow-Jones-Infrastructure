<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:param name="transactionType"></xsl:param>
	<xsl:template match="/*">
		<xsl:choose>
			<xsl:when test="$transactionType='share'">
				<ShareFolderResponse>
					<xsl:copy-of select="//Status"/>
					<xsl:apply-templates select ="//ResultSet"/>
				</ShareFolderResponse>
			</xsl:when>
      <xsl:when test="$transactionType='unshare'">
        <UnShareFolderResponse>
          <xsl:copy-of select="//Status"/>
          <xsl:apply-templates select ="//ResultSet"/>
        </UnShareFolderResponse>
      </xsl:when>
      <xsl:when test="$transactionType='SetFolderShareProperties'">
        <SetFolderSharePropertiesResponse>
          <xsl:copy-of select="//Status"/>
          <xsl:apply-templates select ="//ResultSet"/>
        </SetFolderSharePropertiesResponse>
      </xsl:when>
    </xsl:choose>
	</xsl:template>
	<xsl:template match="//ResultSet">
		<shareFolderListResultSet count="{@count}">
			<xsl:apply-templates select ="Result"/>
		</shareFolderListResultSet>
	</xsl:template>
	<xsl:template match="//Result">
		<shareFolderList>
			<status>
				<xsl:value-of select ="@status"/>
			</status>
			<folderId>
				<xsl:value-of select ="@folderId"/>
			</folderId>
		</shareFolderList>
	</xsl:template>
</xsl:stylesheet>
