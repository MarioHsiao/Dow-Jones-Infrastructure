<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<msxsl:script language="JScript" implements-prefix="user"><![CDATA[
	
function GetTimeOnly(timeValue)
	{
		var timeOnlyValue = "";

		if (timeValue!= null && timeValue.length > 0)
		{
			var start = timeValue.indexOf('|')-6;
			var end = timeValue.indexOf('|');
			timeOnlyValue = timeValue.substring(start, end);
		}
		else
		{
			timeOnlyValue = timeValue;
		}
			
		return timeOnlyValue;
	}
	]]></msxsl:script>
	<xsl:template match="/*">
		<xsl:element name="GetTimeZoneResponse">
			<xsl:apply-templates select="//CLASS/ITEM/VALUE"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//CLASS/ITEM/VALUE">
		<xsl:element name="timeZone">
			<xsl:value-of select="user:GetTimeOnly(string(.))"/>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>
