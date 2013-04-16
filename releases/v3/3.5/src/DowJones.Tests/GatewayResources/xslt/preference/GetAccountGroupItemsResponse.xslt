<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />
  <xsl:template match="/*">
    <xsl:element name="GetAccountGroupItemsResponse">
      <xsl:copy-of select="Control"/>
      <xsl:copy-of select="Status"/>
      <xsl:for-each select="ResultSet/Result/child::*">
        <xsl:choose >
          <xsl:when test="local-name() = 'ITEM'">
            <xsl:element name="Item">
              <xsl:for-each select="child::*">
                <xsl:choose>
                  <xsl:when test="local-name() = 'Subscribable'">
                    <xsl:element name="Subscribable">
                      <xsl:choose>
                        <xsl:when test ="(.)[text()] = 'Y'">true</xsl:when>
                        <xsl:otherwise>false</xsl:otherwise>
                      </xsl:choose>
                    </xsl:element>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:copy-of select ="."/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:for-each>
            </xsl:element>
          </xsl:when>
          <xsl:when test="local-name() = 'Exists'">
            <xsl:element name="Exists">
              <xsl:choose>
                <xsl:when test ="(.)[text()] = 'Y'">true</xsl:when>
                <xsl:otherwise>false</xsl:otherwise>
              </xsl:choose>
            </xsl:element>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select ="."/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>

    </xsl:element>
  </xsl:template>
</xsl:stylesheet>

<!--Output Format-->
<!--
<ITEM>
  <ITEM_BLOB>
    <CLASS NAME="byline">
      <ITEM>
        <NAME>ByLine</NAME>
        <VALUE>on</VALUE>
      </ITEM>
    </CLASS>
  </ITEM_BLOB>
  <ITEM_CLASS>45</ITEM_CLASS>
  <ITEM_ID>214846</ITEM_ID>
  <ITEM_INSTANCE_NAME>ByLine</ITEM_INSTANCE_NAME>
  <Subscribable>true</Subscribable>
</ITEM>

<ITEM>
  <ITEM_BLOB>
    <CLASS NAME="companysearch">
      <ITEM>
        <NAME>CompanySearch</NAME>
        <VALUE>s,,yy,,zzz,</VALUE>
      </ITEM>
    </CLASS>
  </ITEM_BLOB>
  <ITEM_CLASS>53</ITEM_CLASS>
  <ITEM_ID>82063</ITEM_ID>
  <ITEM_INSTANCE_NAME>CompanySearch</ITEM_INSTANCE_NAME>
  <Subscribable>false</Subscribable>
</ITEM>
<Exists>true</Exists>
<ERROR_CODE>0</ERROR_CODE>
<ERROR_GENERAL_MSG>Success</ERROR_GENERAL_MSG>
-->