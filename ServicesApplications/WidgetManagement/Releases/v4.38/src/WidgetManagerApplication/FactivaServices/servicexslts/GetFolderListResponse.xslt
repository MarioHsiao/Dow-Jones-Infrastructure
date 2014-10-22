<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
    <xsl:include href="FolderList.xslt"/>
    <xsl:template match="/*">
        <GetFolderListResponse>
		<folderListResponse>
			<xsl:copy-of select="//Status"/>
			<xsl:apply-templates select="//ResultSet"/>
		</folderListResponse>
       </GetFolderListResponse>
    </xsl:template>
    <xsl:template match="//ResultSet">
	<xsl:choose>
		<xsl:when test="position()=1">
			<folderListResultSet>
			    	<xsl:attribute name="count">
			    		<xsl:value-of select="count(//Result[@status='0'])"/>
			    	</xsl:attribute>
				<xsl:apply-templates select="//Result[@status='0']">
				</xsl:apply-templates>
			</folderListResultSet>
		</xsl:when>
	</xsl:choose>
  </xsl:template>

  <xsl:template match="FolderSharing">
    <folderSharing>
      <xsl:element name="assetId">
        <xsl:value-of select="@assetId"/>
      </xsl:element>
      <xsl:element name="rootId">
        <xsl:value-of select="rootId"/>
      </xsl:element>
      <assetType>
        <xsl:choose>
          <xsl:when test="@assetType='personal'">Personal</xsl:when>
          <xsl:when test="@assetType='assigned'">Assigned</xsl:when>
          <xsl:when test="@assetType='subscribed'">Subscribed</xsl:when>
        </xsl:choose>
      </assetType>
      <status>
        <xsl:choose>
          <xsl:when test="status='active'">Active</xsl:when>
          <xsl:when test="status='inactive'">Inactive</xsl:when>
          <xsl:when test="status='deleted'">Deleted</xsl:when>
        </xsl:choose>
      </status>
      <SharingData>
        <xsl:if test ="acScope">
          <accessControlScope>
            <xsl:value-of select ="acScope"/>
          </accessControlScope>
        </xsl:if>
        <xsl:if test ="listScope">
          <listingScope>
            <xsl:value-of select ="listScope"/>
          </listingScope>
        </xsl:if>
        <xsl:if test ="assignScope">
          <assignedScope>
            <xsl:value-of select ="assignScope"/>
          </assignedScope>
        </xsl:if>
        <xsl:if test ="sharePromotion">
          <sharePromotion>
            <xsl:value-of select ="sharePromotion"/>
          </sharePromotion>
        </xsl:if>
        <!-- Not procssing allowcopy and other as we do deserialize this and not needed.-->
      </SharingData>
    </folderSharing>
  </xsl:template>
 </xsl:stylesheet>
