<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

	<xsl:template match="*">
		<xsl:apply-templates select="/*/Status"/>
		<xsl:apply-templates select="GetArchiveObjectResponse"/>
	</xsl:template>

	<xsl:template match="GetArchiveObjectResponse">
		<GetWebArticleUrlResponse>
			<xsl:copy-of select="Status"/>
			<xsl:apply-templates select ="//ResultSet"/>
		</GetWebArticleUrlResponse>
	</xsl:template>

	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match ="/*/ResultSet">
		<xsl:element name="webArticleResultSet">
			<xsl:attribute name="count">
				<xsl:copy-of select="number(@count)"/>
			</xsl:attribute>
			<xsl:apply-templates select="Result"/>
		</xsl:element>
	</xsl:template>


	<xsl:template match="Result">
		<xsl:element name="webArticle">
			<xsl:if test="number(@status)!=0">
				<!--<xsl:attribute name="status"><xsl:value-of select="number(@status)"/></xsl:attribute>-->
				<status>
					<xsl:attribute name="value">
						<xsl:copy-of select="number(@status)"/>
					</xsl:attribute>
					<type/>
					<message/>
				</status>
				<accessionNo>
					<xsl:value-of select="@accessionno"/>
				</accessionNo>
				<reference>
					distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="@accessionno"/>
				</reference>
			</xsl:if>
			<xsl:if test="number(@status)=0">
				<accessionNo>
					<xsl:value-of select="@accessionno"/>
				</accessionNo>
				<reference>
					distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="@accessionno"/>
				</reference>

				<xsl:copy-of select=".//property"/>
			</xsl:if>
		</xsl:element>
	</xsl:template>



</xsl:stylesheet>