<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

	<xsl:template name="GetError">
		<xsl:element name="ERROR_CODE">
			<xsl:value-of select="//ERROR_CODE"/>
		</xsl:element>
		<xsl:element name="ERROR_GENERAL_MSG">
			<xsl:value-of select="//ERROR_GENERAL_MSG"/>
		</xsl:element>
	</xsl:template>

	<xsl:template match="//Result">

		<xsl:apply-templates select="START_NUM"/>
		<xsl:apply-templates select="TOTAL_NUM"/>
		<xsl:apply-templates select="CONTEXT"/>
		<xsl:apply-templates select="ACCOUNT_INFO"/>
		<xsl:apply-templates select="USER_INFO"/>
	</xsl:template>

	<xsl:template match="ACCOUNT_INFO">
		<accountInfo>

			<xsl:apply-templates select="ACCOUNT_ID"/>
			<xsl:apply-templates select="ACCOUNT_TYPE"/>
			<xsl:apply-templates select="COMPANY_NAME"/>
			<xsl:apply-templates select="EMAIL"/>
			<xsl:apply-templates select="FAX"/>
			<xsl:apply-templates select="GEO_CODE"/>
			<xsl:apply-templates select="IDLE_TIMEOUT"/>
			<xsl:apply-templates select="INDUSTRY_CODE"/>
			<xsl:apply-templates select="INV_ACCESS_FLAG"/>
			<xsl:apply-templates select="JOB_TITLE"/>
			<xsl:apply-templates select="MAX_NUMBER_OF_USERS"/>
			<xsl:apply-templates select="MAX_SESSIONS"/>
			<xsl:apply-templates select="PLAN_ID"/>
			<xsl:apply-templates select="PRODUCT_ID"/>
			<xsl:apply-templates select="TELEPHONE"/>
			<xsl:apply-templates select="TERMINATION_DATE"/>
			<xsl:apply-templates select="THIRD_PARTY_ID"/>
			<xsl:apply-templates select="TITLE"/>
			<xsl:apply-templates select="USED_SESSIONS"/>
			<xsl:apply-templates select="USED_USERS"/>

			<xsl:apply-templates select="FIRST_NAME"/>
			<xsl:apply-templates select="LAST_NAME"/>
			<xsl:apply-templates select="STATE"/>
			<xsl:apply-templates select="CITY"/>
			<xsl:apply-templates select="COUNTRY_CODE"/>
			<xsl:apply-templates select="ADDR1"/>
			<xsl:apply-templates select="ADDR2"/>
			<xsl:apply-templates select="ZIP_CODE"/>
		</accountInfo>
	</xsl:template>

	<xsl:template match="USER_INFO">
		<accountUsers>
			<xsl:attribute name="count">
				<xsl:value-of select="count(//USER)" />
			</xsl:attribute>

			<xsl:apply-templates select="//USER_INFO/USER"/>
		</accountUsers>
	</xsl:template>

	<xsl:template match="//USER_INFO/USER">
		<UserInfo>
			<xsl:apply-templates select="ADDR1"/>
			<xsl:apply-templates select="ADDR2"/>
			<xsl:apply-templates select="CITY"/>
			<xsl:apply-templates select="COUNTRY_CODE"/>
			<xsl:apply-templates select="DeptDescription"/>
			<xsl:apply-templates select="FIRST_NAME"/>
			<xsl:apply-templates select="LAST_NAME"/>
			<xsl:apply-templates select="STATE"/>
			<xsl:apply-templates select="USER_ID"/>
			<xsl:apply-templates select="USER_STATUS"/>
			<xsl:apply-templates select="ZIP_CODE"/>

		</UserInfo>
	</xsl:template>
	
	<xsl:template match="ACCOUNT_ID">
		<accountID>
			<xsl:value-of select="."/>
		</accountID>
	</xsl:template>

	<xsl:template match="ACCOUNT_TYPE">
		<accountType>
			<xsl:value-of select="."/>
		</accountType>
	</xsl:template>

	<xsl:template match="COMPANY_NAME">
		<companyName>
			<xsl:value-of select="."/>
		</companyName>
	</xsl:template>

	<xsl:template match="EMAIL">
		<emailAddress>
			<xsl:value-of select="."/>
		</emailAddress>
	</xsl:template>

	<xsl:template match="FAX">
		<fax>
			<xsl:value-of select="."/>
		</fax>
	</xsl:template>

	<xsl:template match="GEO_CODE">
		<geoCode>
			<xsl:value-of select="."/>
		</geoCode>
	</xsl:template>

	<xsl:template match="IDLE_TIMEOUT">
		<idleTimeOut>
			<xsl:value-of select="."/>
		</idleTimeOut>
	</xsl:template>

	<xsl:template match="INDUSTRY_CODE">
		<industryCode>
			<xsl:value-of select="."/>
		</industryCode>
	</xsl:template>

	<xsl:template match="INV_ACCESS_FLAG">
		<invAccessFlag>
			<xsl:value-of select="."/>
		</invAccessFlag>
	</xsl:template>

	<xsl:template match="JOB_TITLE">
		<jobTitle>
			<xsl:value-of select="."/>
		</jobTitle>
	</xsl:template>

	<xsl:template match="MAX_NUMBER_OF_USERS">
		<maxNumberOfUsers>
			<xsl:value-of select="."/>
		</maxNumberOfUsers>
	</xsl:template>

	<xsl:template match="MAX_SESSIONS">
		<maxNumberOfSessions>
			<xsl:value-of select="."/>
		</maxNumberOfSessions>
	</xsl:template>

	<xsl:template match="PLAN_ID">
		<planID>
			<xsl:value-of select="."/>
		</planID>
	</xsl:template>

	<xsl:template match="PRODUCT_ID">
		<productID>
			<xsl:value-of select="."/>
		</productID>
	</xsl:template>

	<xsl:template match="TELEPHONE">
		<phoneNumber>
			<xsl:value-of select="."/>
		</phoneNumber>
	</xsl:template>

	<xsl:template match="TERMINATION_DATE">
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(.)) &gt; 0">
				<terminationDate>
					<xsl:value-of select="."/>
				</terminationDate>
			</xsl:when>
			<xsl:otherwise></xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="THIRD_PARTY_ID">
		<thirdPartyID>
			<xsl:value-of select="."/>
		</thirdPartyID>
	</xsl:template>

	<xsl:template match="TITLE">
		<title>
			<xsl:value-of select="."/>
		</title>
	</xsl:template>

	<xsl:template match="USED_SESSIONS">
		<numberOfUsedSessions>
			<xsl:value-of select="."/>
		</numberOfUsedSessions>
	</xsl:template>

	<xsl:template match="USED_USERS">
		<numberOfUsedUsers>
			<xsl:value-of select="."/>
		</numberOfUsedUsers>
	</xsl:template>

	<xsl:template match="ADDR1">
		<address1>
			<xsl:value-of select="."/>
		</address1>
	</xsl:template>

	<xsl:template match="ADDR2">
		<address2>
			<xsl:value-of select="."/>
		</address2>
	</xsl:template>

	<xsl:template match="CITY">
		<city>
			<xsl:value-of select="."/>
		</city>
	</xsl:template>

	<xsl:template match="COUNTRY_CODE">
		<countryCode>
			<xsl:value-of select="."/>
		</countryCode>
	</xsl:template>

	<xsl:template match="DeptDescription">
		<deptDescription>
			<xsl:value-of select="."/>
		</deptDescription>
	</xsl:template>

	<xsl:template match="FIRST_NAME">
		<firstName>
			<xsl:value-of select="."/>
		</firstName>
	</xsl:template>

	<xsl:template match="LAST_NAME">
		<lastName>
			<xsl:value-of select="."/>
		</lastName>
	</xsl:template>

	<xsl:template match="STATE">
		<state>
			<xsl:value-of select="."/>
		</state>
	</xsl:template>

	<xsl:template match="USER_ID">
		<userID>
			<xsl:value-of select="."/>
		</userID>
	</xsl:template>

	<xsl:template match="USER_STATUS">
		<userStatus>
			<xsl:value-of select="."/>
		</userStatus>
	</xsl:template>

	<xsl:template match="ZIP_CODE">
		<zipCode>
			<xsl:value-of select="."/>
		</zipCode>
	</xsl:template>

	<xsl:template match="START_NUM">
		<indexOfFirstResult>
			<xsl:value-of select="."/>
		</indexOfFirstResult>
	</xsl:template>

	<xsl:template match="TOTAL_NUM">
		<totalCount>
			<xsl:value-of select="."/>
		</totalCount>
	</xsl:template>

	<xsl:template match="CONTEXT">
		<accountUserContext>
			<xsl:value-of select="."/>
		</accountUserContext>
	</xsl:template>
	
</xsl:stylesheet>
