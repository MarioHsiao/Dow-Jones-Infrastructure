<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

	<xsl:include href="../xslt/commonDistDocElements.xslt"/>
	<msxsl:script language="JScript" implements-prefix="user"><![CDATA[
		
		function ChangeDateFormat(DateVal)
		{
			var RetVal = new String("");
			RetVal = DateVal.substr(0,4)+"-"+DateVal.substr(4,2)+"-"+DateVal.substr(6);
			return RetVal;
		}
		
		function ChangeTimeFormat(TimeVal)
		{
			var RetVal = new String("");
			RetVal = TimeVal.substr(0,2)+":"+TimeVal.substr(2,2)+":"+TimeVal.substr(4);
			return RetVal;
		}
		function ReplaceStr(strOrig, strFind, strReplace)
		{
			var RetVal = new String("");
			RetVal = strOrig.replace (strFind, strReplace);
			return RetVal;
		}
		]]>	</msxsl:script>

	<xsl:template match="/*">
		<GetArticleResponse>
			<articleResponse>
<!--					<xsl:apply-templates select="/*/Control"/>-->
					<xsl:apply-templates select="/*/Status"/>
					<xsl:apply-templates select="/*/ResultSet"/>
			</articleResponse>
		</GetArticleResponse>
	</xsl:template>

<!--	<xsl:template match="/*/Control">
		<xsl:copy-of select="."/>
	</xsl:template>-->

	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="/*/ResultSet">
		<articleResultSet>
			<xsl:attribute name="count"><xsl:copy-of select="number(@count)"/></xsl:attribute>
			<xsl:apply-templates select="Result"/>
		</articleResultSet>
	</xsl:template>

	<xsl:template match="Result">
		<article>
			<xsl:if test="number(@status)!=0">
				<!--<xsl:attribute name="status"><xsl:value-of select="number(@status)"/></xsl:attribute>-->
				<status>
					<xsl:attribute name="value"><xsl:copy-of select="number(@status)"/></xsl:attribute>
					<type/>
					<message/>
				</status>
				<!--<accessionNo><xsl:value-of select="@accessionno"/></accessionNo>-->
				<reference>distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="@accessionno"/></reference>
			</xsl:if>
			<xsl:if test="number(@status)=0">
				<!-- <reference>distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="AccessionNo/@value"/></reference> -->
				<reference>distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="@accessionno"/></reference>
				<xsl:apply-templates select=".//AccessionNo"/>
				<xsl:apply-templates select=".//Art"/>
				<xsl:apply-templates select=".//BaseLang"/>
				<xsl:apply-templates select=".//Byline"/>
				<xsl:apply-templates select=".//Credit"/>
				<xsl:variable name="CSetCount"><xsl:value-of select="count(.//CSet[@fid='co'])+count(.//CSet[@fid='ns'])+count(.//CSet[@fid='re'])+count(.//CSet[@fid='in'])"/></xsl:variable>
				<xsl:if test="number($CSetCount)>0">
					<indexingCodeSets>
						<xsl:attribute name="count"><xsl:value-of select="$CSetCount"/></xsl:attribute>
						<xsl:apply-templates select=".//CSet"/>
					</indexingCodeSets>
				</xsl:if>
				<xsl:apply-templates select=".//ColumnName"/>
				<xsl:apply-templates select=".//Contact"/>
				<xsl:apply-templates select=".//Copyright"/>
				<xsl:apply-templates select=".//Corrections"/>
				<xsl:apply-templates select=".//PubEdition"/>
				<xsl:apply-templates select=".//Headline"/>
				<xsl:apply-templates select=".//LeadPara"/>
				<xsl:apply-templates select=".//Notes"/>
				<xsl:if test=".//PubPage">
					<pages>
						<xsl:apply-templates select=".//PubPage"/>
					</pages>
				</xsl:if>
				<xsl:apply-templates select=".//Date[@fid='pd']"/>
				<xsl:apply-templates select=".//Time[@fid='et']"/>
				<xsl:apply-templates select=".//PubGroupN"/>
				<xsl:apply-templates select=".//PubGroupC"/>
				<xsl:apply-templates select=".//PublisherN"/>

				<xsl:apply-templates select=".//SectionName"/>
				<xsl:apply-templates select=".//SrcCode"/>
				<xsl:apply-templates select=".//SrcName"/>
				<xsl:apply-templates select=".//TailParas "/>
				<xsl:apply-templates select=".//Pubvol"/>
				<xsl:apply-templates select=".//Num[@fid='wc']"/>
				<xsl:apply-templates select=".//AdocTOC"/>
			</xsl:if>
		</article>
	</xsl:template>

	<xsl:template match="AccessionNo">
		<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
			<accessionNo>
				<xsl:value-of select="normalize-space(string(@value))"/>
			</accessionNo>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Art">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<artWork>
				<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(string(.))"/>-->
			</artWork>
		</xsl:if>
	</xsl:template>

	<xsl:template match="BaseLang">
		<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
			<baseLanguage>
				<xsl:value-of select="normalize-space(string(@value))"/>
			</baseLanguage>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Byline">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<byline>
				<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(string(.))"/>-->
			</byline>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Credit">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<credit>
				<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(string(.))"/>-->
			</credit>
		</xsl:if>
	</xsl:template>

	<xsl:template match="CSet">
		<xsl:if test="@fid='co' or @fid='ns' or @fid='re' or @fid='in'">
			<codeSet>
				<xsl:attribute name="codeCategory"><xsl:value-of select="@fid"/></xsl:attribute>
				<xsl:apply-templates select="Code"/>
			</codeSet>
		</xsl:if>	
	</xsl:template>

	<xsl:template match="Code">
		<code>
			<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			<xsl:apply-templates select="CodeD"/>
		</code>
	</xsl:template>

	<xsl:template match="CodeD">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<codeDescription>
				<xsl:attribute name="lang"><xsl:value-of select="@lang"/></xsl:attribute>
				<xsl:value-of select="normalize-space(string(.))"/>
			</codeDescription>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ColumnName">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<columnName>
				<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(string(.))"/>-->
			</columnName>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Contact">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
		 	<contact>
		 		<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(string(.))"/>-->
		 	</contact>
		 </xsl:if>
	</xsl:template>

	<xsl:template match="Copyright">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<copyright>
				<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(string(.))"/>-->
			</copyright>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Corrections">
		<corrections>
			<xsl:apply-templates select="Para"/>
		</corrections>
	</xsl:template>

	<xsl:template match="PubEdition">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<edition>
				<xsl:value-of select="normalize-space(string(.))"/>
			</edition>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Headline">
		<headline>
			<xsl:apply-templates select="Para"/>
		</headline>
	</xsl:template>

	<xsl:template match="LeadPara">
		<leadParagraph>
			<xsl:apply-templates select="Para"/>
		</leadParagraph>
	</xsl:template>

	<xsl:template match="Notes">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<notes>
				<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(string(.))"/>-->
			</notes>
		</xsl:if>
	</xsl:template>

	<xsl:template match="PubPage">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<page>
				<xsl:value-of select="normalize-space(string(.))"/>
			</page>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Date[@fid='pd']">
		<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
			<publicationDate>
				<xsl:value-of select="user:ChangeDateFormat(normalize-space(string(@value)))"/>
			</publicationDate>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Time[@fid='et']">
		<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
			<publicationTime>
				<xsl:value-of select="user:ChangeTimeFormat(normalize-space(string(@value)))"/>
			</publicationTime>
		</xsl:if>
	</xsl:template>

	<xsl:template match="PubGroupN">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<publisherGroupName>
				<xsl:value-of select="normalize-space(string(.))"/>
			</publisherGroupName>
		</xsl:if>
	</xsl:template>

	<xsl:template match="PubGroupC">
		<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
			<publisherGroupCode>
				<xsl:value-of select="normalize-space(string(@value))"/>
			</publisherGroupCode>
		</xsl:if>
	</xsl:template>

	<xsl:template match="PublisherN">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<publisherName>
				<xsl:value-of select="normalize-space(string(.))"/>
			</publisherName>
		</xsl:if>
	</xsl:template>

	<xsl:template match="SectionName">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<sectionName>
				<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(string(.))"/>-->
			</sectionName>
		</xsl:if>
	</xsl:template>

	<xsl:template match="SrcCode">
		<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
			<sourceCode>
				<xsl:value-of select="normalize-space(string(@value))"/>
			</sourceCode>
		</xsl:if>
	</xsl:template>

	<xsl:template match="SrcName">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<sourceName>
				<xsl:value-of select="normalize-space(string(.))"/>
			</sourceName>
		</xsl:if>
	</xsl:template>

	<xsl:template match="TailParas ">
		<tailParagraphs>
			<xsl:apply-templates select="Para"/>
		</tailParagraphs>
	</xsl:template>

	<xsl:template match="Para">
		<paragraph>
			<xsl:if test="@truncation!=''">
				<xsl:attribute name="truncation"><xsl:value-of select="concat(translate(substring(@truncation,1,1), 'p', 'P'), substring(@truncation,2))"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@display!=''">
				<xsl:choose>
					<xsl:when test="@display='norm'">
						<xsl:attribute name="display">Proportional</xsl:attribute>
					</xsl:when>
					<xsl:when test="@display='asis'">
						<xsl:attribute name="display">Fixed</xsl:attribute>
					</xsl:when>
				</xsl:choose>
			</xsl:if>
			<xsl:if test="@lang!=''">
				<xsl:copy-of select="@lang"/>
			</xsl:if>
			<xsl:call-template name="commonDistDocElements"/>
		</paragraph>
	</xsl:template>

	<xsl:template match="Pubvol">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<volume>
				<xsl:value-of select="normalize-space(string(.))"/>
			</volume>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Num[@fid='wc']">
		<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
			<wordCount>
				<xsl:value-of select="normalize-space(string(@value))"/>
			</wordCount>
		</xsl:if>
	</xsl:template>

<xsl:template match="AdocTOC">
		<!--Xform AdocTOC to ContentReferences-->
			<contentParts>
				<xsl:attribute name="contentType">
					<xsl:choose>
						<xsl:when test="@adoctype='article'">Article</xsl:when>
						<xsl:when test="@adoctype='file'">HTML</xsl:when>
						<xsl:when test="@adoctype='pdf'">PDF</xsl:when>
						<xsl:when test="@adoctype='picture'">Picture</xsl:when>
						<xsl:when test="@adoctype='webpage'">WebPage</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="primaryReference">
					<xsl:choose>
						<xsl:when test="@adoctype='webpage'"><xsl:value-of select="Item[@type='webpage']/@ref"/></xsl:when>
						<xsl:otherwise><xsl:value-of select="Item[@type='arttext']/@ref"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:otherwise>
					</xsl:choose>
					<!--<xsl:value-of select="@adoctype"/>-->
				</xsl:attribute>
				<!--<xsl:if test="not(@adoctype='file' and Item/@type!='html')">-->
					<xsl:apply-templates select="Item"/>
				<!--</xsl:if>-->
			</contentParts>
	</xsl:template>

	<xsl:template match="Item">
		<xsl:choose>
			<xsl:when test="@type='invtext'">
				<!--Do Nothing-->
			</xsl:when>
			<xsl:when test="parent::AdocTOC/@adoctype='file' and @type!='html'">
				<!--Do Nothing-->
			</xsl:when>
			<xsl:otherwise>
				<part>
					<!-- only processing 2 attributes.. should all be xformed??-->
					<xsl:if test="string-length(@type)!=0">
						<xsl:attribute name="type"><!--newsArticle-->
							<xsl:choose>
								<xsl:when test="@type='arttext'">NewsArticle</xsl:when>
								<xsl:when test="@type='html'">HTML</xsl:when>
								<xsl:when test="@type='pdf'">PDF</xsl:when>
								<xsl:when test="@type='tnail'">ThumbNail</xsl:when>
								<xsl:when test="@type='dispix'">Display</xsl:when>
								<xsl:when test="@type='prtpix'">Print</xsl:when>
								<xsl:when test="@type='bigpix'">Full</xsl:when>
								<xsl:when test="@type='webpage'">URL</xsl:when>
								<xsl:otherwise><xsl:value-of select="@type"/></xsl:otherwise>
							</xsl:choose>
							<!--<xsl:value-of select="@type"/>-->
						</xsl:attribute>
					</xsl:if>
					<xsl:if test="string-length(@subtype)!=0">
						<xsl:attribute name="subType"><xsl:value-of select="@subtype"/></xsl:attribute>
					</xsl:if>
		
					<xsl:if test="string-length(@mimetype)!=0">
						<xsl:attribute name="mimeType"><xsl:value-of select="@mimetype"/></xsl:attribute>
					</xsl:if>
		
					<xsl:if test="string-length(@size)!=0">
					<xsl:attribute name="size"><xsl:value-of select="@size"/></xsl:attribute>
					</xsl:if>
					<xsl:attribute name="reference">
						<xsl:choose>
							<xsl:when test="parent::AdocTOC/@adoctype='file' and @type='html'">
								<xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_text_html:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="@type='arttext'"><xsl:value-of select="@ref"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='html'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_text_html:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='pdf'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_application_pdf:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='tnail'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_thumbnail:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='dispix'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_display:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='prtpix'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_print:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='bigpix'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_full:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='webpage'"><xsl:value-of select="@ref"/></xsl:when>
									<xsl:otherwise><xsl:value-of select="@ref"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</part>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
</xsl:stylesheet>