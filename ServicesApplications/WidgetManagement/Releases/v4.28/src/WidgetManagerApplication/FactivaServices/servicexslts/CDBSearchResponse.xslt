<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl user" exclude-result-prefixes="user">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="../xslt/commonDistDocElements.xslt"/>
	<msxsl:script language="JScript" implements-prefix="user"><![CDATA[
		function ChangeDateFormat(DateVal)
		{
			var RetVal = "";
			RetVal = DateVal.substr(0,4)+"-"+DateVal.substr(4,2)+"-"+DateVal.substr(6,2);
			return RetVal;
		}
		function toLowerCase(x)
		{
			return x.toLowerCase();
		}
		]]></msxsl:script>
	<xsl:template match="//Control">
		<xsl:if test="position()=1">
			<xsl:copy-of select="."/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="//Status">
		<xsl:if test="position()=1">
			<xsl:copy-of select="."/>
		</xsl:if>
	</xsl:template>


	<xsl:template match="//ContContextString" >
		<xsl:if test="position()=1">
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(.)) &gt; 0"><searchContext><xsl:value-of select="normalize-space(.)"/></searchContext></xsl:when>
			<xsl:otherwise><searchContext/></xsl:otherwise>
		</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template match="*" mode="ReplyItem">
		<xsl:choose>
			<xsl:when test="(local-name()='IPPubInfo')">
				<xsl:apply-templates select="./*"/>
			</xsl:when>
			<xsl:when test="(local-name()='DocData')">
				<xsl:apply-templates select="./*"/>
			</xsl:when>
			<xsl:when test="(local-name()='BillingDist ')">
				<xsl:apply-templates select="./*"/>
			</xsl:when>
			<xsl:when test="(local-name()='Indexing')">
				<xsl:apply-templates select="./*"/>
			</xsl:when>
			<xsl:when test="(local-name()='GroupInfo')">
				<xsl:apply-templates select="./*"/>
				<xsl:call-template name="CreateParentGroupCode"/>
				<xsl:call-template name="nonPublicationDays"/>
				<xsl:if test="count(./Desc)!=0"><xsl:call-template name="description"/></xsl:if>
				<xsl:if test="count(./DescNL)!=0"><xsl:call-template name="descriptionNL"/></xsl:if>
			</xsl:when>
			<xsl:when test="(local-name()='SrcInfo')">
				<xsl:if test="count(Groups/Code)!=0">
					<groupCodes><xsl:call-template name="groupCodes"/></groupCodes>
				</xsl:if>
				<xsl:apply-templates select="./*"/>
				<xsl:call-template name="nonPublicationDays"/>
				<xsl:if test="count(./Desc)!=0"><xsl:call-template name="description"/></xsl:if>
				<xsl:if test="count(./DescNL)!=0"><xsl:call-template name="descriptionNL"/></xsl:if>
			</xsl:when>
			<xsl:when test="(local-name()='DataFormat')">
				<xsl:call-template name="publicationFormats"/>
			</xsl:when>
			<xsl:otherwise>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="IPPubInfo/*">
		<xsl:choose>
			<xsl:when test="(local-name()='Circulation') and string-length(normalize-space(@v)) &gt; 0">
				<circulation><xsl:value-of select="normalize-space(@v)"/></circulation>
			</xsl:when>
			<xsl:when test="(local-name()='CircSource') and string-length(normalize-space(@v)) &gt; 0">
				<circulationSource><xsl:value-of select="normalize-space(.)"/></circulationSource>
			</xsl:when>
			<xsl:when test="(local-name()='CircDate') and string-length(normalize-space(@v)) &gt; 0">
				<circulationDate><xsl:value-of select="user:ChangeDateFormat(string(normalize-space(@v)))"/></circulationDate>
			</xsl:when>
			<xsl:when test="(local-name()='Copyright') and string-length(normalize-space(@v)) &gt; 0">
				<copyright><xsl:value-of select="normalize-space(.)"/></copyright>
			</xsl:when>
			<xsl:when test="(local-name()='Logo')">
				<xsl:if test="string-length(normalize-space(@src)) &gt; 0 or string-length(normalize-space(@img)) &gt; 0">
				<publisherLogo>
					<xsl:attribute name="source"><xsl:value-of select="normalize-space(@src)"/></xsl:attribute>
					<xsl:attribute name="image"><xsl:value-of select="normalize-space(@img)"/></xsl:attribute>
				</publisherLogo>
				</xsl:if>
			</xsl:when>
			<xsl:when test="(local-name()='Publisher')">
				<xsl:if test="string-length(normalize-space(Code/@v)) &gt; 0">
					<publisherCode><xsl:value-of select="normalize-space(Code/@v)"/></publisherCode>
				</xsl:if>
				<xsl:if test="string-length(normalize-space(Name)) &gt; 0">
					<publisherName><xsl:value-of select="normalize-space(Name)"/></publisherName>
				</xsl:if>
				<xsl:if test="string-length(normalize-space(URL)) &gt; 0">
					<publisherWebAddress><xsl:value-of select="normalize-space(URL)"/></publisherWebAddress>
				</xsl:if>
				<xsl:if test="string-length(normalize-space(./PromoTxt)) &gt; 0">
					<publisherPromotion>
						<xsl:apply-templates select="./PromoTxt/Para"/>
					</publisherPromotion>
				</xsl:if>
			</xsl:when>
		</xsl:choose>
	</xsl:template>


	<xsl:template match="Para">
		<paragraph><xsl:call-template name="commonDistDocElements"><xsl:with-param name="processELink">true</xsl:with-param></xsl:call-template></paragraph>
	</xsl:template>
	<xsl:template name="publicationFormats">
		<publicationFormats>
			<xsl:apply-templates select="."/>
		</publicationFormats>
	</xsl:template>
<!--	<xsl:template match="DataFormat">
		<publicationFormat><xsl:value-of select="."/></publicationFormat>
	</xsl:template>
	-->
	
	<xsl:template match="DocData/*">
	</xsl:template>

	<xsl:template name="applicableContentCategories">
		<xsl:if test="count(./*/GroupInfo/GroupApplicFMT)!=0">
			<applicableContentCategories>
				<xsl:apply-templates select="./*//GroupApplicFMT/Code"/>
			</applicableContentCategories>
		</xsl:if>
	</xsl:template>

	<xsl:template match="//GroupApplicFMT/Code">
		<xsl:if test="normalize-space(@v)='article' or 'file' or 'report'">
			<xsl:if test="position()=1">
				<contentCategory>Publications</contentCategory>
			</xsl:if>	
		</xsl:if>	
		<xsl:if test="@v='picture'">
			<contentCategory>Pictures</contentCategory>
		</xsl:if>	
		<xsl:if test="@v='webpage'">
			<contentCategory>WebSites</contentCategory>
		</xsl:if>	
		<!-- SM- reports not a valid contentcategory for CDB response...
		<xsl:if test="@v='inventory'">
			<contentCategory>Reports</contentCategory>
		</xsl:if>	
		-->
		
	</xsl:template>
	<xsl:template match="BillingDist/*">
	
	</xsl:template>

	<xsl:template match="Indexing/*">
	</xsl:template>

		<xsl:template name="CreateParentGroupCode">
		<xsl:if test="string-length(normalize-space(./Parent[position() = 1]/Code/@v)) &gt; 0"><parentGroupCode><xsl:value-of select="normalize-space(./Parent[position() = 1]/Code/@v)"/></parentGroupCode></xsl:if>
	</xsl:template>

	
	<xsl:template match="GroupInfo/*">
		<xsl:choose>
			<xsl:when test="(local-name()='Code')">
				<groupCode><xsl:value-of select="@v"/></groupCode>
			</xsl:when> 
			<xsl:when test="(local-name()='GroupName')">
				<xsl:if test="string-length(normalize-space(.)) &gt; 0"><groupName><xsl:value-of select="normalize-space(.)"/></groupName></xsl:if>
			</xsl:when>
		
			<xsl:when test="(local-name()='SortName')">
				<xsl:if test="string-length(normalize-space(.)) &gt; 0"><sortName><xsl:value-of select="normalize-space(.)"/></sortName></xsl:if>
			</xsl:when>
			<xsl:when test="(local-name()='Status')">
				<xsl:if test="normalize-space(@v)='active'">
					<availabilityStatus>Active</availabilityStatus>
				</xsl:if>
				<xsl:if test="normalize-space(@v)='discont'">
					<availabilityStatus>Discountiued</availabilityStatus>
				</xsl:if>
				<xsl:if test="normalize-space(@v)='deleted'">
					<availabilityStatus>Deleted</availabilityStatus>
				</xsl:if>
				<xsl:if test="normalize-space(@v)='pending'">
					<availabilityStatus>Pending</availabilityStatus>
				</xsl:if>
			</xsl:when>
			<xsl:when test="(local-name()='NumOfChildren')">
				<xsl:if test="string-length(normalize-space(@v)) &gt; 0"><numberOfChildren><xsl:value-of select="@v"/></numberOfChildren></xsl:if>
			</xsl:when>
			<!--
			<xsl:when test="(local-name()='Parent')">
				<xsl:if test="string-length(normalize-space(Code/@v)) &gt; 0"><parentGroupCode><xsl:value-of select="normalize-space(Code/@v)"/></parentGroupCode></xsl:if>
			</xsl:when>
			-->
			<xsl:when test="(local-name()='ChildType')">
				<xsl:if test="normalize-space(@v)='source'">
				<childrenTypes>Source</childrenTypes>
				</xsl:if>
				<xsl:if test="normalize-space(@v)='group'">
				<childrenTypes>Group</childrenTypes>
				</xsl:if>
				<xsl:if test="normalize-space(@v)='mixed'">
				<childrenTypes>Mixed</childrenTypes>
				</xsl:if>								
			</xsl:when>
		</xsl:choose>	
	</xsl:template>

	<xsl:template match="SrcInfo/*">
		<xsl:choose>
			<xsl:when test="(local-name()='Lang')">
			<!--
				SM -bug from Matt 1/26/06 fixed 
				Not sure why it is there in the first place. 
				removing this fixes the bug with no lang coming up for source txs-->
			<!--<xsl:if test="position()=1">-->
			<xsl:if test="string-length(normalize-space(@v)) &gt; 0">
<baseLanguage><xsl:value-of select="user:toLowerCase(string(normalize-space(@v)))"/></baseLanguage>			</xsl:if>
			<!--</xsl:if>-->
			</xsl:when>
			<xsl:when test="(local-name()='Code')">
				<xsl:if test="string-length(normalize-space(@v)) &gt; 0"><sourceCode><xsl:value-of select="normalize-space(@v)"/></sourceCode></xsl:if>
			</xsl:when>
			<xsl:when test="(local-name()='SourceName')">
				<xsl:if test="string-length(normalize-space(.)) &gt; 0"><sourceName><xsl:value-of select="normalize-space(.)"/></sourceName></xsl:if>
			</xsl:when>
		
			<xsl:when test="(local-name()='SourceNameNL')">
				<xsl:if test="string-length(normalize-space(.)) &gt; 0"><sourceNameNationalLanguage><xsl:value-of select="normalize-space(.)"/></sourceNameNationalLanguage></xsl:if>
			</xsl:when>
		
			<xsl:when test="(local-name()='SortName')">
				<xsl:if test="string-length(normalize-space(.)) &gt; 0"><sortName><xsl:value-of select="normalize-space(.)"/></sortName></xsl:if>
			</xsl:when>
		
			<xsl:when test="(local-name()='SortNameNL')">
				<xsl:if test="string-length(normalize-space(.)) &gt; 0"><sortNameNationalLanguage><xsl:value-of select="normalize-space(.)"/></sortNameNationalLanguage></xsl:if>
			</xsl:when>
		
			<xsl:when test="(local-name()='ListName')">
				<xsl:if test="string-length(normalize-space(.)) &gt; 0"><listName><xsl:value-of select="normalize-space(.)"/></listName></xsl:if>
			</xsl:when>
		
			<xsl:when test="(local-name()='Coverage')">
				<xsl:if test="normalize-space(@v)='FUCOV'">
					<coverage>Full</coverage>	
				</xsl:if>
				<xsl:if test="normalize-space(@v)='SECOV'">
					<coverage>Partial</coverage>	
				</xsl:if>
				
			</xsl:when>
			<xsl:when test="(local-name()='Online')">
				<xsl:if test="string-length(normalize-space(Date/@v)) &gt; 0">
<onlineAvailability><xsl:value-of select="user:ChangeDateFormat(string(Date/@v))"/></onlineAvailability>
				</xsl:if>	
			</xsl:when>
			<xsl:when test="(local-name()='FirstIssue')">
				<xsl:if test="string-length(normalize-space(Date/@v)) &gt; 0">
					<firstIssue><xsl:value-of select="user:ChangeDateFormat(string(normalize-space(Date/@v)))"/></firstIssue>
				</xsl:if>
			</xsl:when>
			<xsl:when test="(local-name()='Discontinue')">
				<xsl:if test="string-length(normalize-space(Date/@v)) &gt; 0">
					<discontinued>
						<xsl:value-of select="user:ChangeDateFormat(string(normalize-space(Date/@v)))"/>
					</discontinued>
				</xsl:if>	
			</xsl:when>

			<xsl:when test="(local-name()='Frequency')">
			<xsl:if test="string-length(normalize-space(Code/@v)) &gt; 0">
				<frequency>
					<xsl:attribute name="code"><xsl:value-of select="normalize-space(Code/@v)"/></xsl:attribute>
					<xsl:value-of select="normalize-space(Desc)"/>
				</frequency>
			</xsl:if>	
			</xsl:when>
			<xsl:when test="(local-name()='NomLagN')">
			<xsl:if test="string-length(normalize-space(.)) &gt; 0">
				<nominalLag><xsl:value-of select="normalize-space(.)"/></nominalLag>
			</xsl:if>	
			</xsl:when>
			<xsl:when test="(local-name()='MaxLag')">
			<xsl:if test="string-length(normalize-space(@v)) &gt; 0">
				<maximumLag><xsl:value-of select="normalize-space(@v)"/></maximumLag>
			</xsl:if>	
			</xsl:when>
			<xsl:when test="(local-name()='MinLag')">
			<xsl:if test="string-length(normalize-space(@v)) &gt; 0">
				<minimumLag><xsl:value-of select="normalize-space(@v)"/></minimumLag>
			</xsl:if>	
			</xsl:when>
			
			<xsl:when test="(local-name()='ContUPD')">
				<xsl:choose>
					<xsl:when test="string(normalize-space(@v))='N' or string(normalize-space(@v))='n' "><continuouslyUpdated>false</continuouslyUpdated></xsl:when>
					<xsl:when test="string(normalize-space(@v))='Y' or string(normalize-space(@v))='y' "><continuouslyUpdated>true</continuouslyUpdated></xsl:when>	
				</xsl:choose>
			</xsl:when>

			<xsl:when test="(local-name()='CompositeSrc')">
			<xsl:if test="string-length(normalize-space(./Code/@v)) &gt; 0">
				<compositeSourceCodes><xsl:value-of select="user:CreateCompositeSourceCodes(string(normalize-space(./Code/@v)))" disable-output-escaping="yes"/></compositeSourceCodes>
			</xsl:if>	
			</xsl:when>
			
			<xsl:when test="(local-name()='NotesExt')">
				<xsl:if test="string-length(normalize-space(Para)) &gt; 0">
					<notes>
						<xsl:apply-templates select="Para"/>
					</notes>
				</xsl:if>
			</xsl:when>

			<xsl:when test="(local-name()='Status')">
				<xsl:if test="normalize-space(@v)='active'">
					<availabilityStatus>Active</availabilityStatus>
				</xsl:if>
				<xsl:if test="normalize-space(@v)='discont'">
					<availabilityStatus>Discountiued</availabilityStatus>
				</xsl:if>
				<xsl:if test="normalize-space(@v)='deleted'">
					<availabilityStatus>Deleted</availabilityStatus>
				</xsl:if>
				<xsl:if test="normalize-space(@v)='pending'">
					<availabilityStatus>Pending</availabilityStatus>
				</xsl:if>
				
			</xsl:when>
			<xsl:when test="(local-name()='ArticleType')">
				<articleCoverage>
					<xsl:if test="normalize-space(@v)='fulltext'">FullText</xsl:if>
					<xsl:if test="normalize-space(@v)='abstract'">Abstract</xsl:if>
				</articleCoverage>
			</xsl:when>
			<xsl:when test="(local-name()='UPDScheduleN')">
				<xsl:if test="string-length(normalize-space(.)) &gt; 0">
				<updateSchedule><xsl:value-of select="normalize-space(.)"/></updateSchedule>
				</xsl:if>
			</xsl:when>
			<xsl:when test="(local-name()='WebAddress')">
				<xsl:if test="string-length(normalize-space(URL)) &gt; 0">
					<publicationWebAddress><xsl:value-of select="normalize-space(URL)"/></publicationWebAddress>
				</xsl:if>	
			</xsl:when>
		</xsl:choose>	
	</xsl:template>



	<xsl:template name="description">
		<description>
			<xsl:apply-templates select="Desc/Para"/>
		</description>
	</xsl:template>

	<xsl:template name="descriptionNL">
		<xsl:if test="DescNL">
			<descriptionNationalLanguage>
				<xsl:apply-templates select="DescNL/Para"/>
			</descriptionNationalLanguage>
		</xsl:if>
	</xsl:template>


	<xsl:template name="groupCodes">
		<xsl:apply-templates select="Groups/Code"/>
	</xsl:template>

	<xsl:template match="Groups/Code">
		<xsl:choose>
			<xsl:when test="string-length(normalize-space(@v)) &gt; 0"><groupCode><xsl:value-of select="normalize-space(@v)"/></groupCode></xsl:when>
<xsl:otherwise><groupCode/></xsl:otherwise>
		</xsl:choose>
		
	</xsl:template>

	<xsl:template name="nonPublicationDays">
		<xsl:if test="count(NonPubDays) &gt; 0">
			<nonPublicationDays>
				<xsl:for-each select="NonPubDays">
					<nonPublicationDay>
						<xsl:attribute name="code"><xsl:value-of select="normalize-space(Code/@v)"/></xsl:attribute>
						<xsl:value-of select="normalize-space(Desc)"/>
					</nonPublicationDay>
				</xsl:for-each>
			</nonPublicationDays>
		</xsl:if>	
	</xsl:template>
	
</xsl:stylesheet>
	