<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/*">
		<GetFolderResponse>
			<xsl:apply-templates select="/*/Status"/>
			<xsl:apply-templates select="/*/ResultSet"/>
		</GetFolderResponse>
	</xsl:template>
	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="/*/ResultSet">
		<folderResponse>
			<xsl:apply-templates select="//Folder"/>
		</folderResponse>
	</xsl:template>
	<xsl:template match="//Folder">
		<folderDetails>
			<folderID>
				<xsl:value-of select="@folderId"/>
			</folderID>
			<folderName>
				<xsl:value-of select="FolderName"/>
			</folderName>
			<productType>
				<xsl:choose>
					<xsl:when test="@productType='fast-track'">Alerts</xsl:when>
					<xsl:when test="@productType='select-headline'">SelectHeadlines</xsl:when>
					<xsl:when test="@productType='select-fulltext'">SelectFullText</xsl:when>
					<xsl:when test="@productType='iff'">Iff</xsl:when>
					<xsl:otherwise>Global</xsl:otherwise>
				</xsl:choose>
			</productType>
			<userQuery>
				<xsl:value-of select="UserQuery"/>
			</userQuery>
			<userData>
				<xsl:value-of select="UserData"/>
			</userData>
			<canonicalQuery>
				<xsl:value-of select="CanonicalQuery"/>
			</canonicalQuery>
			<deliveryMethod>
				<xsl:choose>
					<xsl:when test="Delivery/@deliveryMethod='continuous'">Continuous</xsl:when>
					<xsl:when test="Delivery/@deliveryMethod='batch'">Batch</xsl:when>
					<xsl:otherwise>Online</xsl:otherwise>
				</xsl:choose>
			</deliveryMethod>
			<xsl:if test="Delivery/@deliveryMethod != 'online'">
				<email>
					<xsl:value-of select="Delivery/@email"/>
				</email>
				<langCode>
					<xsl:value-of select="Delivery/@langCode"/>
				</langCode>
				<documentFormat>
					<xsl:value-of select="Delivery/@documentFormat"/>
				</documentFormat>
				<highlightQuery>
					<xsl:choose>
						<xsl:when test="Delivery/@highlightQuery='yes'">true</xsl:when>
						<xsl:when test="Delivery/@highlightQuery='no'">false</xsl:when>
						<xsl:otherwise>false</xsl:otherwise>
					</xsl:choose>
				</highlightQuery>
				<xsl:if test="Delivery/@dispositionType">
					<dispositionType>
						<xsl:choose>
							<xsl:when test="Delivery/@dispositionType='inline'">Inline</xsl:when>
							<xsl:when test="Delivery/@dispositionType='attachment'">Attachment</xsl:when>
						</xsl:choose>
					</dispositionType>
				</xsl:if>
				<wirelessfriendly>
					<xsl:choose>
						<xsl:when test="Delivery/@wirelessfriendly='y'">true</xsl:when>
						<xsl:when test="Delivery/@wirelessfriendly='n'">false</xsl:when>
						<xsl:otherwise>false</xsl:otherwise>
					</xsl:choose>
				</wirelessfriendly>
				<deliveryTimes>
					<xsl:choose>
						<xsl:when test="Delivery/DeliveryTimes/@time='a'">Morning</xsl:when>
						<xsl:when test="Delivery/DeliveryTimes/@time='p'">Afternoon</xsl:when>
						<xsl:when test="Delivery/DeliveryTimes/@time='b'">Both</xsl:when>
						<xsl:otherwise>None</xsl:otherwise>
					</xsl:choose>
				</deliveryTimes>
				<timeZone>
					<xsl:value-of select="Delivery/@timeZone"/>
				</timeZone>
			</xsl:if>
			<xsl:if test="Delivery/@documentType">
				<documentType>
					<xsl:value-of select="Delivery/@documentType"/>
				</documentType>
			</xsl:if>
			<relevanceThreshold>
				<xsl:value-of select="@relevenceThreshold"/>
			</relevanceThreshold>
			<xsl:if test="./FolderSharing">
				<xsl:apply-templates select="./FolderSharing"/>
			</xsl:if>
		</folderDetails>
	</xsl:template>

	<xsl:template match="//FolderSharing">
		<folderSharing>
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
