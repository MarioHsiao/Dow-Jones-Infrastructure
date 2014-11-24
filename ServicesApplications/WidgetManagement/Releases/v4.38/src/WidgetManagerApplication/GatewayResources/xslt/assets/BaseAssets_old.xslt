<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:user="user" xmlns:fcp="urn:factiva:fcp:v2_0" xmlns:msxsl="urn:schemas-microsoft-com:xslt" extension-element-prefixes="msxsl user fcp xsl" xmlns="http://global.factiva.com/fvs/1.0">
  <xsl:import href="AssetUtility.xslt"/>
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" omit-xml-declaration="yes" />
  <msxsl:script implements-prefix="user" language="CSharp">
    <![CDATA[
			
		]]>
  </msxsl:script>
  <!--Entry/Main Template - used for branching			-->
  <xsl:template match="/">
    <GetAssetsResponse>
      <xsl:for-each select="/GetArchiveObjectResponse/ResultSet/Result/DistDoc">
        <!-- Overview and History -->
        <xsl:if test="./ArchiveDoc/overviewAndHistory">
          <assets xsi:type="OverviewAndHistoryAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>coovh</referenceType>
            <assetData xsi:type="OverviewAndHistoryAssetData">
              <xsl:for-each select="./ArchiveDoc/overviewAndHistory/*">
                <xsl:copy-of  select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- Long Business Description-->
        <xsl:if test="./ArchiveDoc/longBusinessDescription">
          <assets xsi:type="LongBusinessDescriptionAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>colbd</referenceType>
            <assetData xsi:type="LongBusinessDescriptionAssetData">
              <description>
                <xsl:for-each select="./ArchiveDoc/longBusinessDescription/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </description>
            </assetData>
          </assets>
        </xsl:if>

        
        <xsl:if test="./ArchiveDoc/businessSegmentInformation">
          <assets xsi:type="BusinessSegmentInformationAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cobsb</referenceType>
            <assetData xsi:type="BusinessSegmentInformationAssetData">
              <segmentInformation>
                <xsl:for-each select="./ArchiveDoc/businessSegmentInformation/@*">
                  <xsl:attribute name="{name()}">
                    <xsl:value-of select="."/>
                  </xsl:attribute>
                </xsl:for-each>
                <xsl:for-each select="./ArchiveDoc/businessSegmentInformation/*">
                  <xsl:copy-of select="." />
                </xsl:for-each>
              </segmentInformation>
            </assetData>
          </assets>
        </xsl:if>

        <xsl:if test="./ArchiveDoc/geographicSegmentInformation">
          <assets xsi:type="GeographicSegmentInformationAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cogsb</referenceType>
            <assetData xsi:type="GeographicSegmentInformationAssetData">
              <segmentInformation>
                <xsl:for-each select="./ArchiveDoc/geographicSegmentInformation/@*">
                  <xsl:attribute name="{name()}">
                    <xsl:value-of select="."/>
                  </xsl:attribute>
                </xsl:for-each>
                <xsl:for-each select="./ArchiveDoc/geographicSegmentInformation/*">
                  <xsl:copy-of select="." />
                </xsl:for-each>
              </segmentInformation>
            </assetData>
          </assets>
        </xsl:if>

        <!-- Executive Description-->
        <xsl:if test="./ArchiveDoc/executives">
          <assets xsi:type="ExecutivesAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>coexe</referenceType>
            <assetData xsi:type="ExecutivesAssetData">
              <xsl:for-each select="./ArchiveDoc/executives/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- Technical-->
        <xsl:if test="./ArchiveDoc/technical">
          <assets xsi:type="TechnicalAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cotec</referenceType>
            <assetData xsi:type="TechnicalAssetData">
              <technical>
                <xsl:for-each select="./ArchiveDoc/technical/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </technical>
            </assetData>
          </assets>
        </xsl:if>

        <!-- TradeNames-->
        <xsl:if test="./ArchiveDoc/tradeNames">
          <assets xsi:type="TradeNamesAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cotrn</referenceType>
            <assetData xsi:type="TradeNamesAssetData">
              <tradeNames>
                <xsl:for-each select="./ArchiveDoc/tradeNames/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </tradeNames>
            </assetData>
          </assets>
        </xsl:if>

        <!-- SWOTAnalysis-->
        <xsl:if test="./ArchiveDoc/swotAnalysis">
          <assets xsi:type="SWOTAnalysisAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>coswt</referenceType>
            <assetData xsi:type="SWOTAnalysisAssetData">
              <xsl:for-each select="./ArchiveDoc/swotAnalysis/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- KeyFinancials-->
        <xsl:if test="./ArchiveDoc/keyFinancials">
          <assets xsi:type="KeyFinancialsAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cokey</referenceType>
            <assetData xsi:type="KeyFinancialsAssetData">
              <xsl:for-each select="./ArchiveDoc/keyFinancials/*">
                <xsl:choose>
                  <!--Check If ChildNode exists-->
                  <xsl:when test="node()">
                    <xsl:copy>
                      <!--Copy Attributes-->
                      <xsl:for-each select ="@*">
                        <xsl:copy/>
                      </xsl:for-each>
                      <xsl:for-each  select="child::node()">
                        <!--Check if Node is not empty. Otherwise skip the node-->
                        <xsl:if test="node() or string(.)">
                          <xsl:copy-of select="."  />
                        </xsl:if>
                      </xsl:for-each>
                    </xsl:copy>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:copy-of select="."  />
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- ProductsAndServices-->
        <xsl:if test="./ArchiveDoc/productsAndServices">
          <assets xsi:type="ProductsAndServicesAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>copas</referenceType>
            <assetData xsi:type="ProductsAndServicesAssetData">
              <xsl:for-each select="./ArchiveDoc/productsAndServices/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- BusinessSegmentInformation-->
        <xsl:if test="./ArchiveDoc/businessSegmentInformation">
          <assets xsi:type="BusinessSegmentInformationAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cobsb</referenceType>
            <assetData xsi:type="BusinessSegmentInformationAssetData">
              <segmentInformation>
                <xsl:for-each select="./ArchiveDoc/businessSegmentInformation/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </segmentInformation>
            </assetData>
          </assets>
        </xsl:if>

        <!-- GeographicSegmentInformation-->
        <xsl:if test="./ArchiveDoc/geographicSegmentInformation">
          <assets xsi:type="GeographicSegmentInformationAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cogsb</referenceType>
            <assetData xsi:type="GeographicSegmentInformationAssetData">
              <segmentInformation>
                <xsl:for-each select="./ArchiveDoc/geographicSegmentInformation/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </segmentInformation>
            </assetData>
          </assets>
        </xsl:if>

        <!-- KeyCompetitors-->
        <xsl:if test="./ArchiveDoc/keyCompetitors">
          <assets xsi:type="KeyCompetitorsAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>
              <xsl:choose>
                <xsl:when test="./MetadataPT/PubData/SrcCode[@fid='sc' and @value='RRSCH']">cokcr</xsl:when>
                <xsl:otherwise>cokcs</xsl:otherwise>
              </xsl:choose>
            </referenceType>
            <assetData xsi:type="KeyCompetitorsAssetData">
              <xsl:for-each select="./ArchiveDoc/keyCompetitors/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>


        <!-- Obsolete : CorporateEvents-->
        <!--<xsl:if test="./ArchiveDoc/corporateEvents">
			  <assets xsi:type="CorporateEventsAsset">
				  <metaData>
					  <xsl:apply-templates select="./MetadataPT" mode="base"/>
				  </metaData>
				  <referenceType>coevt</referenceType>
				  <assetData xsi:type="CorporateEventsAssetData">
					  <event>
						  <xsl:for-each select="./ArchiveDoc/corporateEvents/*">
							  <xsl:copy-of select="."  />
						  </xsl:for-each>
					  </event>
				  </assetData>
			  </assets>
		  </xsl:if>-->

        <!-- SubsidiariesAffiliates Description-->
        <xsl:if test="./ArchiveDoc/subsidiariesAffiliates">
          <assets xsi:type="SubsidiariesAffiliatesAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cosub</referenceType>
            <assetData xsi:type="SubsidiariesAffiliatesAssetData">
              <subsidiariesAffiliates>
                <xsl:for-each select="./ArchiveDoc/subsidiariesAffiliates/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </subsidiariesAffiliates>
            </assetData>
          </assets>
        </xsl:if>

        <!-- IndexAgggregatesAsset -->
        <xsl:if test="./ArchiveDoc/indexAggregates">
          <assets xsi:type="IndexAgggregatesAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>in500</referenceType>
            <assetData xsi:type="AggregatesAssetData">
              <xsl:for-each select="./ArchiveDoc/indexAggregates/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>


        <!-- IndustryAggregatesAsset -->
        <xsl:if test="./ArchiveDoc/industryAggregates">
          <assets xsi:type="IndustryAggregatesAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>inrat</referenceType>
            <assetData xsi:type="AggregatesAssetData">
              <xsl:for-each select="./ArchiveDoc/industryAggregates/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>


        <!-- IndustrySectorAggregates -->
        <xsl:if test="./ArchiveDoc/industrySectorAggregates">
          <assets xsi:type="IndustrySectorAggregatesAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>insrt</referenceType>
            <assetData xsi:type="AggregatesAssetData">
              <xsl:for-each select="./ArchiveDoc/industrySectorAggregates/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- Biography -->
        <xsl:if test="./ArchiveDoc/biography">
          <assets xsi:type="BiographyAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>exbio</referenceType>
            <assetData xsi:type="BiographyAssetData">
              <biography>
                <xsl:for-each select="./ArchiveDoc/biography/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </biography>
            </assetData>
          </assets>
        </xsl:if>

        <!-- CompanyInformation -->
        <xsl:if test="./ArchiveDoc/companyInformation">
          <assets xsi:type="CompanyInformationAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cogen</referenceType>
            <assetData xsi:type="CompanyInformationAssetData">
              <companyInformation>
                <xsl:for-each select="./ArchiveDoc/companyInformation/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </companyInformation>
            </assetData>
          </assets>
        </xsl:if>

        <!-- Registrations -->
        <xsl:if test="./ArchiveDoc/registrations">
          <assets xsi:type="RegistrationsAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>coreg</referenceType>
            <assetData xsi:type="RegistrationsAssetData">
              <xsl:for-each select="./ArchiveDoc/registrations/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- Shareholders -->
        <xsl:if test="./ArchiveDoc/shareholders">
          <assets xsi:type="ShareholdersAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>coreg</referenceType>
            <assetData xsi:type="ShareholdersAssetData">
              <xsl:for-each select="./ArchiveDoc/shareholders/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- ImportExports -->
        <xsl:if test="./ArchiveDoc/importExports">
          <assets xsi:type="ImportExportsAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>coimp</referenceType>
            <assetData xsi:type="ImportExportsAssetData">
              <xsl:for-each select="./ArchiveDoc/importExports/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- keyFigures -->
        <xsl:if test="./ArchiveDoc/keyFigures">
          <assets xsi:type="KeyFiguresAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cokfg</referenceType>
            <assetData xsi:type="KeyFiguresAssetData">
              <keyFigures>
                <xsl:for-each select="./ArchiveDoc/keyFigures/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </keyFigures>
            </assetData>
          </assets>
        </xsl:if>

        <!-- CompanyStatement -->
        <xsl:if test="./ArchiveDoc/companyStatement">
          <assets xsi:type="CompanyStatementAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>costa</referenceType>
            <assetData xsi:type="CompanyStatementAssetData">
              <companyStatement>
                <xsl:for-each select="./ArchiveDoc/companyStatement/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </companyStatement>
            </assetData>
          </assets>
        </xsl:if>

        <!-- CustomerInformation -->
        <xsl:if test="./ArchiveDoc/customerInformation">
          <assets xsi:type="CustomerInformationAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>cocus</referenceType>
            <assetData xsi:type="CustomerInformationAssetData">
              <customerInformation>
                <xsl:for-each select="./ArchiveDoc/customerInformation/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </customerInformation>
            </assetData>
          </assets>
        </xsl:if>

        <!-- IncomeStatement -->
        <xsl:if test="./ArchiveDoc/incomeStatement">
          <assets xsi:type="IncomeStatementAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>
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
            </referenceType>
            <assetData xsi:type="IncomeStatementAssetData">
              <incomeStatement>
				<xsl:copy-of select="./ArchiveDoc/incomeStatement/@*"/>
                <xsl:for-each select="./ArchiveDoc/incomeStatement/*">
					<xsl:choose>
						<xsl:when test="local-name()='fiscalPeriod'">
							<xsl:element name="fiscalPeriod">
								<xsl:copy-of select="./@*"/>
								<xsl:for-each select="./*">
									<xsl:choose>
										<xsl:when test="local-name()='items'">
											<xsl:element name="items">
												<xsl:choose>
													<xsl:when test="./@xsi:type">
														<xsl:copy-of select="./@xsi:type"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:attribute name="xsi:type">MultexIncomeStatementItems</xsl:attribute>
													</xsl:otherwise>
												</xsl:choose>
												<xsl:for-each select="./*">
													<xsl:copy-of select="."  />
												</xsl:for-each>
											</xsl:element>
										</xsl:when>
										<xsl:otherwise>
											<xsl:copy-of select="."  />
										</xsl:otherwise>
									</xsl:choose>
								</xsl:for-each>
							</xsl:element>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="."  />
						</xsl:otherwise>
					</xsl:choose>
                </xsl:for-each>
              </incomeStatement>
            </assetData>
          </assets>
        </xsl:if>

        <!-- CashFlow -->
        <xsl:if test="./ArchiveDoc/cashFlow">
          <assets xsi:type="CashFlowAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>
              <xsl:choose>
                <xsl:when test="./ArchiveDoc/cashFlow/fiscalPeriod/@type != 'Interim'">coacf</xsl:when>
                <xsl:otherwise>coicf</xsl:otherwise>
              </xsl:choose>
            </referenceType>
            <assetData xsi:type="CashFlowAssetData">
              <cashFlow>
                <xsl:for-each select="./ArchiveDoc/cashFlow/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </cashFlow>
            </assetData>
          </assets>
        </xsl:if>


        <!-- BalanceSheet -->
        <xsl:if test="./ArchiveDoc/balanceSheet">
          <assets xsi:type="BalanceSheetAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>
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
            </referenceType>
            <assetData xsi:type="BalanceSheetAssetData">
              <balanceSheet>
				<xsl:copy-of select="./ArchiveDoc/balanceSheet/@*"/>
				<xsl:for-each select="./ArchiveDoc/balanceSheet/*">
					<xsl:choose>
						<xsl:when test="local-name()='fiscalPeriod'">
							<xsl:element name="fiscalPeriod">
								<xsl:copy-of select="./@*"/>
								<xsl:for-each select="./*">
									<xsl:choose>
										<xsl:when test="local-name()='items'">
											<xsl:element name="items">
												<xsl:choose>
													<xsl:when test="./@xsi:type">
														<xsl:copy-of select="./@xsi:type"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:attribute name="xsi:type">MultexBalanceSheetItems</xsl:attribute>
													</xsl:otherwise>
												</xsl:choose>
												<xsl:for-each select="./*">
													<xsl:copy-of select="."  />
												</xsl:for-each>
												</xsl:element>
										</xsl:when>
										<xsl:otherwise>
											<xsl:copy-of select="."  />
										</xsl:otherwise>
									</xsl:choose>
								</xsl:for-each>
							</xsl:element>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="."  />
						</xsl:otherwise>
					</xsl:choose>
                </xsl:for-each>
              </balanceSheet>
            </assetData>
          </assets>
        </xsl:if>

        <!-- Ratios -->
        <xsl:if test="./ArchiveDoc/ratios">
          <assets xsi:type="RatiosAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>corat</referenceType>
            <assetData xsi:type="RatiosAssetData">
              <xsl:for-each select="./ArchiveDoc/ratios/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>

            </assetData>
          </assets>
        </xsl:if>

        <!-- HoldingsInformation -->
        <xsl:if test="./ArchiveDoc/holdingsInformation">
          <assets xsi:type="HoldingsInformationAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <assetData xsi:type="HoldingsInformationAssetData">
              <xsl:for-each select="./ArchiveDoc/holdingsInformation/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- AnalystAndEstimateInformation -->
        <xsl:if test="./ArchiveDoc/analystAndEstimateInformation">
          <assets xsi:type="AnalystAndEstimateInformationAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <assetData xsi:type="AnalystAndEstimateInformationAssetData">
              <xsl:for-each select="./ArchiveDoc/analystAndEstimateInformation/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <!-- InsiderTransactionInformation -->
        <xsl:if test="./ArchiveDoc/insiderTransactionInformation">
          <assets xsi:type="InsiderTransactionInformationAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <assetData xsi:type="InsiderTransactionInformationAssetData">
              <xsl:for-each select="./ArchiveDoc/insiderTransactionInformation/*">
                <xsl:copy-of select="."  />
              </xsl:for-each>
            </assetData>
          </assets>
        </xsl:if>

        <xsl:if test="./ArchiveDoc/ratiosA[@group='USData']">
          <assets xsi:type="USRatiosAAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>corta</referenceType>
            <assetData xsi:type="RatiosAAssetData">
              <ratiosA>
                <xsl:for-each select="./ArchiveDoc/ratiosA/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </ratiosA>
            </assetData>
          </assets>
        </xsl:if>

        <xsl:if test="./ArchiveDoc/ratiosA[@group='UKData']">
          <assets xsi:type="UKRatiosAAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>

            <referenceType>corta</referenceType>
            <ratiosAGroup>UKData</ratiosAGroup>

            <assetData xsi:type="RatiosAAssetData">
              <ratiosA>
                <xsl:for-each select="./ArchiveDoc/ratiosA/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </ratiosA>
            </assetData>
          </assets>
        </xsl:if>

        <xsl:if test="./ArchiveDoc/ratiosA[@group='DBEURData']">
          <assets xsi:type="UKRatiosAAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>

            <referenceType>corta</referenceType>
            <ratiosAGroup>DBEURData</ratiosAGroup>

            <assetData xsi:type="RatiosAAssetData">
              <ratiosA>
                <xsl:for-each select="./ArchiveDoc/ratiosA/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </ratiosA>
            </assetData>
          </assets>
        </xsl:if>

        <xsl:if test="./ArchiveDoc/ratiosA[@group='NordicData']">
          <assets xsi:type="UKRatiosAAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>

            <referenceType>corta</referenceType>
            <ratiosAGroup>NordicData</ratiosAGroup>

            <assetData xsi:type="RatiosAAssetData">
              <ratiosA>
                <xsl:for-each select="./ArchiveDoc/ratiosA/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </ratiosA>
            </assetData>
          </assets>
        </xsl:if>

        <!-- RatiosB -->
        <xsl:if test="./ArchiveDoc/ratiosB">
          <assets xsi:type="RatiosBAsset">
            <metaData>
              <xsl:apply-templates select="./MetadataPT" mode="base"/>
            </metaData>
            <referenceType>
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
            </referenceType>
            <assetData xsi:type="RatiosBAssetData">
              <ratiosB>
                <xsl:for-each select="./ArchiveDoc/ratiosB/*">
                  <xsl:copy-of select="."  />
                </xsl:for-each>
              </ratiosB>
            </assetData>
          </assets>
        </xsl:if>
      </xsl:for-each>
    </GetAssetsResponse>
  </xsl:template>


  <!--MetadataPT Processing template						-->
  <xsl:template name="metaData" match="MetadataPT" mode="base">
    <accessionNo fid="an" value="{./DocData/AccessionNo/@value}"/>
    <baseLanguage>
      <xsl:value-of select="./DocData/BaseLang/@value"/>
    </baseLanguage>
    <xsl:if test="string-length ( normalize-space( ./DocData/Date[@fid='pd']/@value ) ) > 0">
      <publicationDate>
        <xsl:value-of select="substring(./DocData/Date[@fid='pd']/@value,1,4)"/>-<xsl:value-of select="substring(./DocData/Date[@fid='pd']/@value,5,2)"/>-<xsl:value-of select="substring(./DocData/Date[@fid='pd']/@value,7)"/>
      </publicationDate>
    </xsl:if>
    <xsl:if test="string-length ( normalize-space( ./DocData/Date[@fid='md']/@value ) ) > 0">
      <modificationDate>
        <xsl:value-of select="substring(./DocData/Date[@fid='md']/@value,1,4)"/>-<xsl:value-of select="substring(./DocData/Date[@fid='md']/@value,5,2)"/>-<xsl:value-of select="substring(./DocData/Date[@fid='md']/@value,7)"/>
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

  <xsl:template name="lastAnnualFiscalPeriod">

  </xsl:template>

</xsl:stylesheet>
