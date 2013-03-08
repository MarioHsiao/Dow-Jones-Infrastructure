<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl user">
<xsl:output method="xml" encoding="UTF-8" omit-xml-declaration="no" indent="yes" doctype-system="CustomerDD5.1.dtd"/>

	<msxsl:script language="JScript" implements-prefix="user">
		<![CDATA[
function ChangeDateFormat(DateVal)
{
var RetVal = "";
RetVal = DateVal.substr(0,4)+"-"+DateVal.substr(4,2)+"-"+DateVal.substr(6,2);
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
RetVal = RetVal.replace (strFind, strReplace);
return RetVal;
}
]]>
	</msxsl:script>

	<xsl:template match="//IndexSearchResponse">
		<IndexSearchResponse>
			<xsl:apply-templates select="ResultSet"/>
		</IndexSearchResponse>
	</xsl:template>
	<xsl:template match="ResultSet">
				<ResultSet>
					<xsl:attribute name="count"><xsl:value-of select="@count"/></xsl:attribute>
					<xsl:attribute name="first"><xsl:value-of select="@first"/></xsl:attribute>
					<xsl:attribute name="total"><xsl:value-of select="@total"/></xsl:attribute>		
			         <xsl:apply-templates select="Result"/>
				</ResultSet>	
	</xsl:template>
		<xsl:template match="Result">
				<Result>
					<xsl:attribute name="accessionno"><xsl:value-of select="@accessionno"/></xsl:attribute>
					<xsl:attribute name="relscore"><xsl:value-of select="@relscore"/></xsl:attribute>
					<xsl:apply-templates select="ReplyItem|AdocTOC|items"/>
				</Result>
	  </xsl:template>
	
	<xsl:template match="ReplyItem">
		<ReplyItem>
			<xsl:attribute name="lang"><xsl:value-of select="@lang"/></xsl:attribute>
			<xsl:apply-templates select="BaseLang"/>
			<xsl:apply-templates select="IPDocId"/>
			<xsl:apply-templates select="AccessionNo"/>
			<xsl:apply-templates select="Num"/>
			<xsl:apply-templates select="Date"/>
			<xsl:apply-templates select="Time"/>
			<xsl:apply-templates select="SrcCode"/>
			<xsl:apply-templates select="SrcName"/>
			<xsl:apply-templates select="TruncRules"/>
			<xsl:apply-templates select="Title"/>
			<xsl:if test="Snippet">
				<Snippet>
					<xsl:apply-templates select="Snippet/Para"/>
				</Snippet>
			</xsl:if>
			<xsl:apply-templates select="Byline"/>
			<xsl:apply-templates select="Credit"/>
			<xsl:apply-templates select="Copyright"/>
			<xsl:apply-templates select="CodeSets"/>
		</ReplyItem>
	</xsl:template>
	<xsl:template match="items">
		<items>
			<xsl:choose>
				<xsl:when test='postbackUrl != ""'>
					<url>
					<xsl:value-of select="postbackUrl" />
					</url>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="@*"/>
					<xsl:copy-of select="url"/>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="title!=''">
				<title>
					<xsl:value-of select="title"/>
				</title>
			</xsl:if>
			<xsl:if test="title=''">
				<title>
					<xsl:value-of select="description"/>
				</title>
			</xsl:if>
			<xsl:copy-of select="description"/>
			<xsl:if test="publicationDate!=''">
				<Date>
					<xsl:variable name="outDate">
						<xsl:value-of select="substring-before(publicationDate, 'T')"/>
					</xsl:variable>
						<xsl:value-of select="user:ReplaceStr(string($outDate), '-','')"/>
				</Date>
				<Time>
					<xsl:variable name="outTime">
						<xsl:value-of select="substring-after(publicationDate, 'T')"/>
					</xsl:variable>

					<xsl:value-of select="user:ReplaceStr(string($outTime), ':','')"/>
				</Time>
			</xsl:if>
		</items>
	</xsl:template>



	<xsl:template match="Logo">
		<Logo>
			<xsl:if test="@img!=''">
				<xsl:attribute name="img"><xsl:value-of select="@img"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="string-length(normalize-space(@link))">
				<xsl:attribute name="link"><xsl:value-of select="@link"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@src!=''">
				<xsl:attribute name="src"><xsl:value-of select="@src"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</Logo>
	</xsl:template>
	<xsl:template match="DescTPC">
		<DescTPC>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates/>
		</DescTPC>
	</xsl:template>
	<xsl:template match="DescField">
		<DescField>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="."/>
		</DescField>
	</xsl:template>
	<xsl:template match="PubGroupN">
		<PubGroupN>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates/>
		</PubGroupN>
	</xsl:template>
	<xsl:template match="PubGroupC">
		<PubGroupC>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="a"/>
			<xsl:apply-templates select="A"/>
		</PubGroupC>
	</xsl:template>
	<xsl:template match="SrcCode">
		<SrcCode>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</SrcCode>
	</xsl:template>
	<xsl:template match="AttribCode">
		<AttribCode>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</AttribCode>
	</xsl:template>
	<xsl:template match="SrcName">
		<SrcName>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="a"/>
			<xsl:apply-templates select="A"/>
		</SrcName>
	</xsl:template>
	<xsl:template match="IPId">
		<IPId>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</IPId>
	</xsl:template>
	<xsl:template match="PublisherN">
		<PublisherN>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="a"/>
			<xsl:apply-templates select="A"/>
		</PublisherN>
	</xsl:template>
	<xsl:template match="Restrictor">
		<Restrictor>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="Code"/>
		</Restrictor>
	</xsl:template>
	<xsl:template match="Misc">
		<Misc>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
		</Misc>
	</xsl:template>
	<xsl:template match="CrossRef">
		<CrossRef>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
		</CrossRef>
	</xsl:template>
	<xsl:template match="IIndexCode">
		<IIndexCode>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</IIndexCode>
	</xsl:template>
	<xsl:template match="LinkageNo">
		<LinkageNo>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
		</LinkageNo>
	</xsl:template>
	<xsl:template match="RevisionNo">
		<RevisionNo>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</RevisionNo>
	</xsl:template>
	<xsl:template match="Editor">
		<Editor>
			<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</Editor>
	</xsl:template>
	<xsl:template match="BaseLang">
		<BaseLang>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</BaseLang>
	</xsl:template>
	<xsl:template match="PubVol">
		<PubVol>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
		</PubVol>
	</xsl:template>
	<xsl:template match="PubPage">
		<PubPage>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
		</PubPage>
	</xsl:template>
	<xsl:template match="PubEdition">
		<PubEdition>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
		</PubEdition>
	</xsl:template>
	<xsl:template match="DocType">
		<DocType>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</DocType>
	</xsl:template>
	<xsl:template match="Title">
		<Title>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="SectionName"/>
			<xsl:apply-templates select="ColumnName"/>
			<xsl:apply-templates select="Headline"/>
		</Title>
	</xsl:template>
	<xsl:template match="Headline">
		<Headline>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="Para"/>
		</Headline>
	</xsl:template>
	<xsl:template match="Corrections">
		<Corrections>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="Para"/>
		</Corrections>
	</xsl:template>
	<xsl:template match="SectionName">
		<SectionName>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="ELink"/>
		</SectionName>
	</xsl:template>
	<xsl:template match="ColumnName">
		<ColumnName>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="ELink"/>
		</ColumnName>
	</xsl:template>
	<xsl:template match="MessageNo">
		<MessageNo>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="value"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</MessageNo>
	</xsl:template>
	<xsl:template match="Keywords">
		<Keywords>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="value"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="Name"/>
		</Keywords>
	</xsl:template>
	<xsl:template match="Name">
		<Name>
			<xsl:if test="@type!=''">
				<xsl:attribute name="type"><xsl:value-of select="@type"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="."/>
		</Name>
	</xsl:template>
	<xsl:template match="OrigSource">
		<OrigSource>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="value"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
		</OrigSource>
	</xsl:template>
	<xsl:template match="AccessionNo">
		<AccessionNo>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</AccessionNo>
	</xsl:template>
	<xsl:template match="Date">
		<Date>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</Date>
	</xsl:template>
	<xsl:template match="Num">
		<Num>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</Num>
	</xsl:template>
	<xsl:template match="Time">
		<Time>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
		</Time>
	</xsl:template>
	<xsl:template match="Fingerprint">
		<Fingerprint>
			<xsl:if test="Fpe">
				<Fpe>
					<xsl:if test="Fpe/@fid!=''">
						<xsl:attribute name="fid"><xsl:value-of select="Fpe/@fid"/></xsl:attribute>
					</xsl:if>
					<xsl:if test="Fpe/@index!=''">
						<xsl:attribute name="index"><xsl:value-of select="Fpe/@index"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="Fpe/text()"/>
				</Fpe>
			</xsl:if>
			<xsl:if test="Fph">
				<Fph>
					<xsl:if test="Fph/@fid!=''">
						<xsl:attribute name="fid"><xsl:value-of select="Fph/@fid"/></xsl:attribute>
					</xsl:if>
					<xsl:if test="Fph/@index!=''">
						<xsl:attribute name="index"><xsl:value-of select="Fph/@index"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="Fph/text()"/>
				</Fph>
			</xsl:if>
			<xsl:if test="Fps">
				<Fps>
					<xsl:if test="Fps/@fid!=''">
						<xsl:attribute name="fid"><xsl:value-of select="Fps/@fid"/></xsl:attribute>
					</xsl:if>
					<xsl:if test="Fps/@index!=''">
						<xsl:attribute name="index"><xsl:value-of select="Fps/@index"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="Fps/text()"/>
				</Fps>
			</xsl:if>
		</Fingerprint>
	</xsl:template>
	<xsl:template match="IPDocId">
		<IPDocId>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
		</IPDocId>
	</xsl:template>
	<xsl:template match="Copyright">
		<Copyright>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="a"/>
			<xsl:apply-templates select="A"/>
			<xsl:apply-templates select="ELink"/>
		</Copyright>
	</xsl:template>
	<xsl:template match="Byline">
		<Byline>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="a"/>
			<xsl:apply-templates select="A"/>
			<xsl:apply-templates select="ELink"/>
		</Byline>
	</xsl:template>
	<xsl:template match="Credit">
		<Credit>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@fid!=''">
				<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="a"/>
			<xsl:apply-templates select="A"/>
			<xsl:apply-templates select="ELink"/>
		</Credit>
	</xsl:template>
	<xsl:template match="TruncRules">
		<TruncRules>
			<xsl:if test="XS">
				<XS>
					<xsl:attribute name="value"><xsl:value-of select="XS/@value"/></xsl:attribute>
				</XS>
			</xsl:if>
			<xsl:if test="S">
				<S>
					<xsl:attribute name="value"><xsl:value-of select="S/@value"/></xsl:attribute>
				</S>
			</xsl:if>
			<xsl:if test="M">
				<M>
					<xsl:attribute name="value"><xsl:value-of select="M/@value"/></xsl:attribute>
				</M>
			</xsl:if>
			<xsl:if test="L">
				<L>
					<xsl:attribute name="value"><xsl:value-of select="L/@value"/></xsl:attribute>
				</L>
			</xsl:if>
			<xsl:if test="Teaser">
				<Teaser>
					<xsl:attribute name="value"><xsl:value-of select="Teaser/@value"/></xsl:attribute>
				</Teaser>
			</xsl:if>
		</TruncRules>
	</xsl:template>
	<xsl:template match="CodeSets">
		<CodeSets>
			<xsl:if test="@prefix!=''">
				<xsl:attribute name="prefix"><xsl:value-of select="@prefix"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="CSet"/>
		</CodeSets>
	</xsl:template>
	<xsl:template match="CSet">
		<xsl:if test="@fid='co' or @fid='ns' or @fid='re' or @fid='in'">
			<CSet>
				<xsl:if test="@fid!=''">
					<xsl:attribute name="fid"><xsl:value-of select="@fid"/></xsl:attribute>
				</xsl:if>
				<xsl:apply-templates select="Code"/>
			</CSet>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Code">
		<Code>
			<xsl:if test="@org!=''">
				<xsl:attribute name="org"><xsl:value-of select="@org"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@rl!=''">
				<xsl:attribute name="rl"><xsl:value-of select="@rl"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="CodeD"/>
			<xsl:apply-templates select="CodeI"/>
		</Code>
	</xsl:template>
	<xsl:template match="CodeD">
		<CodeD>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@lang!=''">
				<xsl:attribute name="lang"><xsl:value-of select="@lang"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
		</CodeD>
	</xsl:template>
	<xsl:template match="CodeI">
		<CodeI>
			<xsl:if test="@org!=''">
				<xsl:attribute name="org"><xsl:value-of select="@org"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@action!=''">
				<xsl:attribute name="action"><xsl:value-of select="@action"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@index!=''">
				<xsl:attribute name="index"><xsl:value-of select="@index"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates select="CodeA"/>
		</CodeI>
	</xsl:template>
	<xsl:template match="CodeA">
		<CodeA>
			<xsl:if test="@name!=''">
				<xsl:attribute name="name"><xsl:value-of select="@name"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@value!=''">
				<xsl:attribute name="value"><xsl:value-of select="@value"/></xsl:attribute>
			</xsl:if>
		</CodeA>
	</xsl:template>
	<xsl:template match="Para">
		<Para>
			<xsl:if test="@display!=''">
				<xsl:attribute name="display"><xsl:value-of select="@display"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@lang!=''">
				<xsl:attribute name="lang"><xsl:value-of select="@lang"/></xsl:attribute>
			</xsl:if>
			<xsl:apply-templates/>
		</Para>
	</xsl:template>
	<xsl:template match="AdocTOC">
		<AdocTOC>
			<xsl:attribute name="adoctype"><xsl:value-of select="./@adoctype"/></xsl:attribute>
			<xsl:apply-templates select="Item"/>
		</AdocTOC>
	</xsl:template>
	<xsl:template match="ev">
		<xsl:apply-templates select="en"/>
		<xsl:apply-templates select="Elink"/>
		<xsl:apply-templates/>
	</xsl:template>
	<xsl:template match="en">
		<xsl:value-of select="text()"/>
	</xsl:template>
	<xsl:template match="ELink">
		<xsl:if test="@type='webpage'">
			<ELink>
				<xsl:attribute name="type"><xsl:value-of select="@type"/></xsl:attribute>
				<xsl:attribute name="ref"><xsl:value-of select="@ref"/></xsl:attribute>
				<xsl:value-of select="."/>
			</ELink>
		</xsl:if>
		<xsl:if test="@type='company'">
			<ELink>
				<xsl:attribute name="type"><xsl:value-of select="@type"/></xsl:attribute>
				<xsl:attribute name="ref"><xsl:value-of select="@ref"/></xsl:attribute>
				<xsl:value-of select="."/>
			</ELink>
		</xsl:if>
		<xsl:if test="@type='pro'">
			<ELink>
				<xsl:attribute name="type"><xsl:value-of select="@type"/></xsl:attribute>
				<xsl:attribute name="ref"><xsl:value-of select="@ref"/></xsl:attribute>
				<xsl:apply-templates select="Item"/>
			</ELink>
		</xsl:if>
		<xsl:if test="@type='graphic'">
			<ELink>
				<xsl:attribute name="type"><xsl:value-of select="@type"/></xsl:attribute>
				<xsl:attribute name="ref"><xsl:value-of select="@ref"/></xsl:attribute>
				<xsl:apply-templates select="Item"/>
			</ELink>
		</xsl:if>
		<xsl:if test="@type='pix'">
			<ELink>
				<xsl:attribute name="type"><xsl:value-of select="@type"/></xsl:attribute>
				<xsl:attribute name="ref"><xsl:value-of select="@ref"/></xsl:attribute>
				<xsl:apply-templates select="Item"/>
			</ELink>
		</xsl:if>
		<xsl:if test="@type='doc'">
			<ELink>
				<xsl:attribute name="type"><xsl:value-of select="@type"/></xsl:attribute>
				<xsl:attribute name="ref"><xsl:value-of select="@ref"/></xsl:attribute>
				<xsl:apply-templates select="Item"/>
			</ELink>
		</xsl:if>
	</xsl:template>
	<xsl:template match="A">
		<A>
			<xsl:copy-of select="@*"/>
			<xsl:value-of select="text()"/>
		</A>
	</xsl:template>
	<xsl:template match="a">
		<a>
			<xsl:copy-of select="@*"/>
			<xsl:value-of select="text()"/>
		</a>
	</xsl:template>
	<xsl:template match="Item">
		<Item>
			<xsl:if test="@subtype!=''">
				<xsl:attribute name="subtype"><xsl:value-of select="@subtype"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@billtype!=''">
				<xsl:attribute name="billtype"><xsl:value-of select="@billtype"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@mimetype!=''">
				<xsl:attribute name="mimetype"><xsl:value-of select="@mimetype"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@size!=''">
				<xsl:attribute name="size"><xsl:value-of select="@size"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@ref!=''">
				<xsl:attribute name="ref"><xsl:value-of select="@ref"/></xsl:attribute>
			</xsl:if>
			<xsl:if test="@type!=''">
				<xsl:attribute name="type"><xsl:value-of select="@type"/></xsl:attribute>
			</xsl:if>
			<xsl:value-of select="text()"/>
		</Item>
	</xsl:template>
</xsl:stylesheet>
