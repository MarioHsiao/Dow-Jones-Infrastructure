<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:template match="/*">
		<GetUserAuthorizationsNoSessionResponse>
			<xsl:apply-templates select="/*/ResultSet/Result/AUTHORIZATION_LIST"/>
		</GetUserAuthorizationsNoSessionResponse>
	</xsl:template>

	<xsl:template match="AUTHORIZATION_LIST">
	
		<accountId>
			<xsl:value-of select="ACCOUNT_ID"/>
		</accountId>

		<xsl:choose>
			<xsl:when test="string-length(normalize-space(ADMIN_FLAG)) &gt; 0">
				<administratorFlag>
					<xsl:value-of select="ADMIN_FLAG"/>
				</administratorFlag>
			</xsl:when>
			<xsl:otherwise></xsl:otherwise>
		</xsl:choose>
		
		<productId>
			<xsl:value-of select="PRODUCT_ID"/>
		</productId>
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
			<xsl:value-of select="USER_TYPE"/>
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

		<emailLoginConversionAllowed>
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(lwrFlag))">
          <xsl:value-of select="lwrFlag"/>
        </xsl:when>
        <xsl:otherwise>NotAllowed</xsl:otherwise>
      </xsl:choose>
    </emailLoginConversionAllowed>

		<emailLoginState>
			<xsl:value-of select="emailLogin"/>
		</emailLoginState>
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

  <xsl:template match="NewsletterDA">
    <isAllowedToSetTtimeToLiveProxyCredentials>
      <xsl:choose>
        <xsl:when test="normalize-space(.) = 'TTLT' ">true</xsl:when>
        <xsl:when test="normalize-space(.) = 'TTLTOVER' ">true</xsl:when>
        <xsl:otherwise>false</xsl:otherwise>
      </xsl:choose>
    </isAllowedToSetTtimeToLiveProxyCredentials>
  </xsl:template>

</xsl:stylesheet>