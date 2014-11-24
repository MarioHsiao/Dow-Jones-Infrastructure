<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template match="Result">
		<xsl:choose>
			<xsl:when test="@status='0'">
				<folderList>
					<xsl:if test="string-length(normalize-space(../@foldertype)) &gt; 0">
			</xsl:if>
					<xsl:choose>
						<xsl:when test="string-length(normalize-space(@folderid)) &gt; 0">
							<folderID>
								<xsl:value-of select="@folderid"/>
							</folderID>
						</xsl:when>
						<xsl:otherwise>
							<folderID/>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:for-each select="*">
						<xsl:if test="name(.)='QueryName'">
							<xsl:if test="string-length(normalize-space(.)) &gt; 0">
								<folderName>
									<xsl:value-of select="normalize-space(.)"/>
								</folderName>
							</xsl:if>
						</xsl:if>
						<xsl:if test="name(.)!='QueryName'">
							<xsl:if test="name(.)='QueryHighlight'">
								<xsl:if test="string-length(normalize-space(.)) &gt; 0">
									<highlightString>
										<xsl:value-of select="normalize-space(.)"/>
									</highlightString>
								</xsl:if>
							</xsl:if>
						</xsl:if>
					</xsl:for-each>
					<xsl:if test="../@foldertype='user'">
						<xsl:if test="string-length(normalize-space(../@foldertype)) &gt; 0">
							<xsl:element name="newHits">
								<xsl:if test="@newhits='yes'">true</xsl:if>
								<xsl:if test="@newhits='no'">false</xsl:if>
							</xsl:element>
						</xsl:if>
						<xsl:if test="string-length(normalize-space(@productType))&gt; 0">
							<xsl:if test="@productType='iff'">
								<productType>Iff</productType>
							</xsl:if>
							<xsl:if test="@productType='global'">
								<productType>Global</productType>
							</xsl:if>
						</xsl:if>
						<xsl:if test="string-length(normalize-space(@deliveryMethod)) &gt; 0">
							<xsl:if test="@deliveryMethod='online'">
								<deliveryMethod>Online</deliveryMethod>
							</xsl:if>
							<xsl:if test="@deliveryMethod='continuous'">
								<deliveryMethod>Continuous</deliveryMethod>
							</xsl:if>
							<xsl:if test="@deliveryMethod='batch'">
								<deliveryMethod>Batch</deliveryMethod>
							</xsl:if>
						</xsl:if>
						<xsl:if test="string-length(normalize-space(@email)) &gt; 0">
							<email>
								<xsl:value-of select="@email"/>
							</email>
						</xsl:if>
						<xsl:if test="string-length(normalize-space(@documentFormat)) &gt; 0">
							<documentFormat>
								<xsl:value-of select="@documentFormat"/>
							</documentFormat>
						</xsl:if>
						<xsl:if test="string-length(normalize-space(@deliveryTimes)) &gt; 0">
							<xsl:if test="@deliveryTimes='a'">
								<deliveryTimes>Morning</deliveryTimes>
							</xsl:if>
							<xsl:if test="@deliveryTimes='p'">
								<deliveryTimes>Afternoon</deliveryTimes>
							</xsl:if>
							<xsl:if test="@deliveryTimes='b'">
								<deliveryTimes>Both</deliveryTimes>
							</xsl:if>
						</xsl:if>
						<xsl:if test="string-length(normalize-space(@timeZone)) &gt; 0">
							<timeZone>
								<xsl:value-of select="@timeZone"/>
							</timeZone>
						</xsl:if>
					</xsl:if>
				</folderList>
			</xsl:when>
			<xsl:otherwise>
				<folderList>
					<xsl:attribute name="status"><xsl:value-of select="@status"/></xsl:attribute>
					<xsl:choose>
						<xsl:when test="string-length(normalize-space(@folderid)) &gt; 0">
							<folderID>
								<xsl:value-of select="@folderid"/>
							</folderID>
						</xsl:when>
						<xsl:otherwise>
							<folderID/>
						</xsl:otherwise>
					</xsl:choose>
				</folderList>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
