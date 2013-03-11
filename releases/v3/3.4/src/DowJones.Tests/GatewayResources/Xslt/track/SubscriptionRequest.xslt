<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions" exclude-result-prefixes="fn xsl xs xsi">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:param name="subscribeOrunsubscribe"/>
	<xsl:template match="/*">
		<xsl:element name="TrackFldrSubscriptionRequest">
			<xsl:attribute name="ver">1.0</xsl:attribute>
			<xsl:element name="Subscription">
				<xsl:apply-templates select="//folderId">
				</xsl:apply-templates>
				<xsl:apply-templates select="//folderName">
				</xsl:apply-templates>
				<xsl:apply-templates select="//deliveryMethod">
				</xsl:apply-templates>
				<xsl:apply-templates select="//deliveryTimes">
				</xsl:apply-templates>
				<xsl:apply-templates select="//timeZone">
				</xsl:apply-templates>
				<xsl:apply-templates select="//documentType">
				</xsl:apply-templates>
				<xsl:apply-templates select="//documentFormat">
				</xsl:apply-templates>
				<xsl:apply-templates select="//dispositionType">
				</xsl:apply-templates>
				<xsl:apply-templates select="//email">
				</xsl:apply-templates>
				<xsl:apply-templates select="//sortBy">
				</xsl:apply-templates>
				<xsl:apply-templates select="//maxNumber">
				</xsl:apply-templates>
				<xsl:apply-templates select="//highlightQuery">
				</xsl:apply-templates>
				<xsl:apply-templates select="//wirelessfriendly"/>
				<xsl:apply-templates select="//deduplicationLevel"/>
				<!--<xsl:apply-templates select="//showDup"/>-->
				<!--force a default-->
				<emailContentType>all</emailContentType>
			</xsl:element>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//deduplicationLevel">
		<xsl:element name="DedupLevel">
			<xsl:attribute name="value"><!--<xsl:choose>
					<xsl:when test=".=None">0</xsl:when>
					<xsl:when test=".=Similar">30</xsl:when>
					<xsl:when test=".=HighlySimilar">50</xsl:when>
					<xsl:when test=".=VirtuallyIdentical">100</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>--><xsl:value-of select="."/></xsl:attribute>
			<xsl:attribute name="showdup"><xsl:choose><xsl:when test="//showDup='true' or //showDup='1'">1</xsl:when><xsl:otherwise>0</xsl:otherwise></xsl:choose></xsl:attribute>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//deliveryMethod">
		<xsl:choose>
			<xsl:when test=".='Batch'">
				<xsl:element name="DeliveryMethod">batch</xsl:element>
			</xsl:when>
			<xsl:when test=".='Continuous'">
				<xsl:element name="DeliveryMethod">continuous</xsl:element>
			</xsl:when>
			<xsl:when test=".='Online'">
				<xsl:element name="DeliveryMethod">online</xsl:element>
			</xsl:when>
			<xsl:otherwise>
				<xsl:element name="DeliveryMethod">online</xsl:element>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="//deliveryTimes">
		<xsl:choose>
			<xsl:when test=".='Morning'">
				<xsl:element name="DeliveryTimes">morning</xsl:element>
			</xsl:when>
			<xsl:when test=".='Afternoon'">
				<xsl:element name="DeliveryTimes">afternoon</xsl:element>
			</xsl:when>
      <xsl:when test=".='EarlyMorning'">
        <xsl:element name="DeliveryTimes">e</xsl:element>
      </xsl:when>
			<xsl:when test=".='Both'">
				<xsl:element name="DeliveryTimes">b</xsl:element>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="//timeZone">
		<xsl:element name="TimeZone">
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//documentType">
		<xsl:element name="DocumentType">
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//documentFormat">
		<xsl:element name="DocumentFormat">
			<xsl:choose>
				<xsl:when test=".='TextPlain'">text/plain</xsl:when>
				<xsl:when test=".='TextHtml'">text/html</xsl:when>
				<xsl:when test=".='ApplicationPdf'">application/pdf</xsl:when>
				<xsl:when test=".='ApplicationPdf'">application/pdf</xsl:when>
				<xsl:otherwise>text/plain</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//dispositionType">
		<xsl:element name="DispositionType">
			<xsl:choose>
				<xsl:when test=".='Inline'">inline</xsl:when>
				<xsl:when test=".='Attachment'">attachment</xsl:when>
				<xsl:otherwise>inline</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//email">
		<EmailAddress>
			<xsl:value-of select="."/>
		</EmailAddress>
	</xsl:template>
	<xsl:template match="//sortBy">
		<SortBy>
			<xsl:choose>
				<xsl:when test=".='Date'">date</xsl:when>
				<xsl:when test=".='Relevance'">relevance</xsl:when>
				<xsl:when test=".='ClipTime'">cliptime</xsl:when>
				<xsl:otherwise>date</xsl:otherwise>
			</xsl:choose>
		</SortBy>
	</xsl:template>
	<xsl:template match="//maxNumber">
		<MaxHeadlines>
			<xsl:value-of select="."/>
		</MaxHeadlines>
	</xsl:template>
	<xsl:template match="//highlightQuery">
		<HighLight>
			<xsl:choose>
				<xsl:when test=".='true'">yes</xsl:when>
				<xsl:otherwise>no</xsl:otherwise>
			</xsl:choose>
		</HighLight>
	</xsl:template>
	<xsl:template match="//wirelessfriendly">
		<WirelessFriendly>
			<xsl:choose>
				<xsl:when test=".='true'">y</xsl:when>
				<xsl:otherwise>n</xsl:otherwise>
			</xsl:choose>
		</WirelessFriendly>
	</xsl:template>
	<xsl:template match="//folderName">
		<FolderName>
			<xsl:value-of select="."/>
		</FolderName>
	</xsl:template>
	<xsl:template match="//folderId">
		<xsl:choose>
			<xsl:when test="$subscribeOrunsubscribe='subscribe'">
				<xsl:attribute name="referenceFolderId"><xsl:value-of select="."/></xsl:attribute>
				<xsl:attribute name="action">add</xsl:attribute>
			</xsl:when>
			<xsl:when test="$subscribeOrunsubscribe='revise'">
				<xsl:attribute name="subscriptionFolderId"><xsl:value-of select="."/></xsl:attribute>
				<xsl:attribute name="action">revise</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="subscriptionFolderId"><xsl:value-of select="."/></xsl:attribute>
				<xsl:attribute name="action">delete</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
