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
			
	 function FormatNumberToPrecision(exponent, val)
      {
         var ret_val;
         var isNegative = false;
         var decPlace = 0;

         //incase we cannot do anything return this.
         ret_val = val;

         try
         {    
                 ret_val = (Math.abs(val) * Math.pow(10,exponent));
                 if(val.toString().indexOf("-") !=-1){
                 ret_val = "-"+ret_val;          
	            }
          }
          catch(e)
          {
                 //do nothing...
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
			<!--<xsl:for-each select="Result">
				<xsl:copy-of select="."/>
			</xsl:for-each>-->
		</historicalDataResultSet>
	</xsl:template>

	<xsl:template match="Result">
		<xsl:choose>
			<xsl:when test="@status!='0'">
				<historicalDataResult>
					<!--<xsl:attribute name="status"><xsl:value-of select="@status"/></xsl:attribute>-->
					<status>
						<xsl:attribute name="value"><xsl:value-of select="@status"/></xsl:attribute>
						<type/>
						<message/>
					</status>
					<requestedSymbol/>  <!--This value is set in HMD.cs -->
				</historicalDataResult>
			</xsl:when>
			<xsl:otherwise>
				<historicalDataResult>
					<xsl:call-template name="req_token"/>
					<xsl:apply-templates select="@code"/>
					<instrumentName/> <!--This value is set in HMD.cs -->
					<instrumentType/> <!--This value is set in HMD.cs -->
					<source>
							<xsl:value-of select="normalize-space(string(@source))"/>
					</source>
					<exchange></exchange>
					<frequency><xsl:value-of select="normalize-space(string(Series/@periodicity))"/></frequency>
					<xsl:apply-templates select="Series/Date[@fid='from']"/>
					<xsl:apply-templates select="Series/Date[@fid='to']"/>
					<xsl:if test="normalize-space(string(Series/@currency)) != ''">
						<currencyCode><xsl:value-of select="normalize-space(string(Series/@currency))"/></currencyCode>
					</xsl:if>
                                <xsl:apply-templates select="Series/Samples"></xsl:apply-templates>
                                </historicalDataResult>
                          </xsl:otherwise>
                   </xsl:choose>
             	</xsl:template>

	<xsl:template name="req_token">
		<requestedSymbol/>  <!--This value is set in HMD.cs -->
		<djTicker/>  <!--This value is set in HMD.cs -->
	</xsl:template>
	<xsl:template match="@code">
		<xsl:if test="string-length(normalize-space(.)) &gt; 0">
			<ric><xsl:value-of select="normalize-space(.)"/></ric>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Series/Date[@fid='from']">
		<xsl:if test="not(@value='-' or @value='N/A')">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<fromDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</fromDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Series/Date[@fid='to']">
		<xsl:if test="not(@value='-' or @value='N/A')">
			<xsl:if test="string-length(normalize-space(@value)) &gt; 0">
				<toDate>
					<xsl:value-of select="(user:ChangeDateFormat(string(@value)))"/>
				</toDate>
			</xsl:if>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="Series/Samples">
		<xsl:apply-templates select="Sample"/>
	</xsl:template>
	<xsl:template match="Sample">
		<dataPoints>
			<xsl:apply-templates select="Date[@fid='SampleDate']"/>
			<xsl:apply-templates select="Num[@fid='High']"/>
			<xsl:apply-templates select="Num[@fid='Low']"/>
			<xsl:apply-templates select="Num[@fid='Open']"/>
			<xsl:apply-templates select="Num[@fid='Close']"/>
			<xsl:apply-templates select="Num[@fid='Volume']"/>
		</dataPoints>
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

</xsl:stylesheet>