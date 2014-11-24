<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
                xmlns:xs="http://www.w3.org/2001/XMLSchema" 
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" 
                xmlns:fn="http://www.w3.org/2005/xpath-functions"
                xmlns:user="user"
                 exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no" omit-xml-declaration="yes"/>
  
  <xsl:include href="../common/FolderSharing.xslt"/>
  <xsl:include href="common.xslt"/>
  <xsl:template match="Result">
    <xsl:choose>
      <xsl:when test="@status='0'">
        <folderList>
          <xsl:if test="string-length(normalize-space(../@foldertype)) &gt; 0">
            <isGroupFolder>
              <xsl:choose>
                <xsl:when test="normalize-space(../@foldertype)='user'">false</xsl:when>
                <xsl:otherwise>true</xsl:otherwise>
              </xsl:choose>
            </isGroupFolder>
          </xsl:if>
          <xsl:if test ="string-length(normalize-space(@dedupLevel)) &gt; 0">
            <deduplicationLevel>
              <xsl:choose>
                <xsl:when test="normalize-space(@dedupLevel)='OFF'">OFF</xsl:when>
                <xsl:when test="normalize-space(@dedupLevel)='SIMILAR'">SIMILAR</xsl:when>
                <xsl:when test="normalize-space(@dedupLevel)='VIRTUALLYIDENTICAL'">VIRTUALLYIDENTICAL</xsl:when>
                <xsl:otherwise>OFF</xsl:otherwise>
              </xsl:choose>
            </deduplicationLevel>
          </xsl:if>
          <xsl:choose>
            <xsl:when test="string-length(normalize-space(@folderid)) &gt; 0">
              <folderID>
                <xsl:value-of select="@folderid"/>
              </folderID>
            </xsl:when>
            <xsl:otherwise>
              <folderID/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:for-each select="*">
            <xsl:if test="name(.)='QueryName'">
              <xsl:if test="string-length(normalize-space(.)) &gt; 0">
                <folderName>
                  <xsl:value-of select="normalize-space(.)"/>
                </folderName>
              </xsl:if>
            </xsl:if>
            <xsl:if test="name(.)!='QueryName'">
              <xsl:if test="name(.)='QueryHighlight'">
                <xsl:if test="string-length(normalize-space(.)) &gt; 0">
                  <highlightString>
                    <xsl:value-of select="normalize-space(.)"/>
                  </highlightString>
                </xsl:if>
              </xsl:if>
            </xsl:if>
          </xsl:for-each>
          <xsl:if test="../@foldertype='user'">
            <xsl:if test="string-length(normalize-space(@queryhitscount)) &gt; 0">
              <queryHitsCount>
                <xsl:value-of select="normalize-space(@queryhitscount)"/>
              </queryHitsCount>
            </xsl:if>

            <xsl:if test="string-length(normalize-space(@deliveryMethod)) &gt; 0">
              <xsl:if test="@deliveryMethod='online'">
                <deliveryMethod>Online</deliveryMethod>
              </xsl:if>
              <xsl:if test="@deliveryMethod='continuous'">
                <deliveryMethod>Continuous</deliveryMethod>
              </xsl:if>
              <xsl:if test="@deliveryMethod='batch'">
                <deliveryMethod>Batch</deliveryMethod>
              </xsl:if>
            </xsl:if>
            <xsl:if test="string-length(normalize-space(@email)) &gt; 0">
              <email>
                <xsl:value-of select="@email"/>
              </email>
            </xsl:if>
            <xsl:if test="string-length(normalize-space(@documentFormat)) &gt; 0">
              <documentFormat>
                <xsl:value-of select="@documentFormat"/>
              </documentFormat>
            </xsl:if>
            <xsl:if test="string-length(normalize-space(@deliveryTimes)) &gt; 0">
              <xsl:value-of disable-output-escaping="yes" select="user:DeliveryTimesToList(normalize-space(string(@deliveryTimes)))"/>
            </xsl:if>
            <xsl:if test="string-length(normalize-space(@timeZone)) &gt; 0">
              <timeZone>
                <xsl:value-of select="@timeZone"/>
              </timeZone>
            </xsl:if>
          </xsl:if>
          <xsl:if test="./FolderSharing">
            <xsl:apply-templates select="./FolderSharing"/>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(@newhits)) &gt; 0">
            <newHits>
              <xsl:choose>
                <xsl:when test="@newhits='yes'">true</xsl:when>
                <xsl:when test="@newhits='y'">true</xsl:when>
                <xsl:otherwise>false</xsl:otherwise>
              </xsl:choose>
            </newHits>
          </xsl:if>

          <xsl:apply-templates select="@productType"/>
          <xsl:if test="string-length(normalize-space(@posthitscount)) &gt; 0">
            <postHitsCount>
              <xsl:value-of select="normalize-space(@posthitscount)"/>
            </postHitsCount>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(@unposthitscount)) &gt; 0">
            <unpostHitsCount>
              <xsl:value-of select="normalize-space(@unposthitscount)"/>
            </unpostHitsCount>
          </xsl:if>
          <xsl:if test="string-length(normalize-space(@postMethod)) &gt; 0">
            <editorPostMethod>
              <xsl:if test="normalize-space(@postMethod)='auto'">Automatic</xsl:if>
              <xsl:if test="normalize-space(@postMethod)='manual'">Manual</xsl:if>
            </editorPostMethod>
          </xsl:if>
          <!-- //sm- added missing items thate needed by UIs 04/15/09 -->
          <xsl:if test="Delivery/@ftype">
            <deduplicationLevel>
              <xsl:choose>
                <xsl:when test="Delivery/@ftype='0'">Off</xsl:when>
                <xsl:when test="Delivery/@ftype='SIMILAR'">Similar</xsl:when>
                <xsl:when test="Delivery/@ftype='VIRTUALLYIDENTICAL'">VirtuallyIdentical</xsl:when>
                <xsl:otherwise>Off</xsl:otherwise>
              </xsl:choose>
            </deduplicationLevel>
          </xsl:if>

          <!--<isGroupFolder>
            <xsl:choose>
              <xsl:when test="normalize-space(../@group)='true'">true</xsl:when>
              <xsl:otherwise>false</xsl:otherwise>
            </xsl:choose>
          </isGroupFolder>-->

        </folderList>
      </xsl:when>
      <xsl:otherwise>
        <folderList>
          <xsl:attribute name="status">
            <xsl:value-of select="@status"/>
          </xsl:attribute>
          <xsl:choose>
            <xsl:when test="string-length(normalize-space(@folderid)) &gt; 0">
              <folderID>
                <xsl:value-of select="@folderid"/>
              </folderID>
            </xsl:when>
            <xsl:otherwise>
              <folderID/>
            </xsl:otherwise>
          </xsl:choose>
        </folderList>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>

