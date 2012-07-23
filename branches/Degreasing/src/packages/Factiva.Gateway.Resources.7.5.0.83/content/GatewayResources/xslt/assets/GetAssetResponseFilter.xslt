<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template  match="/">
    <xsl:apply-templates select="@* | node()"/>
  </xsl:template>

  <!--elements-->
  <xsl:template match="node()">
    <xsl:choose>
      <!--AdvancesRecieved<<CustomerAdvances-->
      <xsl:when test="local-name()='customerAdvances' and local-name(parent::*)='items' and (../@*) = 'DNBBalanceSheetItems'">
        <xsl:element name="advancesReceived">
          <xsl:apply-templates select="@* | node()" />
        </xsl:element>
      </xsl:when>
      <!--amortisationCosts<<amortizationCosts-->
      <xsl:when test="local-name()='amortizationCosts' and local-name(parent::*)='items' and (../@*) = 'DNBBalanceSheetItems'">
        <xsl:element name="amortisationCosts">
          <xsl:apply-templates select="@* | node()" />
        </xsl:element>
      </xsl:when>
      <!--inventoriesFinishedGoods<<finishedGoods-->
      <xsl:when test="local-name()='finishedGoods' and local-name(parent::*)='items' and (../@*) = 'DNBBalanceSheetItems'">
        <xsl:element name="inventoriesFinishedGoods">
          <xsl:apply-templates select="@* | node()" />
        </xsl:element>
      </xsl:when>
      <!--moneyDepositAtBank<<cashAtBank-->
      <xsl:when test="local-name()='cashAtBank' and local-name(parent::*)='items' and (../@*) = 'DNBBalanceSheetItems'">
        <xsl:element name="moneyOnDepositAtBank">
          <xsl:apply-templates select="@* | node()" />
        </xsl:element>
      </xsl:when>
      <!--taxes<<taxLiabilities-->
      <xsl:when test="local-name()='taxLiabilities' and local-name(parent::*)='items' and (../@*) = 'DNBBalanceSheetItems'">
        <xsl:element name="taxes">
          <xsl:apply-templates select="@* | node()" />
        </xsl:element>
      </xsl:when>
      <!--receivablesDueAfter1Year<<receivablesOver1Year-->
      <xsl:when test="local-name()='receivablesOver1Year' and local-name(parent::*)='items' and (../@*) = 'DNBBalanceSheetItems'">
        <xsl:element name="receivablesDueAfter1Year">
          <xsl:apply-templates select="@* | node()" />
        </xsl:element>
      </xsl:when>
      <!--Remove workInProgress from DNBBalanceSheetItems-->
      <xsl:when test="local-name(parent::*)='items' and local-name() = 'workInProgress' and (../@*) = 'DNBBalanceSheetItems'">
      </xsl:when>
      <!--Remove items from DNBIncomeStatementItems-->
      <xsl:when test="(../@*) = 'DNBIncomeStatementItems' 
                  and local-name(parent::*)='items' and 
                (local-name()='incomeBeforeDepreciation'   or 
                local-name()='profitBeforeFinancialItems' or 
                local-name()='badDebtsDepreciated' or 
                local-name()='otherFinancialExpenses' or 
                local-name()='extraordinaryItems' or 
                local-name()='incomeBeforeAllocations'  or 
                local-name()='groupContribution' or 
                local-name()='dividend' or 
                local-name()='depreciation' or 
                local-name()='netIncome' or 
                local-name()='incomeProfitAfterFinancialItems')">
      </xsl:when>
      <!--Remove employeesGrowth3year from KeyFinancialsItems-->
      <xsl:when test="local-name(parent::*)='items' and (ancestor::node())[2]/child::node()[2]/@* = 'KeyFinancialsAsset' and local-name() = 'employeesGrowth3year'">
      </xsl:when>
      <!--Remove auditorsRemunerationNonAuditFeesUSDollars from KeyFinancialsItems-->
      <xsl:when test="local-name(parent::*)='items' and (ancestor::node())[2]/child::node()[2]/@* = 'KeyFinancialsAsset' and local-name() = 'auditorsRemunerationNonAuditFeesUSDollars'">
      </xsl:when>
      <!--Remove auditorsRemunerationNonAuditFeesUSDollars from KeyFinancialsItems-->
      <xsl:when test="local-name(parent::*)='items' and (ancestor::node())[2]/child::node()[2]/@* = 'KeyFinancialsAsset' and local-name() = 'auditorsRemunerationUSDollars'">
      </xsl:when>
      <!--Remove salaries, pledgedAssets, contingentLiabilities from FinancialStrengthRatiosType-->
      <xsl:when test="local-name(parent::*)='financialStrengthRatios' and local-name() = 'salaries'">
      </xsl:when>
      <xsl:when test="local-name(parent::*)='financialStrengthRatios' and local-name() = 'pledgedAssets'">
      </xsl:when>
      <xsl:when test="local-name(parent::*)='financialStrengthRatios' and local-name() = 'contingentLiabilities'">
      </xsl:when>
      <!--Remove location from screeningcompanyData-->
      <xsl:when test="local-name(parent::*)='screeningcompanyData' and local-name() = 'location'">
      </xsl:when>
      <!--Remove companySource from screeningcompanyData-->
      <xsl:when test="local-name(parent::*)='screeningcompanyData' and local-name() = 'companySource'">
      </xsl:when>
      <!--Remove ticker from screeningcompanyData-->
      <xsl:when test="local-name(parent::*)='screeningcompanyData' and local-name() = 'ticker'">
      </xsl:when>
      <!--Remove priceCurrency from screeningcompanyData-->
      <xsl:when test="local-name(parent::*)='screeningcompanyData' and local-name() = 'priceCurrency'">
      </xsl:when>
      <!--Remove priceToReportingExchangeRate from screeningcompanyData-->
      <xsl:when test="local-name(parent::*)='screeningcompanyData' and local-name() = 'priceToReportingExchangeRate'">
      </xsl:when>
      <!--Remove priceCurrentDate from screeningcompanyData-->
      <xsl:when test="local-name(parent::*)='screeningcompanyData' and local-name() = 'priceCurrentDate'">
      </xsl:when>
      <!--auditorsRemuneration to long-->
      <xsl:when test="local-name(parent::*)='items' and (ancestor::node())[2]/child::node()[2]/@* = 'KeyFinancialsAsset' and local-name()='auditorsRemuneration'">
        <xsl:element name="auditorsRemuneration">
          <xsl:value-of select="format-number((.),'#')"/>
        </xsl:element>
      </xsl:when>
      <!--auditorsRemunerationNonAuditFees to long-->
      <xsl:when test="local-name(parent::*)='items' and (ancestor::node())[2]/child::node()[2]/@* = 'KeyFinancialsAsset' and local-name()='auditorsRemunerationNonAuditFees'">
        <xsl:element name="auditorsRemunerationNonAuditFees">
          <xsl:value-of select="format-number((.),'#')"/>
        </xsl:element>
      </xsl:when>
      <!--Remove sales type-->
      <!--<xsl:when test="local-name()='sales'">
      </xsl:when>-->
      <xsl:when test="self::*">
        <xsl:element name="{local-name()}" namespace="">
          <xsl:apply-templates select="@* | node()"/>
        </xsl:element>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="@*">
    <xsl:choose>
      <!--group attribute-->
      <xsl:when test="local-name()='group'">
        <xsl:attribute name="group">
          <xsl:choose>
            <xsl:when test="(.)='NordicData' or (.)='DBEURData'">
              <xsl:value-of select="'OtherFinancial'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="(.)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </xsl:when>
      <!--replace ipcinsecondarysic and ipcinprimarynaics enum values with ipic-->
      <xsl:when test="local-name()='fid' and local-name(parent::*) = 'DescField'">
        <xsl:attribute name="fid">
          <xsl:choose>
            <xsl:when test="(.)='ipcinsecondarysic' or (.)='ipcinprimarynaics'">
              <xsl:value-of select="'ipc'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="(.)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </xsl:when>
      <!--Remove accuracy attribute from Location-->
      <xsl:when test="local-name(parent::*)='location' and local-name()='accuracy'">
      </xsl:when>
      <!--Remove industry group from Financial statement and (local-name(parent::*)='balanceSheet' or local-name(parent::*)='cashFlow' or local-name(parent::*)='customerInformation' or local-name(parent::*)='incomeStatement' or local-name(parent::*)='ratiosA' or local-name(parent::*)='ratiosB' or local-name(parent::*)='segmentInformation'-->
      <xsl:when test="local-name()='industryGroup' ">
      </xsl:when>
      <!-- Remove consolidated from Sales-->
      <!--<xsl:when test="local-name()='consolidated' and local-name(parent::*)='sales'">
      </xsl:when>-->
      <xsl:otherwise>
        <xsl:copy/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
