<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:user="user" extension-element-prefixes="msxsl">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

  <msxsl:script language="JScript" implements-prefix="user">
    <![CDATA[
	function ChangeDateFormat(DateVal)
	{
  if(DateVal.indexOf('/') >= 0) return DateVal
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

  <xsl:template match="/*/Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="/*/ResultSet">
    <historicalDataResultSet>
      <xsl:copy-of select="@*"/>
      <xsl:apply-templates select="Result"/>
    </historicalDataResultSet>
  </xsl:template>

  <xsl:template match="Result">
    <xsl:choose>
      <xsl:when test="@status!='0' and string(number(@status))!='NaN'">
        <historicalDataResult>
          <status>
            <xsl:attribute name="value">
              <xsl:value-of select="@status"/>
            </xsl:attribute>
            <type/>
            <message/>
          </status>
        </historicalDataResult>
      </xsl:when>
      <xsl:otherwise>
        <historicalDataResult>
          <xsl:call-template name="req_token"/>
          <xsl:apply-templates select="@code"/>
          <xsl:apply-templates select="@provider"></xsl:apply-templates>
          <source>
            <xsl:value-of select="normalize-space(string(@source))"/>
          </source>
          <xsl:apply-templates select="Series"></xsl:apply-templates>
          <xsl:apply-templates select="CSV"></xsl:apply-templates>
          <!--Incase of CSV security does nto have Series wrapper node-->
          <xsl:apply-templates select="Security"></xsl:apply-templates>
          <xsl:if test="string-length(normalize-space(string(Name))) &gt; 0">
            <symbName>
              <xsl:value-of select="Name"/>
            </symbName>
          </xsl:if>
          <xsl:apply-templates select="DataProvider"></xsl:apply-templates>
        </historicalDataResult>
      </xsl:otherwise>
    </xsl:choose>
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
  <xsl:template match="CSV">
    <xsl:if test="string-length(normalize-space(string(.))) &gt; 0">
      <csv>
        <xsl:value-of select="."/>
      </csv>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Series">
    <xsl:apply-templates select="Security"></xsl:apply-templates>
    <xsl:if test="string-length(normalize-space(string(@periodicity))) &gt; 0">
      <frequency>
        <xsl:variable name="periodicity" select="normalize-space(string(@periodicity))"/>
        <xsl:choose>
          <xsl:when test="$periodicity = 'Daily'">D</xsl:when>
          <xsl:when test="$periodicity = 'Weekly'">W</xsl:when>
          <xsl:when test="$periodicity = 'Monthly'">M</xsl:when>
          <xsl:when test="$periodicity = 'Quarterly'">Q</xsl:when>
          <xsl:when test="$periodicity = 'Yearly'">Y</xsl:when>
        </xsl:choose>
      </frequency>
    </xsl:if>
    <xsl:apply-templates select="Date[@fid='from']"/>
    <xsl:apply-templates select="Date[@fid='to']"/>
    <xsl:apply-templates select="@from"/>
    <xsl:apply-templates select="@to"/>
    <xsl:if test="normalize-space(string(@currency)) != ''">
      <currencyCode>
        <xsl:value-of select="normalize-space(string(@currency))"/>
      </currencyCode>
    </xsl:if>
    <xsl:apply-templates select="Samples"></xsl:apply-templates>
    <!--In some case date are not enclosed in Samples node-->
    <xsl:apply-templates select="Sample"/>
  </xsl:template>

  <xsl:template match="Security">
    <xsl:apply-templates select="Exchange"></xsl:apply-templates>
    <xsl:apply-templates select="InstrumentName"></xsl:apply-templates>
    <xsl:apply-templates select="Name"></xsl:apply-templates>
    <xsl:apply-templates select="Description"></xsl:apply-templates>
    <xsl:apply-templates select="Code[@fid='TLID']"></xsl:apply-templates>
    <xsl:apply-templates select="Code[@fid='Cusip']"></xsl:apply-templates>
    <xsl:apply-templates select="Code[@fid='Sedol']"></xsl:apply-templates>
    <xsl:apply-templates select="Code[@fid='Stad']"></xsl:apply-templates>
    <xsl:apply-templates select="Code[@fid='Isin']"></xsl:apply-templates>
    <xsl:apply-templates select="SecurityType"></xsl:apply-templates>
    <xsl:apply-templates select="Date[@fid='LastPointDate']"></xsl:apply-templates>
    <xsl:apply-templates select="Date[@fid='BeginPointDate']"></xsl:apply-templates>
  </xsl:template>

  <xsl:template match="SecurityType">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
    <securityType>
      <xsl:value-of select="@value"/>
    </securityType>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Date[@fid='LastPointDate']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <lastPointDate>
          <xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
        </lastPointDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Date[@fid='BeginPointDate']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <beginPointDate>
          <xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
        </beginPointDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Code[@fid='Stad']">
    <stad>
      <xsl:value-of select="@value"/>
    </stad>
  </xsl:template>
  <xsl:template match="Code[@fid='Isin']">
    <isin>
      <xsl:value-of select="@value"/>
    </isin>
  </xsl:template>
  <xsl:template match="Code[@fid='Sedol']">
    <sedol>
      <xsl:value-of select="@value"/>
    </sedol>
  </xsl:template>
  <xsl:template match="Code[@fid='TLID']">
    <tlid>
      <xsl:value-of select="@value"/>
    </tlid>
  </xsl:template>
  <xsl:template match="Code[@fid='Cusip']">
    <cusip>
      <xsl:value-of select="@value"/>
    </cusip>
  </xsl:template>
  <xsl:template match="Name">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
    <name>
      <xsl:value-of select="@value"/>
    </name>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Description">
    <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
      <description>
        <xsl:value-of select="@value"/>
      </description>
    </xsl:if>
  </xsl:template>
  <xsl:template match="InstrumentName">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <instrumentName>
        <xsl:value-of select="."/>
      </instrumentName>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Exchange">
    <exchange>
      <code>
        <xsl:value-of select="@value"/>
      </code>
      <value>
        <xsl:value-of select="."/>
      </value>
    </exchange>
  </xsl:template>
  <xsl:template name="req_token">
    <requestedSymbol>
      <xsl:value-of select="@req_token"/>
    </requestedSymbol>
    <djTicker>
      <xsl:value-of select="@dj_code"/>
    </djTicker>
  </xsl:template>
  <xsl:template match="@code">
    <xsl:if test="string-length(normalize-space(.)) &gt; 0">
      <ric>
        <xsl:value-of select="normalize-space(.)"/>
      </ric>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Date[@fid='from']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <fromDate>
          <xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
        </fromDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="@from">
    <xsl:if test="not((.)='-' or (.)='N/A')">
      <xsl:if test="string-length(normalize-space((.))) &gt; 0">
        <fromDate>
          <xsl:value-of select="(user:ChangeDateFormat(string((.))))"/>
        </fromDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Date[@fid='to']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <toDate>
          <xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
        </toDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="@to">
    <xsl:if test="not((.)='-' or (.)='N/A')">
      <xsl:if test="string-length(normalize-space((.))) &gt; 0">
        <toDate>
          <xsl:value-of select="(user:ChangeDateFormat(string((.))))"/>
        </toDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Samples">
    <xsl:apply-templates select="Sample"/>
  </xsl:template>
  <xsl:template match="Sample">
    <dataPoints>
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@Date)) &gt; 0">
          <xsl:if test="not(@Date='-' or @Date='N/A')">
            <xsl:if test="string-length(normalize-space(@Date)) &gt; 0">
              <date>
                <xsl:value-of select="(user:ChangeDateFormat(string(@Date)))"/>
              </date>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Date[@fid='SampleDate']"/>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@High)) &gt; 0">
          <xsl:if test="not(@High='-' or @High='N/A')">
            <xsl:if test="string-length(normalize-space(@High)) &gt; 0">
              <dayHighPrice>
                <xsl:value-of select="@High"/>
              </dayHighPrice>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Num[@fid='High']"/>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@Low)) &gt; 0">
          <xsl:if test="not(@Low='-' or @Low='N/A')">
            <xsl:if test="string-length(normalize-space(@Low)) &gt; 0">
              <dayLowPrice>
                <xsl:value-of select="@Low"/>
              </dayLowPrice>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Num[@fid='Low']"/>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@Open)) &gt; 0">
          <xsl:if test="not(@Open='-' or @Open='N/A')">
            <xsl:if test="string-length(normalize-space(@Open)) &gt; 0">
              <openPrice>
                <xsl:value-of select="@Open"/>
              </openPrice>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Num[@fid='Open']"/>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@Close)) &gt; 0">
          <xsl:if test="not(@Close='-' or @Close='N/A')">
            <xsl:if test="string-length(normalize-space(@Close)) &gt; 0">
              <closePrice>
                <xsl:value-of select="@Close"/>
              </closePrice>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Num[@fid='Close']"/>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@Volume)) &gt; 0">
          <xsl:if test="not(@Volume='-' or @Volume='N/A')">
            <xsl:if test="string-length(normalize-space(@Volume)) &gt; 0">
              <volume>
                <xsl:value-of select="@Volume"/>
              </volume>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Num[@fid='Volume']"/>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@VWAP)) &gt; 0">
          <xsl:if test="not(@VWAP='-' or @VWAP='N/A')">
            <xsl:if test="string-length(normalize-space(@VWAP)) &gt; 0">
              <vwap>
                <xsl:value-of select="@VWAP"/>
              </vwap>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Num[@fid='VWAP']"/>
        </xsl:otherwise>
      </xsl:choose>
      
      
    </dataPoints>
    <dividend>
      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@DividendType)) &gt; 0">
          <dividendType>
            <xsl:value-of select="@DividendType"/>
          </dividendType>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Code[@fid='DividendType']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@ExDate)) &gt; 0">
          <xsl:if test="not(@ExDate='-' or @ExDate='N/A')">
            <xsl:if test="string-length(normalize-space(@ExDate)) &gt; 0">
              <exDate>
                <xsl:value-of select="(user:ChangeDateFormat(string(@ExDate)))"/>
              </exDate>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Date[@fid='ExDate']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@RecordDate)) &gt; 0">
          <xsl:if test="not(@RecordDate='-' or @RecordDate='N/A')">
            <xsl:if test="string-length(normalize-space(@RecordDate)) &gt; 0">
              <recordDate>
                <xsl:value-of select="(user:ChangeDateFormat(string(@RecordDate)))"/>
              </recordDate>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Date[@fid='RecordDate']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@PaymentDate)) &gt; 0">
          <xsl:if test="not(@PaymentDate='-' or @PaymentDate='N/A')">
            <xsl:if test="string-length(normalize-space(@PaymentDate)) &gt; 0">
              <paymentDate>
                <xsl:value-of select="(user:ChangeDateFormat(string(@PaymentDate)))"/>
              </paymentDate>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Date[@fid='PaymentDate']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@TaxCode)) &gt; 0">
          <taxCode>
            <xsl:value-of select="@TaxCode"/>
          </taxCode>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Code[@fid='TaxCode']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@LatenessRevisionMethodCode)) &gt; 0">
          <latenessRevisionMethodCode>
            <xsl:value-of select="@LatenessRevisionMethodCode"/>
          </latenessRevisionMethodCode>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Code[@fid='LatenessRevisionMethodCode']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@DividendRate)) &gt; 0">
          <xsl:if test="not(@DividendRate='-' or @DividendRate='N/A' or @DividendRate='NA')">
            <xsl:if test="string-length(normalize-space(@DividendRate)) &gt; 0">
              <dividendRate>
                <xsl:value-of select="@DividendRate"/>
              </dividendRate>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Num[@fid='DividendRate']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@PaymentMethodCode)) &gt; 0">
          <paymentMethodCode>
            <xsl:value-of select="@PaymentMethodCode"/>
          </paymentMethodCode>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Code[@fid='PaymentMethodCode']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@PaymentOrderCode)) &gt; 0">
          <paymentOrderCode>
            <xsl:value-of select="@PaymentOrderCode"/>
          </paymentOrderCode>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Code[@fid='PaymentOrderCode']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@TransferCode)) &gt; 0">
          <transferCode>
            <xsl:value-of select="@TransferCode"/>
          </transferCode>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Code[@fid='TransferCode']"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:choose>
        <xsl:when test="string-length(normalize-space(@EntryRevisionDate)) &gt; 0">
          <xsl:if test="not(@EntryRevisionDate='-' or @EntryRevisionDate='N/A')">
            <xsl:if test="string-length(normalize-space(@EntryRevisionDate)) &gt; 0">
              <entryRevisionDate>
                <xsl:value-of select="(user:ChangeDateFormat(string(@EntryRevisionDate)))"/>
              </entryRevisionDate>
            </xsl:if>
          </xsl:if>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Date[@fid='EntryRevisionDate']"></xsl:apply-templates>

        </xsl:otherwise>
      </xsl:choose>

      <xsl:apply-templates select="DividendDesc"></xsl:apply-templates>
      <xsl:apply-templates select="TDesc"></xsl:apply-templates>
    </dividend>
  </xsl:template>
  <xsl:template match="PaymentMethodCode">
    <paymentMethodCode>
      <xsl:value-of select="@value"/>
    </paymentMethodCode>
  </xsl:template>
  <xsl:template match="PaymentOrderCode">
    <paymentOrderCode>
      <xsl:value-of select="@value"/>
    </paymentOrderCode>
  </xsl:template>
  <xsl:template match="TransferCode">
    <transferCode>
      <xsl:value-of select="@value"/>
    </transferCode>
  </xsl:template>
  <xsl:template match="EntryRevisionDate">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <entryRevisionDate>
          <xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
        </entryRevisionDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="DividendDesc">
    <fullDividendDesc>
      <xsl:value-of select="@value"/>
    </fullDividendDesc>
  </xsl:template>
  <xsl:template match="TDesc">
    <dividendTaxDesc>
      <xsl:value-of select="@value"/>
    </dividendTaxDesc>
  </xsl:template>
  <xsl:template match="Code[@fid='LatenessRevisionMethodCode']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <latenessRevisionMethodCode>
          <xsl:value-of select="(string(@value))"/>
        </latenessRevisionMethodCode>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Code[@fid='DividendType']">
    <dividendType>
      <xsl:if test="not(@value='-' or @value='N/A')">
        <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
            <xsl:value-of select="string(@value)"/>
        </xsl:if>
      </xsl:if>
    </dividendType>
  </xsl:template>
  <xsl:template match="Code[@fid='TaxCode']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <taxCode>
          <xsl:value-of select="(string(@value))"/>
        </taxCode>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Date[@fid='PaymentDate']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <paymentDate>
          <xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
        </paymentDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Date[@fid='ExDate']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <exDate>
          <xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
        </exDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Date[@fid='RecordDate']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <recordDate>
          <xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
        </recordDate>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Num[@fid='DividendRate']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <dividendRate>
          <xsl:value-of select="user:FormatNumberToPrecision(string((@exp)),string(@value))"/>
        </dividendRate>
      </xsl:if>
    </xsl:if>
  </xsl:template>
  <xsl:template match="Date[@fid='SampleDate']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <date>
          <xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
        </date>
      </xsl:if>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Num[@fid='High']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <dayHighPrice>
          <xsl:value-of select="user:FormatNumberToPrecision(string((@exp)),string(@value))"/>
        </dayHighPrice>
      </xsl:if>
    </xsl:if>
  </xsl:template>


  <xsl:template match="Num[@fid='Low']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <dayLowPrice>
          <xsl:value-of select="user:FormatNumberToPrecision(string((@exp)),string(@value))"/>
        </dayLowPrice>
      </xsl:if>
    </xsl:if>
  </xsl:template>


  <xsl:template match="Num[@fid='Open']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <openPrice>
          <xsl:value-of select="user:FormatNumberToPrecision(string((@exp)),string(@value))"/>
        </openPrice>
      </xsl:if>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Num[@fid='Close']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <closePrice>
          <xsl:value-of select="user:FormatNumberToPrecision(string((@exp)),string(@value))"/>
        </closePrice>
      </xsl:if>
    </xsl:if>
  </xsl:template>


  <xsl:template match="Num[@fid='Volume']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <volume>
          <xsl:value-of select="user:FormatNumberToPrecision(string((@exp)),string(@value))"/>
        </volume>
      </xsl:if>
    </xsl:if>
  </xsl:template>

  <xsl:template match="Num[@fid='VWAP']">
    <xsl:if test="not(@value='-' or @value='N/A')">
      <xsl:if test="string-length(normalize-space(@value)) &gt; 0">
        <vwap>
          <xsl:value-of select="user:FormatNumberToPrecision(string((@exp)),string(@value))"/>
        </vwap>
      </xsl:if>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>