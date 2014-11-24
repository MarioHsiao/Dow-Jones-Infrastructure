<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

	<msxsl:script language="JScript" implements-prefix="user">
		<![CDATA[
	function ChangeDateFormat(DateVal)
	{
			var RetVal = "";
			RetVal = DateVal.substr(0,4)+"-"+DateVal.substr(4,2)+"-"+DateVal.substr(6,2);
			return RetVal;
	}

	function ChangeTimeFormat(TimeVal)
	{
			var RetVal = new String("");
			RetVal = TimeVal.substr(0,2)+":"+TimeVal.substr(2,2)+":"+TimeVal.substr(4);
			return RetVal;
	}
			
	 function FormatNumberToPrecision(exp, value)
      {
        var ret_val = value;
        var isNegative = 0;
        var decPlace = -1;
        if (exp!="" && exp!="0"){
              if (value.indexOf("-")>-1) {
                              value = value.substr(1);                //Skip the 1st character "-"
                              isNegative = 1;
              }
              decPlace = value.indexOf(".");  //Look for presence of decimal point
              if (decPlace>-1) {
                              value = value.substr(0,decPlace) + value.substr(decPlace+1);    //Value is without decimal point.
                              decPlace = value.length + 1 - decPlace;  //Get the decimal point location 
              }
              //If exponent is negative add zeros on left side of value and move decimal point appropriately,
              if (exp.indexOf("-")>-1) {                                      
                              while (value.length-Math.abs(exp)-decPlace < 2){
                                              value = "0" + value;
                              }
                              ret_val = value.substr(0, value.length-Math.abs(exp)-decPlace-1) + "." + value.substr(value.length-Math.abs(exp)-decPlace-1, Math.abs(exp) + decPlace+1)
              }else{
                              //If exponent is positive add zeros on right side of value and move decimal point appropriately.
                              if (Math.abs(exp)-decPlace > 1) {
                                              for (var i=1; i<Math.abs(exp)-decPlace; i++){
                                                              value = value+"0";
                                              }
                                              ret_val = value;
                              } else{
                                              if (Math.abs(exp)-decPlace < 1) {
                                                              ret_val = value.substr(0,value.length+Math.abs(exp)-decPlace-1) + "." + value.substr(value.length+Math.abs(exp)-decPlace-1,decPlace-Math.abs(exp) +1);
                                              }else{
                                                              ret_val = value;
                                              }
                              }
              }
        }
        ret_val = Number(ret_val).toString();
        if (isNegative == 1) {
              ret_val = "-" + ret_val;
        }
        return ret_val;
      }
]]>
	</msxsl:script>
	<xsl:template match="/*">
		<GetQuoteResponse>
			<quoteResponse>
				<!--				<xsl:copy-of select="."/>-->
				<!--				<xsl:apply-templates select="/*/Control"/>-->
				<!--				<xsl:apply-templates select="/*/Status"/>-->
				<xsl:apply-templates select="/*/ResultSet"/>
			</quoteResponse>
		</GetQuoteResponse>
	</xsl:template>

	<!--	<xsl:template match="/*/Control">
		<xsl:copy-of select="."/>
	</xsl:template>-->

	<xsl:template match="/*/Status">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="/*/ResultSet">
		<quoteResultSet>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates select="Result"/>
			<!--<xsl:for-each select="Result">
				<xsl:copy-of select="."/>
			</xsl:for-each>-->
		</quoteResultSet>
	</xsl:template>

	<xsl:template match="Result">
		<xsl:choose>
			<xsl:when test="@status!='0'">
				<quote>
					<status>
						<xsl:attribute name="value">
							<xsl:value-of select="@status"/>
						</xsl:attribute>
						<type/>
						<message/>
					</status>
					<xsl:choose>
						<xsl:when test="string-length(normalize-space(@req_token)) &gt; 0">
							<requestedSymbol>
								<xsl:value-of select="normalize-space(string(@req_token))"/>
							</requestedSymbol>
						</xsl:when>
						<xsl:otherwise>
							<requestedSymbol/>
						</xsl:otherwise>
					</xsl:choose>
					<!--					<Status>
						<xsl:attribute name="value"><xsl:value-of select="@status"/></xsl:attribute>
					</Status>-->
				</quote>
			</xsl:when>
			<xsl:otherwise>
				<quote>
					<xsl:attribute name="timeliness">
						<xsl:value-of select="@data_quality"/>
					</xsl:attribute>
					<xsl:apply-templates select="Sample/InstrumentName"/>
					<instrumentName/>
					<!-- Blank instrumentName, will be filled in C# code -->
					<xsl:apply-templates select="@code"/>
					<xsl:call-template name="itype"/>
					<xsl:call-template name="req_token"/>
          <xsl:apply-templates select="@provider" />
          <xsl:apply-templates select="@source" />
					<xsl:apply-templates select="Sample/Exchange"/>
					<xsl:apply-templates select="Sample/Currency"/>
					<!--<sample>-->
					<!--<xsl:attribute name="date"><xsl:value-of select="user:ChangeDateFormat(string(Sample/@date))"/></xsl:attribute>-->
					<xsl:apply-templates select="Sample/Num[@fid='AskPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='BidPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='PreviousAskPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='PreviousBidPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='ChangeFromHistoric']"/>
					<xsl:apply-templates select="Sample/Date[@fid='CloseDate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='ClosePrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='TodayHighBidPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='TodayLowBidPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='TodayHighPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='TodayLowPrice']"/>
					<xsl:apply-templates select="Sample/InstrumentType"/>
					<xsl:apply-templates select="Sample/Num[@fid='DiffInAssetValue']"/>
					<xsl:apply-templates select="Sample/Num[@fid='Dividend']"/>
					<xsl:apply-templates select="Sample/Date[@fid='DividendDate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='Earnings']"/>
					<xsl:apply-templates select="Sample/Date[@fid='exDividendDate']"/>
					<xsl:apply-templates select="Sample/FreqInterestPayment"/>
					<xsl:apply-templates select="Sample/Date[@fid='IssueDate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='LastTradePrice']"/>
					<xsl:apply-templates select="Sample/DateTime[@fid='LastTradeTime']"/>
					<xsl:apply-templates select="Sample/Date[@fid='LifeHighDate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='LifeHighPrice']"/>
					<xsl:apply-templates select="Sample/Date[@fid='LifeLowDate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='LifeLowPrice']"/>
					<xsl:apply-templates select="Sample/Date[@fid='BondMaturesDate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='MidPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='NetAssetValue']"/>
					<xsl:apply-templates select="Sample/Date[@fid='NextInterestDate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='NextInterestRate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='OfferPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='OpenPrice']"/>
					<xsl:apply-templates select="Sample/Date[@fid='YearHighDate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='YearHighPrice']"/>
					<xsl:apply-templates select="Sample/Date[@fid='YearLowDate']"/>
					<xsl:apply-templates select="Sample/Num[@fid='YearLowPrice']"/>
					<xsl:apply-templates select="Sample/Num[@fid='PERatio']"/>
					<xsl:apply-templates select="Sample/Num[@fid='PctChangeFromHistory']"/>
					<xsl:apply-templates select="Sample/Num[@fid='PreviousNetAssetValue']"/>
					<xsl:apply-templates select="Sample/Date[@fid='PreviousNetAssetValueDate']"/>
					<xsl:apply-templates select="Sample/Rating"/>
					<xsl:apply-templates select="Sample/RatingID"/>
					<xsl:apply-templates select="Sample/SpotRateInverted"/>
					<xsl:apply-templates select="Sample/DateTime[@fid='UpdateTime']"/>
					<xsl:apply-templates select="Sample/Num[@fid='Volume']"/>
					<xsl:apply-templates select="Sample/Num[@fid='Yield']"/>
          <xsl:apply-templates select="DataProvider"></xsl:apply-templates>
          <!--</sample>-->
				</quote>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
  <xsl:template match="@source">
    <source>
      <xsl:value-of select="."/>
    </source>
  </xsl:template>
  <xsl:template match="@provider">
    <provider>
      <xsl:value-of select="."/>
    </provider>
  </xsl:template>
  <xsl:template match="DataProvider">
    <dataProvider>
      <name>
        <xsl:value-of select="@name"/>
      </name>
      <ref>
        <xsl:value-of select="."/>
      </ref>
    </dataProvider>
  </xsl:template>
	<xsl:template match="Sample/InstrumentName">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<companyName>
				<xsl:value-of select="normalize-space(.)"/>
			</companyName>
		</xsl:if>
	</xsl:template>
	<xsl:template match="@code">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<ric>
				<xsl:value-of select="normalize-space(.)"/>
			</ric>
		</xsl:if>
	</xsl:template>
	<xsl:template name="itype">
		<instrumentType></instrumentType>
	</xsl:template>
	<xsl:template name="req_token">
		<xsl:if test="string-length(normalize-space(@req_token)) &gt; 0">
			<requestedSymbol>
				<xsl:value-of select="normalize-space(@req_token)"/>
			</requestedSymbol>
			<djTicker>
				<xsl:value-of select="normalize-space(@dj_code)"/>
			</djTicker>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Sample/Exchange">
		<xsl:if test="@value!='-'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<exchange>
					<xsl:value-of select="normalize-space(@value)"/>
				</exchange>
			</xsl:if>
		</xsl:if>
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <exchangeName>
        <xsl:value-of select="normalize-space(.)"/>
      </exchangeName>
    </xsl:if>
	</xsl:template>
	<xsl:template match="Sample/Currency">
		<xsl:if test="@value!='-'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<currency>
					<xsl:value-of select="normalize-space(@value)"/>
				</currency>
			</xsl:if>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Sample/SpotRateInverted">
		<xsl:if test="@value!='-'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<spotRateInverted>
					<xsl:value-of select="normalize-space(@value)"/>
				</spotRateInverted>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='AskPrice']">

		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<askPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>	
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</askPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='BidPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<bidPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</bidPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='PreviousAskPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<previousAskPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</previousAskPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='PreviousBidPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<previousBidPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</previousBidPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='ChangeFromHistoric']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<change>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</change>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='CloseDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<closeDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</closeDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='ClosePrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<closePrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</closePrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='TodayHighBidPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<dayHighBidPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</dayHighBidPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='TodayLowBidPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<dayLowBidPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</dayLowBidPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='TodayHighPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<dayHighPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</dayHighPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='TodayLowPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<dayLowPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</dayLowPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/InstrumentType">
		<xsl:if test="@value!='-'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<debtType>
					<xsl:value-of select="normalize-space(@value)"/>
				</debtType>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='DiffInAssetValue']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<changeInNetAssetValue>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</changeInNetAssetValue>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='Dividend']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<dividend>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</dividend>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='DividendDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<dividendDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</dividendDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='Earnings']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<earningsPerShare>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</earningsPerShare>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='exDividendDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<exDividendDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</exDividendDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/FreqInterestPayment">
		<xsl:if test="@value!='-'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<interestPaymentFrequency>
					<xsl:value-of select="normalize-space(@value)"/>
				</interestPaymentFrequency>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='IssueDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<issueDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</issueDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='LastTradePrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<lastTradePrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</lastTradePrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/DateTime[@fid='LastTradeTime']">
		<xsl:if test="string-length(normalize-space(@date))+string-length(normalize-space(@time))  &gt; 0">
			<xsl:if test="string(number(@date))!='NaN' and  string(number(@time))!='NaN'">
				<lastTradeDateTime>
					<xsl:value-of select="(user:ChangeDateFormat(string(@date)))"/>T<xsl:value-of select="(user:ChangeTimeFormat(string(@time)))"/>
				</lastTradeDateTime>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='LifeHighDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<lifetimeHighDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</lifetimeHighDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='LifeHighPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<lifetimeHighPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</lifetimeHighPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='LifeLowDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<lifetimeLowDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</lifetimeLowDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='LifeLowPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<lifetimeLowPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</lifetimeLowPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='BondMaturesDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<maturityDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</maturityDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='MidPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<midDayPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</midDayPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='NetAssetValue']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<netAssetValue>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</netAssetValue>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='NextInterestDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<nextInterestDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</nextInterestDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='NextInterestRate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<nextInterestRate>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</nextInterestRate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='OfferPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<offerPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</offerPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='OpenPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<openPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</openPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='YearHighDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<last52WeekHighDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</last52WeekHighDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='YearHighPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<last52WeekHighPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</last52WeekHighPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='YearLowDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<last52WeekLowDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</last52WeekLowDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='YearLowPrice']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<last52WeekLowPrice>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</last52WeekLowPrice>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='PERatio']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<pERatio>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</pERatio>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='PctChangeFromHistory']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<percentageChange>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</percentageChange>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='PreviousNetAssetValue']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<previousNetAssetValue>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</previousNetAssetValue>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Date[@fid='PreviousNetAssetValueDate']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<previousNetAssetValueDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</previousNetAssetValueDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Rating">
		<xsl:if test="@value!='-'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<rating>
					<xsl:value-of select="normalize-space(@value)"/>
				</rating>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/RatingID">
		<xsl:if test="@value!='-'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<ratingID>
					<xsl:value-of select="normalize-space(@value)"/>
				</ratingID>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/DateTime[@fid='UpdateTime']">
		<xsl:if test="string-length(normalize-space(@date))+string-length(normalize-space(@time))  &gt; 0">
			<updateTime>
				<xsl:value-of select="(user:ChangeDateFormat(string(@date)))"/>T<xsl:value-of select="(user:ChangeTimeFormat(string(@time)))"/>
			</updateTime>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='Volume']">
		<xsl:if test="string(number(@value))!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<volume>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</volume>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="Sample/Num[@fid='Yield']">
		<!--<xsl:if test="@value!='-'">-->
		<xsl:if test="number('-')!='NaN'">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<yield>
					<xsl:value-of select="user:FormatNumberToPrecision(string(@exp),normalize-space(@value))"/>
					<!--<xsl:if test="@exp!=''">
						<xsl:attribute name="exp"><xsl:value-of select="@exp"/></xsl:attribute>
					</xsl:if>
					<xsl:value-of select="normalize-space(@value)"/>-->
				</yield>
			</xsl:if>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>