<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:param name="category"/>


	<xsl:template match="*">
		<xsl:apply-templates select="/*/Status"/>
		<xsl:apply-templates select="GetArchiveObjectResponse"/>
	</xsl:template>

	<xsl:template match="GetArchiveObjectResponse">
		<xsl:choose>
			<xsl:when test="$category='multimedia'">
				<xsl:element name="GetMultimediaArticleUrlResponse">
					<xsl:copy-of select="Status"/><xsl:apply-templates select ="//ResultSet"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="$category='webpage'">
				<xsl:element name="GetWebArticleUrlResponse">
					<xsl:copy-of select="Status"/>
					<xsl:apply-templates select ="//ResultSet"/>
				</xsl:element>
			</xsl:when>
			<xsl:otherwise>
				<xsl:element name="GetWebArticleUrlResponse">
					<xsl:copy-of select="Status"/>
					<xsl:apply-templates select ="//ResultSet"/>
				</xsl:element>				
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match ="/*/ResultSet">
		<xsl:choose>
			<xsl:when test="$category='multimedia'">
				<xsl:element name="multimediaArticleResultSet">
					<xsl:attribute name="count">
						<xsl:copy-of select="number(@count)"/>
					</xsl:attribute>
					<xsl:apply-templates select="Result"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="$category='webpage'">
				<xsl:element name="webArticleResultSet">
					<xsl:attribute name="count">
						<xsl:copy-of select="number(@count)"/>
					</xsl:attribute>
					<xsl:apply-templates select="Result"/>
				</xsl:element>
			</xsl:when>
			<xsl:otherwise>
				<xsl:element name="webArticleResultSet">
					<xsl:attribute name="count">
						<xsl:copy-of select="number(@count)"/>
					</xsl:attribute>
					<xsl:apply-templates select="Result"/>
				</xsl:element>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template match="Result">
		<xsl:choose>
			<xsl:when test="$category='multimedia'">
				<xsl:element name="multimediaArticle">
					<xsl:call-template name="articleProperties"></xsl:call-template>
				</xsl:element>
				</xsl:when>
			<xsl:when test="$category='webpage'">
				<xsl:element name="webArticle">
					<xsl:call-template name="articleProperties"></xsl:call-template>
				</xsl:element>
			</xsl:when>
			<xsl:otherwise>
				<xsl:element name="webArticle">
					<xsl:call-template name="articleProperties"></xsl:call-template>
				</xsl:element>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>



	<xsl:template name="articleProperties">
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
	</xsl:template>
</xsl:stylesheet>