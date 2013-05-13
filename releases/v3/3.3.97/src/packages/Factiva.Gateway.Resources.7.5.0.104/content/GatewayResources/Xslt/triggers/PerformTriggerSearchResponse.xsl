<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:triggerns="urn:dowjones:emg:tr:v1_0" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" omit-xml-declaration="no" />

  <msxsl:script implements-prefix='user' language='C#'>
    <msxsl:assembly name="System.Web"/>
    <![CDATA[
          public XmlNode ParceXml(string xmlString, string name)
          {
              System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
              
              xmldoc.LoadXml(String.Format("<{0}>{1}</{0}>", name, xmlString));
              return xmldoc.DocumentElement;
          }
        
        
          public string ReplaceStr(string strOrig, string strFind, string strReplace)
          {
              return ( strOrig.Replace( strFind, strReplace ) );
          }     
    ]]>
  </msxsl:script>

  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="triggerns:documentHeadline">
    <contentHeadline xmlns="http://types.factiva.com/search">
      <xsl:if test="string-length(normalize-space(@accessionNo)) > 0">
        <accessionNo>
          <xsl:value-of select="@accessionNo" />
        </accessionNo>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(@wordCount)) > 0">
        <wordCount>
          <xsl:value-of select="@wordCount"/>
        </wordCount>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(@baseLanguage)) > 0">
        <baseLanguage>
          <xsl:value-of select="@baseLanguage"/>
        </baseLanguage>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:ipDocumentId)) > 0">
        <ipDocumentID>
          <xsl:value-of select="triggerns:ipDocumentId"/>
        </ipDocumentID>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:pubDate)) > 0">
        <publicationDate>
          <xsl:value-of select="triggerns:pubDate"/>
        </publicationDate>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:pubTime)) > 0">
        <publicationTime>
          <xsl:value-of select="triggerns:pubTime"/>
        </publicationTime>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:sourceCode)) > 0">
        <sourceCode>
          <xsl:value-of select="triggerns:sourceCode"/>
        </sourceCode>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:sourceName)) > 0">
        <sourceName>
          <xsl:value-of select="triggerns:sourceName"/>
        </sourceName>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:sectionName)) > 0">
        <sectionName>
          <para>
            <xsl:value-of select="triggerns:sectionName"/>
          </para>
        </sectionName>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:columnName)) > 0">
        <columnName>
          <para>
            <xsl:value-of select="triggerns:columnName"/>
          </para>
        </columnName>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:byline)) > 0">
        <byline>
          <para>
            <xsl:value-of select="triggerns:byline"/>
          </para>
        </byline>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:credit)) > 0">
        <credit>
          <para>
            <xsl:value-of select="triggerns:credit"/>
          </para>
        </credit>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:copyright)) > 0">
        <copyright>
          <para>
            <xsl:value-of select="triggerns:copyright"/>
          </para>
        </copyright>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:headline)) > 0">
        <xsl:apply-templates select="triggerns:headline" mode="convert"/>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:snippet)) > 0">
        <xsl:apply-templates select="triggerns:snippet" mode="convert"/>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:adocTOC)) > 0">
        <xsl:apply-templates select="triggerns:adocTOC" mode="convert">
          <xsl:with-param name="an">
            <xsl:value-of select="@accessionNo"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:if>
      <xsl:if test="string-length(normalize-space(triggerns:truncationRules)) > 0">
        <xsl:apply-templates select="triggerns:truncationRules" mode="convert"/>
      </xsl:if>
    </contentHeadline>
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

  <xsl:template match="triggerns:headline" mode="convert">
    <xsl:variable name="innertext">
      <xsl:value-of select="." disable-output-escaping="yes"/>
    </xsl:variable>
    <xsl:variable name="headline" select="user:ParceXml($innertext, 'headline')"/>
    <headline xmlns="http://types.factiva.com/search">
      <para>
        <xsl:for-each select="msxsl:node-set($headline)//Para">
          <xsl:value-of select="."/>.
        </xsl:for-each>
      </para>
    </headline>
  </xsl:template>

  <xsl:template match="triggerns:snippet" mode="convert">
    <xsl:variable name="innertext">
      <xsl:value-of select="." disable-output-escaping="yes"/>
    </xsl:variable>
    <xsl:variable name="snip" select="user:ParceXml($innertext, 'snippet')"/>
    <snippet xmlns="http://types.factiva.com/search">
      <para>
        <xsl:for-each select="msxsl:node-set($snip)//Para">
          <xsl:value-of select="."/>.
        </xsl:for-each>
      </para>
    </snippet>
  </xsl:template>

  <xsl:template match="triggerns:adocTOC" mode="convert">
    <xsl:param  name="an"/>
    <xsl:variable name="innertext">
      <xsl:value-of select="." disable-output-escaping="yes"/>
    </xsl:variable>
    <xsl:variable name="adoctoc" select="user:ParceXml($innertext, 'adocTOC')"/>
    <!--Xform AdocTOC to ContentReferences-->
    <contentItems xmlns="http://types.factiva.com/search">
      <xsl:element name="contentType">
        <xsl:value-of select="msxsl:node-set($adoctoc)/AdocTOC/@adoctype"/>
      </xsl:element>
      <xsl:element name="primaryRef">
        <xsl:choose>
          <xsl:when test="msxsl:node-set($adoctoc)/AdocTOC/@adoctype='webpage'">
            <xsl:choose>
              <xsl:when test="string-length(normalize-space(msxsl:node-set($adoctoc)/AdocTOC/Item[@type='arttext']/@ref)) > 0">
                <xsl:value-of select="msxsl:node-set($adoctoc)/AdocTOC/Item[@type='arttext']/@ref"/>/<xsl:value-of select="$an"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="msxsl:node-set($adoctoc)/AdocTOC/Item[@type='webpage']/@ref"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="msxsl:node-set($adoctoc)/AdocTOC/@adoctype='analyst'">
            distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="$an"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="msxsl:node-set($adoctoc)/AdocTOC/Item[@type='arttext']/@ref"/>/<xsl:value-of select="$an"/>
          </xsl:otherwise>
        </xsl:choose>
        <!--<xsl:value-of select="@adoctype"/>-->
      </xsl:element>
      <!--<xsl:if test="not(@adoctype='file' and Item/@type!='html')">-->
      <xsl:for-each select="msxsl:node-set($adoctoc)/AdocTOC/Item">
        <xsl:call-template name="Item" >
          <xsl:with-param name="an" select="$an"/>
        </xsl:call-template>
      </xsl:for-each>
      <!--</xsl:if>-->
    </contentItems>
  </xsl:template>

  <xsl:template name="Item" >
    <xsl:param  name="an"/>
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
              <xsl:value-of select="@type"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="string-length(@subtype)!=0">
            <xsl:attribute name="subType">
              <xsl:value-of select="@subtype"/>
            </xsl:attribute>
          </xsl:if>

          <xsl:if test="string-length(@mimetype)!=0 or @type='ivtxco' or @type='ivtxin'">
            <!-- 06-13-2005 : SM changes to supress html references information-->
            <!--09-12-2006" HD adding the HTML ref again -->
            <xsl:choose>
              <xsl:when test="@type='ivtxco'">
                <xsl:attribute name="mimeType">application/pdf</xsl:attribute>
              </xsl:when>
              <xsl:when test="@type='ivtxin'">
                <xsl:attribute name="mimeType">application/pdf</xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="mimeType">
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
          <xsl:attribute name="ref">
            <xsl:choose>
              <xsl:when test="@type='html'">
                <xsl:value-of select="@ref"/>
              </xsl:when>
              <xsl:when test="@type='pdf'">
                <xsl:value-of select="@ref"/>
              </xsl:when>
              <xsl:when test="@type='tnail'">
                <xsl:value-of select="@ref"/>
              </xsl:when>
              <xsl:when test="@type='fnail'">
                <xsl:value-of select="@ref"/>
              </xsl:when>
              <xsl:when test="@type='dispix'">
                <xsl:value-of select="@ref"/>
              </xsl:when>
              <xsl:when test="@type='prtpix'">
                <xsl:value-of select="@ref"/>
              </xsl:when>
              <xsl:when test="@type='bigpix'">
                <xsl:value-of select="@ref"/>
              </xsl:when>
              <xsl:when test="@type='webpage'">
                <xsl:value-of select="@ref"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="@ref"/>/<xsl:value-of select="$an"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </item>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="triggerns:truncationRules" mode="convert">
    <xsl:variable name="truncrules" select="user:ParceXml(text(), 'truncationRules')"/>
    <truncationRules xmlns="http://types.factiva.com/search">
      <xsl:apply-templates select="msxsl:node-set($truncrules)/child::node()"/>
    </truncationRules>
  </xsl:template>

  <xsl:template match="XS">
    <extraSmall>
      <xsl:value-of select="normalize-space(@value)"/>
    </extraSmall>
  </xsl:template>
  <xsl:template match="S">
    <small>
      <xsl:value-of select="normalize-space(@value)"/>
    </small>
  </xsl:template>
  <xsl:template match="M">
    <medium>
      <xsl:value-of select="normalize-space(@value)"/>
    </medium>
  </xsl:template>
  <xsl:template match="L">
    <large>
      <xsl:value-of select="normalize-space(@value)"/>
    </large>
  </xsl:template>

</xsl:stylesheet>

