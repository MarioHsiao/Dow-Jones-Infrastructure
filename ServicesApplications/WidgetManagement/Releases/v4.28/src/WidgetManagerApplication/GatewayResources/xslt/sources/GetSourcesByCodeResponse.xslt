<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl user" exclude-result-prefixes="user">	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="TitleSearchResponse.xslt"/>


	<xsl:template match="/*">
		<GetSourcesByCodeResponse>
			<sourcesByCodeResponse>
					<xsl:call-template name="ResultSet"/>
			</sourcesByCodeResponse>
		</GetSourcesByCodeResponse>
	</xsl:template>

</xsl:stylesheet>
