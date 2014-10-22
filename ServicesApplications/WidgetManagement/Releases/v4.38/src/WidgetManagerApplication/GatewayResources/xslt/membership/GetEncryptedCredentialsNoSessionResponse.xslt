<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:include href="CommonInfo.xslt"/>
	<xsl:template match="/*">
		<GetEncryptedCredentialsNoSessionResponse>
			<xsl:call-template name="GetError"/>
			<xsl:apply-templates select="//Result"/>
			<EncryptedCredentials>
				<xsl:value-of select="//Result/ENCRYPTED_CREDENTIALS"/>
			</EncryptedCredentials>
		</GetEncryptedCredentialsNoSessionResponse>
	</xsl:template>
</xsl:stylesheet>
