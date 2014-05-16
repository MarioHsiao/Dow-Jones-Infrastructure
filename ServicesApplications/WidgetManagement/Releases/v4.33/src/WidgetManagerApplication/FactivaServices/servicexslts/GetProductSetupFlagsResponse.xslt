<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<msxsl:script language="JScript" implements-prefix="user"><![CDATA[
	function GetNumOfSetupAssistant(setupValue)
	{
		var arySetupValue = setupValue.split('|');
		return arySetupValue.length;
	}
function GetSetupAssistant(setupValue)
	{
		var arySetupValue;
		var arySingleSetupValue;
		var strSetupValue = "";
		if (setupValue!= null && setupValue.length > 0)
		{
			arySetupValue = setupValue.split('|');
			for (var i = 0; i < arySetupValue.length; i++)
			{
				strSetupValue = strSetupValue + '<productSetup>';
				arySingleSetupValue = arySetupValue[i].split('=');
				if (arySingleSetupValue.length > 1)
				{
					strSetupValue = strSetupValue + '<strPrefix>' + arySingleSetupValue[0] + '</strPrefix><bSetup>' + arySingleSetupValue[1] + '</bSetup>';
				}
				else
				{
					strSetupValue = strSetupValue + '<strPrefix>GL</strPrefix><bSetup>' + arySingleSetupValue[0] + '</bSetup>';
				}
				strSetupValue = strSetupValue + '</productSetup>';
			}
		}
			
		return strSetupValue;
	}
	]]></msxsl:script>
	<xsl:template match="/*">
		<xsl:element name="GetProductSetupFlagsResponse">
			<xsl:apply-templates select="//CLASS/ITEM/VALUE"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//CLASS/ITEM/VALUE">
		<xsl:element name="productSetupFlagsResultSet">
			<xsl:attribute name="count"><xsl:value-of select="user:GetNumOfSetupAssistant(string(.))"/></xsl:attribute>
			<xsl:value-of  disable-output-escaping="yes" select="user:GetSetupAssistant(string(.))"/>
			<xsl:apply-templates select="//ITEM_ID"/>
			<xsl:apply-templates select="//ITEM_INSTANCE_NAME"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//ITEM_ID">
		<xsl:element name="itemID">
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="//ITEM_INSTANCE_NAME">
		<xsl:element name="instanceName">
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>
