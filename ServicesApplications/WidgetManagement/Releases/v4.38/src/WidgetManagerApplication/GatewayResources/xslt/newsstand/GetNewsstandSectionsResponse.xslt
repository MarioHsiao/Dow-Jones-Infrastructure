<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  extension-element-prefixes="msxsl">
    <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

	<xsl:template match="/*">
		<GetNewsstandSectionsResponse>
			<newsstandSectionsResponse>
			<newsstandSectionsResult>
				<xsl:choose>
					<xsl:when test="//SourceList/@count='0'">
						<Status>
							<xsl:attribute name="value">30001</xsl:attribute>
						</Status>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="//Status"/>
						<xsl:choose>
							<xsl:when test="string-length(normalize-space(substring-after(//SourceName,'|'))) &gt; 0"><sourceName><xsl:value-of select="normalize-space(substring-after(//SourceName,'|'))"/></sourceName></xsl:when>
<xsl:otherwise><sourceName/></xsl:otherwise>
						</xsl:choose>
						<xsl:choose>
							<xsl:when test="string-length(normalize-space(//Source/@sourcecode)) &gt; 0"><sourceCode><xsl:value-of select="normalize-space(//Source/@sourcecode)"/></sourceCode></xsl:when>
							<xsl:otherwise><sourceCode/></xsl:otherwise>
						</xsl:choose>
						<xsl:choose>
							<xsl:when test="string-length(normalize-space(//DaysToKeep/@value)) &gt; 0"><daysKept><xsl:value-of select="normalize-space(//DaysToKeep/@value)"/></daysKept></xsl:when>
<xsl:otherwise><daysKept>0</daysKept></xsl:otherwise>
						</xsl:choose>
						
						<xsl:apply-templates select="//SourceList"/>
					</xsl:otherwise>
				</xsl:choose>
			</newsstandSectionsResult>
			</newsstandSectionsResponse>
		</GetNewsstandSectionsResponse>
	</xsl:template>
	
	<xsl:template match="//Status">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="//SourceList">
		<xsl:apply-templates select="//SectionList"/>
	</xsl:template>

	<xsl:template match="//SectionList">
             <newsstandSectionsResultSet>
             		<xsl:attribute name="count">
             			<xsl:value-of select="count(//Section)"/>
             		</xsl:attribute>
                   <xsl:apply-templates select="//Section"/>
             </newsstandSectionsResultSet>
       </xsl:template>

       
        <xsl:template match="//Section">
	            <sectionInfo>
	                     <xsl:attribute name="defaultSection" >
	                     	<xsl:choose>
						<xsl:when test="normalize-space(@defaultsection)='N'">false</xsl:when>
						<xsl:otherwise>true</xsl:otherwise>
					</xsl:choose>
	                     </xsl:attribute>
	                     <xsl:choose>
	                     	<xsl:when test="string-length(normalize-space(@folderid)) &gt; 0"><sectionID><xsl:value-of select="normalize-space(@folderid)"/></sectionID></xsl:when>
<xsl:otherwise><sectionID/></xsl:otherwise>
	                     </xsl:choose>
	                     <xsl:choose>
					<xsl:when test="string-length(normalize-space(substring-after(SectionName,'|'))) &gt; 0"> 					<sectionName><xsl:value-of select="normalize-space(substring-after(SectionName,'|'))"/></sectionName>
</xsl:when>
					<xsl:otherwise><sectionName/></xsl:otherwise>
				</xsl:choose>
	                     
	                     <xsl:apply-templates select="DateList"/>
	             </sectionInfo>
        </xsl:template>
                
        <xsl:template match="DateList">
        	<publicationDates>
        		<xsl:choose>
        			<xsl:when test="string-length(normalize-space(@count)) &gt; 0">
				<xsl:attribute name="count"><xsl:value-of select="normalize-space(@count)"/></xsl:attribute>
        			</xsl:when>
        			<xsl:otherwise><xsl:attribute name="count"/>	</xsl:otherwise>
        		</xsl:choose>
                    <xsl:apply-templates select="PubDate"/>
	 	</publicationDates>
	 </xsl:template>
	      
        <xsl:template match="PubDate">
			<xsl:if test="string-length(normalize-space(@value))=8">
				<publicationDate><xsl:value-of select="concat(substring(@value,1,4),'-',substring(@value,5,2),'-',substring(@value,7,2))" /></publicationDate>
			</xsl:if>
        </xsl:template>
</xsl:stylesheet>

