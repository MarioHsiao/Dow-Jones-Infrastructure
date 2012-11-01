<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl user" exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:include href="../common/ReplyItem.xslt"/>
  <xsl:include href="../common/FolderSharing.xslt"/>
  <xsl:include href="common.xslt"/>
  <xsl:template match="//Status">
    <xsl:copy-of select="."/>
  </xsl:template>
  <xsl:template match="//ResultSet">
    <folderHeadlinesResultSet>
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@count)) &gt; 0">
          <xsl:attribute name="count">
            <xsl:value-of select="@count"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="count">0</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:apply-templates select="Result" mode="folderHeadline"/>
    </folderHeadlinesResultSet>

    <xsl:choose>
      <xsl:when test="PerformContentSearchResponse">
        <xsl:apply-templates select="PerformContentSearch"/>
      </xsl:when>
      <xsl:otherwise>
        <!-- populate both folderheadlines and Perform content search response-->
        <xsl:choose>
          <xsl:when test="string-length(normalize-space(@count)) &gt; 0">
            <PerformContentSearchResponse>
              <contentSearchResult hitCount="{FolderInfo/@queryhitscount}" xmlns="http://types.factiva.com/search">
                <contentHeadlineResultSet count="{@count}" first="{FolderInfo/@first}"  >
                  <xsl:apply-templates select="Result" mode="contentHeadline"/>
                </contentHeadlineResultSet>
                <combinedSearchString/>
                <canonicalQueryString/>
                <queryTransformSet count="0"/>
                <codeNavigatorSet count="0"/>
                <timeNavigatorSet count="0"/>
              </contentSearchResult>
            </PerformContentSearchResponse>
          </xsl:when>
          <xsl:otherwise>
            <PerformContentSearchResponse>
              <contentSearchResult hitCount="0" xmlns="http://types.factiva.com/search">
                <combinedSearchString/>
                <canonicalQueryString/>
                <queryTransformSet count="0"/>
                <codeNavigatorSet count="0"/>
                <timeNavigatorSet count="0"/>
                <contentHeadlineResultSet count="0" first="0" />
              </contentSearchResult>
            </PerformContentSearchResponse>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="Result" mode="folderHeadline">
    <folderHeadline>
      <!--<xsl:attribute name="relevance"><xsl:value-of select="@rl"/></xsl:attribute>-->
      <xsl:apply-templates select="ReplyItem"/>
      <editor>
        <xsl:choose>
          <xsl:when test="normalize-space(@priority)='hot'">
            <priority>Hot</priority>
          </xsl:when>
          <xsl:when test="normalize-space(@priority)='new'">
            <priority>New</priority>
          </xsl:when>
          <xsl:when test="normalize-space(@priority)='must read'">
            <priority>MustRead</priority>
          </xsl:when>
          <xsl:when test="normalize-space(@priority)='none'">
            <priority>None</priority>
          </xsl:when>
          <xsl:otherwise>
            <priority>None</priority>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:if test="normalize-space(@comment) !='-1'">
          <comment>
            <xsl:value-of select="normalize-space(CommentText)"/>
          </comment>
        </xsl:if>
      </editor>
    </folderHeadline>
  </xsl:template>

  <xsl:template match="FolderInfo">
    <xsl:choose>
      <xsl:when test="@status='0'">
        <folder>

          <xsl:apply-templates select="@productType"/>

          <xsl:if test="string-length(normalize-space(QueryName)) &gt; 0">
            <folderName>
              <xsl:value-of select="normalize-space(QueryName)"/>
            </folderName>
          </xsl:if>
          <xsl:choose>
            <xsl:when test="string-length(normalize-space(@folderid)) &gt; 0">
              <folderID>
                <xsl:value-of select="normalize-space(@folderid)"/>
              </folderID>
            </xsl:when>
            <xsl:otherwise>
              <folderID/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="string-length(normalize-space(@dedupLevel)) &gt; 0">
              <deduplicationLevel>
                <xsl:choose>
                  <xsl:when test="normalize-space(@dedupLevel)='OFF'">OFF</xsl:when>
                  <xsl:when test="normalize-space(@dedupLevel)='SIMILAR'">SIMILAR</xsl:when>
                  <xsl:when test="normalize-space(@dedupLevel)='VIRTUALLYIDENTICAL'">VIRTUALLYIDENTICAL</xsl:when>
                  <xsl:otherwise>OFF</xsl:otherwise>
                </xsl:choose>
              </deduplicationLevel>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(QueryHighlight)) &gt; 0">
            <highlightString>
              <xsl:value-of select="normalize-space(QueryHighlight)"/>
            </highlightString>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(@bookmark)) &gt; 0">
            <bookmark>
              <xsl:value-of select="normalize-space(@bookmark)"/>
            </bookmark>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(@sessionmark)) &gt; 0">
            <sessionmark>
              <xsl:value-of select="normalize-space(@sessionmark)"/>
            </sessionmark>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(./Contact)) &gt; 0">
            <contact>
              <xsl:value-of select="normalize-space(./Contact)"/>
            </contact>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(@postMethod)) &gt; 0">
            <editorPostMethod>
              <xsl:if test="normalize-space(@postMethod)='auto'">Automatic</xsl:if>
              <xsl:if test="normalize-space(@postMethod)='manual'">Manual</xsl:if>
            </editorPostMethod>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(@queryhitscount)) &gt; 0">
            <queryHitCount>
              <xsl:value-of select="normalize-space(@queryhitscount)"/>
            </queryHitCount>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(@moreheadline)) &gt; 0">
            <moreHeadlines>
              <xsl:if test="normalize-space(@moreheadline)='yes'">true</xsl:if>
              <xsl:if test="normalize-space(@moreheadline)='no'">false</xsl:if>
            </moreHeadlines>
          </xsl:if>
          <xsl:apply-templates select="parent::ResultSet"/>
          <xsl:apply-templates select="../PerformContentSearchResponse"/>
          <xsl:if test="./FolderSharing">
            <xsl:apply-templates select="./FolderSharing"/>
          </xsl:if>
        </folder>
      </xsl:when>
      <xsl:otherwise>
        <folder>
          <xsl:attribute name="status">
            <xsl:value-of select="@status"/>
          </xsl:attribute>
          <xsl:choose>
            <xsl:when test="string-length(normalize-space(@folderid)) &gt; 0">
              <folderID>
                <xsl:value-of select="normalize-space(@folderid)"/>
              </folderID>
            </xsl:when>
            <xsl:otherwise>
              <folderID/>
            </xsl:otherwise>
          </xsl:choose>
        </folder>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="//folderHeadlinesResultSet">
    <xsl:variable name="count" select="GetNewsstandHeadlinesResponse/newsstandHeadlinesResponse/newsstandHeadlinesResultSet/section/sectionHeadlinesResultSet/@count"/>
  </xsl:template>

  <xsl:template match="PerformContentSearchResponse">
    <xsl:element name="PerformContentSearchResponse">
      <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
    </xsl:element>
  </xsl:template>

  <xsl:template match="node()" mode="AddNamespace">
    <xsl:choose>
      <xsl:when test="self::*">
        <xsl:choose>
          <xsl:when test="name() = 'contentHeadlinesResultSet'">
            <xsl:element name="contentHeadlineResultSet" namespace="http://types.factiva.com/search">
              <xsl:attribute name="count">
                <xsl:value-of select="@count"/>
              </xsl:attribute>
              <xsl:attribute name="first">
                <xsl:choose>
                  <xsl:when test="string-length(normalize-space(../@indexOfFirstHeadline)) > 0">
                    <xsl:value-of select="../@indexOfFirstHeadline"/>
                  </xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="name() = 'bucket'">
            <xsl:element name="bucket" namespace="http://types.factiva.com/search">
              <xsl:attribute name="id">
                <xsl:value-of select="@value"/>
              </xsl:attribute>
              <xsl:attribute name="hitCount">
                <xsl:value-of select="@hitCount"/>
              </xsl:attribute>
              <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="name() = 'primaryReference'">
            <xsl:element name="primaryRef" namespace="http://types.factiva.com/search">
              <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="name() = 'descriptor'">
            <xsl:value-of select="."/>
          </xsl:when>
          <xsl:when test="name() = 'paragraph'">
            <xsl:element name="para" namespace="http://types.factiva.com/search">
              <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="name() = 'featureVector'">
            <xsl:element name="documentVector" namespace="http://types.factiva.com/search">
              <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="name() = 'contentParts'">
            <xsl:element name="contentItems" namespace="http://types.factiva.com/search">
              <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="name() = 'copyright'">
            <xsl:element name="copyright" namespace="http://types.factiva.com/search">
              <xsl:element name="para" namespace="http://types.factiva.com/search">
                <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
              </xsl:element>
            </xsl:element>
          </xsl:when>
          <xsl:when test="name() = 'byline'">
            <xsl:if test="string-length(normalize-space(.)) > 0">
              <xsl:element name="byline" namespace="http://types.factiva.com/search">
                <xsl:element name="para" namespace="http://types.factiva.com/search">
                  <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
                </xsl:element>
              </xsl:element>
            </xsl:if>
          </xsl:when>
          <xsl:when test="name() = 'credit'">
            <xsl:if test="string-length(normalize-space(.)) > 0">
              <xsl:element name="credit" namespace="http://types.factiva.com/search">
                <xsl:element name="para" namespace="http://types.factiva.com/search">
                  <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
                </xsl:element>
              </xsl:element>
            </xsl:if>
          </xsl:when>
          <xsl:when test="name() = 'sectionName'">
            <xsl:if test="string-length(normalize-space(.)) > 0">
              <xsl:element name="sectionName" namespace="http://types.factiva.com/search">
                <xsl:element name="para" namespace="http://types.factiva.com/search">
                  <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
                </xsl:element>
              </xsl:element>
            </xsl:if>
          </xsl:when>
          <xsl:when test="name() = 'columnName'">
            <xsl:if test="string-length(normalize-space(.)) > 0">
              <xsl:element name="columnName" namespace="http://types.factiva.com/search">
                <xsl:element name="para" namespace="http://types.factiva.com/search">
                  <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
                </xsl:element>
              </xsl:element>
            </xsl:if>
          </xsl:when>
          <xsl:when test="name() = 'part'">
            <xsl:element name="item" namespace="http://types.factiva.com/search">
              <xsl:attribute name="type">
                <xsl:value-of select="@type" />
              </xsl:attribute>
              <xsl:attribute name="size">
                <xsl:value-of select="@size" />
              </xsl:attribute>
              <xsl:attribute name="ref">
                <xsl:value-of select="@reference" />
              </xsl:attribute>
              <xsl:if test="string-length(normalize-space(@mimeType)) > 0">
                <xsl:attribute name="mimetype">
                  <xsl:value-of select="@mimeType" />
                </xsl:attribute>
              </xsl:if>
              <xsl:if test="string-length(normalize-space(@subType)) > 0">
                <xsl:attribute name="subtype">
                  <xsl:value-of select="@subType" />
                </xsl:attribute>
              </xsl:if>
              <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:when>
          <!--CodeSets-->
          <xsl:when test="name() = 'codesets'">
            <xsl:element name="codeSets" namespace="http://types.factiva.com/search">
              <xsl:attribute name="count">
                <xsl:value-of select="count(CSet)" />
              </xsl:attribute>
              <xsl:apply-templates select="node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="name() = 'CSet'">
            <xsl:element name="codeSet" namespace="http://types.factiva.com/search">
              <xsl:attribute name="id">
                <xsl:value-of select="@fid" />
              </xsl:attribute>
              <xsl:apply-templates select="node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:when>
          <xsl:when test="name() = 'Code'">
            <xsl:element name="code" namespace="http://types.factiva.com/search">
              <xsl:attribute name="id">
                <xsl:value-of select="@value" />
              </xsl:attribute>
              <xsl:value-of select="."/>
            </xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <xsl:element name="{name()}" namespace="http://types.factiva.com/search">
              <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
            </xsl:element>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="@*" mode="AddNamespace">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" mode="AddNamespace"/>
    </xsl:copy>
  </xsl:template>

  <!-- New ones -->
  <xsl:template match="Result" mode="contentHeadline">
    <contentHeadline xmlns="http://types.factiva.com/search">
      <!-- Start accessionNo -->
      <xsl:if test="string-length(normalize-space(@accessionno)) > 0">
        <accessionNo>
          <xsl:value-of select="normalize-space(@accessionno)"/>
        </accessionNo>
      </xsl:if>
      <!-- Start ipDocumentID -->
      <xsl:if test="string-length(normalize-space(ReplyItem/IpDocId)) > 0">
        <ipDocumentID>
          <xsl:value-of select="normalize-space(ReplyItem/IpDocId)"/>
        </ipDocumentID>
      </xsl:if>
      <!-- Start wordCount -->
      <xsl:if test="string-length(normalize-space(ReplyItem/Num[@fid='wc']/@value)) > 0">
        <wordCount>
          <xsl:value-of select="normalize-space(ReplyItem/Num[@fid='wc']/@value)"/>
        </wordCount>
      </xsl:if>
      <!-- Start publicationDate -->
      <xsl:if test="string-length(normalize-space(ReplyItem/Date[@fid='pd']/@value)) > 0">
        <publicationDate>
          <xsl:value-of select="user:ChangeDateFormat(normalize-space(ReplyItem/Date[@fid='pd']/@value))"/>
        </publicationDate>
      </xsl:if>
      <!-- Start publicationTime -->
      <xsl:if test="string-length(normalize-space(ReplyItem/Time/@value)) > 0">
        <publicationTime>
          <xsl:value-of select="user:ChangeTimeFormatWithZ(normalize-space(ReplyItem/Time/@value))"/>
        </publicationTime>
      </xsl:if>
      <!-- Start baseLanguage -->
      <xsl:if test="string-length(normalize-space(ReplyItem/BaseLang[@fid='la']/@value)) > 0">
        <baseLanguage>
          <xsl:value-of select="normalize-space(ReplyItem/BaseLang[@fid='la']/@value)"/>
        </baseLanguage>
      </xsl:if>
      <!-- Start sourceCode -->
      <xsl:if test="string-length(normalize-space(ReplyItem/SrcCode[@fid='sc']/@value)) > 0">
        <sourceCode>
          <xsl:value-of select="normalize-space(ReplyItem/SrcCode[@fid='sc']/@value)"/>
        </sourceCode>
      </xsl:if>
      <!-- Start sourceName -->
      <xsl:if test="string-length(normalize-space(ReplyItem/SrcName[@fid='sn'])) > 0">
        <sourceName>
          <xsl:value-of select="normalize-space(ReplyItem/SrcName[@fid='sn'])"/>
        </sourceName>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(ReplyItem/Title/SectionName)) > 0">
        <sectionName>
          <para>
            <xsl:value-of select="normalize-space(ReplyItem/Title/SectionName)"/>
          </para>
        </sectionName>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(ReplyItem/Title/ColumnName)) > 0">
        <columnName>
          <para>
            <xsl:value-of select="normalize-space(ReplyItem/Title/ColumnName)"/>
          </para>
        </columnName>
      </xsl:if>
      <!-- Start headline -->
      <xsl:if test="count(ReplyItem/Title/Headline) > 0 and count(ReplyItem/Title/Headline/Para) > 0">
        <headline>
          <xsl:for-each select="ReplyItem/Title/Headline/Para">
            <para>
              <xsl:attribute name="lang">
                <xsl:value-of select="@lang"/>
              </xsl:attribute>
              <xsl:copy-of select="normalize-space(./text())"/>
            </para>
          </xsl:for-each>
        </headline>
      </xsl:if>
      <!-- Start snippet -->
      <xsl:if test="count(ReplyItem/Snippet) > 0 and count(ReplyItem/Snippet/Para) > 0">
        <snippet>
          <xsl:for-each select="ReplyItem/Snippet/Para">
            <para>
              <xsl:copy-of select="normalize-space(./text())"/>
            </para>
          </xsl:for-each>
        </snippet>
      </xsl:if>
      <!-- Start copyright -->
      <xsl:if test="string-length(normalize-space(ReplyItem/Copyright)) > 0">
        <copyright>
          <para>
            <xsl:value-of select="normalize-space(ReplyItem/Copyright)"/>
          </para>
        </copyright>
      </xsl:if>
      <!-- Start byline -->
      <xsl:if test="string-length(normalize-space(ReplyItem/Byline)) > 0">
        <byline>
          <para>
            <xsl:value-of select="normalize-space(ReplyItem/Byline)"/>
          </para>
        </byline>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(ReplyItem/Credit)) > 0">
        <credit>
          <para>
            <xsl:value-of select="normalize-space(ReplyItem/Credit)"/>
          </para>
        </credit>
      </xsl:if>
      <xsl:apply-templates select="AdocTOC" mode="thisAdocToc"/>
    </contentHeadline>
  </xsl:template>

  <xsl:template match="//contentParts">
    <contentItems>
      <xsl:element name="contentType">
        <xsl:choose>
          <xsl:when test="@contentType='Article'">article</xsl:when>
          <xsl:when test="@contentType='HTML'">file</xsl:when>
          <xsl:when test="@contentType='PDF'">pdf</xsl:when>
          <xsl:when test="@contentType='Picture'">picture</xsl:when>
          <xsl:when test="@contentType='WebPage'">webpage</xsl:when>
        </xsl:choose>
      </xsl:element>
      <xsl:element name="primaryRef">
        <xsl:value-of select="@primaryReference"/>
      </xsl:element>
      <xsl:copy-of select="./part"/>
    </contentItems>
  </xsl:template>


  <xsl:template match="AdocTOC" mode="thisAdocToc">
    <!--Xform AdocTOC to ContentReferences-->
    <contentItems xmlns="http://types.factiva.com/search">
      <xsl:element name="contentType">
        <xsl:choose>
          <xsl:when test="@adoctype='article'">Article</xsl:when>
          <xsl:when test="@adoctype='file'">HTML</xsl:when>
          <xsl:when test="@adoctype='pdf'">PDF</xsl:when>
          <xsl:when test="@adoctype='picture'">Picture</xsl:when>
          <xsl:when test="@adoctype='webpage'">
            <xsl:choose>
              <xsl:when test="string-length(normalize-space(Item[@type='arttext']/@ref)) > 0">WebPage</xsl:when>
              <xsl:otherwise>InternalWebPage</xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@adoctype='analyst'">PDF</xsl:when>
        </xsl:choose>
      </xsl:element>
      <xsl:element name="primaryRef">
        <xsl:choose>
          <xsl:when test="@adoctype='webpage'">
            <xsl:choose>
              <xsl:when test="string-length(normalize-space(Item[@type='arttext']/@ref)) > 0">
                <xsl:value-of select="Item[@type='arttext']/@ref"/>distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="ancestor::Result/@accessionno"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Item[@type='webpage']/@ref"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@adoctype='analyst'">
            distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="ancestor::report/@accessionno"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="Item[@type='arttext']/@ref"/>distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="ancestor::Result/@accessionno"/>
          </xsl:otherwise>
        </xsl:choose>
        <!--<xsl:value-of select="@adoctype"/>-->
      </xsl:element>
      <!--<xsl:if test="not(@adoctype='file' and Item/@type!='html')">-->
      <xsl:apply-templates select="Item" mode="thisItem"/>
      <!--</xsl:if>-->
    </contentItems>
  </xsl:template>

  <xsl:template match="Item" mode="thisItem">
    <xsl:choose>
      <xsl:when test="@type='invtext'">
        <!--Do Nothing-->
      </xsl:when>
      <xsl:when test="parent::AdocTOC/@adoctype='file' and @type!='html'">
        <!--Do Nothing-->
      </xsl:when>
      <xsl:otherwise>
        <item xmlns="http://types.factiva.com/search">
          <!-- only processing 2 attributes.. should all be xformed??-->
          <xsl:if test="string-length(@type)!=0">
            <xsl:attribute name="type">
              <!--newsArticle-->
              <xsl:choose>
                <xsl:when test="@type='arttext'">NewsArticle</xsl:when>
                <xsl:when test="@type='html'">HTML</xsl:when>
                <xsl:when test="@type='pdf'">PDF</xsl:when>
                <xsl:when test="@type='ivtxco'">PDF</xsl:when>
                <xsl:when test="@type='ivtxin'">PDF</xsl:when>
                <xsl:when test="@type='tnail'">ThumbNail</xsl:when>
                <xsl:when test="@type='fnail'">FingerNail</xsl:when>
                <xsl:when test="@type='dispix'">Display</xsl:when>
                <xsl:when test="@type='prtpix'">Print</xsl:when>
                <xsl:when test="@type='bigpix'">Full</xsl:when>
                <xsl:when test="@type='webpage'">WebPage</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="@type"/>
                </xsl:otherwise>
              </xsl:choose>
              <!--<xsl:value-of select="@type"/>-->
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="string-length(@subtype)!=0">
            <xsl:attribute name="subtype">
              <xsl:value-of select="@subtype"/>
            </xsl:attribute>
          </xsl:if>

          <xsl:if test="string-length(@mimetype)!=0 or @type='ivtxco' or @type='ivtxin'">
            <!-- 06-13-2005 : SM changes to supress html references information-->
            <!--09-12-2006" HD adding the HTML ref again -->
            <xsl:choose>
              <xsl:when test="@type='ivtxco'">
                <xsl:attribute name="mimetype">application/pdf</xsl:attribute>
              </xsl:when>
              <xsl:when test="@type='ivtxin'">
                <xsl:attribute name="mimetype">application/pdf</xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="mimetype">
                  <xsl:value-of select="@mimetype"/>
                </xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:if>
          <!-- SM: 06162005 set the size to be same as artText as the reference is such-->
          <xsl:if test="string-length(@size)!=0">
            <xsl:attribute name="size">
              <xsl:value-of select="@size"/>
            </xsl:attribute>
          </xsl:if>

          <!-- 06-13-2005 : SM changes to supress html references and create regular article reference even for type=html. this is because of bug rerported by ptg for invalid xml when charset not
						set to what is in the html.. fdk defaults to utf-8 and that is not good enough for some chars like Euro, which need western european charset-->
          <xsl:attribute name="ref">
            <xsl:choose>
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
                  <xsl:when test="@type='ivtxco'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'ivtx:gateway', 'probj_application_pdf:investext')"/>/<xsl:value-of select="ancestor::report/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='ivtxin'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'ivtx:gateway', 'probj_application_pdf:investext')"/>/<xsl:value-of select="ancestor::report/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='tnail'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_thumbnail:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
                  </xsl:when>
                  <xsl:when test="@type='fnail'">
                    <xsl:value-of select="user:ReplaceStr(string(@ref), 'probj:archive', 'probj_image_jpeg_fingernail:archive')"/>/<xsl:value-of select="ancestor::Result/@accessionno"/>
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
            </xsl:choose>
          </xsl:attribute>
        </item>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
