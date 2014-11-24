<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <xsl:param name="category"/>
  <xsl:param name="transVersion"/>

  <msxsl:script language="JScript" implements-prefix="user">
    <![CDATA[
		
    function prettyCasing(strValue)
    {
        var retval = new String("");
        retval = strValue.charAt(0).toUpperCase() + strValue.slice(1);
        return "" +retval; 
    }
		]]>
  </msxsl:script>

  <xsl:template match="*">
    <xsl:apply-templates select="/*/Status"/>
    <xsl:apply-templates select="GetArchiveObjectResponse"/>
  </xsl:template>

  <xsl:template match="GetArchiveObjectResponse">
    <xsl:choose>
      <xsl:when test="$category='multimedia'">
        <xsl:element name="GetMultimediaArticleUrlResponse">
          <xsl:copy-of select="Status"/>
          <xsl:apply-templates select ="//ResultSet"/>
        </xsl:element>
      </xsl:when>
      <xsl:when test="$category='webpage'">
        <xsl:element name="GetWebArticleUrlResponse">
          <xsl:copy-of select="Status"/>
          <xsl:apply-templates select ="//ResultSet"/>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="GetWebArticleUrlResponse">
          <xsl:copy-of select="Status"/>
          <xsl:apply-templates select ="//ResultSet"/>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match ="/*/ResultSet">
    <xsl:choose>
      <xsl:when test="$category='multimedia'">
        <xsl:element name="multimediaArticleResultSet">
          <xsl:attribute name="count">
            <xsl:copy-of select="number(@count)"/>
          </xsl:attribute>
          <xsl:apply-templates select="Result"/>
        </xsl:element>
      </xsl:when>
      <xsl:when test="$category='webpage'">
        <xsl:element name="webArticleResultSet">
          <xsl:attribute name="count">
            <xsl:copy-of select="number(@count)"/>
          </xsl:attribute>
          <xsl:apply-templates select="Result"/>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="webArticleResultSet">
          <xsl:attribute name="count">
            <xsl:copy-of select="number(@count)"/>
          </xsl:attribute>
          <xsl:apply-templates select="Result"/>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="Result">
    <xsl:choose>
      <xsl:when test="$category='multimedia'">
        <xsl:element name="multimediaArticle">
          <xsl:call-template name="articleProperties"></xsl:call-template>
        </xsl:element>
      </xsl:when>
      <xsl:when test="$category='webpage'">
        <xsl:element name="webArticle">
          <xsl:call-template name="articleProperties"></xsl:call-template>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:element name="webArticle">
          <xsl:call-template name="articleProperties"></xsl:call-template>
        </xsl:element>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>



  <xsl:template name="articleProperties">
    <status>
      <xsl:copy-of select="number(@status)"/>
    </status>
    <xsl:if test="number(@status)!=0">
      <!--<xsl:attribute name="status"><xsl:value-of select="number(@status)"/></xsl:attribute>-->
      <accessionNo>
        <xsl:value-of select="@accessionno"/>
      </accessionNo>
      <reference>
        distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="@accessionno"/>
      </reference>
    </xsl:if>
    <xsl:if test="number(@status)=0">
      <accessionNo>
        <xsl:value-of select="@accessionno"/>
      </accessionNo>
      <reference>
        distdoc:archive/ArchiveDoc::Article/<xsl:value-of select="@accessionno"/>
      </reference>

      <xsl:choose>
        <xsl:when test="$transVersion='1'">
          <!-- Copy only URL to match V1 response -->
          <xsl:apply-templates select=".//Property[@name='url']"/>
        </xsl:when>
        <xsl:when test="$transVersion='2'">
          <xsl:apply-templates select=".//Property"/>
        </xsl:when>
      </xsl:choose>
      <xsl:apply-templates select="./AdocTOC"/>
    </xsl:if>
  </xsl:template>
  <xsl:template match="//Property">
    <xsl:if test="string-length(normalize-space(@value)) > 0">
      <xsl:element name="property">
        <xsl:attribute name="name">
          <xsl:value-of select ="@name"/>
        </xsl:attribute>
        <xsl:attribute name="value">
          <xsl:value-of select ="@value"/>
        </xsl:attribute>
      </xsl:element>
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
          <xsl:otherwise>
            <xsl:value-of select="user:prettyCasing(normalize-space(@adoctype))"/>
          </xsl:otherwise>
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
              <xsl:value-of select="format-number(@size,'0')"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:attribute name="reference">
            <xsl:choose>
              <xsl:when test="@type='tnail'and string-length(@ref)=0">
                <xsl:choose>
                  <xsl:when test="string-length(ancestor::Item[@type='dispix']/@ref)!=0">
                    <xsl:value-of select="ancestor::Item[@type='dispix']/@ref"/>
                  </xsl:when>
                  <xsl:when test="string-length(ancestor::Item[@type='prtpix']/@ref)!=0">
                    <xsl:value-of select="ancestor::Item[@type='prtpix']/@ref"/>
                  </xsl:when>
                  <xsl:when test="string-length(ancestor::Item[@type='bigpix']/@ref)!=0">
                    <xsl:value-of select="ancestor::Item[@type='bigpix']/@ref"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="@type='dispix'and string-length(@ref)=0">
                <xsl:choose>
                  <xsl:when test="string-length(ancestor::Item[@type='tnail']/@ref)!=0">
                    <xsl:value-of select="ancestor::Item[@type='tnail']/@ref"/>
                  </xsl:when>
                  <xsl:when test="string-length(ancestor::Item[@type='prtpix']/@ref)!=0">
                    <xsl:value-of select="ancestor::Item[@type='prtpix']/@ref"/>
                  </xsl:when>
                  <xsl:when test="string-length(ancestor::Item[@type='bigpix']/@ref)!=0">
                    <xsl:value-of select="ancestor::Item[@type='bigpix']/@ref"/>
                  </xsl:when>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="@ref"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </part>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="CreateImageReference">
    <xsl:param name="refType"/>
    <part>
      <xsl:attribute name="type">
        <xsl:choose>
          <xsl:when test="$refType='ThumbNail'">ThumbNail</xsl:when>
          <xsl:when test="$refType='Display'">Display</xsl:when>
        </xsl:choose>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="$refType='ThumbNail'">
          <xsl:choose>
            <xsl:when test="string-length(./Item[@type='dispix']/@ref)!=0">
              <xsl:apply-templates select="./Item[@type='dispix']" mode="ImageReference"/>
            </xsl:when>
            <xsl:when test="string-length(./Item[@type='prtpix']/@ref)!=0">
              <xsl:apply-templates select="./Item[@type='prtpix']" mode="ImageReference"/>
            </xsl:when>
            <xsl:when test="string-length(./Item[@type='bigpix']/@ref)!=0">
              <xsl:apply-templates select="./Item[@type='bigpix']" mode="ImageReference"/>
            </xsl:when>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="$refType='Display'">
          <xsl:choose>
            <xsl:when test="string-length(./Item[@type='tnail']/@ref)!=0">
              <xsl:apply-templates select="./Item[@type='tnail']" mode="ImageReference"/>
            </xsl:when>
            <xsl:when test="string-length(./Item[@type='prtpix']/@ref)!=0">
              <xsl:apply-templates select="./Item[@type='prtpix']" mode="ImageReference"/>
            </xsl:when>
            <xsl:when test="string-length(./Item[@type='bigpix']/@ref)!=0">
              <xsl:apply-templates select="./Item[@type='bigpix']" mode="ImageReference"/>
            </xsl:when>
          </xsl:choose>
        </xsl:when>
      </xsl:choose>
    </part>
  </xsl:template>
  <xsl:template match="Item" mode="ImageReference">
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
    </xsl:attribute>
  </xsl:template>
</xsl:stylesheet>