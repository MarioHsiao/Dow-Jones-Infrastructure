<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:user="user" extension-element-prefixes="msxsl"
                exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no" omit-xml-declaration="yes"/>
  <msxsl:script language="JScript" implements-prefix="user">
    <![CDATA[
		
		function CommaSeparatedToList(val,tag){
		var arr = val.split(",");
		var tempStr = "";
		for (var i=0; i<arr.length; i++) 
		{
			tempStr = tempStr + "<"+ tag +">" + arr[i] + "</" + tag + ">";
		}
		return tempStr;
	}
		]]>
  </msxsl:script>
  <xsl:param name="category"/>

  <xsl:template match="/*">
    <xsl:choose>
      <xsl:when test="$category='nocache'">
        <xsl:element name="GetUserAuthorizationsNoCacheResponse">
          <xsl:apply-templates select="/*/ResultSet/Result/AUTHORIZATION_LIST"/>
        </xsl:element>
      </xsl:when>
      <xsl:when test="$category='nosession'">
        <xsl:element name="GetUserAuthorizationsNoSessionResponse">
          <xsl:apply-templates select="/*/ResultSet/Result/AUTHORIZATION_LIST"/>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="GetUserAuthorizationsResponse">
          <xsl:apply-templates select="/*/ResultSet/Result/AUTHORIZATION_LIST"/>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="AUTHORIZATION_LIST">

    <accountId>
      <xsl:value-of select="ACCOUNT_ID"/>
    </accountId>
    <administratorFlag>
      <xsl:choose>
        <xsl:when test="ADMIN_FLAG = 'Y'">Y</xsl:when>
        <xsl:when test="ADMIN_FLAG = 'G'">G</xsl:when>
        <xsl:otherwise>N</xsl:otherwise>
      </xsl:choose>
    </administratorFlag>
    <productId>
      <xsl:value-of select="PRODUCT_ID"/>
    </productId>
    <planId>
      <xsl:value-of select="PLAN_ID"/>
    </planId>
    <ruleSet>
      <xsl:value-of select="RULE_SET"/>
    </ruleSet>
    <userId>
      <xsl:value-of select="USER_ID"/>
    </userId>
    <emailAddress>
      <xsl:value-of select="EMAIL_ADDRESS"/>
    </emailAddress>
    <userType>
      <xsl:choose>
        <xsl:when test="USER_TYPE='A'">A</xsl:when>
        <xsl:when test="USER_TYPE='B'">B</xsl:when>
        <xsl:when test="USER_TYPE='C'">C</xsl:when>
        <xsl:when test="USER_TYPE='I'">I</xsl:when>
        <xsl:when test="USER_TYPE='S'">S</xsl:when>
        <xsl:otherwise>Unspecified</xsl:otherwise>
      </xsl:choose>
    </userType>
    <userStatus>
      <xsl:choose>
        <xsl:when test="USER_STATUS='A'">A</xsl:when>
        <xsl:when test="USER_TYPE='I'">I</xsl:when>
        <xsl:when test="USER_TYPE='D'">D</xsl:when>
        <xsl:when test="USER_TYPE='S'">S</xsl:when>
        <xsl:otherwise>Unspecified</xsl:otherwise>
      </xsl:choose>
    </userStatus>
    <customerType>
      <xsl:choose>
        <xsl:when test="CUSTOMER_TYPE='A'">A</xsl:when>
        <xsl:when test="CUSTOMER_TYPE='B'">B</xsl:when>
        <xsl:when test="CUSTOMER_TYPE='C'">C</xsl:when>
        <xsl:when test="CUSTOMER_TYPE='I'">I</xsl:when>
        <xsl:when test="CUSTOMER_TYPE='S'">S</xsl:when>
        <xsl:when test="CUSTOMER_TYPE='G'">G</xsl:when>
        <xsl:when test="CUSTOMER_TYPE='M'">M</xsl:when>
        <xsl:when test="CUSTOMER_TYPE='R'">R</xsl:when>
        <xsl:otherwise>Unspecified</xsl:otherwise>
      </xsl:choose>
    </customerType>
    <countryCode>
      <xsl:value-of select="COUNTRY_CODE"/>
    </countryCode>
    <authorizationMatrix>
      <xsl:if test="boolean(AUTH_MATRIX/CDB)">
        <cdb>
          <xsl:apply-templates select="AUTH_MATRIX/CDB/*" />
        </cdb>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/CUSTOMERPORTAL)">
        <customerPortal>
          <xsl:apply-templates select="AUTH_MATRIX/CUSTOMERPORTAL/*" />
        </customerPortal>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/INSIGHT)">
        <insight>
          <xsl:apply-templates select="AUTH_MATRIX/INSIGHT/*" />
        </insight>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/MMONITOR)">
        <mMonitor>
          <xsl:apply-templates select="AUTH_MATRIX/MMONITOR/*" />
        </mMonitor>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/MRM)">
        <mrm>
          <xsl:apply-templates select="AUTH_MATRIX/MRM/*" />
        </mrm>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/PFA)">
        <pfa>
          <xsl:apply-templates select="AUTH_MATRIX/PFA/*" />
        </pfa>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/REALTIME)">
        <realtime>
          <xsl:apply-templates select="AUTH_MATRIX/REALTIME/*" />
        </realtime>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/SCREENING)">
        <screening>
          <xsl:apply-templates select="AUTH_MATRIX/SCREENING/*" />
        </screening>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/SNAPSHOT)">
        <snapshot>
          <xsl:apply-templates select="AUTH_MATRIX/SNAPSHOT/*" />
        </snapshot>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/ARCHIVE)">
        <archive>
          <xsl:apply-templates select="AUTH_MATRIX/ARCHIVE/*" />
        </archive>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/CIBS)">
        <cibs>
          <xsl:apply-templates select="AUTH_MATRIX/CIBS/*" />
        </cibs>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/TRACK)">
        <track>
          <xsl:apply-templates select="AUTH_MATRIX/TRACK/*" />
        </track>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/EMAIL)">
        <email>
          <xsl:apply-templates select="AUTH_MATRIX/EMAIL/*" />
        </email>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/INDEX)">
        <index>
          <xsl:apply-templates select="AUTH_MATRIX/INDEX/*" />
        </index>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/MDS)">
        <mds>
          <xsl:apply-templates select="AUTH_MATRIX/MDS/*" />
        </mds>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/MEMBERSHIP)">
        <membership>
          <xsl:apply-templates select="AUTH_MATRIX/MEMBERSHIP/*" />
        </membership>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/NDS)">
        <nds>
          <xsl:apply-templates select="AUTH_MATRIX/NDS/*" />
        </nds>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/SYMBOLOGY)">
        <symbology>
          <xsl:apply-templates select="AUTH_MATRIX/SYMBOLOGY/*" />
        </symbology>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/INTERFACE)">
        <interface>
          <xsl:apply-templates select="AUTH_MATRIX/INTERFACE/*" />
        </interface>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/UER)">
        <uer>
          <xsl:apply-templates select="AUTH_MATRIX/UER/*" />
        </uer>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/MIGRATION)">
        <migration>
          <xsl:apply-templates select="AUTH_MATRIX/MIGRATION/*" />
        </migration>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/REPMAN)">
        <repman>
          <xsl:apply-templates select="AUTH_MATRIX/REPMAN/*" />
        </repman>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/FSINTERFACE)">
        <fsinterface>
          <xsl:apply-templates select="AUTH_MATRIX/FSINTERFACE/*" />
        </fsinterface>
        <fsInterface>
          <xsl:apply-templates select="AUTH_MATRIX/FSINTERFACE/*" />
        </fsInterface>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/PMKTS)">
        <pmkts>
          <xsl:apply-templates select="AUTH_MATRIX/PMKTS/*" />
        </pmkts>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/CVD)">
        <cvd>
          <xsl:apply-templates select="AUTH_MATRIX/CVD/*" />
        </cvd>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/PAM)">
        <pam>
          <xsl:apply-templates select="AUTH_MATRIX/PAM/*" />
        </pam>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/HEATMAP)">
        <heatMap>
          <xsl:apply-templates select="AUTH_MATRIX/HEATMAP/*" />
        </heatMap>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/MADE)">
        <communicator>
          <xsl:apply-templates select="AUTH_MATRIX/MADE/*" />
        </communicator>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/OASEARCH)">
        <oaSearch>
          <xsl:apply-templates select="AUTH_MATRIX/OASEARCH/*" />
        </oaSearch>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/OPENACCESS)">
        <openAccess>
          <xsl:apply-templates select="AUTH_MATRIX/OPENACCESS/*" />
        </openAccess>
      </xsl:if>

      <xsl:for-each select="AUTH_MATRIX/*">
        <authMatrixService>
          <serviceName>
            <xsl:value-of select="name(.)"/>
          </serviceName>
          <xsl:apply-templates select="."/>
          <xsl:for-each select="./*">
            <nvp>
              <Key>
                <xsl:value-of select="name(.)"/>
              </Key>
              <Value>
                <xsl:value-of select="."/>
              </Value>
            </nvp>
          </xsl:for-each>
        </authMatrixService>
      </xsl:for-each>
    </authorizationMatrix>

    <emailLoginConversionAllowed>
      <xsl:choose>
        <xsl:when  test="lwrFlag='C' or lwrFlag='c'">C</xsl:when>
        <xsl:when  test="lwrFlag='L' or lwrFlag='l'">L</xsl:when>
        <xsl:otherwise>NotAllowed</xsl:otherwise>
      </xsl:choose>
    </emailLoginConversionAllowed>

    <lwrFlag>
      <xsl:value-of select="lwrFlag" />
    </lwrFlag>

    <emailLogin>
      <xsl:value-of select="emailLogin" />
    </emailLogin>

    <emailLoginState>
      <xsl:choose>
        <xsl:when  test="emailLogin='E' or emailLogin='e'">E</xsl:when>
        <xsl:when  test="emailLogin='D' or emailLogin='d'">D</xsl:when>
        <xsl:when  test="emailLogin='P' or emailLogin='p'">P</xsl:when>
        <xsl:otherwise>Unspecified</xsl:otherwise>
      </xsl:choose>
    </emailLoginState>

    <externalReaderFlag>
      <xsl:choose>
        <xsl:when test="erFlag='Y'">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </externalReaderFlag>

    <idleTimeout>
      <xsl:value-of select="IDLE_TIMEOUT"/>
    </idleTimeout>

    <maxSessions>
      <xsl:value-of select="MAX_SESSIONS"/>
    </maxSessions>

    <usedSessions>
      <xsl:value-of select="USED_SESSIONS"/>
    </usedSessions>
    
    <clientBillingAllow>
      <xsl:choose>
        <xsl:when test="ClientBillingAllowed='Y'">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </clientBillingAllow>
    <xsl:apply-templates select="UUID"></xsl:apply-templates>
  <xsl:apply-templates select="EID"></xsl:apply-templates>
  </xsl:template>

  <xsl:template match="UUID">
    <xsl:element name="UUID" >
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  <xsl:template match="EID">
    <xsl:element name="EID" >
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
  
  <xsl:template match="AC1">
    <ac1>
      <xsl:value-of select="." />
    </ac1>
  </xsl:template>
  <xsl:template match="AC2">
    <ac2>
      <xsl:value-of select="." />
    </ac2>
  </xsl:template>
  <xsl:template match="AC3">
    <ac3>
      <xsl:value-of select="." />
    </ac3>
  </xsl:template>
  <xsl:template match="AC4">
    <ac4>
      <xsl:value-of select="." />
    </ac4>
  </xsl:template>
  <xsl:template match="AC5">
    <ac5>
      <xsl:value-of select="." />
    </ac5>
  </xsl:template>
  <xsl:template match="AC6">
    <ac6>
      <xsl:value-of select="." />
    </ac6>
  </xsl:template>
  <xsl:template match="AC7">
    <ac7>
      <xsl:value-of select="." />
    </ac7>
  </xsl:template>
  <xsl:template match="AC8">
    <ac8>
      <xsl:value-of select="." />
    </ac8>
  </xsl:template>
  <xsl:template match="AC9">
    <ac9>
      <xsl:value-of select="." />
    </ac9>
  </xsl:template>
  <xsl:template match="AC10">
    <ac10>
      <xsl:value-of select="." />
    </ac10>
  </xsl:template>
  <xsl:template match="Da1">
    <da1>
      <xsl:value-of select="." />
    </da1>
  </xsl:template>
  <xsl:template match="Da2">
    <da2>
      <xsl:value-of select="." />
    </da2>
  </xsl:template>
  <xsl:template match="Da3">
    <da3>
      <xsl:value-of select="." />
    </da3>
  </xsl:template>
  <xsl:template match="Da4">
    <da4>
      <xsl:value-of select="." />
    </da4>
  </xsl:template>
  <xsl:template match="Da5">
    <da5>
      <xsl:value-of select="." />
    </da5>
  </xsl:template>
  <xsl:template match="Da6">
    <da6>
      <xsl:value-of select="." />
    </da6>
  </xsl:template>
  <xsl:template match="Da7">
    <da7>
      <xsl:value-of select="." />
    </da7>
  </xsl:template>
  <xsl:template match="Da8">
    <da8>
      <xsl:value-of select="." />
    </da8>
  </xsl:template>
  <xsl:template match="Da9">
    <da9>
      <xsl:value-of select="." />
    </da9>
  </xsl:template>

  <xsl:template match="DA1">
    <da1>
      <xsl:value-of select="." />
    </da1>
  </xsl:template>
  <xsl:template match="DA2">
    <da2>
      <xsl:value-of select="." />
    </da2>
  </xsl:template>
  <xsl:template match="DA3">
    <da3>
      <xsl:value-of select="." />
    </da3>
  </xsl:template>
  <xsl:template match="DA4">
    <da4>
      <xsl:value-of select="." />
    </da4>
  </xsl:template>
  <xsl:template match="DA5">
    <da5>
      <xsl:value-of select="." />
    </da5>
  </xsl:template>
  <xsl:template match="DA6">
    <da6>
      <xsl:value-of select="." />
    </da6>
  </xsl:template>
  <xsl:template match="DA7">
    <da7>
      <xsl:value-of select="." />
    </da7>
  </xsl:template>
  <xsl:template match="DA8">
    <da8>
      <xsl:value-of select="." />
    </da8>
  </xsl:template>
  <xsl:template match="DA9">
    <da9>
      <xsl:value-of select="." />
    </da9>
  </xsl:template>
  <xsl:template match="DAE">
    <DAE>
      <xsl:value-of select="." />
    </DAE>
  </xsl:template>
  <xsl:template match="FTODE">
    <FTODE>
      <xsl:value-of select="." />
    </FTODE>
  </xsl:template>
  <xsl:template match="DAEDITGRP">
    <xsl:value-of disable-output-escaping="yes" select="user:CommaSeparatedToList(normalize-space(string(.)),'DAEDITGroup')"/>
  </xsl:template>
  <xsl:template match="GripDefault">
    <gripDefault>
      <xsl:value-of select="." />
    </gripDefault>
  </xsl:template>
  <xsl:template match="GripAdmin">
    <gripAdmin>
      <xsl:value-of select="." />
    </gripAdmin>
  </xsl:template>

  <xsl:template match="COMPANY">
    <company>
      <xsl:value-of select="." />
    </company>
  </xsl:template>
  <xsl:template match="REGION">
    <region>
      <xsl:value-of select="." />
    </region>
  </xsl:template>
  <xsl:template match="INDUSTRY">
    <industry>
      <xsl:value-of select="." />
    </industry>
  </xsl:template>
  <xsl:template match="DEPT">
    <department>
      <xsl:value-of select="." />
    </department>
  </xsl:template>

  <xsl:template match="DBID">
    <dbId>
      <xsl:value-of select="." />
    </dbId>
  </xsl:template>
  <xsl:template match="PASSWORD">
    <password>
      <xsl:value-of select="." />
    </password>
  </xsl:template>
  <xsl:template match="USERID">
    <userId>
      <xsl:value-of select="." />
    </userId>
  </xsl:template>

  <xsl:template match="PERSONALIZATION">
    <personalization>
      <xsl:value-of select="." />
    </personalization>
  </xsl:template>

  <xsl:template match="SHARING">
    <sharingDA>
      <xsl:value-of select="." />
    </sharingDA>
  </xsl:template>

  <xsl:template match="MMEDIA">
    <multiMediaDA>
      <xsl:value-of select="." />
    </multiMediaDA>
  </xsl:template>

  <xsl:template match="NewsletterDA">
    <newsletterDA>
      <xsl:value-of select="." />
    </newsletterDA>
  </xsl:template>

  <xsl:template match="SHAREPOINT">
    <allowSharePointWidget>
      <xsl:value-of select="." />
    </allowSharePointWidget>
  </xsl:template>

  <xsl:template match="ADS">
    <projectVisibleAds>
      <xsl:value-of select="." />
    </projectVisibleAds>
  </xsl:template>

  <xsl:template match="CVD">
    <xsl:choose>
      <xsl:when test="local-name(parent::*)='CVD'">
        <xsl:value-of disable-output-escaping="yes" select="user:CommaSeparatedToList(normalize-space(string(.)),'CVD')"/>
      </xsl:when>
      <xsl:otherwise>
        <CVD>
          <xsl:choose>
            <xsl:when test="translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'on'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </CVD>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="CVIM">
    <CVIM>
      <xsl:value-of select="." />
    </CVIM>
  </xsl:template>
  <xsl:template match="UIERRORS">
    <UIErrors>
      <xsl:value-of select="." />
    </UIErrors>
  </xsl:template>
  <xsl:template match="USGREPORT">
    <UsageReport>
      <xsl:value-of select="." />
    </UsageReport>
  </xsl:template>

  <xsl:template match="MYFACTIVA">
    <IsMyDJFactivaEnabled>
      <xsl:choose>
        <xsl:when test="translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'off'">false</xsl:when>
        <xsl:otherwise>true</xsl:otherwise>
      </xsl:choose>
    </IsMyDJFactivaEnabled>
  </xsl:template>

  <xsl:template match="RCENTER">
    <rCenter>
      <xsl:value-of select="."/>
    </rCenter>
  </xsl:template>

  <xsl:template match="MCEMAIL">
    <mcemail>
      <xsl:value-of select="."/>
    </mcemail>
  </xsl:template>

  <xsl:template match="SEARCHASSIST">
    <searchAssist>
      <xsl:value-of select="."/>
    </searchAssist>
  </xsl:template>

  <xsl:template match="PageMonitor">
    <pageMonitor>
      <xsl:value-of select="."/>
    </pageMonitor>
  </xsl:template>

  <xsl:template match="Baynote">
    <bayNote>
      <xsl:value-of select="."/>
    </bayNote>
  </xsl:template>

  <xsl:template match="BLOGDA">
    <blogDa>
      <xsl:value-of select="."/>
    </blogDa>
  </xsl:template>

  <xsl:template match="DULINK">
    <duLinkBuilder>
      <xsl:value-of select="."/>
    </duLinkBuilder>
  </xsl:template>

  <xsl:template match="SubDomain">
    <subDomain>
      <xsl:value-of select="."/>
    </subDomain>
  </xsl:template>

  <xsl:template match="DULINK">
    <duLink>
      <xsl:value-of select="."/>
    </duLink>
  </xsl:template>

  <xsl:template match="SS_PROMOTION_POPUP">
    <ssPromotionPopup>
      <xsl:value-of select="."/>
    </ssPromotionPopup>
  </xsl:template>

  <xsl:template match="EXECDQTEST">
    <execDQTest>
      <xsl:value-of select="."/>
    </execDQTest>
  </xsl:template>


  <xsl:template match="TRANSLATE">
    <IsTranslateDAEnabled>
      <xsl:choose>
        <xsl:when test="normalize-space(.) = 'OFF' ">false</xsl:when>
        <xsl:otherwise>true</xsl:otherwise>
      </xsl:choose>
    </IsTranslateDAEnabled>
  </xsl:template>

  <xsl:template match="AUTHADMIN">
    <authAdmin>
      <xsl:value-of select="."/>
    </authAdmin>
  </xsl:template>

  <xsl:template match="CONNECTME">
    <connectMe>
      <xsl:value-of select="."/>
    </connectMe>
  </xsl:template>

  <!--<xsl:template match="RCENTER">
    <rCenter>
      <xsl:choose>
        <xsl:when test="translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'on'">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </rCenter>
  </xsl:template>

  <xsl:template match="MCEMAIL">
    <searchAssist>
      <xsl:choose>
        <xsl:when test="translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'on'">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </searchAssist>
  </xsl:template>

  <xsl:template match="SEARCHASSIST">
    <mcemail>
      <xsl:choose>
        <xsl:when test="translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'on'">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </mcemail>
  </xsl:template>-->

</xsl:stylesheet>