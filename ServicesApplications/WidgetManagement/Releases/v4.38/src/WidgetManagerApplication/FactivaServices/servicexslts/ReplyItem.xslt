<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl user">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="../xslt/commonDistDocElements.xslt"/>
	<msxsl:script language="JScript" implements-prefix="user">
		<![CDATA[
		function ChangeDateFormat(DateVal){
		
			var _ret = "";
			DateVal = DateVal+"";
			_ret = DateVal.substr(0,4)+"-"+DateVal.substr(4,2)+"-"+DateVal.substr(6);
			return _ret+"";

		}
		function ChangeTimeFormat(TimeVal){
			var _ret = "";
			_ret = TimeVal.substr(0,2)+'-'+TimeVal.substr(2,2)+'-'+TimeVal.substr(4);
			return _ret+"";
		}
		function ReplaceStr(strOrig, strFind, strReplace){
			return strOrig.replace(strFind,strReplace)+"";
		}
		]]>
	</msxsl:script>

	<xsl:template match="ReplyItem">
			<xsl:param name="type"/>
			<xsl:apply-templates select="*" mode="ReplyItem">
				<xsl:with-param name="type" select="$type"/>
			</xsl:apply-templates>
			<xsl:apply-templates select="parent::Result/AdocTOC"/>
			<xsl:apply-templates select="parent::report/AdocTOC"/>
	</xsl:template>

	<xsl:template match="*" mode="ReplyItem">
		<xsl:param name="type"/>
		<xsl:choose>
			<!-- Xform Title-->
			<xsl:when test="(local-name()='Title')">
                   <xsl:apply-templates select="."><xsl:with-param name="type" select="$type"/></xsl:apply-templates>
			</xsl:when>
			<!-- Xform Snippet-->
			<xsl:when test="(local-name()='Snippet')">
				<xsl:apply-templates select="."/>
			</xsl:when>
			<!-- Xform TruncRules-->
			<xsl:when test="(local-name()='TruncRules')">
				<xsl:apply-templates select="."/>
			</xsl:when>
			<!-- Xform AccessionNo-->
			<xsl:when test="(local-name()='AccessionNo')">
				<xsl:choose>
					<xsl:when test="string-length(normalize-space(@value)) &gt; 0"><accessionNo><xsl:value-of select="normalize-space(@value)"/></accessionNo>
</xsl:when>
<xsl:otherwise><accessionNo/></xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<!-- Xform Date-->
			<xsl:when test="(local-name()='Date')">
	<xsl:choose>
					<xsl:when test="string-length(normalize-space(@value)) &gt; 0"><publicationDate><xsl:value-of select="normalize-space(user:ChangeDateFormat(string(@value)))" /></publicationDate>
</xsl:when>
<xsl:otherwise><publicationDate/></xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<!-- Xform Time-->
			<xsl:when test="(local-name()='Time')">
		<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
		<publicationTime><xsl:value-of select="user:ChangeTimeFormat(string(@value))" /></publicationTime>
		</xsl:if>
			</xsl:when>
			<!-- Xform BaseLang to language-->
			<xsl:when  test="(local-name()='BaseLang')">
				<xsl:choose>
					<xsl:when test="string-length(normalize-space(@value)) &gt; 0"><baseLanguage><xsl:value-of select="normalize-space(@value)"/></baseLanguage></xsl:when>
<xsl:otherwise><baseLanguage/></xsl:otherwise>
				</xsl:choose>
				
			</xsl:when>
			<!-- Xform IPDocId to IPDocumentID-->
			<xsl:when test="(local-name()='IPDocId')">
<xsl:choose>
					<xsl:when test="string-length(normalize-space(.)) &gt; 0"><ipDocumentID><xsl:value-of select="normalize-space(.)" /></ipDocumentID></xsl:when>
<xsl:otherwise><ipDocumentID/></xsl:otherwise>
				</xsl:choose>
				
			</xsl:when>
			<!-- Xform Num@wc  to WordCount-->
			<xsl:when test="(local-name()='Num' and @fid='wc')">
				<xsl:choose>
				<xsl:when test="normalize-space(@value)=''"><wordCount>0</wordCount></xsl:when>
				<xsl:otherwise><wordCount><xsl:value-of select="normalize-space(@value)" /></wordCount></xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<!-- Xform SectionName  to Nothing-->
			<xsl:when test="local-name()='SectionName'">
				<xsl:if test="$type=''">
				<xsl:if  test="string-length(normalize-space(.)) &gt; 0"><sectionName><xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(.)"/>--></sectionName></xsl:if>
			
				</xsl:if>
			</xsl:when>
			<!-- Xform SrcCode  to Nothing-->
			<xsl:when test="(local-name()='SrcCode' and @fid='sc')">
				<xsl:if test="$type=''">
<xsl:choose>
				<xsl:when test="string-length(normalize-space(@value)) &gt; 0"><sourceCode><xsl:value-of select="@value"/></sourceCode></xsl:when>
				<xsl:otherwise><sourceCode/></xsl:otherwise>
				</xsl:choose>
				
				</xsl:if>
			</xsl:when>
			<!-- Xform SrcName  to Nothing-->
			<xsl:when test="(local-name()='SrcName' and @fid='sn')">
				<xsl:if test="$type=''">
<xsl:choose>
					<xsl:when test="string-length(normalize-space(.)) &gt; 0"><sourceName><xsl:value-of select="normalize-space(.)"/></sourceName></xsl:when>
<xsl:otherwise><sourceName/></xsl:otherwise>
				</xsl:choose>
				
				
				</xsl:if>
			</xsl:when>
			<!-- Xform Copyright  to remove fid-->
			<xsl:when test="(local-name()='Copyright')">
				<xsl:if test="string-length(normalize-space(.))!='0'">
					<copyright><xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(.)"/>--></copyright>
				</xsl:if>	
			</xsl:when>
			<!-- Xform Byline  to remove fid-->
			<xsl:when test="(local-name()='Byline')">
				<xsl:if test="string-length(normalize-space(.))!='0'">
					<byline><xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(.)"/>--></byline>
				</xsl:if>	
			</xsl:when>
			<!-- Xform Creditto remove fid-->
			<xsl:when test="(local-name()='Credit')">
				<xsl:if test="string-length(normalize-space(.))!='0'">
					<credit><xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(.)"/>--></credit>
				</xsl:if>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="AdocTOC">
		<!--Xform AdocTOC to ContentReferences-->
			<contentParts>
				<xsl:attribute name="contentType">
					<xsl:choose>
						<xsl:when test="@adoctype='article'">Article</xsl:when>
						<xsl:when test="@adoctype='file'">Article</xsl:when>
						<xsl:when test="@adoctype='pdf'">PDF</xsl:when>
						<xsl:when test="@adoctype='picture'">Picture</xsl:when>
						<xsl:when test="@adoctype='webpage'">WebPage</xsl:when>
						<xsl:when test="@adoctype='analyst'">PDF</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="primaryReference">
					<xsl:choose>
						<xsl:when test="@adoctype='webpage'"><xsl:value-of select="Item[@type='webpage']/@ref"/></xsl:when>
						<xsl:when test="@adoctype='analyst'">distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="ancestor::report/@accessionno"/></xsl:when>
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
								<xsl:when test="@type='html'">NewsArticle</xsl:when>
								<xsl:when test="@type='pdf'">PDF</xsl:when>
								<xsl:when test="@type='ivtxco'">PDF</xsl:when>
								<xsl:when test="@type='ivtxin'">PDF</xsl:when>
								<xsl:when test="@type='tnail'">ThumbNail</xsl:when>
								<xsl:when test="@type='fnail'">FingerNail</xsl:when>
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
		
					<xsl:if test="string-length(@mimetype)!=0 or @type='ivtxco' or @type='ivtxin'">
						<!-- 06-13-2005 : SM changes to supress html references information-->
						<xsl:choose>
							<xsl:when test="@type='ivtxco'"><xsl:attribute name="mimeType">application/pdf</xsl:attribute></xsl:when>
							<xsl:when test="@type='ivtxin'"><xsl:attribute name="mimeType">application/pdf</xsl:attribute></xsl:when>
							<xsl:when test="@type!='html'"><xsl:attribute name="mimeType"><xsl:value-of select="@mimetype"/></xsl:attribute></xsl:when>
						</xsl:choose>
					</xsl:if>
					<!-- SM: 06162005 set the size to be same as artText as the reference is such-->
					<xsl:if test="string-length(@size)!=0">
					<xsl:choose>
						<xsl:when test="@type!='html'"><xsl:attribute name="size"><xsl:value-of select="@size"/></xsl:attribute></xsl:when>
						<xsl:otherwise>
							<xsl:if test="string-length(../Item[@type='arttext']/@size)!=0">
							<xsl:attribute name="size"><xsl:value-of select="../Item[@type='arttext']/@size"/></xsl:attribute>
							</xsl:if>
						</xsl:otherwise>
					</xsl:choose>	
					</xsl:if>
					
					<!-- 06-13-2005 : SM changes to supress html references and create regular article reference even for type=html. this is because of bug rerported by ptg for invalid xml when charset not
						set to what is in the html.. fdk defaults to utf-8 and that is not good enough for some chars like Euro, which need western european charset-->
					<xsl:attribute name="reference">
						<xsl:choose>
							<xsl:when test="parent::AdocTOC/@adoctype='file' and @type='html'"><xsl:value-of select="../Item[@type='arttext']/@ref"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="@type='arttext'"><xsl:value-of select="@ref"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='html'"><xsl:value-of select="../Item[@type='arttext']/@ref"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='pdf'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_application_pdf:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='ivtxco'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'ivtx:gateway', 'probj_application_pdf:investext')"/>/<xsl:value-of select="ancestor::report/@accessionno"/></xsl:when>
									<xsl:when test="@type='ivtxin'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'ivtx:gateway', 'probj_application_pdf:investext')"/>/<xsl:value-of select="ancestor::report/@accessionno"/></xsl:when>
									<xsl:when test="@type='tnail'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_thumbnail:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
									<xsl:when test="@type='fnail'"><xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_fingernail:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/></xsl:when>
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


	<xsl:template match="//TruncRules">
		<xsl:if test="string-length(normalize-space(XS)) &gt; 0 or string-length(normalize-space(S)) &gt; 0 or string-length(normalize-space(M)) &gt; 0 or string-length(normalize-space(L)) &gt; 0">
		<truncationRules>
			<xsl:apply-templates/>
		</truncationRules>
</xsl:if>		
	</xsl:template>

	<xsl:template match="//XS">
		<extraSmall><xsl:value-of select="normalize-space(@value)"/></extraSmall>
	</xsl:template>
	<xsl:template match="//S">
		<small><xsl:value-of select="normalize-space(@value)"/></small>
	</xsl:template>
	<xsl:template match="//M">
		<medium><xsl:value-of select="normalize-space(@value)"/></medium>
	</xsl:template>
	<xsl:template match="//L">
		<large><xsl:value-of select="normalize-space(@value)"/></large>
	</xsl:template>

	<xsl:template match="//Snippet">
		<snippet>
			<xsl:apply-templates/>
		</snippet>
	</xsl:template>


	<xsl:template match="//Title">
		<xsl:param name="type"/>
		<!--		<title>-->
		<!--			<xsl:copy-of select="@*"/>-->
		<xsl:for-each select="child::node()">
		<xsl:choose>
		<xsl:when test="(local-name()='SectionName')">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<xsl:if test="$type=''">
				<xsl:element name="sectionName">
					<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="normalize-space(SectionName)"/>-->
				</xsl:element>
			</xsl:if>
		</xsl:if>
		</xsl:when>
		<xsl:when test="(local-name()='ColumnName')">
			<xsl:if test="string-length(normalize-space(.)) &gt; 0">
				<xsl:element name="columnName">
					<xsl:call-template name="commonDistDocElements"/><!--<xsl:value-of select="ColumnName"/>-->
				</xsl:element>
			</xsl:if>
		</xsl:when>
		<!--</xsl:if>-->
		</xsl:choose>
		</xsl:for-each>
		<xsl:apply-templates select="Headline"/>
		<!--		</title>-->
	</xsl:template>
	<xsl:template match="//Headline">
		<headline>
<!--			<xsl:copy-of select="@*"/>-->
			<xsl:apply-templates/>
		</headline>
	</xsl:template>

	<xsl:template match="//Para">
		<paragraph>
			<xsl:if test="@truncation!=''">
				<xsl:attribute name="truncation"><xsl:value-of select="concat(translate(substring(@truncation,1,1), 'p', 'P'), substring(@truncation,2))"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@display!=''">
				<xsl:choose>
					<xsl:when test="@display='norm'"><xsl:attribute name="display">Proportional</xsl:attribute>
					</xsl:when>
					<xsl:when test="@display='asis'"><xsl:attribute name="display">Fixed</xsl:attribute>
					</xsl:when>
				</xsl:choose>
			</xsl:if>
			<xsl:if test="@lang!=''">
				<xsl:copy-of select="@lang"/>
			</xsl:if>
			<!-- only Para needs to  worry about elink rest will not set this and will not process eLink -->
			<xsl:call-template name="commonDistDocElements"><xsl:with-param name="processELink">true</xsl:with-param></xsl:call-template>
		</paragraph>
	</xsl:template>
</xsl:stylesheet>
