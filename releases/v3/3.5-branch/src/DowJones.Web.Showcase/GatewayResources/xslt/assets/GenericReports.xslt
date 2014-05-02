<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:user="user" xmlns:fcp="urn:factiva:fcp:v2_0" xmlns:msxsl="urn:schemas-microsoft-com:xslt" extension-element-prefixes="msxsl user fcp xsl" xmlns="http://global.factiva.com/fvs/1.0">
	<xsl:import href="AssetUtility.xslt"/>
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
	<msxsl:script implements-prefix="user" language="CSharp">
		<![CDATA[
			
		]]>
	</msxsl:script>
	<!--Entry/Main Template - used for branching			-->
	<xsl:template match="/">
		<reportResponse>
			<xsl:for-each select="/GetArchiveObjectResponse/ResultSet/Result/DistDoc">

				<xsl:if test ="./ArchiveDoc/registrations">
					<registrationReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>coreg</reportTypeCode>
						<xsl:call-template name="Registration">
							<xsl:with-param name="base" select="./ArchiveDoc/registrations"/>
						</xsl:call-template>
					</registrationReport>
				</xsl:if>
				
				<xsl:if test ="./ArchiveDoc/shareholders">
					<shareholdersReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>coshh</reportTypeCode>
						<xsl:call-template name="Shareholders">
							<xsl:with-param name="base" select="./ArchiveDoc/shareholders"/>
						</xsl:call-template>
					</shareholdersReport>					
				</xsl:if>
				<xsl:if test ="./ArchiveDoc/importExports">
					<importExportReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>coimp</reportTypeCode>
						<xsl:call-template name="ImportExport">
							<xsl:with-param name="base" select="./ArchiveDoc/importExports"/>
						</xsl:call-template>
					</importExportReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/keyFigures">
					<keyFiguresReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>cokfg</reportTypeCode>
						<xsl:call-template name="KeyFiguresHoppenstedt">
							<xsl:with-param name="base" select="./ArchiveDoc/keyFigures"/>
						</xsl:call-template>
					</keyFiguresReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/longBusinessDescription">
					<longBusinessDescriptionReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>colbd</reportTypeCode>
						<xsl:call-template name="LongBusinessDescription">
							<xsl:with-param name="base" select="./ArchiveDoc/longBusinessDescription"/>
						</xsl:call-template>
					</longBusinessDescriptionReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/companyStatement">
					<companyStatementReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>costa</reportTypeCode>
						<xsl:call-template name="CompanyStatement">
							<xsl:with-param name="base" select="./ArchiveDoc/companyStatement"/>
						</xsl:call-template>
					</companyStatementReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/swotAnalysis">
					<swotAnalysisReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>coswt</reportTypeCode>
						<xsl:call-template name="SwotAnalysis">
							<xsl:with-param name="base" select="./ArchiveDoc/swotAnalysis"/>
						</xsl:call-template>
					</swotAnalysisReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/subsidiariesAffiliates">
					<subsidiariesAffiliatesReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>cosub</reportTypeCode>
						<xsl:call-template name="SubsidiariesAffiliates">
							<xsl:with-param name="base" select="./ArchiveDoc/subsidiariesAffiliates"/>
						</xsl:call-template>
					</subsidiariesAffiliatesReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/corporateEvents">
					<corporateEventsReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>cosub</reportTypeCode>
						<xsl:call-template name="CorporateEvents">
							<xsl:with-param name="base" select="./ArchiveDoc/corporateEvents"/>
						</xsl:call-template>
					</corporateEventsReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/biography">
					<executiveBiographyReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>exbio</reportTypeCode>
						<xsl:call-template name="Biography">
							<xsl:with-param name="base" select="./ArchiveDoc/biography"/>
							<xsl:with-param name="meta" select="./MetadataPT"/>
						</xsl:call-template>
					</executiveBiographyReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/industryAggregates">
					<industryAggregatesReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>inrat</reportTypeCode>
						<xsl:call-template name="IndustryAggregates">
							<xsl:with-param name="base" select="./ArchiveDoc/industryAggregates"/>
						</xsl:call-template>
					</industryAggregatesReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/industrySectorAggregates">
					<industrySectorAggregatesReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>insrt</reportTypeCode>
						<xsl:call-template name="IndustrySectorAggregates">
							<xsl:with-param name="base" select="./ArchiveDoc/industrySectorAggregates"/>
						</xsl:call-template>
					</industrySectorAggregatesReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/indexAggregates">
					<indexAggregatesReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>in500</reportTypeCode>
						<xsl:call-template name="IndexAggregates">
							<xsl:with-param name="base" select="./ArchiveDoc/indexAggregates"/>
						</xsl:call-template>
					</indexAggregatesReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/companyInformation">
					<companyInformationReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>cogen</reportTypeCode>
						<xsl:call-template name="CompanyInformation">
							<xsl:with-param name="base" select="./ArchiveDoc/companyInformation"/>
							<xsl:with-param name="metaBase" select="./MetadataPT"/>
						</xsl:call-template>
					</companyInformationReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/balanceSheet">
					<genericReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>
							<xsl:variable name="fintype">
								<xsl:if test="translate(./ArchiveDoc/balanceSheet/@consolidation, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'unconsolidated'">coabu</xsl:if>
								<xsl:if test="translate(./ArchiveDoc/balanceSheet/@consolidation, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'consolidated'">coabc</xsl:if>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="$fintype = 'coabu' or $fintype = 'coabc'">
									<xsl:value-of select="$fintype"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="./ArchiveDoc/balanceSheet/fiscalPeriod/@type != 'Interim'">coabs</xsl:when>
										<xsl:otherwise>coibs</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</reportTypeCode>
						<reportGroup>
							<xsl:value-of select="./ArchiveDoc/balanceSheet/@group"/>
						</reportGroup>
						<xsl:variable name="cType">
							<xsl:choose>
								<xsl:when test="./ArchiveDoc/balanceSheet/@group">
									<xsl:value-of select="./ArchiveDoc/balanceSheet/@group"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="./ArchiveDoc/balanceSheet/@industryGroup"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:call-template name="BalanceSheet">
							<xsl:with-param name="base" select="./ArchiveDoc/balanceSheet"/>
							<xsl:with-param name="type" select="$cType"/>
							<xsl:with-param name="report">balanceSheet</xsl:with-param>
						</xsl:call-template>
					</genericReport>
				</xsl:if>
				<!-- temporary if statement to fix integration data problem-->
				<xsl:if test="./ArchiveDoc/annualCashFlow">
					<genericReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<xsl:variable name="cType">
							<xsl:choose>
								<xsl:when test="./ArchiveDoc/annualCashFlow/@group">
									<xsl:value-of select="./ArchiveDoc/annualCashFlow/@group"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="./ArchiveDoc/annualCashFlow/@industryGroup"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<reportTypeCode>
							<xsl:choose>
								<xsl:when test="./ArchiveDoc/annualCashFlow/fiscalPeriod/@type != 'Interim'">coacf</xsl:when>
								<xsl:otherwise>coicf</xsl:otherwise>
							</xsl:choose>
						</reportTypeCode>
						<xsl:call-template name="CashFlow">
							<xsl:with-param name="base" select="./ArchiveDoc/annualCashFlow"/>
							<xsl:with-param name="type" select="$cType"/>
							<xsl:with-param name="report">cashFlow</xsl:with-param>
						</xsl:call-template>
					</genericReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/cashFlow">
					<genericReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<xsl:variable name="cType">
							<xsl:choose>
								<xsl:when test="./ArchiveDoc/cashFlow/@group">
									<xsl:value-of select="./ArchiveDoc/cashFlow/@group"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="./ArchiveDoc/cashFlow/@industryGroup"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<reportTypeCode>
							<xsl:choose>
								<xsl:when test="./ArchiveDoc/cashFlow/fiscalPeriod/@type != 'Interim'">coacf</xsl:when>
								<xsl:otherwise>coicf</xsl:otherwise>
							</xsl:choose>
						</reportTypeCode>
						<xsl:call-template name="CashFlow">
							<xsl:with-param name="base" select="./ArchiveDoc/cashFlow"/>
							<xsl:with-param name="type" select="$cType"/>
							<xsl:with-param name="report">cashFlow</xsl:with-param>
						</xsl:call-template>
					</genericReport>
				</xsl:if>
				<!-- temporary if statement to fix integration data problem -->
				<xsl:if test="./ArchiveDoc/annualIncomeStatement">
					<genericReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<xsl:variable name="cType">
							<xsl:choose>
								<xsl:when test="./ArchiveDoc/annualIncomeStatement/@group">
									<xsl:value-of select="./ArchiveDoc/annualIncomeStatement/@group"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="./ArchiveDoc/annualIncomeStatement/@industryGroup"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<reportTypeCode>
							<xsl:choose>
								<xsl:when test="./ArchiveDoc/annualIncomeStatement/fiscalPeriod/@type != 'Interim'">coais</xsl:when>
								<xsl:otherwise>coiis</xsl:otherwise>
							</xsl:choose>
						</reportTypeCode>
						<xsl:value-of select="$cType"/>
						<xsl:call-template name="IncomeStatement">
							<xsl:with-param name="base" select="./ArchiveDoc/annualIncomeStatement"/>
							<xsl:with-param name="type" select="$cType"/>
							<xsl:with-param name="report">incomeStatement</xsl:with-param>
						</xsl:call-template>
					</genericReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/incomeStatement">
					<genericReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<xsl:variable name="cType">
							<xsl:choose>
								<xsl:when test="./ArchiveDoc/incomeStatement/@group">
									<xsl:choose>
										<!--<xsl:when test="translate(./ArchiveDoc/incomeStatement/@accountingStandard,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'hgb gesamtkosten'">
										HoppenstedtIndustryHGBGesamtkosten
										</xsl:when>-->									
										<xsl:when test="(./ArchiveDoc/incomeStatement/@accountingStandard)">
											<xsl:variable name="vGr">
												<xsl:value-of select="./ArchiveDoc/incomeStatement/@group"/>
											</xsl:variable>
											<xsl:variable name="vAcSt">
												<xsl:value-of select="./ArchiveDoc/incomeStatement/@accountingStandard"/>
											</xsl:variable>
											<xsl:value-of select="concat($vGr,'_',$vAcSt)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="./ArchiveDoc/incomeStatement/@group"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="./ArchiveDoc/incomeStatement/@industryGroup"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<reportGroup>
							<xsl:value-of select="./ArchiveDoc/incomeStatement/@group"/>
						</reportGroup>
						<reportTypeCode>
							<xsl:variable name="fintype">
								<xsl:if test="translate(./ArchiveDoc/incomeStatement/@consolidation, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'unconsolidated'">coaiu</xsl:if>
								<xsl:if test="translate(./ArchiveDoc/incomeStatement/@consolidation,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'consolidated'" >coaic</xsl:if>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="$fintype = 'coaiu' or $fintype = 'coaic'">
									<xsl:value-of select="$fintype"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="./ArchiveDoc/incomeStatement/fiscalPeriod/@type != 'Interim'">coais</xsl:when>
										<xsl:otherwise>coiis</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</reportTypeCode>
						<xsl:value-of select="$cType"/>
						<xsl:call-template name="IncomeStatement">
							<xsl:with-param name="base" select="./ArchiveDoc/incomeStatement"/>
							<xsl:with-param name="type" select="$cType"/>
							<xsl:with-param name="report">incomeStatement</xsl:with-param>
						</xsl:call-template>
					</genericReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/businessSegmentInformation or ./ArchiveDoc/geographicSegmentInformation">
					<segmentReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<xsl:if test="./ArchiveDoc/businessSegmentInformation ">
							<reportTypeCode>cobsb</reportTypeCode>
							<xsl:call-template name="businessSegmentInformation">
								<xsl:with-param name="base" select="./ArchiveDoc/businessSegmentInformation"/>
								<xsl:with-param name="report">businessSegmentInformation</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="./ArchiveDoc/geographicSegmentInformation">
							<reportTypeCode>cogsb</reportTypeCode>
							<xsl:call-template name="geographicSegmentInformation">
								<xsl:with-param name="base" select="./ArchiveDoc/geographicSegmentInformation"/>
								<xsl:with-param name="report">geographicSegmentInformation</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
					</segmentReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/ratiosA[@group='USData']">
					<usRatiosReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>corta</reportTypeCode>
						<xsl:call-template name="USRatios">
							<xsl:with-param name="base" select="./ArchiveDoc/ratiosA"/>
						</xsl:call-template>
					</usRatiosReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/ratiosA[@group='UKData']">
					<ukRatiosReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>corta</reportTypeCode>
						<xsl:call-template name="UKRatios">
							<xsl:with-param name="base" select="./ArchiveDoc/ratiosA"/>
						</xsl:call-template>
					</ukRatiosReport>
				</xsl:if>

				<xsl:if test="./ArchiveDoc/ratiosA[@group='DBEURData']">
					<ukRatiosReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>corta</reportTypeCode>
						<xsl:call-template name="UKRatios2">
							<xsl:with-param name="base" select="./ArchiveDoc/ratiosA"/>
							<xsl:with-param name="group">DBEURData</xsl:with-param>
						</xsl:call-template>
					</ukRatiosReport>
				</xsl:if>


				<xsl:if test="./ArchiveDoc/ratiosA[@group='NordicData']">
					<ukRatiosReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>corta</reportTypeCode>
						<xsl:call-template name="UKRatios2">
							<xsl:with-param name="base" select="./ArchiveDoc/ratiosA"/>
							<xsl:with-param name="group">NordicData</xsl:with-param>
						</xsl:call-template>
					</ukRatiosReport>
				</xsl:if>

				


				<xsl:if test="./ArchiveDoc/ratios">
					<keyRatiosReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>corat</reportTypeCode>
						<xsl:choose>
							<xsl:when test="./MetadataPT/PubData/SrcCode[@value = 'RCDE']">
								<xsl:call-template name="RussianRatios">
									<xsl:with-param name="base" select="./ArchiveDoc/ratios"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="KeyRatios">
									<xsl:with-param name="base" select="./ArchiveDoc/ratios"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>


					</keyRatiosReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/ratiosB">
					<genericReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>
							<xsl:variable name="fintype">
								<xsl:if test="translate(./ArchiveDoc/ratiosB/@consolidation,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'unconsolidated'">cortu</xsl:if>
								<xsl:if test="translate(./ArchiveDoc/ratiosB/@consolidation,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz') = 'consolidated'">cortc</xsl:if>
							</xsl:variable>
							<xsl:choose>								
								<xsl:when test="$fintype = 'cortu' or $fintype = 'cortc'">
									<xsl:value-of select="$fintype"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="_Undefined"></xsl:value-of>
								</xsl:otherwise>
							</xsl:choose>
						</reportTypeCode>
						<xsl:variable name="cType">
							<xsl:choose>
								<xsl:when test="./ArchiveDoc/ratiosB/@group">
									<xsl:value-of select="./ArchiveDoc/ratiosB/@group"/>
								</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<xsl:call-template name="RatiosBHoppenstedt">
							<xsl:with-param name="base" select="./ArchiveDoc/ratiosB"/>
							<xsl:with-param name="type" select="$cType"/>
							<xsl:with-param name="report">ratiosB</xsl:with-param>
						</xsl:call-template>
					</genericReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/executives">
					<executivesandOfficersReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>coexe</reportTypeCode>
						<xsl:call-template name="Executives">
							<xsl:with-param name="base" select="./ArchiveDoc/executives"/>
						</xsl:call-template>
					</executivesandOfficersReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/productsAndServices">
					<productAndServicesReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>copas</reportTypeCode>
						<xsl:call-template name="ProductAndServices">
							<xsl:with-param name="base" select="./ArchiveDoc/productsAndServices"/>
						</xsl:call-template>
					</productAndServicesReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/overviewAndHistory">
					<overviewAndHistoryReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>coovh</reportTypeCode>
						<xsl:call-template name="OverviewAndHistory">
							<xsl:with-param name="base" select="./ArchiveDoc/overviewAndHistory"/>
						</xsl:call-template>
					</overviewAndHistoryReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/keyCompetitors">
					<keyCompetitorsReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>
							<xsl:choose>
								<xsl:when test="./MetadataPT/PubData/SrcCode[@fid='sc' and @value='RRSCH']">cokcr</xsl:when>
								<xsl:otherwise>cokcs</xsl:otherwise>
							</xsl:choose>
						</reportTypeCode>
						<xsl:call-template name="KeyCompetitors">
							<xsl:with-param name="base" select="./ArchiveDoc/keyCompetitors"/>
						</xsl:call-template>
					</keyCompetitorsReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/customerInformation">
					<customerInformationReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>cocus</reportTypeCode>
						<xsl:call-template name="CustomerInformation">
							<xsl:with-param name="base" select="./ArchiveDoc/customerInformation"/>
						</xsl:call-template>
					</customerInformationReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/keyFinancials">
					<keyFinancialsReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>cokey</reportTypeCode>
						<xsl:call-template name="FinancialInformation">
							<xsl:with-param name="base" select="./ArchiveDoc/keyFinancials"/>
						</xsl:call-template>
					</keyFinancialsReport>
				</xsl:if>
				<xsl:if test="./ArchiveDoc/technical">
					<technologyInformationReport>
						<reportMetaData>
							<xsl:apply-templates select="./MetadataPT" mode="base"/>
						</reportMetaData>
						<reportTypeCode>cotec</reportTypeCode>
						<xsl:call-template name="TechnologyInformation">
							<xsl:with-param name="base" select="./ArchiveDoc/technical"/>
						</xsl:call-template>
					</technologyInformationReport>
				</xsl:if>
        <!-- // mt 08/2007 - add company brands -->
        <xsl:if test="./ArchiveDoc/tradeNames">
          <companyBrandsReport>
            <reportMetaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </reportMetaData>
            <reportTypeCode>cotrn</reportTypeCode>
            <xsl:call-template name="CompanyBrands">
              <xsl:with-param name="base" select="./ArchiveDoc/tradeNames"/>
            </xsl:call-template>
          </companyBrandsReport>
        </xsl:if>
      </xsl:for-each>
		</reportResponse>
	</xsl:template>
	<!-- Corporate Events [coevt] 							-->
	<xsl:template name="CorporateEvents">
		<xsl:param name="base"/>
		<corporateEventsReportData>
			<xsl:for-each select="$base/event">
				<events>
					<date>
						<date>
							<xsl:value-of select="./date"/>
						</date>
					</date>
					<title>
						<xsl:value-of select="./title"/>
					</title>
				</events>
			</xsl:for-each>
		</corporateEventsReportData>
	</xsl:template>
	<!-- Key Subsidiaries [cosub] 							-->
	<xsl:template name="SubsidiariesAffiliates">
		<xsl:param name="base"/>
		<subsidiariesAffiliatesReportData>
			<xsl:for-each select="$base/subsidiaryAffiliate">
				<subsidiariesAffiliates code="{./factivaCode/@code}">
					<name>
						<xsl:value-of select="./name"/>
					</name>				
				</subsidiariesAffiliates>				
			</xsl:for-each>
		</subsidiariesAffiliatesReportData>
		<credSubsidiariesAffiliatesReportData>
			<xsl:for-each select="$base/subsidiaryAffiliateInfo">
				<credSubsidiariesAffiliates>
					<code>
						<xsl:value-of select="./id/code/@code"></xsl:value-of>
					</code>
					<name>
						<xsl:value-of select="./id/name"/>
					</name>
					<country>
						<xsl:value-of select="./country"/>
					</country>
					<percentDirectlyHeld>
						<xsl:call-template name="rawData">
							<xsl:with-param name="dataNodeSet" select="./percentDirectlyHeld"/>
							<xsl:with-param name="formatType">Percent</xsl:with-param>
						</xsl:call-template>
					</percentDirectlyHeld>					
				</credSubsidiariesAffiliates>
			</xsl:for-each>
		</credSubsidiariesAffiliatesReportData>
	</xsl:template>
	
	<!-- Biography [exbio] 									-->
	<xsl:template name="Biography">
		<xsl:param name="base"/>
		<xsl:param name="meta"/>
		<biographyReportData>
			<generalInfo>
				<xsl:if test="$meta/PubData/SrcCode/@value = 'ZOOMI'">
					<providerDocumentId>
						<xsl:choose>
							<xsl:when test="string-length(normalize-space($meta/DocData/IPDocId[@fid='id'])) &gt; 0">
								<xsl:value-of select="$meta/DocData/IPDocId[@fid='id']"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$meta/DocData/IPDocId[@fid='id']/@value"/>
							</xsl:otherwise>
							<!-- Need until Angie make changes -->
						</xsl:choose>

					</providerDocumentId>
				</xsl:if>
				<xsl:call-template name="stripDownMetaData">
					<xsl:with-param name="meta" select="$meta"/>
				</xsl:call-template>
				<xsl:if test="boolean($base/companyInformation/name)">
					<company code="{$base/companyInformation/factivaCode/@code}">
						<name>
							<xsl:value-of select="$base/companyInformation/name"/>
						</name>
						<xsl:if test="boolean($base/companyInformation/address)">
							<xsl:call-template name="Address">
								<xsl:with-param name="baseAddress" select="$base/companyInformation/address"/>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="boolean($base/companyInformation/phoneNumber/number)">
							<phone>
								<xsl:value-of select="$base/companyInformation/phoneNumber/number"/>
							</phone>
						</xsl:if>
						<xsl:if test="boolean($base/companyInformation/phoneNumber/cityAreaCode)">
							<phoneAreaCode>
								<xsl:value-of select="$base/companyInformation/phoneNumber/cityAreaCode"/>
							</phoneAreaCode>
						</xsl:if>
						<xsl:if test="boolean($base/companyInformation/phoneNumber/countryRegionCode)">
							<phoneCountryCode>
								<xsl:value-of select="$base/companyInformation/phoneNumber/countryRegionCode"/>
							</phoneCountryCode>
						</xsl:if>
						<xsl:if test="boolean($base/companyInformation/faxNumber/number)">
							<fax>
								<xsl:value-of select="$base/companyInformation/faxNumber/number"/>
							</fax>
						</xsl:if>
						<xsl:if test="boolean($base/companyInformation/faxNumber/cityAreaCode)">
							<faxAreaCode>
								<xsl:value-of select="$base/companyInformation/faxNumber/cityAreaCode"/>
							</faxAreaCode>
						</xsl:if>
						<xsl:if test="boolean($base/companyInformation/faxNumber/countryRegionCode)">
							<faxCountryCode>
								<xsl:value-of select="$base/companyInformation/faxNumber/countryRegionCode"/>
							</faxCountryCode>
						</xsl:if>
						<xsl:if test="boolean($base/companyInformation/secretary)">
							<secretary>
								<xsl:apply-templates select="$base/companyInformation/secretary/name"/>
								<xsl:if test="boolean($base/companyInformation/secretary/phoneNumber)">
									<phoneNumber>
										<xsl:call-template name="PhoneNumber">
											<xsl:with-param name="baseNumber" select="$base/companyInformation/secretary/phoneNumber"/>
										</xsl:call-template>
									</phoneNumber>
								</xsl:if>
								<xsl:if test="boolean($base/companyInformation/secretary/faxNumber)">
									<faxNumber>
										<xsl:call-template name="faxNumber">
											<xsl:with-param name="baseNumber" select="$base/companyInformation/secretary/faxNumber"/>
										</xsl:call-template>
									</faxNumber>
								</xsl:if>
								<xsl:if test="boolean($base/companyInformation/secretary/email)">
									<email>
										<xsl:value-of select="$base/companyInformation/secretary/email"/>
									</email>
								</xsl:if>
							</secretary>
						</xsl:if>
						<xsl:call-template name="ProviderObject">
							<xsl:with-param name="providerId">
								<xsl:value-of select="$meta/PubData/SrcCode/@value" />
							</xsl:with-param>
						</xsl:call-template>
					</company>
				</xsl:if>
				<xsl:apply-templates select="$base/name"/>
				<xsl:if test="$base/email">
					<email>
						<xsl:value-of select="$base/email"/>
					</email>
				</xsl:if>
				<xsl:if test="$base/phone">
					<phone>
						<xsl:value-of select="$base/phone"/>
					</phone>
				</xsl:if>
				<xsl:if test="$base/fax">
					<fax>
						<xsl:value-of select="$base/fax"/>
					</fax>
				</xsl:if>
				<xsl:if test="$base/age">
					<age>
						<xsl:value-of select="$base/age"/>
					</age>
				</xsl:if>
				<xsl:if test="string-length( normalize-space($base/consolidationId)) > 0 and number($base/consolidationId) > 0">
					<consolidatedId>
						<xsl:value-of select="$base/consolidationId" />
					</consolidatedId>
				</xsl:if>
				<xsl:if test="boolean($base/@isBoardMember)">
					<isBoardMember>
						<xsl:value-of select="$base/@isBoardMember"/>
					</isBoardMember>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="$base/@level = 'Both'">
						<isDirector>true</isDirector>
						<isOfficer>true</isOfficer>
						<level>Both</level>
					</xsl:when>
					<xsl:when test="$base/@level = 'Director'">
						<isDirector>true</isDirector>
						<level>Director</level>
					</xsl:when>
					<xsl:when test="$base/@level = 'Officer'">
						<isOfficer>true</isOfficer>
						<level>Officer</level>
					</xsl:when>
					<xsl:otherwise>
						<level>Unspecified</level>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:for-each select="$base/jobTitle">
					<position>
						<name>
							<xsl:value-of select="."/>
						</name>
					</position>
				</xsl:for-each>
				<!-- Career - JobPosition -->
				<careerInfo>
					<xsl:if test="boolean($base/career/jobPosition)">
						<xsl:for-each select="$base/career/jobPosition">
							<jobPosition>
								<name>
									<xsl:choose>
										<xsl:when test="./title">
											<xsl:value-of select="./title"/>
										</xsl:when>
										<!-- leave this for old code... -->
										<xsl:when test="./description">
											<xsl:value-of select="./description"/>
										</xsl:when>
									</xsl:choose>
								</name>
								<xsl:if test="boolean(./description)">
									<description>
										<xsl:value-of select="./description"/>
									</description>
								</xsl:if>
								<xsl:if test="boolean(./startDate)">
									<startYear>
										<xsl:value-of select="./startDate/@year"/>
									</startYear>
								</xsl:if>
								<xsl:if test="boolean(./endDate)">
									<endYear>
										<xsl:value-of select="./endDate/@year"/>
									</endYear>
								</xsl:if>
								<xsl:if test="boolean(./company)">
									<company code="./company/@code">
										<name>
											<xsl:value-of select="./company/name"/>
										</name>
									</company>
								</xsl:if>
							</jobPosition>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="boolean($base/career/careerItem)">
						<xsl:for-each select="$base/career/careerItem">
							<careerItem>
								<description>
									<xsl:value-of select="./description"/>
								</description>
								<xsl:if test="boolean(./startDate)">
									<start>
										<xsl:call-template name="YearMonthDay">
											<xsl:with-param name="baseNode" select="./startDate"/>
										</xsl:call-template>
									</start>
								</xsl:if>
								<xsl:if test="boolean(./endDate)">
									<end>
										<xsl:call-template name="YearMonthDay">
											<xsl:with-param name="baseNode" select="./endDate"/>
										</xsl:call-template>
									</end>
								</xsl:if>
								<xsl:if test="boolean(./instantDate)">
									<instant>
										<xsl:call-template name="YearMonthDay">
											<xsl:with-param name="baseNode" select="./instantDate"/>
										</xsl:call-template>
									</instant>
								</xsl:if>
							</careerItem>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="boolean($base/career/tenureDates)">
						<xsl:for-each select="$base/career/tenureDates">
							<tenureDates>
								<xsl:if test="boolean(./officerStart)">
									<officerStart>
										<xsl:call-template name="YearMonthDay">
											<xsl:with-param name="baseNode" select="./officerStart"/>
										</xsl:call-template>
									</officerStart>
								</xsl:if>
								<xsl:if test="boolean(./officerEnd)">
									<officerEnd>
										<xsl:call-template name="YearMonthDay">
											<xsl:with-param name="baseNode" select="./officerEnd"/>
										</xsl:call-template>
									</officerEnd>
								</xsl:if>
								<xsl:if test="boolean(./directorStart)">
									<directorStart>
										<xsl:call-template name="YearMonthDay">
											<xsl:with-param name="baseNode" select="./directorStart"/>
										</xsl:call-template>
									</directorStart>
								</xsl:if>
								<xsl:if test="boolean(./directorEnd)">
									<directorEnd>
										<xsl:call-template name="YearMonthDay">
											<xsl:with-param name="baseNode" select="./directorEnd"/>
										</xsl:call-template>
									</directorEnd>
								</xsl:if>
							</tenureDates>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="boolean($base/career/directorship)">
						<xsl:for-each select="$base/career/directorship">
							<directorship>
								<xsl:value-of select="."/>
							</directorship>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="boolean($base/career/additionalInformation)">
						<additionalInformation>
							<xsl:value-of select="$base/career/additionalInformation"/>
						</additionalInformation>
					</xsl:if>
				</careerInfo>
			</generalInfo>
			<biography>
				<xsl:if test="boolean($base/gender)">
					<gender>
						<xsl:value-of select="$base/gender"/>
					</gender>
				</xsl:if>
				<xsl:if test="boolean($base/born)">
					<xsl:if test="string-length( normalize-space( $base/born/@year ) ) > 0">
						<birthYear>
							<xsl:value-of select="$base/born/@year"/>
						</birthYear>
					</xsl:if>
					<xsl:if test="string-length( normalize-space( $base/born/@year ) ) > 0 and string-length( normalize-space( $base/born/@month ) ) > 0 and string-length( normalize-space( $base/born/@day ) ) > 0">
						<birthDate>
							<date>
								<xsl:value-of select="$base/born/@year"/>-<xsl:value-of select="$base/born/@month"/>-<xsl:value-of select="$base/born/@day"/>
							</date>
						</birthDate>
					</xsl:if>
				</xsl:if>
				<xsl:if test="boolean($base/birthPlace)">
					<birthPlace>
						<xsl:call-template name="Address">
							<xsl:with-param name="baseAddress" select="$base/birthPlace"/>
						</xsl:call-template>
					</birthPlace>
				</xsl:if>
				<xsl:if test="$base/parents">
					<parents>
						<xsl:value-of select="$base/parents"/>
					</parents>
				</xsl:if>
				<xsl:if test="boolean($base/degree)">
					<education>
						<xsl:for-each select="$base/degree">
							<degree>
								<level>
									<xsl:value-of select="./@level"/>
								</level>
								<xsl:if test="boolean(./college)">
									<school>
										<xsl:value-of select="./college"/>
									</school>
								</xsl:if>
								<xsl:if test="string-length( normalize-space( ./qualification/@code ) ) > 0">
									<type>
										<xsl:value-of select="./qualification/@code"/>
									</type>
								</xsl:if>
								<xsl:if test="boolean(./graduationDate)">
									<xsl:if test="string-length( normalize-space( ./graduationDate ) ) > 0">
										<graduationDate>
											<date>
												<xsl:value-of select="./graduationDate"/>
											</date>
										</graduationDate>
									</xsl:if>
									<graduationYear>
										<xsl:value-of select="./graduationDate/@year"/>
									</graduationYear>
								</xsl:if>
								<xsl:if test="boolean(./major)">
									<major>
										<xsl:value-of select="./major"/>
									</major>
								</xsl:if>
							</degree>
						</xsl:for-each>
					</education>
				</xsl:if>
				<xsl:if test="boolean($base/education)">
					<xsl:for-each select="$base/education">
						<xsl:if test="string-length( normalize-space(.) ) > 0">
							<additionalEducation>
								<xsl:value-of select="."/>
							</additionalEducation>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="boolean($base/fraternity)">
					<fraternalMembership>
						<xsl:value-of select="$base/fraternity"/>
					</fraternalMembership>
				</xsl:if>
				<xsl:if test="boolean($base/trusteeships)">
					<trusteeships>
						<xsl:value-of select="$base/trusteeships"/>
					</trusteeships>
				</xsl:if>
				<xsl:if test="boolean($base/committees)">
					<committees>
						<xsl:value-of select="$base/committees"/>
					</committees>
				</xsl:if>
				<xsl:if test="boolean($base/awards)">
					<awards>
						<xsl:value-of select="$base/awards"/>
					</awards>
				</xsl:if>
				<xsl:if test="boolean($base/interests)">
					<interests>
						<xsl:value-of select="$base/interests"/>
					</interests>
				</xsl:if>
				<xsl:if test="boolean($base/nationality)">
					<nationality>
						<xsl:value-of select="$base/nationality"/>
					</nationality>
				</xsl:if>
				<xsl:if test="boolean($base/languages)">
					<languages>
						<xsl:value-of select="$base/languages"/>
					</languages>
				</xsl:if>
				<xsl:if test="boolean($base/politics)">
					<politicalMembership>
						<xsl:value-of select="$base/politics"/>
					</politicalMembership>
				</xsl:if>
				<xsl:if test="boolean($base/religion)">
					<religiousMembership>
						<xsl:value-of select="$base/religion"/>
					</religiousMembership>
				</xsl:if>
				<xsl:for-each select="$base/creativeWorks">
					<creativeWorks>
						<xsl:value-of select="."/>
					</creativeWorks>
				</xsl:for-each>
				<xsl:for-each select="$base/achievements">
					<achievements>
						<xsl:value-of select="."/>
					</achievements>
				</xsl:for-each>
				<xsl:for-each select="$base/book">
					<book>
						<xsl:value-of select="name"/>
					</book>
				</xsl:for-each>
				<xsl:for-each select="$base/thoughts">
					<thoughts>
						<xsl:value-of select="."/>
					</thoughts>
				</xsl:for-each>
				<xsl:for-each select="$base/certification">
					<certifications>
						<xsl:value-of select="."/>
					</certifications>
				</xsl:for-each>
				<xsl:for-each select="$base/memberships">
					<memberships>
						<xsl:value-of select="."/>
					</memberships>
				</xsl:for-each>
				<xsl:for-each select="$base/node()[name()='civicInformation' or name()='civic']">
					<civicInformation>
						<xsl:value-of select="."/>
					</civicInformation>
				</xsl:for-each>
				<xsl:for-each select="$base/military">
					<militaryInformation>
						<xsl:value-of select="."/>
					</militaryInformation>
				</xsl:for-each>
				<xsl:for-each select="$base/avocations">
					<avocations>
						<xsl:value-of select="."/>
					</avocations>
				</xsl:for-each>
				<xsl:if test="$base/career/additionalInformation">
					<additionalInformation>
						<xsl:value-of select="$base/career/additionalInformation"/>
					</additionalInformation>
				</xsl:if>
				<xsl:if test="$base/biographicalText">
					<biographicalText>
						<xsl:value-of select="$base/biographicalText"/>
					</biographicalText>
				</xsl:if>
			</biography>
			<xsl:for-each select="$base/remuneration">
				<compensation>
					<xsl:if test="boolean(./@periodEndDate)">
						<fiscalYearEnding>
							<date>
								<xsl:value-of select="./@periodEndDate"/>
							</date>
						</fiscalYearEnding>
					</xsl:if>
					<xsl:if test="boolean(./currency/@code)">
						<currency code="{./currency/@code}"/>
					</xsl:if>
					<xsl:if test="boolean(./compensation/salary)">
						<salary xsi:type="DoubleNumber" value="{./compensation/salary}"/>
					</xsl:if>
					<xsl:if test="boolean(./compensation/bonus)">
						<bonus xsi:type="DoubleNumber" value="{./compensation/bonus}"/>
					</xsl:if>
					<xsl:if test="boolean(./compensation/totalShortTerm)">
						<totalShortTerm xsi:type="DoubleNumber" value="{./compensation/totalShortTerm}"/>
					</xsl:if>
					<xsl:if test="boolean(./compensation/otherShortTerm)">
						<otherShortTerm xsi:type="DoubleNumber" value="{./compensation/otherShortTerm}"/>
					</xsl:if>
					<xsl:if test="boolean(./compensation/longTermIncentive)">
						<longTermIncentive xsi:type="DoubleNumber" value="{./compensation/longTermIncentive}"/>
					</xsl:if>
					<xsl:if test="boolean(./compensation/otherLongTerm)">
						<otherLongTerm xsi:type="DoubleNumber" value="{./compensation/otherLongTerm}"/>
					</xsl:if>
					<xsl:if test="boolean(./compensation/total)">
						<total xsi:type="DoubleNumber" value="{./compensation/total}"/>
					</xsl:if>
					<xsl:if test="boolean(./options/exercisable/number)">
						<numberOptionsExercisable xsi:type="WholeNumber" value="{./options/exercisable/number}"/>
					</xsl:if>
					<xsl:if test="boolean(./options/exercisable/value)">
						<valueOptionsExercisable xsi:type="DoubleNumber" value="{./options/exercisable/value}"/>
					</xsl:if>
					<xsl:if test="boolean(./options/unexercised/number)">
						<numberOptionsUnexercised xsi:type="WholeNumber" value="{./options/unexercised/number}"/>
					</xsl:if>
					<xsl:if test="boolean(./options/unexercised/value)">
						<valueOptionsUnexercised xsi:type="DoubleNumber" value="{./options/unexercised/value}"/>
					</xsl:if>
					<xsl:if test="boolean(./options/exercised/number)">
						<numberOptionsExercised xsi:type="WholeNumber" value="{./options/exercised/number}"/>
					</xsl:if>
					<xsl:if test="boolean(./options/exercised/value)">
						<valueOptionsExercised xsi:type="DoubleNumber" value="{./options/exercised/value}"/>
					</xsl:if>
				</compensation>
			</xsl:for-each>
		</biographyReportData>
	</xsl:template>
	<!-- IndustryAggregates [inrat] 						-->
	<xsl:template name="IndustryAggregates">
		<xsl:param name="base"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<xsl:variable name="codes" select="document('ratios.xml')"/>
		<xsl:variable name="ratioComparison" select="$codes/codeSets/codeSet[@type= 'ratioComparison']/*"/>
		<industryRatiosReportData>
			<reportType>IndustryRatios</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<aggregatesDescriptor>
				<xsl:value-of select="$base/aggregateCode/descriptor"/>
			</aggregatesDescriptor>
			<ratioComparisonPanels>
				<xsl:call-template name="keyRatioPanels">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="codePanels" select="$ratioComparison"/>
					<xsl:with-param name="currency"/>
				</xsl:call-template>
			</ratioComparisonPanels>
		</industryRatiosReportData>
	</xsl:template>
	<!-- IndustrySectorAggregates [insec] 					-->
	<xsl:template name="IndustrySectorAggregates">
		<xsl:param name="base"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<xsl:variable name="codes" select="document('ratios.xml')"/>
		<xsl:variable name="ratioComparison" select="$codes/codeSets/codeSet[@type= 'ratioComparison']/*"/>
		<industrySecRatiosReportData>
			<reportType>IndustryRatios</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<aggregatesDescriptor>
				<xsl:value-of select="$base/aggregateCode/descriptor"/>
			</aggregatesDescriptor>
			<ratioComparisonPanels>
				<xsl:call-template name="keyRatioPanels">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="codePanels" select="$ratioComparison"/>
					<xsl:with-param name="currency"/>
				</xsl:call-template>
			</ratioComparisonPanels>
		</industrySecRatiosReportData>
	</xsl:template>
	<!-- IndexAggregates [in500] 							-->
	<xsl:template name="IndexAggregates">
		<xsl:param name="base"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<xsl:variable name="codes" select="document('ratios.xml')"/>
		<xsl:variable name="ratioComparison" select="$codes/codeSets/codeSet[@type= 'ratioComparison']/*"/>
		<industry500RatiosReportData>
			<reportType>IndustryRatios</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<ratioComparisonPanels>
				<xsl:call-template name="keyRatioPanels">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="codePanels" select="$ratioComparison"/>
					<xsl:with-param name="currency"/>
				</xsl:call-template>
			</ratioComparisonPanels>
		</industry500RatiosReportData>
	</xsl:template>
	<!-- FinancialInformation [cokey] 						-->
	<xsl:template name="FinancialInformation">
		<xsl:param name="base"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<xsl:variable name="codes" select="document('CoKey.xml')"/>
		<keyFinancialsReportData>
			<reportType>KeyFinancials</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<xsl:if test="boolean($base/netProfitMargin)">
				<netProfitMargin xsi:type="Percent" value="{$base/netProfitMargin}"/>
			</xsl:if>
			<xsl:if test="boolean($base/profitMargin)">
				<profitMargin xsi:type="Percent" value="{$base/profitMargin}"/>
			</xsl:if>
			<xsl:if test="boolean($base/marketCapitalization)">
				<marketCap xsi:type="DoubleMoney" value="{$base/marketCapitalization}">
          <!-- mt 08/2007 misc. enhancement -->
          <!--<currency code="{$currency}"/>-->
          <currency code="{$base/marketCapitalization/@currency}"/>
        </marketCap>
			</xsl:if>
			<xsl:if test="boolean($base/lastAnnualFiscalPeriod)">
				<lastAnnualFiscalPeriod>
					<xsl:call-template name="fiscalPeriodDetail">
						<xsl:with-param name="base" select="$base/lastAnnualFiscalPeriod"/>
						<xsl:with-param name="type">Annual</xsl:with-param>
					</xsl:call-template>
					<keyFinancialItems>
						<xsl:call-template name="keyFiancialItems">
							<xsl:with-param name="base" select="$base/lastAnnualFiscalPeriod"/>
							<xsl:with-param name="codeSets" select="$codes/codeSets/codeSet[@type='lastAnnualFiscalPeriod']"/>
							<xsl:with-param name="currency" select="$currency"/>
						</xsl:call-template>
					</keyFinancialItems>
				</lastAnnualFiscalPeriod>
			</xsl:if>
			<xsl:if test="boolean($base/previousAnnualFiscalPeriod)">
				<xsl:for-each select="$base/lastAnnualFiscalPeriod">
					<previousAnnualFiscalPeriod>
						<xsl:call-template name="fiscalPeriodDetail">
							<xsl:with-param name="base" select="."/>
							<xsl:with-param name="type">Annual</xsl:with-param>
						</xsl:call-template>
						<keyFinancialItems>
							<xsl:call-template name="keyFiancialItems">
								<xsl:with-param name="base" select="."/>
								<xsl:with-param name="codeSets" select="$codes/codeSets/codeSet[@type='lastAnnualFiscalPeriod']"/>
								<xsl:with-param name="currency" select="$currency"/>
							</xsl:call-template>
						</keyFinancialItems>
					</previousAnnualFiscalPeriod>
				</xsl:for-each>
				<xsl:for-each select="$base/previousAnnualFiscalPeriod">
					<previousAnnualFiscalPeriod>
						<xsl:call-template name="fiscalPeriodDetail">
							<xsl:with-param name="base" select="."/>
							<xsl:with-param name="type">Annual</xsl:with-param>
						</xsl:call-template>
						<keyFinancialItems>
							<xsl:call-template name="keyFiancialItems">
								<xsl:with-param name="base" select="."/>
								<xsl:with-param name="codeSets" select="$codes/codeSets/codeSet[@type='lastAnnualFiscalPeriod']"/>
								<xsl:with-param name="currency" select="$currency"/>
							</xsl:call-template>
						</keyFinancialItems>
					</previousAnnualFiscalPeriod>
				</xsl:for-each>
			</xsl:if>
			<xsl:if test="boolean($base/lastInterimFiscalPeriod)">
				<lastInterimFiscalPeriod>
					<xsl:call-template name="fiscalPeriodDetail">
						<xsl:with-param name="base" select="$base/lastInterimFiscalPeriod"/>
						<xsl:with-param name="type">Interim</xsl:with-param>
					</xsl:call-template>
					<keyFinancialItems>
						<xsl:call-template name="keyFiancialItems">
							<xsl:with-param name="base" select="$base/lastInterimFiscalPeriod"/>
							<xsl:with-param name="codeSets" select="$codes/codeSets/codeSet[@type='lastInterimFiscalPeriod']"/>
							<xsl:with-param name="currency" select="$currency"/>
						</xsl:call-template>
					</keyFinancialItems>
				</lastInterimFiscalPeriod>
			</xsl:if>
			<xsl:if test="boolean($base/interimOneYearAgoFiscalPeriod)">
				<interimOneYearAgoFiscalPeriod>
					<xsl:call-template name="fiscalPeriodDetail">
						<xsl:with-param name="base" select="$base/interimOneYearAgoFiscalPeriod"/>
						<xsl:with-param name="type">Interim</xsl:with-param>
					</xsl:call-template>
					<keyFinancialItems>
						<xsl:call-template name="keyFiancialItems">
							<xsl:with-param name="base" select="$base/interimOneYearAgoFiscalPeriod"/>
							<xsl:with-param name="codeSets" select="$codes/codeSets/codeSet[@type='interimOneYearAgoFiscalPeriod']"/>
							<xsl:with-param name="currency" select="$currency"/>
						</xsl:call-template>
					</keyFinancialItems>
				</interimOneYearAgoFiscalPeriod>
			</xsl:if>
			<xsl:if test="boolean($base/lastAnnualFiscalPeriod/items/sharesOutstandingCommonStockPrimaryIssue)">
				<sharesOutstandingCommonStockPrimaryIssue xsi:type="WholeNumber" value="{$base/lastAnnualFiscalPeriod/items/sharesOutstandingCommonStockPrimaryIssue}"/>
			</xsl:if>
			<xsl:if test="boolean($base/lastAnnualFiscalPeriod/items/sharesOutstandingPreferredStockPrimaryIssue)">
				<sharesOutstandingPreferredStockPrimaryIssue xsi:type="WholeNumber" value="{$base/lastAnnualFiscalPeriod/items/sharesOutstandingPreferredStockPrimaryIssue}"/>
			</xsl:if>
			<xsl:if test="boolean($base/lastAnnualFiscalPeriod/items/employeesDataAccuracy)">
				<employeesDataAccuracy>
					<xsl:call-template name="DataAccuracyMapper">
						<xsl:with-param name="type">
							<xsl:value-of select="$base/lastAnnualFiscalPeriod/items/employeesDataAccuracy"/>
						</xsl:with-param>
					</xsl:call-template>
				</employeesDataAccuracy>
			</xsl:if>
			<xsl:if test="boolean($base/lastAnnualFiscalPeriod/items/salesDataAccuracy)">
				<salesDataAccuracy>
					<xsl:call-template name="DataAccuracyMapper">
						<xsl:with-param name="type">
							<xsl:value-of select="$base/lastAnnualFiscalPeriod/items/salesDataAccuracy"/>
						</xsl:with-param>
					</xsl:call-template>
				</salesDataAccuracy>
			</xsl:if>
			<xsl:if test="boolean($base/lastAnnualFiscalPeriod/auditor)">
				<auditor code="{$base/lastAnnualFiscalPeriod/auditor/code/@code}" scheme="{$base/lastAnnualFiscalPeriod/auditor/code/@scheme}">
					<name>
						<xsl:value-of select="$base/lastAnnualFiscalPeriod/auditor/name"/>
					</name>
				</auditor>
			</xsl:if>
			<xsl:if test="boolean($base/lastAnnualFiscalPeriod/filingDate)">
				<filingDate>
					<date>
						<xsl:value-of select="$base/lastAnnualFiscalPeriod/filingDate"/>
					</date>
				</filingDate>
			</xsl:if>
		</keyFinancialsReportData>
	</xsl:template>

	<xsl:template name="TechnologyInformation">
		<xsl:param name="base"/>
		<technologyInformationReportData>
			<reportType>TechnologyInformation</reportType>
			<xsl:if test="boolean($base/groupwareSystems/system)">
				<xsl:for-each select="$base/groupwareSystems/system[@type='primary']">
					<primaryGroupware>
						<xsl:value-of select="." />
					</primaryGroupware>
				</xsl:for-each>
				<xsl:for-each select="$base/groupwareSystems/system[@type='secondary']">
					<secondaryGroupware>
						<xsl:value-of select="." />
					</secondaryGroupware>
				</xsl:for-each>
			</xsl:if>
			<xsl:if test="boolean($base/ermSystems/system)">
				<xsl:for-each select="$base/ermSystems/system[@type='primary']">
					<primaryERP>
						<xsl:value-of select="." />
					</primaryERP>
				</xsl:for-each>
				<xsl:for-each select="$base/ermSystems/system[@type='secondary']">
					<secondaryERP>
						<xsl:value-of select="." />
					</secondaryERP>
				</xsl:for-each>
			</xsl:if>
			<xsl:if test="boolean($base/crmSystems/system)">
				<xsl:for-each select="$base/crmSystems/system[@type='primary']">
					<primaryCRM>
						<xsl:value-of select="." />
					</primaryCRM>
				</xsl:for-each>
				<xsl:for-each select="$base/crmSystems/system[@type='secondary']">
					<secondaryCRM>
						<xsl:value-of select="." />
					</secondaryCRM>
				</xsl:for-each>
			</xsl:if>
			<xsl:if test="boolean($base/pcs)">
				<personalComputers xsi:type="WholeNumber" value="{$base/pcs}" />
			</xsl:if>

			<!--q207
      pcsDataAccuracy
      serversDataAccuracy
      extensionsDataAccuracy
      -->
			<xsl:if test="boolean($base/pcsDataAccuracy)">
				<personalComputersAccuracy>
					<xsl:call-template name="DataAccuracyMapper">
						<xsl:with-param name="type">
							<xsl:value-of select="$base/pcsDataAccuracy"/>
						</xsl:with-param>
					</xsl:call-template>
				</personalComputersAccuracy>
			</xsl:if>
			<xsl:if test="boolean($base/serversDataAccuracy)">
				<serversAccuracy>
					<xsl:call-template name="DataAccuracyMapper">
						<xsl:with-param name="type">
							<xsl:value-of select="$base/serversDataAccuracy"/>
						</xsl:with-param>
					</xsl:call-template>
				</serversAccuracy>
			</xsl:if>
			<xsl:if test="boolean($base/extensionsDataAccuracy)">
				<phoneExtensionsAccuracy>
					<xsl:call-template name="DataAccuracyMapper">
						<xsl:with-param name="type">
							<xsl:value-of select="$base/extensionsDataAccuracy"/>
						</xsl:with-param>
					</xsl:call-template>
				</phoneExtensionsAccuracy>
			</xsl:if>


			<xsl:if test="boolean($base/servers)">
				<servers xsi:type="WholeNumber" value="{$base/servers}"/>
			</xsl:if>
			<xsl:if test="boolean($base/extensions)">
				<phoneExtensions xsi:type="WholeNumber" value="{$base/extensions}"/>
			</xsl:if>
			<xsl:if test="string-length(normalize-space($base/broadband)) > 0">
				<broadband>
					<xsl:value-of select="$base/broadband" />
				</broadband>
			</xsl:if>
			<xsl:if test="string-length(normalize-space($base/wifi)) > 0" >
				<wifi>
					<xsl:value-of select="$base/wifi" />
				</wifi>
			</xsl:if>
			<xsl:if test="string-length(normalize-space($base/erp)) > 0">
				<erpSoftwareExistance>
					<xsl:value-of select="$base/erp" />
				</erpSoftwareExistance>
			</xsl:if>
			<xsl:if test="string-length(normalize-space($base/crm)) > 0">
				<crmSoftwareExistance>
					<xsl:value-of select="$base/crm" />
				</crmSoftwareExistance>
			</xsl:if>
			<xsl:if test="boolean($base/decisionMakersIT)">
				<xsl:call-template name="Executives">
					<xsl:with-param name="base" select="$base/decisionMakersIT"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:if test="boolean($base/enterpriseTotals)">
				<enterpriseTotals>
					<xsl:call-template name="EnterpriseTotals">
						<xsl:with-param name="baseNode" select="$base/enterpriseTotals"/>
					</xsl:call-template>
				</enterpriseTotals>
			</xsl:if>
		</technologyInformationReportData>
	</xsl:template>

  <!-- // mt 08/2007 - add company brands -->
  <xsl:template name="CompanyBrands">
    <xsl:param name="base"/>
    <companyBrandsReportData>
      <reportType>CompanyBrands</reportType>
      <xsl:for-each select="$base/tradeNameInfo">
        <tradeNameInfo>
          <tradeName>
            <xsl:if test="boolean(tradeName/name)">
              <xsl:value-of select="tradeName/name" />
            </xsl:if>
          </tradeName>
          <genericProductCode>
            <xsl:if test="boolean(genericProductCode)">
              <xsl:call-template name="BrandItem">
                <xsl:with-param name="base" select="genericProductCode"/>
              </xsl:call-template>
            </xsl:if>
          </genericProductCode>
          <industry>
            <xsl:if test="boolean(industry)">
              <xsl:call-template name="BrandItem">
                <xsl:with-param name="base" select="industry"/>
              </xsl:call-template>
            </xsl:if>
          </industry>
        </tradeNameInfo>
      </xsl:for-each>
    </companyBrandsReportData>
  </xsl:template>

  <!-- // mt 08/2007 - add company brands -->
  <xsl:template name="BrandItem">
    <xsl:param name="base"/>
    <xsl:attribute name="code">
      <xsl:if test="boolean($base/@code)">
        <xsl:value-of select="$base/@code" />
      </xsl:if>
    </xsl:attribute>
    <xsl:attribute name="scheme">
      <xsl:if test="boolean($base/@scheme)">
        <xsl:value-of select="$base/@scheme" />
      </xsl:if>
    </xsl:attribute>
    <descriptor>
      <xsl:if test="boolean($base/descriptor)">
        <xsl:value-of select="$base/descriptor" />
      </xsl:if>
    </descriptor>
  </xsl:template>
  
	<!-- CustomerInformation [cocus]						-->
	<xsl:template name="CustomerInformation">
		<xsl:param name="base"/>
		<xsl:variable name="customerOrderNodesAnnual" select="$base/fiscalPeriod[@type='Annual']/items/customerOrder"/>
		<xsl:variable name="customerOrderListAnnual" select="$base/fiscalPeriod[@type='Annual']/items/customerOrder/@orderNumber[ not(.=preceding::customerOrder/@orderNumber)]"/>
		<!--<xsl:variable name="customerOrderNameListAnnual" select="$base/fiscalPeriod[@type='Annual']/items/customerOrder/*[name() != '']/node()/name()"/>-->
		<xsl:variable name="customerOrderNameListAnnual" select="$base/fiscalPeriod[@type='Annual']/customerOrder/@orderNumber[ not(.=preceding::customerOrder/@orderNumber)]"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<!--<xsl:for-each select="$customerOrderNameListAnnual">
			<xsl:sort select="." order="ascending" data-type="number"/>
			<xsl:value-of select="." />|
		</xsl:for-each>-->
		<customerInformationData>
			<reportType>CustomerInformation</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<customerPeriods>
				<xsl:for-each select="$base/fiscalPeriod[@type='Annual']">
					<customerPeriod>
						<xsl:call-template name="fiscalPeriodDetail">
							<xsl:with-param name="base" select="."/>
						</xsl:call-template>
						<xsl:for-each select="./customerOrder">
							<xsl:variable name="n" select="translate( normalize-space( ./name ) , 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz' ) = 'total revenue' "/>
							<customerOrder>
								<customerCode>
									<xsl:choose>
										<xsl:when test="$n">totalRevenue</xsl:when>
										<xsl:otherwise>customer</xsl:otherwise>
									</xsl:choose>
								</customerCode>
								<name>
									<xsl:value-of select="./name"/>
								</name>
								<revenue xsi:type="DoubleMoney" value="{ normalize-space( ./revenue ) }">
									<currency code="{$currency}"/>
								</revenue>
								<revenuePercent xsi:type="Percent" value="{ normalize-space( ./revenuePercent ) }"/>
							</customerOrder>
						</xsl:for-each>
					</customerPeriod>
				</xsl:for-each>
			</customerPeriods>
		</customerInformationData>
	</xsl:template>
	<!-- GeneralInformation [cogen]							-->
	<xsl:template name="CompanyInformation">
		<xsl:param name="base"/>
		<xsl:param name="metaBase"/>
		<companyInformationReportData>
			<reportType>GeneralInformation</reportType>
			<generalInformation>
				<name>
					<xsl:choose>
						<xsl:when test="string-length($base/name) &gt; 0">
							<xsl:value-of select="$base/name"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$metaBase/DescTPC/DescField[@fid = 'ipcconame']"/>
						</xsl:otherwise>
					</xsl:choose>
				</name>

				<xsl:for-each select="$base/tradingName">
					<tradingName>
						<xsl:value-of select="."></xsl:value-of>
					</tradingName>
				</xsl:for-each>

				<code>
					<xsl:value-of select="$metaBase/CodeSets/CSet[@fid='co']/Code[1]/@value"/>
				</code>
				<companyLocationInformation>
					<xsl:call-template name="Address">
						<xsl:with-param name="baseAddress" select="$base/address"/>
					</xsl:call-template>
					<phone>
						<xsl:value-of select="$base/phoneNumber/number"/>
					</phone>
					<xsl:if test="count($base/phoneNumber/cityAreaCode) &gt; 0">
						<phoneAreaCode>
							<xsl:value-of select="normalize-space($base/phoneNumber/cityAreaCode)"/>
						</phoneAreaCode>
					</xsl:if>
					<xsl:if test="count($base/phoneNumber/countryRegionCode) &gt; 0">
						<phoneCountryCode>
							<xsl:value-of select="normalize-space($base/phoneNumber/countryRegionCode)"/>
						</phoneCountryCode>
					</xsl:if>
					<fax>
						<xsl:value-of select="$base/faxNumber/number"/>
					</fax>
					<xsl:if test="count($base/faxNumber/cityAreaCode) &gt; 0">
						<faxAreaCode>
							<xsl:value-of select="normalize-space($base/faxNumber/cityAreaCode)"/>
						</faxAreaCode>
					</xsl:if>
					<xsl:if test="count($base/faxNumber/countryRegionCode) &gt; 0">
						<faxCountryCode>
							<xsl:value-of select="normalize-space($base/faxNumber/countryRegionCode)"/>
						</faxCountryCode>
					</xsl:if>
				</companyLocationInformation>
				<xsl:if test="count($base/dunsNumber[@scheme = 'DUNS']) &gt; 0">
					<dunsNumber>
						<xsl:value-of select="normalize-space($base/dunsNumber[@scheme = 'DUNS']/@code)"/>
					</dunsNumber>
				</xsl:if>
				<xsl:if test="count($base/bankDetails) &gt; 0">
					<xsl:for-each select="$base/bankDetails">
						<bankerDetails>
							<bankerNameAndAddress>
								<xsl:value-of select="./nameAndAddress"/>
							</bankerNameAndAddress>
							<bankerSortCode>
								<xsl:value-of select="./sortCode"/>
							</bankerSortCode>
						</bankerDetails>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="string-length(normalize-space($metaBase/DescTPC/DescField[@fid = 'ipccodj'])) > 0">
					<primaryDJInstrument code="{normalize-space($metaBase/DescTPC/DescField[@fid = 'ipccodj'])}"/>
				</xsl:if>
				<xsl:if test="string-length(normalize-space($metaBase/DescTPC/DescField[@fid = 'ipccoric'])) > 0">
					<primaryRicInstrument code="{normalize-space($metaBase/DescTPC/DescField[@fid = 'ipccoric'])}"/>
				</xsl:if>
				<xsl:if test="string-length(normalize-space($base/publicListing/exchange[@scheme='FactivaExchange']/@code)) > 0">
					<primaryExchange code="{normalize-space($base/publicListing/exchange[@scheme='FactivaExchange']/@code)}"/>
				</xsl:if>
				<xsl:if test="count($base/publicListing/splits) > 0">
					<splits>
						<xsl:for-each select="$base/publicListing/splits/split">
							<split>
								<splitDate>
									<date>
										<xsl:value-of select="@date"/>
									</date>
								</splitDate>
								<splitValue>
									<xsl:value-of select="."/>
								</splitValue>
							</split>
						</xsl:for-each>
					</splits>
				</xsl:if>
				<businessDescription>
					<xsl:value-of select="$base/businessDescription"/>
				</businessDescription>
				<xsl:if test="count($base/DocumentInformation) > 0">
					<documentInformation>
						<creationDate>
							<xsl:value-of select="$base/DocumentInformation/CreationDate"/>
						</creationDate>
						<language>
							<xsl:value-of select="$base/DocumentInformation/Language"/>
						</language>
					</documentInformation>
				</xsl:if>
				<xsl:if test="count($base/mainContact) > 0">
					<contact type="main">
						<xsl:apply-templates select="$base/mainContact/name"/>
						<xsl:for-each select="$base/mainContact/jobTitle">
							<xsl:for-each select="./jobTitle">
								<position>
									<name>
										<xsl:value-of select="."/>
									</name>
								</position>
							</xsl:for-each>
						</xsl:for-each>
					</contact>
				</xsl:if>
				<xsl:for-each select="$base/topShareholder">
					<xsl:if test="./name">
						<topShareholder>
							<xsl:value-of select="./name"/>
						</topShareholder>
					</xsl:if>
				</xsl:for-each>
				<primaryIndustryClassification>
					<xsl:if test="count($metaBase/CodeSets/CSet[@fid='in']/Code/@value) > 0">
						<industry code="{$metaBase/CodeSets/CSet[@fid='in']/Code/@value}"/>
					</xsl:if>
					<xsl:for-each select="$base/primaryIndustry">
						<xsl:choose>
							<xsl:when test="@scheme = 'SIC'">
								<sicIndustry code="{@code}"/>
							</xsl:when>
							<xsl:when test="@scheme = 'NACE'">
								<naceIndustry code="{@code}"/>
							</xsl:when>
							<xsl:when test="@scheme = 'NAICS'">
								<naicsIndustry code="{@code}"/>
							</xsl:when>
							<xsl:when test="@scheme = 'SICUS'">
								<sicusIndustry code="{@code}"/>
							</xsl:when>
							<xsl:when test="@scheme = 'OKVED'">
								<okvedIndustry code="{@code}"/>
							</xsl:when>
						</xsl:choose>
					</xsl:for-each>
				</primaryIndustryClassification>
				<secondaryIndustryClassification>
					<xsl:for-each select="$base/secondaryIndustry">
						<xsl:choose>
							<xsl:when test="@scheme = 'SIC'">
								<sicIndustry code="{@code}"/>
							</xsl:when>
							<xsl:when test="@scheme = 'NACE'">
								<naceIndustry code="{@code}"/>
							</xsl:when>
							<xsl:when test="@scheme = 'NAICS'">
								<naicsIndustry code="{@code}"/>
							</xsl:when>
							<xsl:when test="@scheme = 'SICUS'">
								<sicusIndustry code="{@code}"/>
							</xsl:when>
							<xsl:when test="@scheme = 'OKVED'">
								<okvedIndustry code="{@code}"/>
							</xsl:when>
						</xsl:choose>
					</xsl:for-each>
				</secondaryIndustryClassification>
				<websites>
					<xsl:for-each select="$base/webSite">
						<website code="{category/@code}">
							<url>
								<xsl:value-of select="url"/>
							</url>
							<description>
								<xsl:value-of select="category/descriptor"/>
							</description>
						</website>
					</xsl:for-each>
				</websites>
				<ownershipType>
					<xsl:call-template name="OwnershipTypeMapper">
						<xsl:with-param name="ownershipType">
							<xsl:value-of select="$base/listingStatus"/>
						</xsl:with-param>
					</xsl:call-template>
				</ownershipType>
				<keyExecutives>
					<xsl:for-each select="$base/keyExecutive">
						<executive>
							<xsl:apply-templates select="./name"/>
							<xsl:for-each select="jobTitle">
								<position>
									<name>
										<xsl:value-of select="."/>
									</name>
								</position>
							</xsl:for-each>
							<!--<xsl:if test="$metaBase/PubData/SrcCode/@value='RCDE'">-->
								<consolidatedId>
									<xsl:value-of select="consolidationId"/>
								</consolidatedId>
							<!--</xsl:if>-->
						</executive>
					</xsl:for-each>
				</keyExecutives>

				<xsl:if test="boolean($base/inn)">
					<inn>
						<xsl:value-of select="$base/inn/@code"/>
					</inn>
				</xsl:if>

				<xsl:if test="boolean($base/okpo)">
					<okpo>
						<xsl:value-of select="$base/okpo/@code"/>						
					</okpo>
				</xsl:if>

				<xsl:if test="boolean($base/legalStatus)">
					<legalStatus scheme="{$base/legalStatus/@scheme}" code="{$base/legalStatus/@code}"/>
					
				</xsl:if>
				<xsl:if test="boolean($base/yearStart)">
					<yearStarted>
						<xsl:value-of select="$base/yearStart"></xsl:value-of>
					</yearStarted>
				</xsl:if>
				<xsl:if test="boolean($base/employeesHere)">
					<employeesHere>
						<xsl:value-of select="$base/employeesHere"></xsl:value-of>
					</employeesHere>
				</xsl:if>

				<xsl:if test="boolean($base/registration)">
					<registration>
						<filingOfficeName>
							<xsl:value-of select="$base/registration/filingOfficeName"></xsl:value-of>
						</filingOfficeName>
						<registrationId>
							<xsl:value-of select="$base/registration/registrationId"></xsl:value-of>
						</registrationId>
						<description>
							<xsl:value-of select="$base/registration/description"></xsl:value-of>
						</description>
						<eventDate>
							<xsl:value-of select="$base/registration/eventDate"></xsl:value-of>
						</eventDate>
					</registration>					
				</xsl:if>				

			</generalInformation>
		</companyInformationReportData>
	</xsl:template>
	<!-- keyCompetitors										-->
	<xsl:template name="KeyCompetitors">
		<xsl:param name="base"/>
		<keyCompetitorsReportData>
			<reportType>KeyCompetitors</reportType>
			<competitors>
				<xsl:for-each select="$base/*">
					<xsl:choose>
						<xsl:when test="./factivaCode">
							<competitor>
								<company code="{./factivaCode/@code}">
									<name>
										<xsl:value-of select="./name"/>
									</name>
								</company>
							</competitor>
						</xsl:when>
						<xsl:otherwise>
							<competitor>
								<company>
									<name>
										<xsl:value-of select="./name"/>
									</name>
								</company>
							</competitor>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</competitors>
		</keyCompetitorsReportData>
	</xsl:template>
	<!-- OverviewAndHistory									-->
	<xsl:template name="OverviewAndHistory">
		<xsl:param name="base"/>
		<overviewAndHistoryReportData>
			<reportType>OverviewAndHistory</reportType>
			<overview>
				<xsl:apply-templates select="$base/overview/Para"/>
			</overview>
			<history>
				<xsl:apply-templates select="$base/history/Para"/>
			</history>
		</overviewAndHistoryReportData>
	</xsl:template>

	<!-- LongBusinessDescription template [colbd]			-->
	<xsl:template name="LongBusinessDescription">
		<xsl:param name="base"/>
		<longBusinessDescriptionReportData>
			<reportType>LongBusinessDescription</reportType>
			<businessDescription>
				<xsl:apply-templates select="$base/Para"/>
			</businessDescription>
		</longBusinessDescriptionReportData>
	</xsl:template>
	<!-- LongBusinessDescription template [costa]			-->
	<xsl:template name="CompanyStatement">
		<xsl:param name="base"/>
		<companyStatementReportData>
			<reportType>CompanyStatement</reportType>
			<companyStatement>
				<xsl:apply-templates select="$base/Para"/>
			</companyStatement>
		</companyStatementReportData>
	</xsl:template>
	<!-- SwotAnalysis template [coswt]			-->
	<xsl:template name="SwotAnalysis">
		<xsl:param name="base"/>
		<swotAnalysisReportData>
			<reportType>SwotAnalysis</reportType>
			<overview>
				<xsl:apply-templates select="$base/overview/Para"/>
			</overview>
			<strength>
				<xsl:call-template name="SummaryAndDetails">
					<xsl:with-param name="sdbase" select="$base/strength"/>
				</xsl:call-template>
			</strength>
			<weakness>
				<xsl:call-template name="SummaryAndDetails">
					<xsl:with-param name="sdbase" select="$base/weakness"/>
				</xsl:call-template>
			</weakness>
			<opportunity>
				<xsl:call-template name="SummaryAndDetails">
					<xsl:with-param name="sdbase" select="$base/opportunity"/>
				</xsl:call-template>
			</opportunity>
			<threat>
				<xsl:call-template name="SummaryAndDetails">
					<xsl:with-param name="sdbase" select="$base/threat"/>
				</xsl:call-template>
			</threat>
		</swotAnalysisReportData>
	</xsl:template>
	<xsl:template name="SummaryAndDetails">
		<xsl:param name="sdbase"/>
		<overview>
			<xsl:apply-templates select="$sdbase/overview/Para"/>
		</overview>
		<details>
			<xsl:apply-templates select="$sdbase/details/Para"/>
		</details>
	</xsl:template>

	<!-- ProductAndServices template						-->
	<xsl:template name="ProductAndServices">
		<xsl:param name="base"/>
		<productAndServicesReportData>
			<reportType>ProductAndServices</reportType>
			<salesByLocation>
				<xsl:apply-templates select="$base/salesByLocation/Para"/>
			</salesByLocation>
			<salesByOperation>
				<xsl:apply-templates select="$base/salesByOperation/Para"/>
			</salesByOperation>
		</productAndServicesReportData>
	</xsl:template>
	<!--Executive Template									-->
	<xsl:template name="Executives">
		<xsl:param name="base"/>
		<xsl:variable name="oBase" select="$base/*[@level != 'Director' or count(@level) = 0]"/>
		<xsl:variable name="eBase" select="$base/*[@level = 'Both' or @level = 'Director']"/>
		<xsl:variable name="cBase" select="$base/*"/>
		<executivesAndOfficersReportData>
			<reportType>Executives</reportType>
			<colleagues>
				<xsl:for-each select="$cBase">
					<colleague>
						<person xsi:type="Person">
							<xsl:if test="string-length( normalize-space(./consolidationId)) > 0 and number(./consolidationId) > 0">
								<consolidatedId>
									<xsl:value-of select="./consolidationId" />
								</consolidatedId>
							</xsl:if>
							<xsl:apply-templates select="./name"/>
							<xsl:if test="./email">
								<email>
									<xsl:value-of select="./email"/>
								</email>
							</xsl:if>
							<xsl:if test="./phone">
								<phone>
									<xsl:value-of select="./phone"/>
								</phone>
							</xsl:if>
							<!--q207-->
							<xsl:if test="./phoneNumber">
								<phoneNumber>
									<xsl:call-template name="PhoneNumber">
										<xsl:with-param name="baseNumber" select="./phoneNumber"/>
									</xsl:call-template>
								</phoneNumber>
							</xsl:if>
							<xsl:if test="./fax">
								<fax>
									<xsl:value-of select="./fax"/>
								</fax>
							</xsl:if>
							<xsl:if test="./age">
								<age>
									<xsl:value-of select="./age"/>
								</age>
							</xsl:if>
							<xsl:for-each select="./jobTitle">
								<position>
									<name>
										<xsl:value-of select="."/>
									</name>
								</position>
							</xsl:for-each>
							<xsl:if test="./@level">
								<level>
									<xsl:value-of select="./@level"/>
								</level>
							</xsl:if>
							<xsl:if test="./@isBoardMember">
								<isBoardMember>
									<xsl:value-of select="./@isBoardMember"/>
								</isBoardMember>
							</xsl:if>
						</person>
					</colleague>
				</xsl:for-each>
			</colleagues>
			<officers>
				<xsl:for-each select="$oBase">
					<officer>
						<person xsi:type="Person">
							<xsl:if test="string-length( normalize-space(./consolidationId)) > 0 and number(./consolidationId) > 0">
								<consolidatedId>
									<xsl:value-of select="./consolidationId" />
								</consolidatedId>
							</xsl:if>
							<xsl:apply-templates select="./name"/>
							<xsl:if test="./email">
								<email>
									<xsl:value-of select="./email"/>
								</email>
							</xsl:if>
							<xsl:if test="./phone">
								<phone>
									<xsl:value-of select="./phone"/>
								</phone>
							</xsl:if>
							<!--q207-->
							<xsl:if test="./fax">
								<fax>
									<xsl:value-of select="./fax"/>
								</fax>
							</xsl:if>
							<xsl:if test="./age">
								<age>
									<xsl:value-of select="./age"/>
								</age>
							</xsl:if>
							<xsl:for-each select="./jobTitle">
								<position>
									<name>
										<xsl:value-of select="."/>
									</name>
								</position>
							</xsl:for-each>
							<xsl:if test="./@level">
								<level>
									<xsl:value-of select="./@level"/>
								</level>
							</xsl:if>
							<xsl:if test="./@isBoardMember">
								<isBoardMember>
									<xsl:value-of select="./@isBoardMember"/>
								</isBoardMember>
							</xsl:if>
						</person>
					</officer>
				</xsl:for-each>
			</officers>
			<directors>
				<xsl:for-each select="$eBase">
					<director>
						<person xsi:type="Person">
							<xsl:if test="string-length( normalize-space(./consolidationId)) > 0 and number(./consolidationId) > 0">
								<consolidatedId>
									<xsl:value-of select="./consolidationId" />
								</consolidatedId>
							</xsl:if>
							<name>
								<fullName>
									<xsl:value-of select="./name/fullName"/>
								</fullName>
								<firstName>
									<xsl:value-of select="./name/firstName"/>
								</firstName>
								<lastName>
									<xsl:value-of select="./name/lastName"/>
								</lastName>
							</name>
							<xsl:if test="./email">
								<email>
									<xsl:value-of select="./email"/>
								</email>
							</xsl:if>
							<xsl:if test="./phone">
								<phone>
									<xsl:value-of select="./phone"/>
								</phone>
							</xsl:if>
							<xsl:if test="./fax">
								<fax>
									<xsl:value-of select="./fax"/>
								</fax>
							</xsl:if>
							<xsl:if test="./age">
								<age>
									<xsl:value-of select="./age"/>
								</age>
							</xsl:if>
							<xsl:for-each select="./jobTitle">
								<position>
									<name>
										<xsl:value-of select="."/>
									</name>
								</position>
							</xsl:for-each>
							<xsl:if test="./@level">
								<level>
									<xsl:value-of select="./@level"/>
								</level>
							</xsl:if>
							<xsl:if test="./@isBoardMember">
								<isBoardMember>
									<xsl:value-of select="./@isBoardMember"/>
								</isBoardMember>
							</xsl:if>
						</person>
					</director>
				</xsl:for-each>
			</directors>
		</executivesAndOfficersReportData>
	</xsl:template>
	<!--UK Ratios template 									-->
	<xsl:template name="UKRatios">
		<xsl:param name="base"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<xsl:variable name="codes" select="document('Ratios.xml')"/>
		<xsl:variable name="tempCodes" select="$codes/codeSets/codeSet[ @type = 'UKRatios' ]/*"/>
		<ukRatiosReportData>
			<reportType>UKRatios</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<genericFiscalPeriodDescriptions>
				<xsl:call-template name="fiscalPeriodDescriptions">
					<xsl:with-param name="base" select="$base"/>
				</xsl:call-template>
			</genericFiscalPeriodDescriptions>
			<reportItems>
				<xsl:for-each select="$tempCodes">
					<xsl:variable name="t">
						<xsl:value-of select="./@factivaCode"/>
					</xsl:variable>
					<xsl:if test="count($base/*/items/*/*[name() = $t]) > 0">
						<xsl:variable name="formatType" select="normalize-space(./@formatType)"/>
						<reportItem>
							<xsl:variable name="factivaCode">
								<xsl:choose>
									<xsl:when test="./@tokenName">
										<xsl:value-of select="normalize-space(./@tokenName)"/>
									</xsl:when>
									<xsl:when test="./@factivaCode">
										<xsl:value-of select="normalize-space(./@factivaCode)"/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>
							<itemDetails multexCode="{normalize-space(./@multexSOA)}" factivaCode="{$factivaCode}" detail="{normalize-space(./@detail)}" summary="{normalize-space(./@summary)}" rollUp="{normalize-space(./@rollUp)}"/>
							<fiscalPeriods>
								<xsl:for-each select="$base/fiscalPeriod">
									<xsl:call-template name="fiscalPeriod">
										<xsl:with-param name="type" select="./@type"/>
										<xsl:with-param name="endDate" select="./@endDate"/>
										<xsl:with-param name="fiscalYear" select="./@fiscalYear"/>
										<xsl:with-param name="dataNodeSet" select="./items/*/*[name() = $t]"/>
										<xsl:with-param name="formatType" select="$formatType"/>
										<xsl:with-param name="currency" select="$currency"/>
									</xsl:call-template>
								</xsl:for-each>
							</fiscalPeriods>
						</reportItem>
					</xsl:if>
				</xsl:for-each>
			</reportItems>
		</ukRatiosReportData>
	</xsl:template>

	<!--UK Ratios template 									-->
	<xsl:template name="UKRatios2">
		<xsl:param name="base"/>
		<xsl:param name="group"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<xsl:variable name="codes" select="document('Ratios.xml')"/>	
		<xsl:variable name="tempCodes" select="$codes/codeSets/codeSet[ @type = $group ]/*"/>
		
		<ukRatiosReportData>
			<reportType>UKRatios</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<genericFiscalPeriodDescriptions>
				<xsl:call-template name="fiscalPeriodDescriptions">
					<xsl:with-param name="base" select="$base"/>
				</xsl:call-template>
			</genericFiscalPeriodDescriptions>
			<reportItems>
				<xsl:for-each select="$tempCodes">
					<xsl:variable name="t">
						<xsl:value-of select="./@factivaCode"/>
					</xsl:variable>
					<xsl:if test="count($base/*/items/*/*[name() = $t]) > 0">
						<xsl:variable name="formatType" select="normalize-space(./@formatType)"/>
						<reportItem>
							<xsl:variable name="factivaCode">
								<xsl:choose>
									<xsl:when test="./@tokenName">
										<xsl:value-of select="normalize-space(./@tokenName)"/>
									</xsl:when>
									<xsl:when test="./@factivaCode">
										<xsl:value-of select="normalize-space(./@factivaCode)"/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>
							<itemDetails multexCode="{normalize-space(./@multexSOA)}" factivaCode="{$factivaCode}" detail="{normalize-space(./@detail)}" summary="{normalize-space(./@summary)}" rollUp="{normalize-space(./@rollUp)}"/>
							<fiscalPeriods>
								<xsl:for-each select="$base/fiscalPeriod">
									<xsl:call-template name="fiscalPeriod">
										<xsl:with-param name="type" select="./@type"/>
										<xsl:with-param name="endDate" select="./@endDate"/>
										<xsl:with-param name="fiscalYear" select="./@fiscalYear"/>
										<xsl:with-param name="dataNodeSet" select="./items/*/*[name() = $t]"/>
										<xsl:with-param name="formatType" select="$formatType"/>
										<xsl:with-param name="currency" select="$currency"/>
									</xsl:call-template>
								</xsl:for-each>
							</fiscalPeriods>
						</reportItem>
					</xsl:if>
				</xsl:for-each>
			</reportItems>
		</ukRatiosReportData>
	</xsl:template>
	
	<!--Ratios template										-->
	<xsl:template name="KeyRatios">
		<xsl:param name="base"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<xsl:variable name="codes" select="document('Ratios.xml')"/>
		<xsl:variable name="keyRatiosCodePanels" select="$codes/codeSets/codeSet[@type = 'Ratios']/*"/>
		<xsl:variable name="finacialHealthCodePanels" select="$codes/codeSets/codeSet[@type= 'FinancialHealth']/*"/>
		<xsl:variable name="ratioComparison" select="$codes/codeSets/codeSet[@type= 'ratioComparison']/*"/>
		<keyRatiosReportData>
			<reportType>Ratios</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<keyRatioPanels>
				<xsl:call-template name="keyRatioPanels">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="codePanels" select="$keyRatiosCodePanels"/>
					<xsl:with-param name="currency"/>
				</xsl:call-template>
			</keyRatioPanels>
			<financialHealthPanels>
				<xsl:call-template name="keyRatioPanels">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="codePanels" select="$finacialHealthCodePanels"/>
					<xsl:with-param name="currency"/>
				</xsl:call-template>
			</financialHealthPanels>
			<ratioComparisonPanels>
				<xsl:call-template name="keyRatioPanels">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="codePanels" select="$ratioComparison"/>
					<xsl:with-param name="currency"/>
				</xsl:call-template>
			</ratioComparisonPanels>
		</keyRatiosReportData>
	</xsl:template>

	<!--Ratios template										-->
	<xsl:template name="RussianRatios">
		<xsl:param name="base"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<xsl:variable name="codes" select="document('Ratios.xml')"/>
		<xsl:variable name="keyRatiosCodePanels" select="$codes/codeSets/codeSet[@type = 'RussianRatios']/*"/>
		<xsl:variable name="finacialHealthCodePanels" select="$codes/codeSets/codeSet[@type= 'FinancialHealth']/*"/>
		<xsl:variable name="ratioComparison" select="$codes/codeSets/codeSet[@type= 'ratioComparison']/*"/>
		<keyRatiosReportData>
			<reportType>Ratios</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<keyRatioPanels>
				<xsl:call-template name="keyRatioPanels">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="codePanels" select="$keyRatiosCodePanels"/>
					<xsl:with-param name="currency"/>
				</xsl:call-template>
			</keyRatioPanels>
			<financialHealthPanels>
				<xsl:call-template name="keyRatioPanels">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="codePanels" select="$finacialHealthCodePanels"/>
					<xsl:with-param name="currency"/>
				</xsl:call-template>
			</financialHealthPanels>
			<ratioComparisonPanels>
				<xsl:call-template name="keyRatioPanels">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="codePanels" select="$ratioComparison"/>
					<xsl:with-param name="currency"/>
				</xsl:call-template>
			</ratioComparisonPanels>
		</keyRatiosReportData>
	</xsl:template>
	<!--Ratios template										-->
	<xsl:template name="USRatios">
		<xsl:param name="base"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<xsl:variable name="codes" select="document('Ratios.xml')"/>
		<xsl:variable name="codePanels" select="$codes/codeSets/codeSet[@type='USRatios']/*"/>
		<usRatiosReportData>
			<reportType>USRatios</reportType>
			<reportCurrency>
				<xsl:value-of select="$currency"/>
			</reportCurrency>
			<genericFiscalPeriodDescriptions>
				<xsl:call-template name="fiscalPeriodDescriptions">
					<xsl:with-param name="base" select="$base"/>
				</xsl:call-template>
			</genericFiscalPeriodDescriptions>
			<reportItems>
				<reportItem>
					<xsl:variable name="factivaCode">
						<xsl:choose>
							<xsl:when test="./@tokenName">
								<xsl:value-of select="normalize-space(./@tokenName)"/>
							</xsl:when>
							<xsl:when test="./@factivaCode">
								<xsl:value-of select="normalize-space(./@factivaCode)"/>
							</xsl:when>
						</xsl:choose>
					</xsl:variable>
					<itemDetails multexCode="" factivaCode="currentRatio" detail="true" summary="true" rollUp="0"/>
					<fiscalPeriods>
						<xsl:for-each select="$base/fiscalPeriod">
							<xsl:call-template name="fiscalPeriod">
								<xsl:with-param name="type" select="./@type"/>
								<xsl:with-param name="endDate" select="./@endDate"/>
								<xsl:with-param name="fiscalYear" select="./@fiscalYear"/>
								<xsl:with-param name="dataNodeSet" select="./items/*/currentRatio"/>
								<xsl:with-param name="formatType" select="DoubleNumber"/>
								<xsl:with-param name="currency" select="$currency"/>
							</xsl:call-template>
						</xsl:for-each>
					</fiscalPeriods>
				</reportItem>
			</reportItems>
			<usRatioPanels>
				<xsl:for-each select="$codePanels">
					<!-- within panel-->
					<usRatioPanel>
						<usRatioName>
							<xsl:value-of select="./@value"/>
						</usRatioName>
						<usRatioDataRows>
							<xsl:for-each select="./*">
								<!-- within block-->
								<usRatioDataRow>
									<xsl:variable name="t1" select="./code[1]/@factivaCode"/>
									<xsl:variable name="t2" select="./code[2]/@factivaCode"/>
									<usRatioItemDetails multexCode="{normalize-space(./code[1]/@multexSOA)}" factivaCode="{normalize-space(./code[1]/@factivaCode)}" detail="{normalize-space(./code[1]/@detail)}" summary="{normalize-space(./code[1]/@summary)}" rollUp="{normalize-space(./code[1]/@rollUp)}"/>
									<usRatioItems>
										<usRatioItem type="firm">
											<xsl:call-template name="rawData">
												<xsl:with-param name="dataNodeSet" select="$base/fiscalPeriod[1]/items/*/*[name() = $t1]"/>
												<xsl:with-param name="formatType" select="./code[1]/@formatType"/>
												<xsl:with-param name="currency" select="$currency"/>
											</xsl:call-template>
										</usRatioItem>
										<usRatioItem type="industry">
											<xsl:call-template name="rawData">
												<xsl:with-param name="dataNodeSet" select="$base/fiscalPeriod[1]/items/*/*[name() = $t2]"/>
												<xsl:with-param name="formatType" select="./code[2]/@formatType"/>
												<xsl:with-param name="currency" select="$currency"/>
											</xsl:call-template>
										</usRatioItem>
									</usRatioItems>
								</usRatioDataRow>
							</xsl:for-each>
						</usRatioDataRows>
					</usRatioPanel>
				</xsl:for-each>
			</usRatioPanels>
		</usRatiosReportData>
	</xsl:template>
	<!--geographicSegmentInformation template				-->
	<xsl:template name="geographicSegmentInformation">
		<xsl:param name="report"/>
		<xsl:param name="base"/>
		<segmentReportData>
			<xsl:call-template name="BaseSegmentReport">
				<xsl:with-param name="report" select="$report"/>
				<xsl:with-param name="base" select="$base"/>
				<xsl:with-param name="type">GeographicSegmentInformation</xsl:with-param>
			</xsl:call-template>
		</segmentReportData>
	</xsl:template>
	<!--businessSegmentInformation template					-->
	<xsl:template name="businessSegmentInformation">
		<xsl:param name="report"/>
		<xsl:param name="base"/>
		<segmentReportData>
			<xsl:call-template name="BaseSegmentReport">
				<xsl:with-param name="report" select="$report"/>
				<xsl:with-param name="base" select="$base"/>
				<xsl:with-param name="type">BusinessSegmentInformation</xsl:with-param>
			</xsl:call-template>
		</segmentReportData>
	</xsl:template>
	<!--BaseSegmentReport template							-->
	<xsl:template name="BaseSegmentReport">
		<xsl:param name="report"/>
		<xsl:param name="type"/>
		<xsl:param name="base"/>
		<xsl:variable name="codes" select="document('Segments.xml')"/>
		<xsl:variable name="segmentsNodesAnnual" select="$base/fiscalPeriod[@type='Annual']/segment"/>
		<xsl:variable name="segmentsNodesInterim" select="$base/fiscalPeriod[@type='Interim']/segment"/>
		<xsl:variable name="segmentsList" select="$codes/codeSets/codeSet/segment/@orderNumber"/>
		<xsl:variable name="tempCodes" select="$codes/codeSets/codeSet[ @type = $type ]/*"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<reportType>
			<xsl:value-of select="$type"/>
		</reportType>
		<reportCurrency>
			<xsl:value-of select="$currency"/>
		</reportCurrency>
		<segmentFiscalPeriodDescriptions>
			<annualFiscalPeriods count="{count($base/fiscalPeriod[@type='Annual'])}">
				<xsl:call-template name="fiscalPeriodDescriptions">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="type">Annual</xsl:with-param>
				</xsl:call-template>
			</annualFiscalPeriods>
			<interimFiscalPeriods count="{count($base/fiscalPeriod[@type='Interim'])}">
				<xsl:call-template name="fiscalPeriodDescriptions">
					<xsl:with-param name="base" select="$base"/>
					<xsl:with-param name="type">Interim</xsl:with-param>
				</xsl:call-template>
			</interimFiscalPeriods>
		</segmentFiscalPeriodDescriptions>
		<segmentReportItems>
			<annualReportItems>
				<xsl:for-each select="$tempCodes">
					<xsl:variable name="t">
						<xsl:value-of select="./@factivaCode"/>
					</xsl:variable>
					<xsl:variable name="name">
						<xsl:value-of select="$t"/>
					</xsl:variable>
					<xsl:variable name="formatType">
						<xsl:value-of select="./@formatType"/>
					</xsl:variable>
					<xsl:if test="count($base/fiscalPeriod[@type='Annual']/segment/items/*[name() = $t]) > 0">
						<xsl:call-template name="segmentReportItem">
							<xsl:with-param name="t" select="$t"/>
							<xsl:with-param name="name" select="$t"/>
							<xsl:with-param name="type">Annual</xsl:with-param>
							<xsl:with-param name="segmentsNodes" select="$segmentsNodesAnnual"/>
							<xsl:with-param name="segmentsList" select="$segmentsList"/>
							<xsl:with-param name="base" select="$base"/>
							<xsl:with-param name="fiscalPeriodType">Annual</xsl:with-param>
							<xsl:with-param name="currency" select="$currency"/>
							<xsl:with-param name="formatType" select="$formatType"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:for-each>
			</annualReportItems>
			<interimReportItems>
				<xsl:for-each select="$tempCodes">
					<xsl:variable name="t">
						<xsl:value-of select="./@factivaCode"/>
					</xsl:variable>
					<xsl:variable name="name">
						<xsl:value-of select="$t"/>
					</xsl:variable>
					<xsl:variable name="formatType">
						<xsl:value-of select="./@formatType"/>
					</xsl:variable>
					<xsl:if test="count($base/fiscalPeriod[@type='Interim']/segment/items/*[name() = $t]) > 0">
						<xsl:call-template name="segmentReportItem">
							<xsl:with-param name="t" select="$t"/>
							<xsl:with-param name="name" select="$t"/>
							<xsl:with-param name="type">Interim</xsl:with-param>
							<xsl:with-param name="segmentsNodes" select="$segmentsNodesInterim"/>
							<xsl:with-param name="segmentsList" select="$segmentsList"/>
							<xsl:with-param name="base" select="$base"/>
							<xsl:with-param name="fiscalPeriodType">Interim</xsl:with-param>
							<xsl:with-param name="currency" select="$currency"/>
							<xsl:with-param name="formatType" select="$formatType"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:for-each>
			</interimReportItems>
		</segmentReportItems>
	</xsl:template>
	<!--segmentReportItem template							-->
	<xsl:template name="segmentReportItem">
		<xsl:param name="t"/>
		<xsl:param name="name"/>
		<xsl:param name="type"/>
		<xsl:param name="segmentsNodes"/>
		<xsl:param name="segmentsList"/>
		<xsl:param name="fiscalPeriodType"/>
		<xsl:param name="currency"/>
		<xsl:param name="base"/>
		<xsl:param name="formatType"/>
		<categoryItem>
			<segment factivaCode="{$t}">
				<descriptor>
					<xsl:value-of select="$name"/>
				</descriptor>
			</segment>
			<reportItems>
				<xsl:for-each select="$segmentsList">
					<xsl:sort select="." order="ascending" data-type="number"/>
					<xsl:variable name="oNumber" select="."/>
					<xsl:variable name="tCode" select="$base/fiscalPeriod[@type=$type]/segment[@orderNumber = $oNumber]/code/@code"/>
					<xsl:if test="$tCode">
						<reportItem>
							<xsl:variable name="tName">
								<xsl:choose>
									<xsl:when test="$tCode = 'SEGMTL'">segmentTotal</xsl:when>
									<xsl:when test="$tCode = 'EXPOTH'">unallocated</xsl:when>
									<xsl:when test="$tCode = 'CONSTL'">consolidatedTotal</xsl:when>
									<xsl:when test="$tCode = 'OTHERS'">others</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$base/fiscalPeriod/segment[@orderNumber = $oNumber]/name"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="tRollUp">
								<xsl:choose>
									<xsl:when test="$tCode = 'SEGMTL'">1</xsl:when>
									<xsl:when test="$tCode = 'EXPOTH'">0</xsl:when>
									<xsl:when test="$tCode = 'CONSTL'">1</xsl:when>
									<xsl:when test="$tCode = 'OTHERS'">0</xsl:when>
									<xsl:otherwise>0</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<itemDetails factivaCode="{$tCode}" rollUp="{$tRollUp}">
								<xsl:value-of select="$tName"/>
							</itemDetails>
							<fiscalPeriods>
								<xsl:for-each select="$base/fiscalPeriod[@type=$fiscalPeriodType]">
									<xsl:call-template name="fiscalPeriod">
										<xsl:with-param name="type" select="./@type"/>
										<xsl:with-param name="endDate" select="./@endDate"/>
										<xsl:with-param name="fiscalYear" select="./@fiscalYear"/>
										<xsl:with-param name="dataNodeSet" select="./segment[@orderNumber = $oNumber]/items/*[name() = $t]"/>
										<xsl:with-param name="currency" select="$currency"/>
										<xsl:with-param name="formatType" select="$formatType"/>
									</xsl:call-template>
								</xsl:for-each>
							</fiscalPeriods>
						</reportItem>
					</xsl:if>
				</xsl:for-each>
			</reportItems>
		</categoryItem>
	</xsl:template>
	<!--BaseGenericReport template							-->
	<xsl:template name="BaseGenericReport">
		<xsl:param name="type"/>
		<xsl:param name="report"/>
		<xsl:param name="codes"/>
		<xsl:param name="base"/>
		<xsl:variable name="tempCodes" select="$codes/codeSets/codeSet[ @type = $type ]/*"/>
		<xsl:variable name="currency" select="$base/reportingCurrency/@code"/>
		<reportType>			
			<xsl:choose>
				<xsl:when test="$report ='balanceSheet'">BalanceSheet</xsl:when>
				<xsl:when test="$report ='cashFlow'">CashFlow</xsl:when>
				<xsl:when test="$report = 'incomeStatement'">IncomeStatement</xsl:when>
				<xsl:when test="$report = 'ratiosB'">RatiosB</xsl:when>
			</xsl:choose>
		</reportType>
		<reportCurrency>
			<xsl:value-of select="$currency"/>
		</reportCurrency>
		<reportAccountingStandard>
			<xsl:value-of select="$base/@accountingStandard"/>
		</reportAccountingStandard>
		<genericFiscalPeriodDescriptions>
			<xsl:call-template name="fiscalPeriodDescriptions">
				<xsl:with-param name="base" select="$base"/>
			</xsl:call-template>
		</genericFiscalPeriodDescriptions>
		<reportItems>			
			<xsl:for-each select="$tempCodes">				
				<xsl:variable name="t">					
					<xsl:value-of select="./@factivaCode"/>
				</xsl:variable>
				<xsl:if test="count($base/*/items/*[name() = $t]) > 0">
					<xsl:variable name="formatType" select="normalize-space(./@formatType)"/>
					<reportItem>
						<xsl:variable name="factivaCode">
							<xsl:choose>
								<xsl:when test="./@tokenName">
									<xsl:value-of select="normalize-space(./@tokenName)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="normalize-space(./@factivaCode)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<itemDetails multexCode="{normalize-space(./@multexSOA)}" factivaCode="{normalize-space($factivaCode)}" detail="{normalize-space(./@detail)}" summary="{normalize-space(./@summary)}" rollUp="{normalize-space(./@rollUp)}"/>
						<fiscalPeriods>
							<xsl:for-each select="$base/fiscalPeriod">
								<xsl:call-template name="fiscalPeriod">
									<xsl:with-param name="type" select="./@type"/>
									<xsl:with-param name="endDate" select="./@endDate"/>
									<xsl:with-param name="fiscalYear" select="./@fiscalYear"/>
									<xsl:with-param name="dataNodeSet" select="./items/*[name() = $t]"/>
									<xsl:with-param name="formatType" select="$formatType"/>
									<xsl:with-param name="currency" select="$currency"/>
								</xsl:call-template>
							</xsl:for-each>
						</fiscalPeriods>
					</reportItem>
				</xsl:if>
			</xsl:for-each>
		</reportItems>
	</xsl:template>
	<!--BalanceSheet template								-->
	<xsl:template name="BalanceSheet">
		<xsl:param name="type"/>
		<xsl:param name="report"/>
		<xsl:param name="base"/>
		<xsl:variable name="codes" select="document('BalanceSheet.xml')"/>
		<genericReportData>
			<xsl:call-template name="BaseGenericReport">
				<xsl:with-param name="type" select="$type"/>
				<xsl:with-param name="report" select="$report"/>
				<xsl:with-param name="codes" select="$codes"/>
				<xsl:with-param name="base" select="$base"/>
			</xsl:call-template>
		</genericReportData>
	</xsl:template>
	<!--CashFlow template									-->
	<xsl:template name="CashFlow">
		<xsl:param name="type"/>
		<xsl:param name="report"/>
		<xsl:param name="base"/>
		<xsl:variable name="codes" select="document('CashFlow.xml')"/>
		<genericReportData>
			<xsl:call-template name="BaseGenericReport">
				<xsl:with-param name="type" select="$type"/>
				<xsl:with-param name="report" select="$report"/>
				<xsl:with-param name="codes" select="$codes"/>
				<xsl:with-param name="base" select="$base"/>
			</xsl:call-template>
		</genericReportData>
	</xsl:template>
	<!--IncomeStatement template							-->
	<xsl:template name="IncomeStatement">
		<xsl:param name="type"/>
		<xsl:param name="report"/>
		<xsl:param name="base"/>
		<xsl:variable name="codes" select="document('IncomeStatement.xml')"/>
		<genericReportData>
			<xsl:call-template name="BaseGenericReport">
				<xsl:with-param name="type" select="$type"/>
				<xsl:with-param name="report" select="$report"/>
				<xsl:with-param name="codes" select="$codes"/>
				<xsl:with-param name="base" select="$base"/>
			</xsl:call-template>
		</genericReportData>
	</xsl:template>
	<!-- StripDownMetaPT -->
	<xsl:template name="stripDownMetaData">
		<xsl:param name="meta"/>
		<xsl:call-template name="ProviderObject">
			<xsl:with-param name="providerId">
				<xsl:value-of select="$meta/PubData/SrcCode/@value"/>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>
	<!--MetadataPT Processing template						-->
	<xsl:template name="metaData" match="MetadataPT" mode="base">
		<accessionNo fid="an" value="{./DocData/AccessionNo/@value}"/>
		<baseLanguage>
			<xsl:value-of select="./DocData/BaseLang/@value"/>
		</baseLanguage>
		<xsl:if test="string-length ( normalize-space( ./DocData/Date[@fid='pd']/@value ) ) > 0">
			<publicationDate>
				<date>
					<xsl:value-of select="substring(./DocData/Date[@fid='pd']/@value,1,4)"/>-<xsl:value-of select="substring(./DocData/Date[@fid='pd']/@value,5,2)"/>-<xsl:value-of select="substring(./DocData/Date[@fid='pd']/@value,7)"/>
				</date>
			</publicationDate>
		</xsl:if>
		<xsl:if test="string-length ( normalize-space( ./DocData/Date[@fid='md']/@value ) ) > 0">
			<modificationDate>
				<date>
					<xsl:value-of select="substring(./DocData/Date[@fid='md']/@value,1,4)"/>-<xsl:value-of select="substring(./DocData/Date[@fid='md']/@value,5,2)"/>-<xsl:value-of select="substring(./DocData/Date[@fid='md']/@value,7)"/>
				</date>
			</modificationDate>
		</xsl:if>
		<xsl:call-template name="ProviderObject">
			<xsl:with-param name="providerId">
				<xsl:value-of select="./PubData/SrcCode/@value"/>
			</xsl:with-param>
		</xsl:call-template>
		<xsl:for-each select="./CodeSets/CSet[@fid='co']/*">
			<company code="{./@value}">
				<xsl:if test="string-length( normalize-space( ./CodeD/@value ) ) > 0">
					<name>
						<xsl:value-of select="./CodeD/@value"/>
					</name>
				</xsl:if>
			</company>
		</xsl:for-each>
		<xsl:for-each select="./CodeSets/CSet[@fid='in']/*">
			<industry code="{./@value}">
				<xsl:if test="string-length( normalize-space( ./CodeD/@value ) ) > 0">
					<name>
						<xsl:value-of select="./CodeD/@value"/>
					</name>
				</xsl:if>
			</industry>
		</xsl:for-each>
		<xsl:for-each select="./CodeSets/CSet[@fid='in']/*">
			<region code="{./@value}">
				<xsl:if test="string-length( normalize-space( ./CodeD/@value ) ) > 0">
					<name>
						<xsl:value-of select="./CodeD/@value"/>
					</name>
				</xsl:if>
			</region>
		</xsl:for-each>
	</xsl:template>
	<!--fiscalPeriod Processing template					-->
	<!-- Function  fiscalPeriod (string type, string endDate, string fiscalyear, string dataNodeSet) -->
	<xsl:template name="fiscalPeriod">
		<xsl:param name="type"/>
		<xsl:param name="endDate"/>
		<xsl:param name="fiscalYear"/>
		<xsl:param name="dataNodeSet"/>
		<xsl:param name="currency"/>
		<xsl:param name="formatType"/>
		<fiscalPeriod type="{$type}">
			<xsl:if test="string-length(normalize-space($endDate)) > 0">
				<endDate>
					<date>
						<xsl:value-of select="$endDate"/>
					</date>
				</endDate>
			</xsl:if>
			<xsl:if test="string-length(normalize-space($fiscalYear)) > 0">
				<fiscalYear>
					<xsl:value-of select="$fiscalYear"/>
				</fiscalYear>
			</xsl:if>
			<xsl:if test="string-length(normalize-space($dataNodeSet)) > 0 and string(number($dataNodeSet))!= 'NaN'">
				<xsl:choose>
					<xsl:when test="$formatType = 'Percent'">
						<rawData xsi:type="Percent" value="{normalize-space($dataNodeSet)}"/>
					</xsl:when>
					<xsl:when test="$formatType = 'WholeNumber'">
						<rawData xsi:type="WholeNumber" value="{normalize-space($dataNodeSet)}"/>
					</xsl:when>
					<xsl:when test="$formatType = 'DoubleMoney'">
						<rawData xsi:type="DoubleMoney" value="{normalize-space($dataNodeSet)}">
							<currency code="{$currency}"/>
						</rawData>
					</xsl:when>
					<xsl:when test="$formatType = 'DoubleNumber'">
						<rawData xsi:type="DoubleNumber" value="{normalize-space($dataNodeSet)}"/>
					</xsl:when>
					<xsl:when test="$formatType = 'DoubleMoneyCurrency'">
						<rawData xsi:type="DoubleMoneyCurrency" value="{normalize-space($dataNodeSet)}">
							<currency code="{$currency}"/>
						</rawData>
					</xsl:when>
					<xsl:when test="$formatType = 'PrecisionNumber'">
						<rawData xsi:type="PrecisionNumber" value="{normalize-space($dataNodeSet)}" precision="1"/>
					</xsl:when>
					<xsl:otherwise>
						<rawData xsi:type="DoubleMoney" value="{normalize-space($dataNodeSet)}">
							<currency code="{$currency}"/>
						</rawData>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</fiscalPeriod>
	</xsl:template>
	<!--fiscalPeriodDescriptions template					-->

	<xsl:template name="rawData">
		<xsl:param name="dataNodeSet"/>
		<xsl:param name="formatType"/>
		<xsl:param name="currency"/>
		<xsl:choose>
			<xsl:when test="$formatType = 'YearMonthDay'">
				<ratioDateYear>
					<xsl:call-template name="YearMonthDay">
						<xsl:with-param name="baseNode" select="$dataNodeSet"/>
					</xsl:call-template>
				</ratioDateYear>
			</xsl:when>
			<xsl:when test="$formatType = 'FormattedDate'">
				<ratioDate>
					<date>
						<xsl:value-of select="$dataNodeSet"/>
					</date>
				</ratioDate>
			</xsl:when>
			<xsl:otherwise>
				
				<!--special case for percent to eliminate NaN when plus sign in value-->
				<xsl:if test="string-length(normalize-space($dataNodeSet)) > 0 and $formatType = 'Percent'">
					<xsl:if test="string(number(translate($dataNodeSet,'+','')))!= 'NaN'">
						<rawData xsi:type="Percent" value= "{translate($dataNodeSet,'+','')}"/>
					</xsl:if>
				</xsl:if>			
				
				<xsl:if test="string-length(normalize-space($dataNodeSet)) > 0 and string(number($dataNodeSet))!= 'NaN'">
					<xsl:choose>
						<!--special case for percent to eliminate NaN when plus sign in value-->
						<!--<xsl:when test="$formatType = 'Percent'">
							<rawData xsi:type="Percent" value="{$dataNodeSet}"/>						
						</xsl:when>-->
						<xsl:when test="$formatType = 'WholeNumber'">
							<rawData xsi:type="WholeNumber" value="{$dataNodeSet}"/>
						</xsl:when>
						<xsl:when test="$formatType = 'DoubleMoney'">
							<rawData xsi:type="DoubleMoney" value="{$dataNodeSet}">
								<currency code="{$currency}"/>
							</rawData>
						</xsl:when>
						<xsl:when test="$formatType = 'DoubleNumber'">
							<rawData xsi:type="DoubleNumber" value="{$dataNodeSet}"/>
						</xsl:when>
						<xsl:when test="$formatType = 'DoubleMoneyCurrency'">
							<rawData xsi:type="DoubleMoneyCurrency" value="{$dataNodeSet}">
								<currency code="{$currency}"/>
							</rawData>
						</xsl:when>
						<xsl:when test="$formatType = 'PrecisionNumber'">
							<rawData xsi:type="PrecisionNumber" value="{$dataNodeSet}" precision="1"/>
						</xsl:when>
						<xsl:otherwise>
							<rawData xsi:type="DoubleMoney" value="{$dataNodeSet}">
								<currency code="{$currency}"/>
							</rawData>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="fiscalPeriodDescriptions">
		<xsl:param name="base"/>
		<xsl:param name="type"/>
		<xsl:choose>
			<xsl:when test="$type">
				<xsl:for-each select="$base/fiscalPeriod[ @type = $type ]">
					<xsl:call-template name="fiscalPeriodDetail">
						<xsl:with-param name="base" select="."/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$base/fiscalPeriod">
					<xsl:call-template name="fiscalPeriodDetail">
						<xsl:with-param name="base" select="."/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!--fiscalPeriod template					-->
	<xsl:template name="fiscalPeriodDetail">
		<xsl:param name="base"/>
		<xsl:if test="$base">
			<fiscalPeriodDescription type="{$base/@type}">
				<xsl:if test="string-length(normalize-space($base/@endDate)) > 0">
					<endDate>
						<date>
							<xsl:value-of select="$base/@endDate"/>
						</date>
					</endDate>
				</xsl:if>
				<xsl:if test="string-length(normalize-space($base/@fiscalYear)) > 0">
					<fiscalYear>
						<xsl:value-of select="$base/@fiscalYear"/>
					</fiscalYear>
				</xsl:if>
				<xsl:if test="$base/@periodNumber">
					<periodNumber>
						<xsl:value-of select="$base/@periodNumber"/>
					</periodNumber>
				</xsl:if>
				<xsl:if test="$base/statementDate and string-length(normalize-space($base/statementDate)) > 0">
					<statementDate>
						<date>
							<xsl:value-of select="$base/statementDate"/>
						</date>
					</statementDate>
				</xsl:if>
				<xsl:if test="$base/updateType">
					<updateType>
						<xsl:value-of select="$base/updateType"/>
					</updateType>
				</xsl:if>
				<xsl:if test="$base/sourceDocument">
					<sourceDocument>
						<xsl:value-of select="$base/sourceDocument"/>
					</sourceDocument>
				</xsl:if>
				<xsl:if test="$base/filingDate">
					<filingDate>
						<date>
							<xsl:value-of select="$base/filingDate"/>
						</date>
					</filingDate>
				</xsl:if>
				<xsl:if test="$base/auditor">
					<auditor code="{$base/auditor/code/@code}" scheme="{$base/auditor/code/@scheme}">
						<name>
							<xsl:value-of select="$base/auditor/name"/>
						</name>
					</auditor>
				</xsl:if>
				<xsl:if test="$base/auditorOpinion">
					<auditorOpinion>
						<xsl:value-of select="$base/auditorOpinion"/>
					</auditorOpinion>
				</xsl:if>
				<xsl:if test="$base/auditorOpinionText">
					<auditorOpinionText>
						<xsl:apply-templates select="$base/auditorOpinionText/Para"/>
					</auditorOpinionText>
				</xsl:if>
				<xsl:if test="$base/fiscalAccountingStandard">
					<fiscalAccountingStandard code="{$base/fiscalAccountingStandard/code/@code}" scheme="{$base/fiscalAccountingStandard/code/@scheme}">
						<name>
							<xsl:value-of select="$base/fiscalAccountingStandard/name"/>
						</name>
					</fiscalAccountingStandard>
				</xsl:if>
				<xsl:if test="$base/items/employees">
					<numberOfEmployees xsi:type="WholeNumber" value="{$base/items/employees}"/>
				</xsl:if>
				<xsl:if test="$base/items/numberOfEstablishmentsWithinTheIndustry">
					<numberOfCompanies xsi:type="WholeNumber" value="{$base/items/numberOfEstablishmentsWithinTheIndustry}"/>
				</xsl:if>
				<xsl:if test="$base/figuresInformation">
					<figuresInformation>
						<xsl:value-of select="$base/figuresInformation"/>
					</figuresInformation>
				</xsl:if>
			</fiscalPeriodDescription>
		</xsl:if>
	</xsl:template>
	<xsl:template name="jobTitle">
		<title>
			<xsl:if test="count(@code) &gt; 0">
				<xsl:attribute name="code">
					<xsl:call-template name="PersonTitleMapper">
						<xsl:with-param name="type">
							<xsl:value-of select="@code"/>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:attribute>
			</xsl:if>
			<xsl:value-of select="."/>
		</title>
	</xsl:template>
	<xsl:template match="Para">
		<xsl:choose>
			<xsl:when test="@display = 'asis'">
				<para type="Fixed">
					<xsl:value-of select="."/>
				</para>
			</xsl:when>
			<xsl:otherwise>
				<para type="Proportional">
					<xsl:value-of select="."/>
				</para>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="addressField">
		<xsl:param name="field"/>
		<xsl:if test="normalize-space($field) != ''">
			<address>
				<xsl:value-of select="$field"/>
			</address>
		</xsl:if>
	</xsl:template>
	<xsl:template name="keyFiancialItems">
		<xsl:param name="base"/>
		<xsl:param name="codeSets"/>
		<xsl:param name="currency"/>
		<xsl:for-each select="$codeSets/*">
			<xsl:choose>
				<xsl:when test="name() = 'code'">
					<xsl:variable name="t" select="./@factivaCode"/>
					<xsl:variable name="tName" select="."/>
					<keyFinancialItem>
						<keyFinancialItemDetails multexCode="{normalize-space(./@multexSOA)}" factivaCode="{normalize-space(./@factivaCode)}" detail="{normalize-space(./@detail)}" summary="{normalize-space(./@summary)}" rollUp="{normalize-space(./@rollUp)}"/>
						<xsl:if test="boolean($base/items/*[name() = $t])">
							<xsl:call-template name="rawData">
								<xsl:with-param name="dataNodeSet" select="$base/items/*[name() = $t]"/>
								<xsl:with-param name="formatType" select="./@formatType"/>
								<xsl:with-param name="currency" select="$currency"/>
							</xsl:call-template>
						</xsl:if>
					</keyFinancialItem>
				</xsl:when>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="keyRatioPanels">
		<xsl:param name="base"/>
		<xsl:param name="codePanels"/>
		<xsl:param name="currency"/>
		<xsl:for-each select="$codePanels">
			<keyRatioPanel>
				<keyRatioName>
					<xsl:value-of select="./@value"/>
				</keyRatioName>
				<keyRatioItems>
					<xsl:for-each select="./*">
						<xsl:choose>
							<xsl:when test="name() = 'code'">
								<xsl:variable name="t" select="./@factivaCode"/>
								<xsl:variable name="tName" select="."/>
								<keyRatioItem>
									<keyRatioItemDetails multexCode="{normalize-space(./@multexSOA)}" factivaCode="{normalize-space(./@factivaCode)}" detail="{normalize-space(./@detail)}" summary="{normalize-space(./@summary)}" rollUp="{normalize-space(./@rollUp)}"/>
									<!--<keyRatioItemDetails multexCode="{normalize-space(./@multexSOA)}" factivaCode="{substring(normalize-space(./@factivaCode),1,50)}" detail="{normalize-space(./@detail)}" summary="{normalize-space(./@summary)}" rollUp="{normalize-space(./@rollUp)}"/>-->
									<xsl:if test="$base/*/*[name() = $t]">
										<xsl:call-template name="rawData">
											<xsl:with-param name="dataNodeSet" select="$base/*/*[name() = $t]"/>
											<xsl:with-param name="formatType" select="./@formatType"/>
											<xsl:with-param name="currency" select="$currency"/>
										</xsl:call-template>
									</xsl:if>
								</keyRatioItem>
							</xsl:when>
							<xsl:when test="name() = 'descision'">
								<xsl:variable name="t1" select="./code[1]/@factivaCode"/>
								<xsl:variable name="t1Name" select="./code[1]/@factivaCode"/>
								<xsl:variable name="t2" select="./code[2]/@factivaCode"/>
								<xsl:variable name="t2Name" select="./code[2]/@factivaCode"/>
								<xsl:choose>
									<xsl:when test="$base/*/*[name() = $t1]">
										<keyRatioItem>
											<keyRatioItemDetails multexCode="{normalize-space(./code[1]/@multexSOA)}" factivaCode="{normalize-space(./code[1]/@factivaCode)}" detail="{normalize-space(./code[1]/@detail)}" summary="{normalize-space(./code[1]/@summary)}" rollUp="{normalize-space(./code[1]/@rollUp)}"/>
											<xsl:call-template name="rawData">
												<xsl:with-param name="dataNodeSet" select="$base/*/*[name() = $t1]"/>
												<xsl:with-param name="formatType" select="./code[1]/@formatType"/>
												<xsl:with-param name="currency" select="$currency"/>
											</xsl:call-template>
										</keyRatioItem>
									</xsl:when>
									<xsl:otherwise>
										<keyRatioItem>
											<keyRatioItemDetails multexCode="{normalize-space(./code[2]/@multexSOA)}" factivaCode="{normalize-space(./code[2]/@factivaCode)}" detail="{normalize-space(./code[2]/@detail)}" summary="{normalize-space(./code[2]/@summary)}" rollUp="{normalize-space(./code[2]/@rollUp)}"/>
											<xsl:if test="$base/*/*[name() = $t2]">
												<xsl:call-template name="rawData">
													<xsl:with-param name="dataNodeSet" select="$base/*/*[name() = $t2]"/>
													<xsl:with-param name="formatType" select="./code[2]/@formatType"/>
													<xsl:with-param name="currency" select="$currency"/>
												</xsl:call-template>
											</xsl:if>
										</keyRatioItem>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
						</xsl:choose>
					</xsl:for-each>
				</keyRatioItems>
			</keyRatioPanel>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="name">
		<name>
			<xsl:if test="boolean(./firstName)">
				<firstName>
					<xsl:value-of select="./firstName"/>
				</firstName>
			</xsl:if>
			<xsl:if test="boolean(./middleNames)">
				<middleNames>
					<xsl:value-of select="./middleNames"/>
				</middleNames>
			</xsl:if>
			<xsl:if test="boolean(./lastName)">
				<lastName>
					<xsl:value-of select="./lastName"/>
				</lastName>
			</xsl:if>
			<xsl:if test="boolean(./suffix)">
				<suffix>
					<xsl:value-of select="./suffix"/>
				</suffix>
			</xsl:if>
			<xsl:if test="boolean(./fullName)">
				<fullName>
					<xsl:value-of select="./fullName"/>
				</fullName>
			</xsl:if>
		</name>
	</xsl:template>
	<xsl:template name="Address">
		<xsl:param name="baseAddress"/>
		<xsl:if test="$baseAddress">
			<xsl:if test="boolean($baseAddress/street1)">
				<address>
					<xsl:value-of select="$baseAddress/street1"/>
				</address>
			</xsl:if>
			<xsl:if test="boolean($baseAddress/street2)">
				<address>
					<xsl:value-of select="$baseAddress/street2"/>
				</address>
			</xsl:if>
			<xsl:if test="boolean($baseAddress/street3)">
				<address>
					<xsl:value-of select="$baseAddress/street3"/>
				</address>
			</xsl:if>
			<xsl:if test="boolean($baseAddress/street4)">
				<address>
					<xsl:value-of select="$baseAddress/street4"/>
				</address>
			</xsl:if>
			<city>
				<xsl:value-of select="$baseAddress/city"/>
			</city>
			<state>
				<xsl:choose>
					<xsl:when test="$baseAddress/stateProvinceCountry">
						<xsl:value-of select="$baseAddress/stateProvinceCountry"/>
					</xsl:when>
					<xsl:when test="$baseAddress/stateProvinceCounty">
						<xsl:value-of select="$baseAddress/stateProvinceCounty"/>
					</xsl:when>
				</xsl:choose>
			</state>
			<zip>
				<xsl:value-of select="$baseAddress/zipPostalCode"/>
			</zip>
			<country>
				<xsl:value-of select="$baseAddress/country"/>
			</country>
		</xsl:if>
	</xsl:template>
	<xsl:template name="PhoneNumber">
		<xsl:param name="baseNumber"/>
		<number>
			<xsl:value-of select="$baseNumber/number"/>
		</number>
		<xsl:if test="count($baseNumber/cityAreaCode) &gt; 0">
			<cityAreaCode>
				<xsl:value-of select="normalize-space($baseNumber/cityAreaCode)"/>
			</cityAreaCode>
		</xsl:if>
		<xsl:if test="count($baseNumber/countryRegionCode) &gt; 0">
			<countryRegionCode>
				<xsl:value-of select="normalize-space($baseNumber/countryRegionCode)"/>
			</countryRegionCode>
		</xsl:if>
		<xsl:if test="count($baseNumber/extension) &gt; 0">
			<extension>
				<xsl:value-of select="normalize-space($baseNumber/extension)"/>
			</extension>
		</xsl:if>
	</xsl:template>
	<xsl:template name="faxNumber">
		<xsl:param name="baseNumber"/>
		Before referencing to faxNumber template do not forget to create one!!!!!
	</xsl:template>
	<xsl:template name="YearMonthDay">
		<xsl:param name="baseNode"/>
		<xsl:attribute name="year">
			<xsl:value-of select="$baseNode/@year"/>
		</xsl:attribute>
		<xsl:if test="boolean($baseNode/@month)">
			<xsl:attribute name="month">
				<xsl:value-of select="$baseNode/@month"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="boolean($baseNode/@day)">
			<xsl:attribute name="day">
				<xsl:value-of select="$baseNode/@day"/>
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	<xsl:template name="EnterpriseTotals">
		<xsl:param name ="baseNode"></xsl:param>
		<xsl:if test="boolean($baseNode/pcs)">
			<personalComputers xsi:type="WholeNumber" value="{$baseNode/pcs}" />
		</xsl:if>
		<xsl:if test="boolean($baseNode/pcsDataAccuracy)">
			<personalComputersAccuracy>
				<xsl:call-template name="DataAccuracyMapper">
					<xsl:with-param name="type">
						<xsl:value-of select="$baseNode/pcsDataAccuracy"/>
					</xsl:with-param>
				</xsl:call-template>
			</personalComputersAccuracy>
		</xsl:if>
		<xsl:if test="boolean($baseNode/servers)">
			<servers xsi:type="WholeNumber" value="{$baseNode/servers}"/>
		</xsl:if>
		<xsl:if test="boolean($baseNode/serversDataAccuracy)">
			<serversAccuracy>
				<xsl:call-template name="DataAccuracyMapper">
					<xsl:with-param name="type">
						<xsl:value-of select="$baseNode/serversDataAccuracy"/>
					</xsl:with-param>
				</xsl:call-template>
			</serversAccuracy>
		</xsl:if>

		<xsl:if test="boolean($baseNode/printers)">
			<printers xsi:type="WholeNumber" value="{$baseNode/printers}"/>
		</xsl:if>
		<xsl:if test="boolean($baseNode/printersDataAccuracy)">
			<printersAccuracy>
				<xsl:call-template name="DataAccuracyMapper">
					<xsl:with-param name="type">
						<xsl:value-of select="$baseNode/printersDataAccuracy"/>
					</xsl:with-param>
				</xsl:call-template>
			</printersAccuracy>
		</xsl:if>

		<xsl:if test="boolean($baseNode/storageCapacity)">
			<storageCapacity xsi:type="WholeNumber" value="{$baseNode/storageCapacity}"/>
		</xsl:if>
		<xsl:if test="boolean($baseNode/storageCapacityDataAccuracy)">
			<storageCapacityAccuracy>
				<xsl:call-template name="DataAccuracyMapper">
					<xsl:with-param name="type">
						<xsl:value-of select="$baseNode/storageCapacityDataAccuracy"/>
					</xsl:with-param>
				</xsl:call-template>
			</storageCapacityAccuracy>
		</xsl:if>
		<xsl:if test="boolean($baseNode/highSpeedLines)">
			<highSpeedLines xsi:type="WholeNumber" value="{$baseNode/highSpeedLines}"/>
		</xsl:if>
		<xsl:if test="boolean($baseNode/highSpeedLinesDataAccuracy)">
			<highSpeedLinesAccuracy>
				<xsl:call-template name="DataAccuracyMapper">
					<xsl:with-param name="type">
						<xsl:value-of select="$baseNode/highSpeedLinesDataAccuracy"/>
					</xsl:with-param>
				</xsl:call-template>
			</highSpeedLinesAccuracy>
		</xsl:if>
	</xsl:template>
	<!-- KeyFiguresHoppenstedt template [cokfg]		q307	-->
	<xsl:template name="KeyFiguresHoppenstedt">
		<xsl:param name="base"/>
		<keyFiguresReportData>
			<tabularReportType>KeyFiguresHoppenstedt</tabularReportType>
			<!--<reportTables>-->
			<xsl:for-each select ="$base/table">
				<reportTable>
					<reportTableType>
						<xsl:value-of select="type"></xsl:value-of>
					</reportTableType>
					<reportTableCurrency>
						<xsl:value-of select="currency/@code"></xsl:value-of>
					</reportTableCurrency>
					<reportTableHeader>
						<reportTableDescription>
							<xsl:value-of select="header/description"></xsl:value-of>
						</reportTableDescription>
						<xsl:for-each select="header/column">
							<xsl:call-template name="headerColumn"/>
						</xsl:for-each>
					</reportTableHeader>
					<xsl:for-each select="row">
						<xsl:call-template name="tableRow"/>
					</xsl:for-each>
				</reportTable>
			</xsl:for-each>
			<!--</reportTables>-->
		</keyFiguresReportData>
	</xsl:template>
	<xsl:template name="tableRow">
		<reportTableRow>
			<reportTableDescription>
				<xsl:value-of select="description"/>
			</reportTableDescription>
			<xsl:for-each select="column">
				<xsl:call-template name="tableColumn"/>
			</xsl:for-each>
		</reportTableRow>
	</xsl:template>
	<xsl:template name="tableColumn">
		<reportTableColumn>
			<type>
				<xsl:value-of select="@type"/>
			</type>
			<xsl:call-template name="rawData">
				<xsl:with-param name="dataNodeSet" select="."/>
				<xsl:with-param name="formatType">WholeNumber</xsl:with-param>
			</xsl:call-template>
		</reportTableColumn>
	</xsl:template>
	<xsl:template name ="headerColumn">
		<headerColumn>
			<xsl:value-of select="."/>
		</headerColumn>
	</xsl:template>

	<!-- RatiosBHoppenstedst template q307-->
	<xsl:template name="RatiosBHoppenstedt">
		<xsl:param name="type"/>
		<xsl:param name="report"/>
		<xsl:param name="base"/>
		<xsl:variable name="codes" select="document('RatiosB.xml')"/>
		<genericReportData>
			<xsl:call-template name="BaseGenericReport">
				<xsl:with-param name="type" select="$type"/>
				<xsl:with-param name="report" select="$report"/>
				<xsl:with-param name="codes" select="$codes"/>
				<xsl:with-param name="base" select="$base"/>
			</xsl:call-template>		
		</genericReportData>
	</xsl:template>

	<!-- ImportExport template q307-->
	<xsl:template name="ImportExport">
		<xsl:param name="base"/>
		<importExportReportData>
			<tabularReportType>ImportExportTable</tabularReportType>
			<xsl:for-each select ="$base/import">
				<importTable>					
					<xsl:call-template name="ImportExportTable"/>
				</importTable>
			</xsl:for-each>
			<xsl:for-each select ="$base/export">
				<exportTable>
					<xsl:call-template name="ImportExportTable"/>
				</exportTable>
			</xsl:for-each>
		</importExportReportData>
	</xsl:template>

	<xsl:template name="ImportExportTable">			
		<fiscalYear>
			<xsl:value-of select="./@fiscalYear"/>
		</fiscalYear>
		<reportTableCurrency>
			<xsl:value-of select="./currency/@code"/>
		</reportTableCurrency>
		<total>
			<!--<xsl:value-of select="./total"/>-->
			<xsl:call-template name="rawData">
				<xsl:with-param name="dataNodeSet" select="./total"/>
				<xsl:with-param name="formatType">WholeNumber</xsl:with-param>
			</xsl:call-template>
		</total>
		<xsl:for-each select="./countryValue">
			<reportTableRow>
				<reportTableDescription>
					<xsl:value-of select="country/descriptor"/>
				</reportTableDescription>
				<reportTableColumn>					
					<xsl:call-template name="rawData">
						<xsl:with-param name="dataNodeSet" select="value"/>
						<xsl:with-param name="formatType">WholeNumber</xsl:with-param>
					</xsl:call-template>
				</reportTableColumn>
				<reportTableColumn>
					<xsl:call-template name="rawData">
						<xsl:with-param name="dataNodeSet" select="percentOfTotal"/>
						<xsl:with-param name="formatType">Percent</xsl:with-param>
					</xsl:call-template>
				</reportTableColumn>
			</reportTableRow>
		</xsl:for-each>		
	</xsl:template>


	<!-- Shareholders template [coshh]		q307 Cred	-->
	<xsl:template name="Shareholders">
		<xsl:param name="base"/>
		<shareholdersReportData>
			<tabularReportType>Shareholders</tabularReportType>
			<shareholdersTable>
				<xsl:for-each select ="$base/shareholder">				
					<xsl:call-template name="ShareholdersTable"/>
				</xsl:for-each>
			</shareholdersTable>
		</shareholdersReportData>
	</xsl:template>
	
	<xsl:template name="ShareholdersTable">
		<reportTableRow>
			<reportTableDescription>
				<xsl:value-of select="id/name"/>
			</reportTableDescription>
			<reportTableColumn>
				<type>String</type>
				<subtype>shareType</subtype>
				<rawString>
					<xsl:value-of select="shareType/@code"/>
				</rawString>
			</reportTableColumn>
			<reportTableColumn>
				<xsl:call-template name="rawData">
					<xsl:with-param name="dataNodeSet" select="percentDirectlyHeld"/>
					<xsl:with-param name="formatType">Percent</xsl:with-param>
				</xsl:call-template>
			</reportTableColumn>
			<reportTableColumn>
				<xsl:call-template name="rawData">
					<xsl:with-param name="dataNodeSet" select="percentIndirectlyHeld"/>
					<xsl:with-param name="formatType">Percent</xsl:with-param>
				</xsl:call-template>
			</reportTableColumn>
			</reportTableRow>	
	</xsl:template>


	<!-- Registration template [coreg]		q307 Cred	-->
	<xsl:template name="Registration">
		<xsl:param name="base"/>
		<registrationReportData>
			<tabularReportType>Registration</tabularReportType>
			<registrationTable>
				<xsl:for-each select ="$base/registration">
					<xsl:call-template name="RegistrationTable"/>
				</xsl:for-each>
			</registrationTable>
		</registrationReportData>	
	</xsl:template>

	<xsl:template name="RegistrationTable">
		<reportTableRow>
			<reportTableDescription>
				<xsl:value-of select="registrationId"/>
			</reportTableDescription>			
			<reportTableColumn>
				<type>String</type>
				<rawString>
					<xsl:value-of select="description"/>
				</rawString>
			</reportTableColumn>
			<reportTableColumn>
				<type>String</type>
				<rawString>
					<xsl:value-of select="eventDate"/>
				</rawString>
			</reportTableColumn>
		</reportTableRow>
	</xsl:template>

	</xsl:stylesheet>