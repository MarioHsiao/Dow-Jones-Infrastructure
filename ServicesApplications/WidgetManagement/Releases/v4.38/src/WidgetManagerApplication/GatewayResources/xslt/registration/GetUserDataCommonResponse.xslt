<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:include href="../common/commonElements.xslt"/>

  <msxsl:script language="JScript" implements-prefix="user">
    <![CDATA[
			function getTitle(str)
			{
				var returnVal;
				switch(str)
				{
					case "Sir":		returnVal = "Sir";break;
					case "Rev.":	returnVal = "Rev.";break;
					case "Dr.":		returnVal = "Dr.";break;
					case "Frau":	returnVal = "Frau";break;
					case "Herr":	returnVal = "Herr";break;
					case "M.":		returnVal = "M.";break;
					case "Mlle.":	returnVal = "Mlle.";break;
					case "Mme.":	returnVal = "Mme.";break;
					case "Mr.":		returnVal = "Mr.";break;
					case "Mrs.":	returnVal = "Mrs.";break;
					case "Ms.":		returnVal = "Ms.";break;
					case "Miss":	returnVal = "Miss";break;
					case "Prof.":	returnVal = "Prof.";break;
					case "Sr.":		returnVal = "Sr.";break;
					case "Sra.":	returnVal = "Sra.";break;
					case "Srta.":	returnVal = "Srta.";break;
					default:		returnVal = "Mr.";
				}
				return returnVal;
			}

			function getDepartmentCategory(str)
			{
				var returnVal;
				switch(str)
				{
					case "BD":		returnVal = "BD";break;
					case "CI":		returnVal = "CI";break;
					case "CN":		returnVal = "CN";break;
					case "EX":		returnVal = "EX";break;
					case "FN":		returnVal = "FN";break;
					case "HR":		returnVal = "HR";break;
					case "IC":		returnVal = "IC";break;
					case "IT":		returnVal = "IT";break;
					case "LG":		returnVal = "LG";break;
					case "MF":		returnVal = "MF";break;
					case "MK":		returnVal = "MK";break;
					case "NR":		returnVal = "NR";break;
					case "OM":		returnVal = "OM";break;
					case "OT":		returnVal = "OT";break;
					case "PR":		returnVal = "PR";break;
					case "PU":		returnVal = "PU";break;
					case "RD":		returnVal = "RD";break;
					case "SL":		returnVal = "SL";break;
					case "SP":		returnVal = "SP";break;
					case "TE":		returnVal = "TE";break;
					default:		returnVal = "";
				}
				return returnVal;
			}
			
			function getCustomerType(str)
			{
				var returnVal;
				switch(str)
				{
					case "A":		returnVal = "Academic";break;
					case "C":		returnVal = "CustomerService";break;
					case "G":		returnVal = "Government";break;
					case "M":		returnVal = "Media";break;
					case "R":		returnVal = "Regular";break;
					default:		returnVal = "";
				}
				return returnVal;
			}

		]]>
  </msxsl:script>
  <xsl:template name="UserDataResponse">
    <UserDataResponse>
      <xsl:copy-of select="Control"/>
      <xsl:copy-of select="/*/Status"/>
      <xsl:apply-templates select="ResultSet/Result"/>

    </UserDataResponse>
  </xsl:template>
  <xsl:variable name="eFM">
    <xsl:value-of select="ResultSet/Result/EMAIL_UPDS_REQ"/>
  </xsl:variable>
  <xsl:variable name="dF">
    <xsl:value-of select="ResultSet/Result/DNB_FLAG"/>
  </xsl:variable>
  <xsl:template match="ResultSet/Result">
    <xsl:apply-templates select="TITLE"/>
    <xsl:apply-templates select="ACCOUNT_ID"/>
    <xsl:apply-templates select="ADMIN_FLAG"/>
    <xsl:apply-templates select="FIRST_NAME"/>
    <xsl:apply-templates select="LAST_NAME"/>
    <xsl:apply-templates select="Suffix"/>
    <xsl:apply-templates select="JOB_TITLE"/>
    <xsl:apply-templates select="DeptDescription"/>
    <xsl:apply-templates select="COMPANY_NAME"/>
    <xsl:apply-templates select="ADDR1"/>
    <xsl:apply-templates select="ADDR2"/>
    <xsl:apply-templates select="CITY"/>
    <xsl:apply-templates select="City2"/>
    <xsl:apply-templates select="ZIP"/>
    <xsl:apply-templates select="PhoneCountryCode"/>
    <xsl:apply-templates select="TEL"/>
    <xsl:apply-templates select="EMAIL"/>
    <xsl:apply-templates select="SECURITY_WORD"/>
    <xsl:apply-templates select="EMAIL_UPDS_REQ"/>
    <xsl:apply-templates select="DNB_FLAG"/>
    <xsl:apply-templates select="DEPT_CODE"/>
    <xsl:apply-templates select="INDUSTRY_CODE"/>
    <xsl:apply-templates select="CUSTOMER_TYPE"/>
    <xsl:apply-templates select="isoCountryCode"/>
    <xsl:apply-templates select="STATE"/>
    <xsl:apply-templates select="THIRD_PARTY_ID"/>
    <xsl:apply-templates select="emailLogin"/>

    <!--<xsl:apply-templates select="BillingPackage"/>-->
    <!--<xsl:apply-templates select="COUNTRY_CODE"/>-->
    <!--<xsl:apply-templates select="FAX"/>-->
    <!--<xsl:apply-templates select="FaxCountryCode"/>-->
    <!--<xsl:apply-templates select="PLAN_ID"/>-->
    <!--<xsl:apply-templates select="RULE_SET"/>-->
    <!--<xsl:apply-templates select="USER_STATUS"/>-->
    <!--<xsl:apply-templates select="USER_TYPE"/>-->

  </xsl:template>
  <!-- Transformed Names -->
  <xsl:template match="ACCOUNT_ID">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">AccountID</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="ADMIN_FLAG">
    <xsl:call-template name="tagBoolean">
      <xsl:with-param name="newNodeName">IsAdministrator</xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="tagBoolean">
    <xsl:param name="newNodeName"/>
    <xsl:element name="{$newNodeName}">
      <xsl:choose>
        <xsl:when test="normalize-space(string(.))='Y'">true</xsl:when>
        <xsl:when test="normalize-space(string(.))='N'">false</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>

  <xsl:template match="ADDR1">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">Address1</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="ADDR2">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">Address2</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="CITY">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">City</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="COMPANY_NAME">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">CompanyName</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="CUSTOMER_TYPE">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <CustomerType>
        <xsl:value-of select="(user:getCustomerType(string(.)))"/>
      </CustomerType>
    </xsl:if>
  </xsl:template>
  <xsl:template match="City2">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">WardNameOrProvince</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="DEPT_CODE">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <DepartmentCategory>
        <xsl:value-of select="(user:getDepartmentCategory(string(.)))"/>
      </DepartmentCategory>
    </xsl:if>
  </xsl:template>
  <xsl:template match="DNB_FLAG">
    <xsl:call-template name="tagBoolean">
      <xsl:with-param name="newNodeName">DnbFlag</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="DeptDescription">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">DepartmentDescription</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="EMAIL">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">EmailAddress</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="EMAIL_UPDS_REQ">
    <xsl:call-template name="excludeFromMailings">
      <xsl:with-param name="newNodeName">ExcludeFromMailings</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="FAX">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">Fax</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="FIRST_NAME">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">FirstName</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="FaxCountryCode">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">FaxCountryCode</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="INDUSTRY_CODE">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">IndustryCode</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="JOB_TITLE">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">JobTitle</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="LAST_NAME">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">LastName</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="PhoneCountryCode">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">TelephoneCountryCode</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="SECURITY_WORD">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">SecurityWord</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="STATE">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">StateOrProvinceOrRegion</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="Suffix">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">Suffix</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="TEL">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">Telephone</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="THIRD_PARTY_ID">
    <xsl:call-template name="tagOptional">
      <xsl:with-param name="newNodeName">ThirdPartyID</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="TITLE">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <Title>
        <xsl:value-of select="(user:getTitle(string(.)))"/>
      </Title>
    </xsl:if>
  </xsl:template>
  <xsl:template match="USER_STATUS">
    <xsl:call-template name="userStatusReq">
      <xsl:with-param name="newNodeName">UserStatus</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template match="ZIP">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">ZipOrPostalCode</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  <xsl:template name="excludeFromMailings">
    <xsl:param name="newNodeName"/>
    <xsl:element name="{$newNodeName}">
      <xsl:choose>
        <xsl:when test="normalize-space(string(.))='Y'">true</xsl:when>
        <xsl:when test="normalize-space(string(.))='N'">false</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <xsl:template name="userStatusReq">
    <xsl:param name="newNodeName"/>
    <xsl:element name="{$newNodeName}">
      <xsl:choose>
        <xsl:when test="normalize-space(string(.))='A'">Active</xsl:when>
        <xsl:when test="normalize-space(string(.))='I'">Inactive</xsl:when>
        <xsl:when test="normalize-space(string(.))='T'">Terminate</xsl:when>
        <xsl:otherwise/>
      </xsl:choose>
    </xsl:element>
  </xsl:template>
  <xsl:template match="emailLogin">
    <EmailLogin>
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(.)) &gt; 0">
          <xsl:value-of select="."/>
        </xsl:when>
        <!--Empty EmailLogin Node-->
        <xsl:otherwise>D</xsl:otherwise>
      </xsl:choose>
    </EmailLogin>
  </xsl:template>
</xsl:stylesheet>