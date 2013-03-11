<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"  xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="user" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
  <xsl:import href="GetDocuments_WebType_Section.xslt"/>
  <xsl:import href="GetDocuments_PubType_Section.xslt"/>
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
			
			return RetVal + "Z";	
		}
		function ReplaceStr(strOrig, strFind, strReplace)
		{
			var RetVal = new String("");
			RetVal = strOrig.replace (strFind, strReplace);
			return RetVal;
		}
    	function ChangeDateFormatOld(DateVal)
		{
			var RetVal = new String("");
			RetVal = DateVal.substr(0,4) + "-" + DateVal.substr(4,2)+ "-" + DateVal.substr(6);
			return RetVal;
		}
		function ChangeTimeFormatOld(TimeVal)
		{
			var RetVal = new String("");
			var secStr = TimeVal.substr(6);
			RetVal = TimeVal.substr(0,2)+":"+TimeVal.substr(2,2)+":"+TimeVal.substr(4,2);
			if (secStr != null && secStr.length > 0)
			 RetVal = RetVal+"."+secStr;
			return RetVal + "Z";	
		}
    function prettyCasing(strValue)
    {
        var retval = new String("");
        retval = strValue.charAt(0).toUpperCase() + strValue.slice(1);
        return "" +retval; 
    }
		]]>
  </msxsl:script>
  <xsl:template match="GetArchiveObjectResponse">
    <GetDocumentResponse xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
      <xsl:apply-templates select="Status"/>
      <xsl:apply-templates select="ResultSet"/>
      <xsl:apply-templates select="ContinuationContext"></xsl:apply-templates>
    </GetDocumentResponse>
  </xsl:template>

  <xsl:template match="ContinuationContext">
    <rawData>
      <xsl:value-of select="normalize-space(.)"/>
    </rawData>
  </xsl:template>

  <xsl:template match="Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="ResultSet">
    <xsl:element name="documentResponseSet">
      <xsl:attribute name="status">
        <xsl:value-of select="parent::node()/Status/@value"/>
      </xsl:attribute>
      <xsl:element name="count">
        <xsl:value-of select="'0'"/>
        <xsl:if test="number(@count) > 0 ">
          <xsl:value-of select="@count"/>
        </xsl:if>
      </xsl:element>
      <xsl:if test="number(@count) > 0">       
          <xsl:apply-templates select="Result"/>          
      </xsl:if>
    </xsl:element>
  </xsl:template>

  <xsl:template match="Result">
    <xsl:choose>
      <xsl:when test="@doctype='blog' or @doctype='webpage'">
        <document  xsi:type="WebArticle">
          <xsl:apply-templates select="current()" mode="webarticle"/>
        </document>
      </xsl:when>
      <xsl:when test="@doctype='multimedia'">
        <document  xsi:type="MultimediaArticle">
          <xsl:apply-templates select="current()" mode="multimediaarticle"/>
        </document>
      </xsl:when>
      <xsl:otherwise>
        <document xsi:type="Article">
          <xsl:apply-templates select="current()" mode="publicationarticle"/>
        </document>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
