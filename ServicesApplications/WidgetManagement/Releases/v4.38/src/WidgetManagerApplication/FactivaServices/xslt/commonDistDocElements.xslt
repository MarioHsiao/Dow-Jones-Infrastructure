<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:template name="commonDistDocElements">
		<xsl:for-each select="child::node()">
			<xsl:choose>
				<xsl:when test="(local-name()='hlt1') or (local-name()='hlt')">
					<hlt>
						<xsl:value-of select="."/>
					</hlt>
				</xsl:when>
				<xsl:when test="(local-name()='en')">
          <!--
          <en>
            <xsl:copy-of select ="@*"/>
            <xsl:call-template name="commonDistDocElements"/>
          </en>
          -->
          <xsl:call-template name="commonDistDocElements"/>
        </xsl:when>
				<xsl:when test="(local-name()='ELink')">
					<xsl:copy-of select="."/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="."/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="hlt1">
		<xsl:for-each select="child::node()">
			<xsl:choose>
				<xsl:when test="(local-name()='hlt1')">
					<hlt>
						<xsl:value-of select="*"/>
					</hlt>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="."/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
