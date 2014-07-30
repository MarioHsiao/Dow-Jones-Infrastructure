<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/*">
    <GetUserProfileResponse>
      <!--<xsl:apply-templates select="/*/Status"/>-->
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
    <xsl:apply-templates select="//isoCountryCode"/>
    <xsl:apply-templates select="//COUNTRY_CODE"/>
    <xsl:apply-templates select="//CUSTOMER_TYPE"/>
    <xsl:apply-templates select="//USER_STATUS"/>
    <xsl:apply-templates select="//ZIP"/>
    <xsl:apply-templates select="//WSJ_TYPE"/>
    <xsl:apply-templates select="//fcode"/>
    <xsl:apply-templates select="//serviceTier"/>
    <xsl:apply-templates select="//ADDR1"/>
    <xsl:apply-templates select="//ADDR2"/>
    <xsl:apply-templates select="//CITY"/>
    <xsl:apply-templates select="//COMPANY_NAME"/>
    <xsl:apply-templates select="//DEPT_CODE"/>
    <xsl:apply-templates select="//DeptDescription"/>
    <xsl:apply-templates select="//FAX"/>
    <xsl:apply-templates select="//TEL"/>
    <xsl:apply-templates select="//Suffix"/>
    <xsl:apply-templates select="//THIRD_PARTY_ID"/>
    <xsl:apply-templates select="//TITLE"/>
    <xsl:apply-templates select="//City2"/>
    <xsl:apply-templates select="//PhoneCountryCode"/>
    <xsl:apply-templates select="//ContactPreference"/>
    <xsl:apply-templates select="//FaxCountryCode"/>
    <xsl:apply-templates select="//SECURITY_WORD"/>
    <xsl:apply-templates select="//DNB_FLAG"/>
    <xsl:apply-templates select="//emailLogin"/>
    <xsl:apply-templates select="//BetaProjectCode"/>
    <xsl:apply-templates select="//BillingPackage"/>
  </xsl:template>
  <xsl:template match="//BillingPackage">
    <billingPackage>
      <xsl:value-of select="."/>
    </billingPackage>
  </xsl:template>
  <xsl:template match="//BetaProjectCode">
    <betaProjectCode>
      <xsl:value-of select="."/>
    </betaProjectCode>
  </xsl:template>
  <xsl:template match="//emailLogin">
    <emailLogin>
      <xsl:value-of select="."/>
    </emailLogin>
  </xsl:template>
  <xsl:template match="//DNB_FLAG">
    <dnbFlag>
      <xsl:choose>
        <xsl:when test="(.) = 'Y'">
          true
        </xsl:when>
        <xsl:when test="(.) = 'N'">
          false
        </xsl:when>
      </xsl:choose>
    </dnbFlag>
  </xsl:template>
  <xsl:template match="//SECURITY_WORD">
    <securityWord>
      <xsl:value-of select="."/>
    </securityWord>
  </xsl:template>
  <xsl:template match="//FaxCountryCode">
    <faxCountryCode>
      <xsl:value-of select="."/>
    </faxCountryCode>
  </xsl:template>
  <xsl:template match="//ContactPreference">
    <contactPreference>
      <xsl:value-of select="."/>
    </contactPreference>
  </xsl:template>
  <xsl:template match="//City2">
    <city2>
      <xsl:value-of select="."/>
    </city2>
  </xsl:template>
  <xsl:template match="//DEPT_CODE">
    <departmentCode>
      <xsl:value-of select="."/>
    </departmentCode>
  </xsl:template>
  <xsl:template match="//DeptDescription">
    <departmentDescription>
      <xsl:value-of select="."/>
    </departmentDescription>
  </xsl:template>
  <xsl:template match="//FAX">
    <fax>
      <xsl:value-of select="."/>
    </fax>
  </xsl:template>
  <xsl:template match="//TEL">
    <phone>
      <xsl:value-of select="."/>
    </phone>
  </xsl:template>
  <xsl:template match="//PhoneCountryCode">
    <phoneCountryCode>
      <xsl:value-of select="."/>
    </phoneCountryCode>
  </xsl:template>
  <xsl:template match="//Suffix">
    <suffix>
      <xsl:value-of select="."/>
    </suffix>
  </xsl:template>
  <xsl:template match="//THIRD_PARTY_ID">
    <thirdPartyId>
      <xsl:value-of select="."/>
    </thirdPartyId>
  </xsl:template>
  <xsl:template match="//TITLE">
    <title>
      <xsl:value-of select="."/>
    </title>
  </xsl:template>
  <xsl:template match="//ADDR1">
    <address1>
      <xsl:value-of select="."/>
    </address1>
  </xsl:template>
  <xsl:template match="//ADDR2">
    <address2>
      <xsl:value-of select="."/>
    </address2>
  </xsl:template>
  <xsl:template match="//CITY">
    <city>
      <xsl:value-of select="."/>
    </city>
  </xsl:template>
  <xsl:template match="//COMPANY_NAME">
    <companyName>
      <xsl:value-of select="."/>
    </companyName>
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
  <xsl:template match="//isoCountryCode">
    <ISOCountryCode>
      <xsl:value-of select="."/>
    </ISOCountryCode>
  </xsl:template>
  <xsl:template match="//COUNTRY_CODE">
    <countryCode>
      <xsl:value-of select="."/>
    </countryCode>
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
  <xsl:template match="//fcode">
    <fcode>
      <xsl:value-of select="."/>
    </fcode>
  </xsl:template>
  <xsl:template match="//serviceTier">
    <serviceTier>
      <xsl:value-of select="."/>
    </serviceTier>
  </xsl:template>
</xsl:stylesheet>
