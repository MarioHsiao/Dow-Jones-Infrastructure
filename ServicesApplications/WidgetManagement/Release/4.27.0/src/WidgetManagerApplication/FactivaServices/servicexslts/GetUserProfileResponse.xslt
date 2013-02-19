<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/*">
		<GetUserProfileResponse>
		    	<xsl:apply-templates select="/*/Status"/>
				<xsl:apply-templates select="/*/ResultSet"/>
			
		</GetUserProfileResponse>
	</xsl:template>
	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>
<xsl:template match="/*/ResultSet">
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates select="Result"/>
	</xsl:template>
<xsl:template match="Result">
<xsl:apply-templates select="//ACCOUNT_ID"/>
			<xsl:apply-templates select="//ADMIN_FLAG"/>
			<xsl:apply-templates select="//EMAIL"/>
			<xsl:apply-templates select="//FIRST_NAME"/>
			<xsl:apply-templates select="//INDUSTRY_CODE"/>
			<xsl:apply-templates select="//JOB_TITLE"/>
			<xsl:apply-templates select="//LAST_NAME"/>
			<xsl:apply-templates select="//PLAN_ID"/>
			<xsl:apply-templates select="//RULE_SET"/>
			<xsl:apply-templates select="//STATE"/>
			<xsl:apply-templates select="//CUSTOMER_TYPE"/>
			<xsl:apply-templates select="//USER_STATUS"/>
			<xsl:apply-templates select="//ZIP"/>
      <xsl:apply-templates select="//WSJ_TYPE"/>  
</xsl:template>
	<xsl:template match="//ACCOUNT_ID">
		<accountID>
			<xsl:value-of select="."/>
		</accountID>
	</xsl:template>
	<xsl:template match="//ADMIN_FLAG">
		<adminFlag>
			<xsl:choose>
				<xsl:when test=".='Y'">true</xsl:when>
				<xsl:when test=".='N'">false</xsl:when>
				<xsl:otherwise>false</xsl:otherwise>
			</xsl:choose>
		</adminFlag>
	</xsl:template>
	<xsl:template match="//EMAIL">
		<email>
			<xsl:value-of select="."/>
		</email>
	</xsl:template>
	<xsl:template match="//FIRST_NAME">
		<firstName>
			<xsl:value-of select="."/>
		</firstName>
	</xsl:template>
	<xsl:template match="//INDUSTRY_CODE">
		<industryCode>
			<xsl:value-of select="."/>
		</industryCode>
	</xsl:template>
	<xsl:template match="//JOB_TITLE">
		<jobTitle>
			<xsl:value-of select="."/>
		</jobTitle>
	</xsl:template>
	<xsl:template match="//LAST_NAME">
		<lastName>
			<xsl:value-of select="."/>
		</lastName>
	</xsl:template>
	<xsl:template match="//PLAN_ID">
		<planID>
			<xsl:value-of select="."/>
		</planID>
	</xsl:template>
	<xsl:template match="//RULE_SET">
		<ruleSet>
			<xsl:value-of select="."/>
		</ruleSet>
	</xsl:template>
	<xsl:template match="//STATE">
		<state>
			<xsl:value-of select="."/>
		</state>
	</xsl:template>
	<xsl:template match="//CUSTOMER_TYPE">
		<userType>
			<xsl:value-of select="."/>
		</userType>
	</xsl:template>
	<xsl:template match="//USER_STATUS">
		<userStatus>
			<xsl:value-of select="."/>
		</userStatus>
	</xsl:template>
	<xsl:template match="//ZIP">
		<zipCode>
			<xsl:value-of select="."/>
		</zipCode>
	</xsl:template>
  <!-- SM 051407 FOR WSJ.COM LINK TO BE DISPALYED IN SEARCH 2.0 
        S - SEAMLESS ACESSS, 
        B - BULK ACCESS. 
        EITHER WAY ALLOWED TO SEE THE LINK. ANYOTHER VALUE OR NO VALUE IS NOTALLOWED.-->
  <xsl:template match ="//WSJ_TYPE">
    <allowedWSJAccess>
      <xsl:choose>
        <xsl:when test=".='S' or .='B'">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>

    </allowedWSJAccess>
  </xsl:template>
</xsl:stylesheet>
