<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:template match="//FolderSharing">
    <folderSharing>
      <xsl:if test="@isOwner">
        <isOwner>
          <xsl:choose>
            <xsl:when test="@isOwner='y'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </isOwner>
      </xsl:if>
      <xsl:if test="@assetId">
        <xsl:element name="assetId">
          <xsl:value-of select="@assetId"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="@rootId">
        <xsl:element name="rootId">
          <xsl:value-of select="@rootId"/>
        </xsl:element>
      </xsl:if>
      <xsl:if test="@assetType">
        <assetType>
          <xsl:choose>
            <xsl:when test="@assetType='personal'">Personal</xsl:when>
            <xsl:when test="@assetType='assigned-personal'">Assigned</xsl:when>
            <xsl:when test="@assetType='subscribed-personal'">Subscribed</xsl:when>
            <xsl:when test="@assetType='unknown'">Unknown</xsl:when>
          </xsl:choose>
        </assetType>
      </xsl:if>
      <xsl:if test="@shareStatus">
        <status>
          <xsl:choose>
            <xsl:when test="@shareStatus='active'">Active</xsl:when>
            <xsl:when test="@shareStatus='inactive'">Inactive</xsl:when>
            <xsl:when test="@shareStatus='deleted'">Deleted</xsl:when>
          </xsl:choose>
        </status>
      </xsl:if>
      <sharingData>
        <xsl:if test=".//acScope">
          <xsl:call-template name="CommonShareScope">
            <xsl:with-param name="nodeName">accessControlScope</xsl:with-param>
            <xsl:with-param name ="scope">
              <xsl:value-of select=".//acScope/@value"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
        <xsl:if test=".//listScope">
          <xsl:call-template name="CommonShareScope">
            <xsl:with-param name="nodeName">listingScope</xsl:with-param>
            <xsl:with-param name ="scope">
              <xsl:value-of select=".//listScope/@value"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
        <xsl:if test=".//assignScope">
          <xsl:call-template name="CommonShareScope">
            <xsl:with-param name="nodeName">assignedScope</xsl:with-param>
            <xsl:with-param name ="scope">
              <xsl:value-of select=".//assignScope/@value"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
        <xsl:if test=".//sharePromotion">
          <xsl:call-template name="CommonShareScope">
            <xsl:with-param name="nodeName">sharePromotion</xsl:with-param>
            <xsl:with-param name ="scope">
              <xsl:value-of select=".//sharePromotion/@value"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
        <!-- Not procssing allowcopy and other as we do deserialize this and not needed.-->

        <xsl:if test=".//intHash">
          <xsl:element name="internalHashKey">
            <xsl:value-of select=".//intHash"/>
          </xsl:element>
        </xsl:if>

        <xsl:if test=".//intAccess">
          <xsl:element name="internalAccess">
            <xsl:choose>
              <xsl:when test=".//intAccess/@value='On'">Allow</xsl:when>
              <xsl:otherwise>Deny</xsl:otherwise>
            </xsl:choose>
          </xsl:element>
        </xsl:if>
        <xsl:if test=".//extHash">
          <xsl:element name="externalHashKey">
            <xsl:value-of select=".//extHash"/>
          </xsl:element>
        </xsl:if>

        <xsl:if test=".//extAccess">
          <xsl:element name="externalAccess">
            <xsl:choose>
              <xsl:when test=".//extAccess/@value='On'">Allow</xsl:when>
              <xsl:otherwise>Deny</xsl:otherwise>
            </xsl:choose>
          </xsl:element>
        </xsl:if>
      </sharingData>
    </folderSharing>
  </xsl:template>
  <xsl:template name="CommonShareScope">
    <xsl:param name ="nodeName"/>
    <xsl:param name="scope"/>
    <xsl:if test="string-length(normalize-space($scope)) &gt; 0">
      <xsl:element name="{$nodeName}">
        <xsl:choose>
          <xsl:when test="$scope='personal'">Personal</xsl:when>
          <xsl:when test="$scope='everyone'">Everyone</xsl:when>
          <xsl:when test="$scope='account'">Account</xsl:when>
          <xsl:otherwise>Personal</xsl:otherwise>
          <!-- Defualt to personal-->
        </xsl:choose>
      </xsl:element>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>