<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<xsl:include href="CommonInfo.xslt"/>
	<xsl:template match="/*">
		<GetEncryptedCredentialsResponse>
			<xsl:call-template name="GetError"/>
			<xsl:apply-templates select="//Result"/>

			<EncryptedCredentials>
				<xsl:value-of select="//Result/ENCRYPTED_CREDENTIALS" />
			</EncryptedCredentials>
			<AccountId>
				<xsl:value-of select="//Result/ACCOUNT_ID" />
			</AccountId>
			<ProductId>
				<xsl:value-of select="//Result/PRODUCT_ID" />
			</ProductId>
			<UserId>
				<xsl:value-of select="//Result/USER_ID" />
			</UserId>
		</GetEncryptedCredentialsResponse>
	</xsl:template>
			
</xsl:stylesheet>
