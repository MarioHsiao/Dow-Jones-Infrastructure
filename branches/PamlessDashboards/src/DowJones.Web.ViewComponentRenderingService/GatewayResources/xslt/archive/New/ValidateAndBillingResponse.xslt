<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
  <msxsl:script language="JScript" implements-prefix="user">
    <![CDATA[
		
		function ChangeDateFormat(DateVal)
		{
			var RetVal = new String("");
			RetVal = DateVal.substr(0,4)+"-"+DateVal.substr(4,2)+"-"+DateVal.substr(6);
			return RetVal;
		}
		
		function ChangeTimeFormat(TimeVal)
		{
			var RetVal = new String("");
			var secStr = TimeVal.substr(6);
			RetVal = TimeVal.substr(0,2)+":"+TimeVal.substr(2,2)+":"+TimeVal.substr(4,2);
			if (secStr != null && secStr.length > 0)
			 RetVal = RetVal+"."+secStr;
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
  <xsl:template match="/ValidateAndBillResponse">
    <ValidateAndBillResponse>
      <xsl:apply-templates select="Status"/>
      <xsl:apply-templates select="Result"/>
    </ValidateAndBillResponse>
  </xsl:template>

  <xsl:template match="Status">
    <Status>
    <xsl:value-of select="number(@value)"/>
    </Status>
  </xsl:template>

  <xsl:template match="Result">
    <Result>
      <status>
        <xsl:value-of select="number(@status)"/>
      </status>
      <accessionNumber>
          <xsl:value-of select="@accessionno"/>
      </accessionNumber>
    </Result>
  </xsl:template>
</xsl:stylesheet>

