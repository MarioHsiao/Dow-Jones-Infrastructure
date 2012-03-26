<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <msxsl:script language="JScript" implements-prefix="user">
    <![CDATA[
		
		function ChangeDateFormat(DateVal)
		{
			var RetVal = new String("");
			RetVal = DateVal.substr(0,4)+"/"+DateVal.substr(5,2)+"/"+DateVal.substr(7);
			return RetVal;
		}
		
		function ChangeTimeFormat(TimeVal)
		{
			var RetVal = new String("");
			var secStr = TimeVal.substr(9);
			RetVal = TimeVal.substr(0,2)+":"+TimeVal.substr(3,2)+":"+TimeVal.substr(6,2);
			
			return RetVal;	
		}
		
		function ReplaceStr(strOrig, strFind, strReplace)
		{
			var RetVal = new String("");
			RetVal = strOrig.replace (strFind, strReplace);
			return RetVal;
		}
		]]>
  </msxsl:script>
  <xsl:template match="/GetArchiveObjectResponse">
    <GetArticleWithLimitResponse>
      <xsl:apply-templates select="Status"/>
      <xsl:apply-templates select="ResultSet"/>
      <xsl:apply-templates select="ContinuationContext" />
    </GetArticleWithLimitResponse>
  </xsl:template>

  <xsl:template match="Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="ResultSet">
    <articleResponseSet>
      <xsl:attribute name="count">
        <xsl:copy-of select="number(@count)"/>
      </xsl:attribute>
      <xsl:apply-templates select="./Result"/>
    </articleResponseSet>
  </xsl:template>

  <xsl:template match="ContinuationContext">
    <rawData>
      <xsl:value-of select="normalize-space(.)"/>
    </rawData>
  </xsl:template>

  <xsl:template match="//Result">
    <article>
      <status>
        <xsl:attribute name="value">
          <xsl:copy-of select="number(@status)"/>
        </xsl:attribute>
        <type/>
        <message/>
      </status>
      <xsl:if test="number(@status)=0">
        <reference>
          distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="@accessionno"/>
        </reference>

        <xsl:apply-templates select="./Art"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='accessionno']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='baselang']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='doctype']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubedition']"/>
        <pages>
          <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubpage']" />
        </pages>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubdate']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubtime']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='pubgroupn']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='pubgroupc']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='publishern']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='ipdocid']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='charcount']"/>

        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='loaddate']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='loadtime']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='moddate']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='modtime']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='origsource']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='revisionno']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='editor']"/>
        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='authorlist']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srccode']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='attribcode']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='ipid']"/>

        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcname']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcprimarytype']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcsecondarytype']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcrightstype']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='allowtranslation']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='circulation']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='webhits']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='firstdate']"/>
        <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='regionoforigin']"/>

        <xsl:choose>
          <xsl:when test="./PropertySet[@group='pubdata']/Property[@name='logoimage']">
            <sourceLogo>
              <xsl:attribute name="image">
                <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logoimage']/@value)" />
              </xsl:attribute>
              <xsl:attribute name="link">
                <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logolink']/@value)" />
              </xsl:attribute>
              <xsl:attribute name="source">
                <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logosrc']/@value)" />
              </xsl:attribute>
            </sourceLogo>
          </xsl:when>
        </xsl:choose>

        <xsl:apply-templates select="./Byline"/>
        <xsl:apply-templates select="./Credit"/>

        <xsl:variable name="CSetCount">
          <xsl:value-of select="count(./Codes/CodeSet)"/>
        </xsl:variable>

        <xsl:if test="number($CSetCount)>0">
          <indexingCodeSets>
            <xsl:attribute name="count">
              <xsl:value-of select="$CSetCount"/>
            </xsl:attribute>
            <xsl:apply-templates select="./Codes/CodeSet"/>
          </indexingCodeSets>
        </xsl:if>

        <xsl:apply-templates select="./ColumnName"/>
        <xsl:apply-templates select="./Contact"/>
        <xsl:apply-templates select="./Copyright"/>
        <xsl:apply-templates select="./Corrections"/>
        <xsl:apply-templates select="./Headline"/>
        <xsl:apply-templates select="./LeadPara"/>
        <xsl:apply-templates select="./TailParas "/>
        <xsl:apply-templates select="./Notes"/>
        <xsl:apply-templates select="./SectionName"/>
        <xsl:apply-templates select ="./Fields" />
        <xsl:apply-templates select ="./Dict" />

        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubvol']"/>

        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='wordcount']"/>

        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='bestdate']"/>

        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='besttime']"/>

        <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='url']"/>

        <xsl:apply-templates select="./PropertySet[@group='replyitem']/Property[@name='snippet']/Snippet"/>

        <xsl:apply-templates select="./AdocTOC"/>
        <xsl:if test="string-length(normalize-space(@auxcount)) &gt; 0">
          <auxiliaryDocCount>
            <xsl:value-of select="normalize-space(string(@auxcount))"/>
          </auxiliaryDocCount>
        </xsl:if>
        <xsl:if test="string-length(normalize-space(@parent)) &gt; 0">
          <parentAccesionNumber>
            <xsl:value-of select="normalize-space(string(@parent))"/>
          </parentAccesionNumber>
        </xsl:if>
        <xsl:if test ="count(@segment) > 0 and string-length(normalize-space(@segment)) &gt; 0">
          <segmentIDs>
            <xsl:call-template name="splitSegment">
              <xsl:with-param name="string" select="normalize-space(string(@segment))" />
            </xsl:call-template>
          </segmentIDs >
        </xsl:if>
        <xsl:if test="string-length(normalize-space(@auxtype)) &gt; 0">
          <xsl:variable name="auxtype" select="normalize-space(string(@auxtype))" />
          <auxiliaryDocType>
            <xsl:choose>
              <xsl:when test="$auxtype = 's'">Summary</xsl:when>
              <xsl:when test="$auxtype = 'r'">RelatedLink</xsl:when>
              <xsl:otherwise>Unknown</xsl:otherwise>
            </xsl:choose>
          </auxiliaryDocType>
        </xsl:if>

        <!--//Apply metadataPT container-->
        <xsl:apply-templates select="./MetadataPT"/>
        <xsl:apply-templates select="./ArchiveDoc/Article"/>
        <xsl:apply-templates select="./DistDoc"></xsl:apply-templates>
      </xsl:if>
    </article>

  </xsl:template>

  <!--Start Distdoc section-->
  <xsl:template match="DistDoc">
    <xsl:apply-templates select="./Art"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='accessionno']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='baselang']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='doctype']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubedition']"/>
    <pages>
      <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubpage']" />
    </pages>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubdate']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubtime']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='pubgroupn']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='pubgroupc']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='publishern']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='ipdocid']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='charcount']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='loaddate']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='loadtime']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='moddate']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='modtime']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='origsource']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='revisionno']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='editor']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='authorlist']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srccode']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='attribcode']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='ipid']"/>

    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcname']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcprimarytype']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcsecondarytype']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcrightstype']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='allowtranslation']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='circulation']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='webhits']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='firstdate']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='regionoforigin']"/>

    <xsl:choose>
      <xsl:when test="./PropertySet[@group='pubdata']/Property[@name='logoimage']">
        <sourceLogo>
          <xsl:attribute name="image">
            <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logoimage']/@value)" />
          </xsl:attribute>
          <xsl:attribute name="link">
            <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logolink']/@value)" />
          </xsl:attribute>
          <xsl:attribute name="source">
            <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logosrc']/@value)" />
          </xsl:attribute>
        </sourceLogo>
      </xsl:when>
    </xsl:choose>

    <xsl:apply-templates select="./Byline"/>
    <xsl:apply-templates select="./Credit"/>

    <xsl:variable name="CSetCount">
      <xsl:value-of select="count(./Codes/CodeSet)"/>
    </xsl:variable>

    <xsl:if test="number($CSetCount)>0">
      <indexingCodeSets>
        <xsl:attribute name="count">
          <xsl:value-of select="$CSetCount"/>
        </xsl:attribute>
        <xsl:apply-templates select="./Codes/CodeSet"/>
      </indexingCodeSets>
    </xsl:if>

    <xsl:apply-templates select="./ColumnName"/>
    <xsl:apply-templates select="./Contact"/>
    <xsl:apply-templates select="./Copyright"/>
    <xsl:apply-templates select="./Corrections"/>
    <xsl:apply-templates select="./Headline"/>
    <xsl:apply-templates select="./LeadPara"/>
    <xsl:apply-templates select="./TailParas "/>
    <xsl:apply-templates select="./Notes"/>
    <xsl:apply-templates select="./SectionName"/>
    <xsl:apply-templates select ="./Fields" />
    <xsl:apply-templates select ="./Dict" />

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubvol']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='wordcount']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='bestdate']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='besttime']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='url']"/>

    <xsl:apply-templates select="./PropertySet[@group='replyitem']/Property[@name='snippet']/Snippet"/>

    <xsl:apply-templates select="./AdocTOC"/>
    <xsl:if test="string-length(normalize-space(@auxcount)) &gt; 0">
      <auxiliaryDocCount>
        <xsl:value-of select="normalize-space(string(@auxcount))"/>
      </auxiliaryDocCount>
    </xsl:if>
    <xsl:if test="string-length(normalize-space(@parent)) &gt; 0">
      <parentAccesionNumber>
        <xsl:value-of select="normalize-space(string(@parent))"/>
      </parentAccesionNumber>
    </xsl:if>
    <xsl:if test ="count(@segment) > 0 and string-length(normalize-space(@segment)) &gt; 0">
      <segmentIDs>
        <xsl:call-template name="splitSegment">
          <xsl:with-param name="string" select="normalize-space(string(@segment))" />
        </xsl:call-template>
      </segmentIDs >
    </xsl:if>
    <xsl:if test="string-length(normalize-space(@auxtype)) &gt; 0">
      <xsl:variable name="auxtype" select="normalize-space(string(@auxtype))" />
      <auxiliaryDocType>
        <xsl:choose>
          <xsl:when test="$auxtype = 's'">Summary</xsl:when>
          <xsl:when test="$auxtype = 'r'">RelatedLink</xsl:when>
          <xsl:otherwise>Unknown</xsl:otherwise>
        </xsl:choose>
      </auxiliaryDocType>
    </xsl:if>

    <!--//Apply metadataPT container-->
    <xsl:apply-templates select="./MetadataPT"/>
    <xsl:apply-templates select="./ArchiveDoc/Article"/>
  </xsl:template>
  <!--EndDistdoc section-->
  <xsl:template match="ArchiveDoc/Article">

    <xsl:apply-templates select="./Art"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='accessionno']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='baselang']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='doctype']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubedition']"/>
    <pages>
      <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubpage']" />
    </pages>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubdate']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubtime']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='pubgroupn']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='pubgroupc']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='publishern']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='ipdocid']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='charcount']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='loaddate']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='loadtime']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='moddate']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='modtime']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='origsource']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='revisionno']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='editor']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='authorlist']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srccode']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='attribcode']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='ipid']"/>

    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcname']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcprimarytype']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcsecondarytype']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcrightstype']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='allowtranslation']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='circulation']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='webhits']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='firstdate']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='regionoforigin']"/>

    <xsl:choose>
      <xsl:when test="./PropertySet[@group='pubdata']/Property[@name='logoimage']">
        <sourceLogo>
          <xsl:attribute name="image">
            <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logoimage']/@value)" />
          </xsl:attribute>
          <xsl:attribute name="link">
            <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logolink']/@value)" />
          </xsl:attribute>
          <xsl:attribute name="source">
            <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logosrc']/@value)" />
          </xsl:attribute>
        </sourceLogo>
      </xsl:when>
    </xsl:choose>

    <xsl:apply-templates select="./Byline"/>
    <xsl:apply-templates select="./Credit"/>

    <xsl:variable name="CSetCount">
      <xsl:choose>
        <xsl:when test="count(./Codes/CodeSet) > 0">
          <xsl:value-of select="count(./Codes/CodeSet)"/>
        </xsl:when>
        <xsl:when test="count(./CodeSets/CSet) > 0">
          <xsl:value-of select="count(./CodeSets/CSet)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="count(./Codes/CodeSet)>0">
        <xsl:if test="number($CSetCount)>0">
          <indexingCodeSets>
            <xsl:attribute name="count">
              <xsl:value-of select="$CSetCount"/>
            </xsl:attribute>
            <xsl:apply-templates select="./Codes/CodeSet"/>
          </indexingCodeSets>
        </xsl:if>
      </xsl:when>
      <xsl:when test="count(./CodeSets/CSet)>0">
        <xsl:if test="number($CSetCount)>0">
          <indexingCodeSets>
            <xsl:attribute name="count">
              <xsl:value-of select="$CSetCount"/>
            </xsl:attribute>
            <xsl:apply-templates select="./CodeSets/CSet"/>
          </indexingCodeSets>
        </xsl:if>
      </xsl:when>
    </xsl:choose>


    <xsl:apply-templates select="./ColumnName"/>
    <xsl:apply-templates select="./Contact"/>
    <xsl:apply-templates select="./Copyright"/>
    <xsl:apply-templates select="./Corrections"/>
    <xsl:apply-templates select="./HandL/Title/Headline"/>
    <xsl:apply-templates select="./HandL/LeadPara"/>
    <xsl:apply-templates select="./TailParas "/>
    <xsl:apply-templates select="./Notes"/>
    <xsl:apply-templates select="./SectionName"/>
    <xsl:apply-templates select ="./Fields" />
    <xsl:apply-templates select ="./Dict" />

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubvol']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='wordcount']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='bestdate']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='besttime']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='url']"/>

    <xsl:apply-templates select="./PropertySet[@group='replyitem']/Property[@name='snippet']/Snippet"/>

    <xsl:apply-templates select="./AdocTOC"/>
    <xsl:if test="string-length(normalize-space(@auxcount)) &gt; 0">
      <auxiliaryDocCount>
        <xsl:value-of select="normalize-space(string(@auxcount))"/>
      </auxiliaryDocCount>
    </xsl:if>
    <xsl:if test="string-length(normalize-space(@parent)) &gt; 0">
      <parentAccesionNumber>
        <xsl:value-of select="normalize-space(string(@parent))"/>
      </parentAccesionNumber>
    </xsl:if>
    <xsl:if test ="count(@segment) > 0 and string-length(normalize-space(@segment)) &gt; 0">
      <segmentIDs>
        <xsl:call-template name="splitSegment">
          <xsl:with-param name="string" select="normalize-space(string(@segment))" />
        </xsl:call-template>
      </segmentIDs >
    </xsl:if>
    <xsl:if test="string-length(normalize-space(@auxtype)) &gt; 0">
      <xsl:variable name="auxtype" select="normalize-space(string(@auxtype))" />
      <auxiliaryDocType>
        <xsl:choose>
          <xsl:when test="$auxtype = 's'">Summary</xsl:when>
          <xsl:when test="$auxtype = 'r'">RelatedLink</xsl:when>
          <xsl:otherwise>Unknown</xsl:otherwise>
        </xsl:choose>
      </auxiliaryDocType>
    </xsl:if>
  </xsl:template>

  <xsl:template match="MetadataPT">

    <xsl:apply-templates select="./Art"/>
    <xsl:apply-templates select="./Properties"/>

    <xsl:apply-templates select="./Byline"/>
    <xsl:apply-templates select="./Credit"/>

    <xsl:variable name="CSetCount">
      <xsl:choose>
        <xsl:when test="count(./Codes/CodeSet) > 0">
          <xsl:value-of select="count(./Codes/CodeSet)"/>
        </xsl:when>
        <xsl:when test="count(./CodeSets/CSet) > 0">
          <xsl:value-of select="count(./CodeSets/CSet)"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="count(./Codes/CodeSet)>0">
        <xsl:if test="number($CSetCount)>0">
          <indexingCodeSets>
            <xsl:attribute name="count">
              <xsl:value-of select="$CSetCount"/>
            </xsl:attribute>
            <xsl:apply-templates select="./Codes/CodeSet"/>
          </indexingCodeSets>
        </xsl:if>
      </xsl:when>
      <xsl:when test="count(./CodeSets/CSet)>0">
        <xsl:if test="number($CSetCount)>0">
          <indexingCodeSets>
            <xsl:attribute name="count">
              <xsl:value-of select="$CSetCount"/>
            </xsl:attribute>
            <xsl:apply-templates select="./CodeSets/CSet"/>
          </indexingCodeSets>
        </xsl:if>
      </xsl:when>
    </xsl:choose>

    <xsl:apply-templates select="./ColumnName"/>
    <xsl:apply-templates select="./Contact"/>
    <xsl:apply-templates select="./Copyright"/>
    <xsl:apply-templates select="./Corrections"/>
    <xsl:apply-templates select="./Headline"/>
    <xsl:apply-templates select="./LeadPara"/>
    <xsl:apply-templates select="./TailParas "/>
    <xsl:apply-templates select="./Notes"/>
    <xsl:apply-templates select="./SectionName"/>
    <xsl:apply-templates select ="./Fields" />
    <xsl:apply-templates select ="./Dict" />



    <xsl:apply-templates select="./AdocTOC"/>
    <xsl:if test="string-length(normalize-space(@auxcount)) &gt; 0">
      <auxiliaryDocCount>
        <xsl:value-of select="normalize-space(string(@auxcount))"/>
      </auxiliaryDocCount>
    </xsl:if>
    <xsl:if test="string-length(normalize-space(@parent)) &gt; 0">
      <parentAccesionNumber>
        <xsl:value-of select="normalize-space(string(@parent))"/>
      </parentAccesionNumber>
    </xsl:if>
    <xsl:if test ="count(@segment) > 0 and string-length(normalize-space(@segment)) &gt; 0">
      <segmentIDs>
        <xsl:call-template name="splitSegment">
          <xsl:with-param name="string" select="normalize-space(string(@segment))" />
        </xsl:call-template>
      </segmentIDs >
    </xsl:if>
    <xsl:if test="string-length(normalize-space(@auxtype)) &gt; 0">
      <xsl:variable name="auxtype" select="normalize-space(string(@auxtype))" />
      <auxiliaryDocType>
        <xsl:choose>
          <xsl:when test="$auxtype = 's'">Summary</xsl:when>
          <xsl:when test="$auxtype = 'r'">RelatedLink</xsl:when>
          <xsl:otherwise>Unknown</xsl:otherwise>
        </xsl:choose>
      </auxiliaryDocType>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Properties">
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='accessionno']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='baselang']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='doctype']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubedition']"/>
    <pages>
      <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubpage']" />
    </pages>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubdate']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubtime']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='pubgroupn']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='pubgroupc']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='publishern']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='ipdocid']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='charcount']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='loaddate']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='loadtime']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='moddate']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='modtime']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='origsource']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='revisionno']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='editor']"/>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='authorlist']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srccode']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='attribcode']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='ipid']"/>

    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcname']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcprimarytype']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcsecondarytype']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='srcrightstype']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='allowtranslation']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='circulation']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='webhits']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='firstdate']"/>
    <xsl:apply-templates select="./PropertySet[@group='pubdata']/Property[@name='regionoforigin']"/>

    <xsl:choose>
      <xsl:when test="./PropertySet[@group='pubdata']/Property[@name='logoimage']">
        <sourceLogo>
          <xsl:attribute name="image">
            <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logoimage']/@value)" />
          </xsl:attribute>
          <xsl:attribute name="link">
            <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logolink']/@value)" />
          </xsl:attribute>
          <xsl:attribute name="source">
            <xsl:value-of select="normalize-space(./PropertySet[@group='pubdata']/Property[@name='logosrc']/@value)" />
          </xsl:attribute>
        </sourceLogo>
      </xsl:when>
    </xsl:choose>
    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='pubvol']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='wordcount']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='bestdate']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='besttime']"/>

    <xsl:apply-templates select="./PropertySet[@group='docdata']/Property[@name='url']"/>

    <xsl:apply-templates select="./PropertySet[@group='replyitem']/Property[@name='snippet']/Snippet"/>
  </xsl:template>

  <xsl:template match="Fields">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <fields>
        <xsl:value-of select="normalize-space(string(.))"/>
      </fields>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Dict">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <dict>
        <xsl:value-of select="normalize-space(string(.))"/>
      </dict>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PropertySet[@group='docdata']/Property[@name='accessionno']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <accessionNo>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </accessionNo>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PropertySet[@group='docdata']/Property[@name='doctype']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <docType>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </docType>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PropertySet[@group='docdata']/Property[@name='bestdate']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <bestDate>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </bestDate>
    </xsl:if>
  </xsl:template>
  <xsl:template match="PropertySet[@group='docdata']/Property[@name='besttime']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <bestTime>
        <xsl:value-of select="user:ChangeTimeFormat(normalize-space(string(@value)))"/>
      </bestTime>
    </xsl:if>
  </xsl:template>
  <xsl:template match="PropertySet[@group='docdata']/Property[@name='url']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <url>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </url>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PropertySet[@group='replyitem']/Property[@name='snippet']/Snippet">
    <snippet>
      <xsl:apply-templates select="Para"/>
    </snippet>
  </xsl:template>

  <xsl:template match="PropertySet[@group='docdata']/Property[@name='loaddate']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <loadDate>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </loadDate>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PropertySet[@group='docdata']/Property[@name='loadtime']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <loadTime>
        <xsl:value-of select="user:ChangeTimeFormat(normalize-space(string(@value)))"/>
      </loadTime>
    </xsl:if>
  </xsl:template>
  <xsl:template match="PropertySet[@group='docdata']/Property[@name='moddate']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <modDate>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </modDate>
    </xsl:if>
  </xsl:template>
  <xsl:template match="PropertySet[@group='docdata']/Property[@name='modtime']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <modTime>
        <xsl:value-of select="user:ChangeTimeFormat(normalize-space(string(@value)))"/>
      </modTime>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PropertySet[@group='docdata']/Property[@name='origsource']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <originalSource>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </originalSource>
    </xsl:if>
  </xsl:template>
  <xsl:template match="PropertySet[@group='docdata']/Property[@name='revisionno']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <revisionNumber>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </revisionNumber>
    </xsl:if>
  </xsl:template>
  <xsl:template match="PropertySet[@group='docdata']/Property[@name='editor']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <editor>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </editor>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PropertySet[@group='docdata']/Property[@name='authorlist']">
    <authors>
      <xsl:apply-templates select="AuthorList/Author"/>
    </authors>
  </xsl:template>

  <xsl:template match="AuthorList/Author">
    <author>
      <xsl:attribute name="enid">
        <xsl:value-of select="normalize-space(string(@enid))"/>
      </xsl:attribute>
      <xsl:attribute name="nnid">
        <xsl:value-of select="normalize-space(string(@nnid))"/>
      </xsl:attribute>
      <xsl:apply-templates select="ExtractedName"/>
      <xsl:apply-templates select="NormalizedName"/>
    </author>
  </xsl:template>

  <xsl:template match="ExtractedName">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <extractedName>
        <xsl:value-of select="normalize-space(string(.))"/>
      </extractedName>
    </xsl:if>
  </xsl:template>

  <xsl:template match="NormalizedName">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <normalizedName>
        <xsl:value-of select="normalize-space(string(.))"/>
      </normalizedName>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PropertySet[@group='docdata']/Property[@name='baselang']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <baseLanguage>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </baseLanguage>
    </xsl:if>
  </xsl:template>



  <xsl:template match="Codes/CodeSet">
    <codeSet>
      <xsl:attribute name="codeCategory">
        <xsl:value-of select="@codeCat"/>
      </xsl:attribute>
      <xsl:apply-templates select="Code"/>
    </codeSet>
  </xsl:template>

  <xsl:template match="CodeSets/CSet">
    <codeSet>
      <xsl:attribute name="codeCategory">
        <xsl:value-of select="@fid"/>
      </xsl:attribute>
      <xsl:apply-templates select="Code"/>
    </codeSet>
  </xsl:template>



  <xsl:template match="PropertySet[@group='docdata']/Property[@name='pubedition']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <edition>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </edition>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='docdata']/Property[@name='pubpage']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <page>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </page>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='docdata']/Property[@name='pubdate']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <publicationDate>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </publicationDate>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='docdata']/Property[@name='pubtime']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <publicationTime>
        <xsl:value-of select="user:ChangeTimeFormat(normalize-space(string(@value)))"/>
      </publicationTime>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='pubgroupn']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <publisherGroupName>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </publisherGroupName>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='pubgroupc']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <publisherGroupCode>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </publisherGroupCode>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='publishern']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <publisherName>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </publisherName>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='attribcode']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <attributionCode>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </attributionCode>
    </xsl:if>
  </xsl:template>




  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='ipid']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <informationProvider>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </informationProvider>
    </xsl:if>
  </xsl:template>




  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='srccode']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <sourceCode>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </sourceCode>
    </xsl:if>
  </xsl:template>


  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='srcname']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <sourceName>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </sourceName>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='srcprimarytype']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <primarySource>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </primarySource>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='srcsecondarytype']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <secondarySource>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </secondarySource>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='srcrightstype']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <sourceDistributionRights>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </sourceDistributionRights>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='allowtranslation']">
    <isTranslationAllowedBySource>
      <xsl:choose>
        <xsl:when test="normalize-space(@value) = 'Y'">
          true
        </xsl:when>
        <xsl:otherwise>
          false
        </xsl:otherwise>
      </xsl:choose>
    </isTranslationAllowedBySource>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='circulation']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <circulation>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </circulation>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='webhits']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <webHits>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </webHits>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='firstdate']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <firstPubDate>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </firstPubDate>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='pubdata']/Property[@name='regionoforigin']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <regionOfOrigin>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </regionOfOrigin>
    </xsl:if>
  </xsl:template>


  <xsl:template match="PropertySet[@group='docdata']/Property[@name='ipdocid']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <ipDocId>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </ipDocId>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='docdata']/Property[@name='charcount']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <charCount>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </charCount>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='docdata']/Property[@name='pubvol']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <volume>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </volume>
    </xsl:if>
  </xsl:template>



  <xsl:template match="PropertySet[@group='docdata']/Property[@name='wordcount']">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <wordCount>
        <xsl:value-of select="normalize-space(string(@value))"/>
      </wordCount>
    </xsl:if>
  </xsl:template>


  <xsl:template match="Art">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <artWork>
        <xsl:call-template name="articleContent"/>
      </artWork>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Byline">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <byline>
        <xsl:call-template name="articleContent"/>
      </byline>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Credit">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <credit>
        <xsl:call-template name="articleContent"/>
      </credit>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Code">
    <code>
      <xsl:attribute name="value">
        <xsl:value-of select="@value"/>
      </xsl:attribute>
      <xsl:apply-templates select="CodeD"/>
      <xsl:apply-templates select="CodeI"/>
    </code>
  </xsl:template>

  <xsl:template match="CodeD">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <codeDescription>
        <xsl:attribute name="lang">
          <xsl:value-of select="@lang"/>
        </xsl:attribute>
        <xsl:value-of select="normalize-space(string(.))"/>
      </codeDescription>
    </xsl:if>
  </xsl:template>

  <xsl:template match="CodeI">
    <xsl:if test="@org='map-djn'">
      <xsl:apply-templates select="CodeA"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="CodeA">
    <xsl:if test="@name='DJNCode'">
      <codeDJNCode>
        <xsl:value-of select="@value"/>
      </codeDJNCode>
    </xsl:if>
  </xsl:template>

  <xsl:template match="ColumnName">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <columnName>
        <xsl:call-template name="articleContent"/>
      </columnName>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Contact">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <contact>
        <xsl:call-template name="articleContent"/>
      </contact>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Copyright">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <copyright>
        <xsl:call-template name="articleContent"/>
      </copyright>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Corrections">
    <corrections>
      <xsl:apply-templates select="Para"/>
    </corrections>
  </xsl:template>

  <xsl:template match="Headline">
    <headline>
      <xsl:apply-templates select="Para"/>
    </headline>
  </xsl:template>

  <xsl:template match="HandL/Title/Headline">
    <headline>
      <xsl:apply-templates select="Para"/>
    </headline>
  </xsl:template>

  <xsl:template match="LeadPara">
    <leadParagraph>
      <xsl:apply-templates select="Para"/>
    </leadParagraph>
  </xsl:template>

  <xsl:template match="HandL/LeadPara">
    <leadParagraph>
      <xsl:apply-templates select="Para"/>
    </leadParagraph>
  </xsl:template>

  <xsl:template match="Notes">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <notes>
        <xsl:call-template name="articleContent"/>
      </notes>
    </xsl:if>
  </xsl:template>

  <xsl:template match="SectionName">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <sectionName>
        <xsl:call-template name="articleContent"/>
      </sectionName>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Logo">
    <xsl:if test="string-length(normalize-space(@img)) &gt; 0">
      <sourceLogo>
        <xsl:attribute name="image">
          <xsl:copy-of select="normalize-space(@img)"/>
        </xsl:attribute>
        <xsl:attribute name="link">
          <xsl:copy-of select="normalize-space(@link)"/>
        </xsl:attribute>
        <xsl:attribute name="source">
          <xsl:copy-of select="normalize-space(@src)"/>
        </xsl:attribute>

      </sourceLogo>
    </xsl:if>
  </xsl:template>

  <xsl:template match="TailParas">
    <tailParagraphs>
      <xsl:apply-templates select="Para"/>
    </tailParagraphs>
  </xsl:template>

  <xsl:template match="//Para">
    <xsl:if test="count(child::node()) > 0">
      <paragraph>
        <xsl:if test="@truncation!=''">
          <xsl:attribute name="truncation">
            <xsl:value-of select="concat(translate(substring(@truncation,1,1), 'p', 'P'), substring(@truncation,2))"/>
          </xsl:attribute>
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
        <xsl:call-template name="articleContent"/>
      </paragraph>
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
          <xsl:when test="@adoctype='webpage'">
            <xsl:value-of select="Item[@type='webpage']/@ref"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="Item[@type='arttext']/@ref"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:apply-templates select="Item"/>
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
            <xsl:attribute name="type">
              <xsl:choose>
                <xsl:when test="@type='arttext'">NewsArticle</xsl:when>
                <xsl:when test="@type='html'">HTML</xsl:when>
                <xsl:when test="@type='pdf'">PDF</xsl:when>
                <xsl:when test="@type='tnail'">ThumbNail</xsl:when>
                <xsl:when test="@type='fnail'">Final</xsl:when>
                <xsl:when test="@type='dispix'">Display</xsl:when>
                <xsl:when test="@type='prtpix'">Print</xsl:when>
                <xsl:when test="@type='bigpix'">Full</xsl:when>
                <xsl:when test="@type='webpage'">URL</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="@type"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="string-length(@subtype)!=0">
            <xsl:attribute name="subType">
              <xsl:value-of select="@subtype"/>
            </xsl:attribute>
          </xsl:if>

          <xsl:if test="string-length(@mimetype)!=0">
            <xsl:attribute name="mimeType">
              <xsl:value-of select="@mimetype"/>
            </xsl:attribute>
          </xsl:if>

          <xsl:if test="string-length(@size)!=0">
            <xsl:attribute name="size">
              <xsl:value-of select="@size"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:attribute name="reference">
            <xsl:value-of select="@ref"/>
            <!--<xsl:choose>
              <xsl:when test="parent::AdocTOC/@adoctype='file' and @type='html'">
                <xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_text_html:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="@type='arttext'">
                    <xsl:value-of select="@ref"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='html'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_text_html:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='pdf'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_application_pdf:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='tnail'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_thumbnail:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='dispix'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_display:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='prtpix'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_print:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='bigpix'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_full:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='webpage'">
                    <xsl:value-of select="@ref"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="@ref"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>-->
          </xsl:attribute>
        </part>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="articleContent">
    <!--local variable added to skip Text under HighlightedText element for Entity Reference node-->
    <xsl:param name ="skipTextInHiglightedText" select="false()"/>
    <xsl:for-each select="child::node()">
      <xsl:choose>

        <xsl:when test="(local-name()='ELink')">
          <xsl:if test="@type='webpage'">
            <eLink>
              <xsl:attribute name="type">
                <xsl:value-of select="@type"/>
              </xsl:attribute>
              <xsl:attribute name="reference">
                <xsl:value-of select="@ref"/>
              </xsl:attribute>
              <text>
                <xsl:value-of select="."/>
              </text>
            </eLink>
          </xsl:if>
          <xsl:if test="@type='company'">
            <eLink>
              <xsl:attribute name="type">
                <xsl:value-of select="@type"/>
              </xsl:attribute>
              <xsl:attribute name="reference">
                <xsl:value-of select="@ref"/>
              </xsl:attribute>
              <xsl:call-template name="articleContent"/>
            </eLink>
          </xsl:if>
          <xsl:if test="@type='pro'">
            <eLink>
              <xsl:attribute name="type">
                <xsl:value-of select="@type"/>
              </xsl:attribute>
              <xsl:attribute name="reference">
                <xsl:value-of select="@ref"/>
              </xsl:attribute>
              <xsl:if test="normalize-space(@caption) != ''">
                <caption>
                  <xsl:value-of select="@caption"/>
                </caption>
              </xsl:if>
              <xsl:apply-templates select="./Item"/>
            </eLink>
          </xsl:if>
          <xsl:if test="@type='doc'">
            <eLink>
              <xsl:attribute name="type">
                <xsl:value-of select="@type"/>
              </xsl:attribute>
              <xsl:attribute name="reference">
                <xsl:value-of select="@ref"/>
              </xsl:attribute>
              <xsl:call-template name="articleContent"/>
            </eLink>
          </xsl:if>
        </xsl:when>
        <xsl:when test="(local-name()='hlt1') or (local-name()='hlt')">
          <xsl:if test="string-length(string(.)) &gt; 0">
            <hlt>
              <xsl:choose>
                <xsl:when test="$skipTextInHiglightedText">
                  <xsl:copy-of select="@*"/>
                  <xsl:value-of select="."/>
                </xsl:when>
                <xsl:otherwise>
                  <text>
                    <xsl:copy-of select="@*"/>
                    <xsl:value-of select="."/>
                  </text>
                </xsl:otherwise>
              </xsl:choose>
            </hlt>
          </xsl:if>
        </xsl:when>
        <xsl:when test="(local-name()='en')">
          <entityRef>
            <xsl:attribute name="code">
              <xsl:value-of select="@ref"/>
            </xsl:attribute>
            <xsl:attribute name="category">
              <xsl:value-of select="@cat"/>
            </xsl:attribute>
            <xsl:attribute name="instRef">
              <xsl:value-of select="@instRef"/>
            </xsl:attribute>
            <xsl:call-template name="articleContent">
              <xsl:with-param name="skipTextInHiglightedText"  select="true()"/>
            </xsl:call-template>
          </entityRef>
        </xsl:when>
        <xsl:when test="(local-name()='ev')">
          <xsl:apply-templates select="."/>
        </xsl:when>
        <xsl:otherwise>
          <text>
            <xsl:value-of select="normalize-space(.)"/>
          </text>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>


  <xsl:template name="splitSegment">
    <xsl:param name="string" />
    <xsl:variable name="pattern" >,</xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($string, $pattern)">
        <xsl:if test="not(starts-with($string, $pattern))">
          <SegmentID>
            <xsl:value-of select="substring-before($string, $pattern)"/>
          </SegmentID>
        </xsl:if>
        <xsl:call-template name="splitSegment">
          <xsl:with-param name="string" select="substring-after($string,$pattern)" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <SegmentID>
          <xsl:value-of select="$string" />
        </SegmentID>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>

