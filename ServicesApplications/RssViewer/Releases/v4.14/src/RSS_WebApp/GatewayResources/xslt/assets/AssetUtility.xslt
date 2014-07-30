<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:msxsl="urn:schemas-microsoft-com:xslt"
				xmlns:user="user"
				extension-element-prefixes="msxsl user"
				xmlns="http://global.factiva.com/fvs/1.0"
>
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

	<xsl:template name="SourceMapper">
		<xsl:param name="providerId"></xsl:param>
		<xsl:choose>
			<xsl:when test="$providerId = 'BVD'">BVD</xsl:when>
			<xsl:when test="$providerId = 'GALEB'">GBRANDS</xsl:when>
			<xsl:when test="$providerId = 'DBCUK'">DBCUK</xsl:when>
			<xsl:when test="$providerId = 'DBPCI'">DBPCI</xsl:when>
			<xsl:when test="$providerId = 'HARTE' or $providerId = 'HARTHNK' or $providerId = 'HHANKS'">HARTHNK</xsl:when>
			<xsl:when test="$providerId = 'DNB' or $providerId = 'DBTEL' or $providerId = 'DBFCP' or $providerId = 'DBPLS' or $providerId = 'DBPULT' or $providerId='BRDBT' or $providerId='DBDLD'">DNB</xsl:when>
			<xsl:when test="$providerId = 'HOV' or $providerId = 'HOOVERS' or $providerId = 'HOVFB' or $providerId = 'HOVFD' or $providerId = 'HOOVF'">Hoovers</xsl:when>
			<xsl:when test="$providerId = 'MRQUS' or $providerId='MARQUI'">Marquis</xsl:when>
			<xsl:when test="$providerId = 'RRSCH' or $providerId = 'MULTEX' or $providerId = 'RIVST' or $providerId = 'BRRIVST' or $providerId = 'Reuters'">ReutersResearch</xsl:when>
			<xsl:when test="$providerId = 'SPEXC' or $providerId = 'SPIND' or $providerId = 'STPR'">StandardAndPoorsRegister</xsl:when>
			<xsl:when test="$providerId = 'THFIN'">Thomson</xsl:when>
			<xsl:when test="$providerId = 'MULTI'">Multex</xsl:when>
			<xsl:when test="$providerId = 'HMTDD' or $providerId = 'HMSTT'">Hemscott</xsl:when>
			<xsl:when test="$providerId = 'Tradeline'">Tradeline</xsl:when>
			<xsl:when test="$providerId = 'ZOOM' or $providerId = 'ZOOMI'">ZoomInfo</xsl:when>
			<xsl:when test="$providerId = 'DATMON' or $providerId = 'DTMON' or $providerId = 'DMFCP'">DataMonitor</xsl:when>
			<xsl:when test="$providerId = 'EDJLF' or $providerId = 'WHOFR' or $providerId = 'WHOFC'">EditionsJacquesLafitte</xsl:when>
			<xsl:when test="$providerId = 'CRDNW' or $providerId ='RCDE'">CredInform</xsl:when>
			<xsl:when test="$providerId = 'HOPPF'">Hoppenstedt</xsl:when>
			<xsl:when test="$providerId = 'BRDBT' or $providerId = 'DBEUR' or $providerId = 'DBCEUR'">DBEUR</xsl:when>
      <xsl:when test="$providerId = 'DJ'">DowJones</xsl:when>
      <xsl:when test="$providerId = 'DJEditorial'">DowJonesEditorial</xsl:when>
      <xsl:when test="$providerId = 'DJGN'">Generate</xsl:when>
      <xsl:when test="$providerId = 'IUSA'">IUSA</xsl:when>
      <xsl:when test="$providerId = 'FACSET'">FACSET</xsl:when>
      <xsl:when test="$providerId = 'DJPM'">DowJonesVenture</xsl:when>
      <xsl:when test="$providerId = 'DJRC'">DJX</xsl:when>
      <xsl:otherwise>Unknown</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="ProviderShortName">
		<xsl:param name="fvsProviderId"></xsl:param>
		<xsl:choose>
			<xsl:when test="$fvsProviderId = 'BVD'">BvDEP</xsl:when>
			<xsl:when test="$fvsProviderId = 'GBRANDS'">Dow Jones &amp; Company</xsl:when>
			<xsl:when test="$fvsProviderId = 'DATMON'">DataMonitor</xsl:when>
			<xsl:when test="$fvsProviderId = 'DNB' or $fvsProviderId = 'DBCUK'">D&amp;B</xsl:when>
			<xsl:when test="$fvsProviderId='DBPCI'">D&amp;B PCI</xsl:when>
			<xsl:when test="$fvsProviderId = 'HARTE' or $fvsProviderId = 'HARTHNK' or $fvsProviderId = 'HHANKS'">Harte-Hanks</xsl:when>
			<xsl:when test="$fvsProviderId = 'Hoovers'">Hoover's Inc.</xsl:when>
			<xsl:when test="$fvsProviderId = 'Marquis'">Marquis Who's Who LLC</xsl:when>
			<xsl:when test="$fvsProviderId = 'ReutersResearch'">Reuters</xsl:when>
			<xsl:when test="$fvsProviderId = 'StandardAndPoorsRegister'">Standard &amp; Poor's</xsl:when>
			<xsl:when test="$fvsProviderId = 'Thomson'">Thomson Financial</xsl:when>
			<xsl:when test="$fvsProviderId = 'Multex'">Multex</xsl:when>
			<xsl:when test="$fvsProviderId = 'Hemscott'">Hemscott</xsl:when>
			<xsl:when test="$fvsProviderId = 'Tradeline'">Tradeline</xsl:when>
			<xsl:when test="$fvsProviderId = 'ZoomInfo'">ZoomInfo</xsl:when>
			<xsl:when test="$fvsProviderId = 'DataMonitor'">DataMonitor</xsl:when>
			<xsl:when test="$fvsProviderId = 'EditionsJacquesLafitte'">Who's Who in France</xsl:when>
			<xsl:when test="$fvsProviderId = 'CredInform'">Bonus-Service</xsl:when>
			<xsl:when test="$fvsProviderId = 'Hoppenstedt'">Hoppenstedt Firmendatenbank</xsl:when>
			<xsl:when test="$fvsProviderId = 'DBEUR' or $fvsProviderId = 'DCBEUR'">D&amp;B</xsl:when>
      <xsl:when test="$fvsProviderId = 'DowJones'">Generate, Inc.</xsl:when>
      <xsl:when test="$fvsProviderId = 'DowJonesEditorial'">Dow Jones Editorial</xsl:when>
      <xsl:when test="$fvsProviderId = 'Generate'">Generate, Inc.</xsl:when>
      <xsl:when test="$fvsProviderId = 'IUSA'">InfoUSA</xsl:when>
      <xsl:when test="$fvsProviderId = 'FACSET'">FactSet</xsl:when>
      <xsl:when test="$fvsProviderId = 'DowJonesVenture'">Dow Jones VentureSource</xsl:when>
      <xsl:when test="$fvsProviderId = 'DJX'">DJX</xsl:when>
    </xsl:choose>
	</xsl:template>
	<xsl:template name="ProviderLongName">
		<xsl:param name="fvsProviderId"></xsl:param>
		<xsl:choose>
			<xsl:when test="$fvsProviderId = 'BVD'">BvDEP</xsl:when>
			<xsl:when test="$fvsProviderId = 'GBRANDS'">Dow Jones &amp; Company</xsl:when>
			<xsl:when test="$fvsProviderId = 'DATMON'">DataMonitor</xsl:when>
			<xsl:when test="$fvsProviderId = 'DNB' or $fvsProviderId = 'DBCUK'">D&amp;B</xsl:when>
			<xsl:when test="$fvsProviderId='DBPCI'">D&amp;B PCI</xsl:when>
			<xsl:when test="$fvsProviderId = 'HARTE' or $fvsProviderId = 'HARTHNK' or $fvsProviderId = 'HHANKS'">Harte-Hanks</xsl:when>
			<xsl:when test="$fvsProviderId = 'Hoovers'">Hoover's Inc.</xsl:when>
			<xsl:when test="$fvsProviderId = 'Marquis'">Marquis Who's Who LLC</xsl:when>
			<xsl:when test="$fvsProviderId = 'ReutersResearch'">Reuters</xsl:when>
			<xsl:when test="$fvsProviderId = 'StandardAndPoorsRegister'">Standard &amp; Poor's, a division of The McGraw-Hill Companies, Inc.</xsl:when>
			<xsl:when test="$fvsProviderId = 'Thomson'">Thomson Financial Inc.</xsl:when>
			<xsl:when test="$fvsProviderId = 'Multex'">Multex</xsl:when>
			<xsl:when test="$fvsProviderId = 'Hemscott'">Hemscott</xsl:when>
			<xsl:when test="$fvsProviderId = 'Tradeline'">Tradeline</xsl:when>
			<xsl:when test="$fvsProviderId = 'ZoomInfo'">ZoomInfo</xsl:when>
			<xsl:when test="$fvsProviderId = 'DataMonitor'">DataMonitor</xsl:when>
			<xsl:when test="$fvsProviderId = 'EditionsJacquesLafitte'">Who's Who in France</xsl:when>
			<xsl:when test="$fvsProviderId = 'CredInform'">Bonus-Service</xsl:when>
			<xsl:when test="$fvsProviderId = 'Hoppenstedt'">Hoppenstedt Firmendatenbank</xsl:when>
			<xsl:when test="$fvsProviderId = 'DBEUR' or $fvsProviderId = 'DCBEUR'">D&amp;B</xsl:when>
      <xsl:when test="$fvsProviderId = 'DowJones'">Dow Jones</xsl:when>
      <xsl:when test="$fvsProviderId = 'DowJonesEditorial'">Dow Jones Editorial</xsl:when>
      <xsl:when test="$fvsProviderId = 'Generate'">Dow Jones Generate</xsl:when>
      <xsl:when test="$fvsProviderId = 'IUSA'">InfoUSA</xsl:when>
      <xsl:when test="$fvsProviderId = 'FACSET'">FactSet Research Systems Inc.</xsl:when>
      <xsl:when test="$fvsProviderId = 'DowJonesVenture'">Dow Jones VentureSource</xsl:when>
      <xsl:when test="$fvsProviderId = 'DJX'">DJX</xsl:when>
    </xsl:choose>
	</xsl:template>

	<xsl:template name="ProviderObject">
		<xsl:param name="providerId"></xsl:param>
		<xsl:if test="string-length($providerId) &gt; 0">
			<xsl:variable name="code">
				<xsl:call-template name="SourceMapper">
					<xsl:with-param name="providerId" select="$providerId" />
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="sName">
				<xsl:call-template name="ProviderShortName">
					<xsl:with-param name="fvsProviderId" select="$code" />
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="lName">
				<xsl:call-template name="ProviderLongName">
					<xsl:with-param name="fvsProviderId" select="$code" />
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="url">
				<xsl:call-template name="ProviderUrl">
					<xsl:with-param name="fvsProviderId" select="$code" />
				</xsl:call-template>
			</xsl:variable>
			<providers code="{$code}">
				<xsl:if test="string-length($sName) &gt; 0">
					<name>
						<xsl:value-of select="$sName" />
					</name>
				</xsl:if>
				<xsl:if test="string-length($lName) &gt; 0">
					<longDescriptor>
						<xsl:value-of select="$lName"/>
					</longDescriptor>
				</xsl:if>
				<xsl:if test="string-length($url) &gt; 0">
					<url>
						<xsl:value-of select="$url"/>
					</url>
				</xsl:if>
				<delistedSource>
					<xsl:call-template name="DelistedSourceMapper">
						<xsl:with-param name="sourceCode" select="$providerId" />
					</xsl:call-template>
				</delistedSource>
			</providers>
		</xsl:if>
	</xsl:template>

	<xsl:template name="ProviderUrl">
		<xsl:param name="fvsProviderId"></xsl:param>
		<xsl:choose>
			<xsl:when test="$fvsProviderId = 'DNB' or $fvsProviderId = 'DBCUK'">http://factiva.telebase.com/cgi-bin/writer.cgi/TermsnConditions.htm</xsl:when>
			<xsl:when test="$fvsProviderId = 'ReutersResearch'">http://about.reuters.com/productinfo/</xsl:when>
			<xsl:when test="$fvsProviderId = 'Tradeline'">http://www.tradeline.com/SunGardTermsandConditions.html</xsl:when>
			<xsl:when test="$fvsProviderId = 'Hoppenstedt'">http://www.hoppenstedt.com</xsl:when>
      <xsl:when test="$fvsProviderId = 'FACSET'">http://www.factset.com</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="DelistedSourceMapper">
		<xsl:param name="sourceCode"></xsl:param>
		<xsl:choose>
			<xsl:when test="$sourceCode = 'DBDLD'">true</xsl:when>
			<xsl:otherwise>false</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>

