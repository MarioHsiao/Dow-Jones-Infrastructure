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

  <xsl:template match="Result">

    <accountID>
      <xsl:value-of select="ACCOUNT_ID"/>
    </accountID>
    <administratorFlag>
      <xsl:choose>
        <xsl:when test="ADMIN_FLAG = 'Y'">Y</xsl:when>
        <xsl:when test="ADMIN_FLAG = 'G'">G</xsl:when>
        <xsl:otherwise>N</xsl:otherwise>
      </xsl:choose>
    </administratorFlag>
    <productID>
      <xsl:value-of select="PRODUCT_ID"/>
    </productID>
    <ruleSet>
      <xsl:value-of select="RULE_SET"/>
    </ruleSet>
    <userID>
      <xsl:value-of select="USER_ID"/>
    </userID>
   
    
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

    <authorizationMatrix>
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
      <xsl:if test="boolean(AUTH_MATRIX/FSINTERFACE)">
        <fsInterface>
          <xsl:apply-templates select="AUTH_MATRIX/FSINTERFACE/*" />
        </fsInterface>
      </xsl:if>
      <xsl:if test="boolean(AUTH_MATRIX/SESSION)">
        <session>
          <xsl:apply-templates select="AUTH_MATRIX/SESSION/*" />
        </session>
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
    </authorizationMatrix>

    <xsl:apply-templates select="CC_REDIRECT"/>
    <xsl:apply-templates select="CC_SEVERITY_FLAG"/>
    <xsl:apply-templates select="CLIENT_CODE_INFO"/>
    <xsl:apply-templates select="COUNTRY_CODE"/>
    <xsl:apply-templates select="isoCountryCode"/>
    <xsl:apply-templates select="CUSTOMER_TYPE"/>
    <xsl:apply-templates select="IDLE_TIMEOUT"/>
    <xsl:apply-templates select="LOGIN_DATE"/>
    <xsl:apply-templates select="MAX_SESSIONS"/>
    <xsl:apply-templates select="REDIRECT"/>
    <xsl:apply-templates select="TOKEN_USER_ID"/>
    <xsl:apply-templates select="TOKEN_EMAIL"/>
    <xsl:apply-templates select="TroubleLoggingInRedirect"/>
    <xsl:apply-templates select="USER_STATUS"/>
    <xsl:apply-templates select="USER_TYPE"/>
    <xsl:apply-templates select="USER_SESSIONS"/>
    <xsl:apply-templates select="WSJ_TYPE"/>
    <xsl:call-template name="DEFAULT_PREFERENCE_ITEMS"/>
    <xsl:apply-templates select="lwrFlag"/>
    <xsl:apply-templates select="emailLogin"/>
    <xsl:apply-templates select="erFlag"/>
    <xsl:apply-templates select="SHNS"/>
    <xsl:apply-templates select="sso"/>
    <xsl:apply-templates select="FIRST_NAME"/>
    <xsl:apply-templates select="LAST_NAME"/>

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

  <xsl:template match="INSIDER">
    <insider>
      <xsl:value-of select="." />
    </insider>
  </xsl:template>
  <!--
	<xsl:template match="PODCASTPOC">
		<podcastPOCDA>
			<xsl:value-of select="." />
		</podcastPOCDA>
	</xsl:template>
	-->
  <xsl:template match="NTTXTRACTIONPOC">
    <nttExtractionPOC>
      <xsl:value-of select="." />
    </nttExtractionPOC>
  </xsl:template>
  <xsl:template match="ADS">
    <projectVisibleAds>
      <xsl:value-of select="." />
    </projectVisibleAds>
  </xsl:template>

  <xsl:template match="NewsletterDA">
    <isAllowedToSetTtimeToLiveProxyCredentials>
      <xsl:choose>
        <xsl:when test="normalize-space(.) = 'TTLT' ">true</xsl:when>
        <xsl:when test="normalize-space(.) = 'TTLTOVER' ">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </isAllowedToSetTtimeToLiveProxyCredentials>
  </xsl:template>

  <xsl:template match="SHAREPOINT">
    <allowSharePointWidget>
      <xsl:choose>
        <xsl:when test="normalize-space(.) = 'ON' ">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </allowSharePointWidget>
  </xsl:template>

  <xsl:template match="lwrFlag">
    <emailLoginConversionAllowed>
      <xsl:choose>
        <xsl:when  test=".='C' or .='c'">C</xsl:when>
        <xsl:when  test=".='L' or .='l'">L</xsl:when>
        <xsl:otherwise>NotAllowed</xsl:otherwise>
      </xsl:choose>
    </emailLoginConversionAllowed>
  </xsl:template>

  <xsl:template match="emailLogin">
    <emailLoginState>
      <xsl:choose>
        <xsl:when  test=".='E' or .='e'">E</xsl:when>
        <xsl:when  test=".='D' or .='d'">D</xsl:when>
        <xsl:when  test=".='P' or .='p'">P</xsl:when>
        <xsl:otherwise>Unspecified</xsl:otherwise>
      </xsl:choose>
    </emailLoginState>
  </xsl:template>

  <xsl:template match="erFlag">
    <externalReaderFlag>
      <xsl:choose>
        <xsl:when test=".='Y'">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </externalReaderFlag>
  </xsl:template>

  <xsl:template match="SHNS">
    <showNamespace>
      <xsl:choose>
        <xsl:when  test=".='Y' or .='y'">Y</xsl:when>
        <xsl:when  test=".='N' or .='n'">N</xsl:when>
        <xsl:otherwise>Unspecified</xsl:otherwise>
      </xsl:choose>
    </showNamespace>
  </xsl:template>
  
  <xsl:template match="CC_REDIRECT">
    <clientCodeRedirect>
      <xsl:value-of select="."/>
    </clientCodeRedirect>
  </xsl:template>

  <xsl:template match="CC_SEVERITY_FLAG">
    <clientCodeSeverityFlag>
      <xsl:value-of select="."/>
    </clientCodeSeverityFlag>
  </xsl:template>

  <xsl:template match="CLIENT_CODE_INFO">
    <clientCodeInfo>
      <xsl:apply-templates/>
    </clientCodeInfo>
  </xsl:template>
  
  <xsl:template match="CC_1">
    <clientCode1Definition>
      <xsl:apply-templates/>
    </clientCode1Definition>
  </xsl:template>

  <xsl:template match="CC_2">
    <clientCode2Definition>
      <xsl:apply-templates/>
    </clientCode2Definition>
  </xsl:template>

  <xsl:template match="CC_3">
    <clientCode3Definition>
      <xsl:apply-templates/>
    </clientCode3Definition>
  </xsl:template>

  <xsl:template match="CC_4">
    <clientCode4Definition>
      <xsl:apply-templates/>
    </clientCode4Definition>
  </xsl:template>

  <xsl:template match="CC_5">
    <clientCode5Definition>
      <xsl:apply-templates/>
    </clientCode5Definition>
  </xsl:template>

  <xsl:template match="CC_PROMPT">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">prompt</xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="CC_VALIDATION_LIST">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">validationList</xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="CC_SEVERITY_FLAG">
    <type>
      <xsl:call-template name="CC_SEVERITY_FLAG_LEVEL"/>
    </type>
  </xsl:template>

  <xsl:template match="CC_COUNT">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">count</xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="CC_DELIMITER">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">delimiter</xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="CC_DESCRIPTION">
    <xsl:call-template name="tagRequired">
      <xsl:with-param name="newNodeName">description</xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="CC_LEVEL">
    <minimumTypeRequired>
      <xsl:call-template name="CC_SEVERITY_FLAG_LEVEL"/>
    </minimumTypeRequired>
  </xsl:template>

  <xsl:template name="CC_SEVERITY_FLAG_LEVEL">
    <xsl:choose>
      <xsl:when test="normalize-space(string(.))='R' or normalize-space(string(.))='M'">M</xsl:when>
      <xsl:when test="normalize-space(string(.))='O' or normalize-space(string(.))='VO'">VO</xsl:when>
      <xsl:when test="normalize-space(string(.))='V' or normalize-space(string(.))='VA'">VA</xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="isoCountryCode">
    <!--Since Yugoslavia (YU) is seperated to Serbia (RS) and Montenegro (ME)-->
    <!--for the backward compatible, we response YU as the countryCode when we get RS or ME-->
    <xsl:choose>
      <xsl:when test="string-length(normalize-space(.)) &gt; 0">
        <xsl:choose>
          <xsl:when test="normalize-space(.)='RS' or normalize-space(.)='ME'">
            <xsl:element name="isoCountryCode">YU</xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <xsl:element name="isoCountryCode">
              <xsl:value-of select="normalize-space(string(.))"/>
            </xsl:element>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="COUNTRY_CODE">
    <countryCode>
      <xsl:value-of select="."/>
    </countryCode>
  </xsl:template>

  <xsl:template match="CUSTOMER_TYPE">
    <customerType>
      <xsl:value-of select="."/>
    </customerType>
  </xsl:template>

  <xsl:template match="IDLE_TIMEOUT">
    <idleTimeout>
      <xsl:value-of select="."/>
    </idleTimeout>
  </xsl:template>

  <xsl:template match="LOGIN_DATE">
    <loginDate>
      <xsl:value-of select="."/>
    </loginDate>
  </xsl:template>

  <xsl:template match="MAX_SESSIONS">
    <maxSessions>
      <xsl:value-of select="."/>
    </maxSessions>
  </xsl:template>

  <xsl:template match="REDIRECT">
    <redirect>
      <xsl:value-of select="."/>
    </redirect>
  </xsl:template>

  <xsl:template match="TOKEN_USER_ID">
    <tokenUserID>
      <xsl:value-of select="."/>
    </tokenUserID>
  </xsl:template>

  <xsl:template match="TOKEN_EMAIL">
    <tokenEmail>
      <xsl:value-of select="."/>
    </tokenEmail>
  </xsl:template>

  <xsl:template match="TroubleLoggingInRedirect">
    <troubleLoggingInRedirect>
      <xsl:value-of select="."/>
    </troubleLoggingInRedirect>
  </xsl:template>

  <xsl:template match="USER_STATUS">
    <userStatus>
      <xsl:value-of select="."/>
    </userStatus>
  </xsl:template>

  <xsl:template match="USER_TYPE">
    <userType>
      <xsl:value-of select="."/>
    </userType>
  </xsl:template>

  <xsl:template match="USED_SESSIONS">
    <usedSessions>
      <xsl:value-of select="."/>
    </usedSessions>
  </xsl:template>
  <xsl:template match="sso">
    <sso>
      <xsl:value-of select="."/>
    </sso>
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

  <xsl:template match="WSJ_TYPE">
    <wsjType>
      <xsl:choose>
        <xsl:when  test=".='S'">S</xsl:when>
        <xsl:when  test=".='B'">B</xsl:when>
        <xsl:otherwise>Unspecified</xsl:otherwise>
      </xsl:choose>
    </wsjType>

    <isAllowedWsjAccess>
      <xsl:choose>
        <xsl:when test=".='S' or .='B'">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </isAllowedWsjAccess>
  </xsl:template>

  <xsl:template name="DEFAULT_PREFERENCE_ITEMS">
    
    <xsl:for-each select="DEFAULT_PREFERENCES">
      <defaultPreferences>
        
        <groupName>
          <xsl:value-of select="//GROUP_NAME"/>
        </groupName>

        <itemBlob>
          <xsl:value-of select="//ITEM_BLOB"/>
        </itemBlob>

        <itemClass>
          <xsl:value-of select="//ITEM_CLASS"/>
        </itemClass>

        <itemId>
          <xsl:value-of select="//ITEM_ID"/>
        </itemId>

        <itemInstanceName>
          <xsl:value-of select="//ITEM_INSTANCE_NAME"/>
        </itemInstanceName>

        <itemIsSubscribed>
          <xsl:choose>
            <xsl:when test="//ITEM_SUBSCRIBE='1'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </itemIsSubscribed>
        
      </defaultPreferences>
    </xsl:for-each>
    
  </xsl:template>
  
  <!--=====COMMON TEMPLATES=====-->
  <xsl:template name="tagOptional">
    <xsl:param name="newNodeName"></xsl:param>
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <xsl:element name="{$newNodeName}">
        <xsl:value-of select="normalize-space(string(.))"/>
      </xsl:element>
    </xsl:if>
  </xsl:template>

  <xsl:template name="tagRequired">
    <xsl:param name="newNodeName"></xsl:param>
    <xsl:choose>
      <xsl:when test="string-length(normalize-space(.)) &gt; 0">
        <xsl:element name="{$newNodeName}">
          <xsl:value-of select="normalize-space(string(.))"/>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="{$newNodeName}"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--=====END COMMON TEMPLATES=====-->
  
</xsl:stylesheet>
