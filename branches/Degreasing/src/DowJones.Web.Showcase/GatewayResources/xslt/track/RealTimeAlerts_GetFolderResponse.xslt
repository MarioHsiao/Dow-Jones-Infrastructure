<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl user" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <msxsl:script language="JScript" implements-prefix="user">
    <![CDATA[
		
		function RemoveXSI(strOrig)
		{
			var RetVal = new String("");
			var temp = strOrig.Replace("<StructuredSearch", "<StructuredSearch xmlns=\"http://types.factiva.com/search\"");
			RetVal = temp.replace ("xsi:nil=\"true\"", "");
			
			return RetVal;
			
		}
		]]>
  </msxsl:script>
  <xsl:template match="/" xmlns="http://types.dowjones.net/foldermanager">
    <GetFolderResponse>
      <folderInfo>
        <xsl:for-each select="//rta:GetFolderResponse/rta:folderInfo/child::*" xmlns:rta="http://types.dowjones.net/foldermanager">
          <xsl:choose>
            <xsl:when test="local-name() = 'userQuery'">
              <xsl:value-of select="user:RemoveXSI(string(.))" disable-output-escaping="yes"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:copy-of select="."/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:for-each>
      </folderInfo>
    </GetFolderResponse>
  </xsl:template>
</xsl:stylesheet>
