<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"  xmlns:user="user" extension-element-prefixes="msxsl" exclude-result-prefixes="user">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="no"/>

  <xsl:template match="/GetArchiveObjectResponse">
    <GetAutoCodeXmlMessageResponse>
      <xsl:apply-templates select="Status"/>
      <Message>
        <xsl:apply-templates select="." mode="transformation"/>
      </Message>
    </GetAutoCodeXmlMessageResponse>
  </xsl:template>

  <xsl:template match="Status">
    <xsl:copy-of select="."/>
  </xsl:template>

  <xsl:template match="GetArchiveObjectResponse" mode="transformation">
    <AutoCode version="1.0">
      <Request>
        <Sevices>
          <CodeDescriptors />
          <Extraction>
            <Param name="entity-markup" value="person_pos" />
            <Param name="entity-markup" value="person" />
            <Param name="entity-markup" value="person_giv" />
            <Param name="entity-markup" value="company" />
            <Param name="entity-markup" value="organization" />
            <Param name="entity-markup" value="city" />
            <Param name="entity-markup" value="state" />
            <Param name="entity-markup" value="country" />
            <Param name="rules-markup" value="Affiliation" />
            <Param name="rules-markup" value="ALLIANCE" />
            <Param name="rules-markup" value="AWARD" />
            <Param name="rules-markup" value="MA" />
            <Param name="rules-markup" value="MANAGEMENTCHANGES" />
            <Param name="rules-markup" value="QUOTE" />
            <Param name="rules-markup" value="SPINOFF" />
          </Extraction>
          <Language lang="en" />
        </Sevices>
        <DistList>
          <xsl:copy-of select="." />
        </DistList>
      </Request>
    </AutoCode>
  </xsl:template>
</xsl:stylesheet>