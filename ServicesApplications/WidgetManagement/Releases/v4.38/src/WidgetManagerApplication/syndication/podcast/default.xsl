<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="html" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN"/>
    <!-- <xsl:output method="html" indent="yes" omit-xml-declaration="yes"/> -->
    <xsl:template match="/">
          	<html><head></head><body>
		<h1><xsl:value-of select="/rss/channel/title"/></h1>
       
            	<b>
                	<xsl:value-of select="/rss/channel/description"/>
                <p>
		<a>
                <xsl:attribute name="href">
                    <xsl:value-of select="/rss/channel/link"/>
                </xsl:attribute>
                Visit the website</a>
		</p>          
		</b>
        <hr/> 
	<i>This Podcast created by Factiva - www.factiva.com</i>
        <hr/> 
        <br/>
        <xsl:for-each select="/rss/channel/item">
            <h2>
                <a>
                    <xsl:attribute name="href">
                        <xsl:value-of select="link"/>
                    </xsl:attribute>
                    <xsl:value-of select="title"/>
               </a>
		</h2> 
                <br/>
                <xsl:value-of select="description" disable-output-escaping="yes"/> 
                <br/>
                <hr/>            
        </xsl:for-each>
</body></html>
    </xsl:template>
</xsl:stylesheet>
