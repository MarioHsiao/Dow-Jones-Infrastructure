<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>
	<xsl:include href="../common/commonElements.xslt"/>
	<xsl:include href="../common/email_auth_common_elements.xslt"/>
	<xsl:strip-space elements="*"/>
	<xsl:template match="/*">
		<SetupUserForEmailAddressLoginWithSessionResponse>
			<xsl:apply-templates select="ResultSet/Result"/>
		</SetupUserForEmailAddressLoginWithSessionResponse>
	</xsl:template>

	<xsl:template match="ResultSet/Result">
		<xsl:apply-templates select="//emailAuthToken"/>
	</xsl:template>

</xsl:stylesheet>
