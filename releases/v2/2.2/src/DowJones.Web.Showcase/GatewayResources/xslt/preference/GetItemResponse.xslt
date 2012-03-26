<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" cdata-section-elements="itemBlob"/>
	<msxsl:script language="JScript" implements-prefix="user"><![CDATA[
		function GetClassName(classID)
		{
			var strClassName="";
			switch(classID)
			{
				case "14": strClassName = "DateFormatPreferenceItem"; break;
				case "25": strClassName = "SearchLanguagePreferenceItem"; break;
				default: strClassName = "PreferenceItem"; break;
			}
			
			return strClassName;
		}
	]]></msxsl:script>
	
	<xsl:template match="/*">
		<xsl:element name="GetItemResponse">
			<xsl:copy-of select="Control"/>
			<xsl:copy-of select="Status"/>
			<items>
				<xsl:apply-templates select="//RESPONSE_LIST"/>
			</items>
		</xsl:element>
	</xsl:template>

	<xsl:template match="RESPONSE_LIST">
		<xsl:element name="item">
			<xsl:attribute name="xsi:type">
				<xsl:value-of disable-output-escaping="yes" select="user:GetClassName(string(ITEM_CLASS))"/>
			</xsl:attribute>
			<xsl:apply-templates select="ITEM_ID"/>
			<xsl:apply-templates select="ITEM_CLASS"/>
			<xsl:apply-templates select="ITEM_INSTANCE_NAME"/>
			<xsl:apply-templates select="ITEM_BLOB"/>
			<xsl:apply-templates select="GROUP_NAME"/>
			<xsl:apply-templates select="ITEM_TYPE"/>
			<xsl:apply-templates select="ITEM_SUBSCRIBE"/>
			<xsl:apply-templates select="ITEM_ACCESS"/>
			<xsl:apply-templates select="ITEM_MODIFIER"/>
			<xsl:apply-templates select="lcDate"/>
		</xsl:element>
	</xsl:template>
	
	<xsl:template match="ITEM_ID">
		<ItemID><xsl:value-of select="."/></ItemID>
	</xsl:template>
	
	<xsl:template match="ITEM_CLASS">
		<ClassID><xsl:value-of select="."/></ClassID>
	</xsl:template>
	
	<xsl:template match="ITEM_INSTANCE_NAME">
		<InstanceName><xsl:value-of select="."/></InstanceName>
	</xsl:template>
	
	<xsl:template match="ITEM_BLOB">
		<ItemBlob><xsl:value-of select="."/></ItemBlob>
	</xsl:template>
	
	<xsl:template match="GROUP_NAME">
		<GroupName><xsl:value-of select="."/></GroupName>
	</xsl:template>
	
	<xsl:template match="ITEM_TYPE">
		<ItemType><xsl:value-of select="."/></ItemType>
	</xsl:template>
	
	<xsl:template match="ITEM_SUBSCRIBE">
		<IsSubscribable>
			<xsl:choose>
				<xsl:when test=".='Y'">true</xsl:when>
				<xsl:otherwise>false</xsl:otherwise>
			</xsl:choose>
		</IsSubscribable>
	</xsl:template>
	
	<xsl:template match="ITEM_ACCESS">
		<IsAccessible>
			<xsl:choose>
				<xsl:when test=".='Y'">true</xsl:when>
				<xsl:otherwise>false</xsl:otherwise>
			</xsl:choose></IsAccessible>
	</xsl:template>
	
	<xsl:template match="ITEM_MODIFIER">
		<IsUpdatable>
			<xsl:choose>
				<xsl:when test=".='Y'">true</xsl:when>
				<xsl:otherwise>false</xsl:otherwise>
			</xsl:choose>
		</IsUpdatable>
	</xsl:template>
	
	<xsl:template match="lcDate">
		<LastModifiedDate><xsl:value-of select="."/></LastModifiedDate>
	</xsl:template>

</xsl:stylesheet>