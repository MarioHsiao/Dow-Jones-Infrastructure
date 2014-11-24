<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:fn="http://www.w3.org/2005/xpath-functions">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:param name="scope"/>
  <xsl:template match="/*">
    <xsl:element name="TrackFldrAccessLevRequest">
      <xsl:attribute name="ver">1.0</xsl:attribute>
      <xsl:element name="FolderSet">
        <xsl:apply-templates select="//folderID"/>
      </xsl:element>
    </xsl:element>
  </xsl:template>
  <xsl:template match ="//folderID">
    <Folder>
      <xsl:attribute name="folderId">
        <xsl:value-of select ="@folderId"/>
      </xsl:attribute>
      <!--Commented By Avinash-->
      <!--<xsl:choose>
        <xsl:when test ="$scope='personal' or $scope='Personal'">

        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="folderId">
            <xsl:value-of select ="@folderId"/>
          </xsl:attribute>

        </xsl:otherwise>
      </xsl:choose>-->

      <SharingData>
        <acScope>
          <xsl:call-template name ="GetXFormedScope">
            <xsl:with-param name="scope">
              <xsl:choose>
                <xsl:when test=".//accessControlScope">
                  <xsl:value-of select =".//accessControlScope"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$scope"/>
                </xsl:otherwise>
              </xsl:choose>

            </xsl:with-param>
          </xsl:call-template>
        </acScope>
        <listScope>
          <xsl:call-template name ="GetXFormedScope">
            <xsl:with-param name="scope">
              <xsl:choose>
                <xsl:when test=".//listingScope">
                  <xsl:value-of select =".//listingScope"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$scope"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
          </xsl:call-template>
        </listScope>
        <assignScope>
          <xsl:call-template name ="GetXFormedScope">
            <xsl:with-param name="scope">
              <xsl:choose>
                <xsl:when test=".//assignedScope">
                  <xsl:value-of select =".//assignedScope"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$scope"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
          </xsl:call-template>
        </assignScope>
        <sharePromotion>
          <xsl:call-template name ="GetXFormedScope">
            <xsl:with-param name="scope">
              <xsl:choose>
                <xsl:when test=".//sharePromotion">
                  <xsl:value-of select =".//sharePromotion"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select ="$scope"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:with-param>
          </xsl:call-template>

        </sharePromotion>

        <!-- 10/24/07 for access status.. no need to set the hashkeys. as they are maintained by back ends and are read-oly.-->
        <xsl:choose>
          <xsl:when test=".//internalAccess='0' or .//internalAccess='Deny'">
            <intAccess value="Off"></intAccess>
          </xsl:when>
          <xsl:when test=".//internalAccess='1' or .//internalAccess='Allow'">
            <intAccess value="On"></intAccess>
          </xsl:when>
        </xsl:choose>

        <xsl:choose>
          <xsl:when test=".//externalAccess='0' or .//externalAccess='Deny'">
            <extAccess value="Off"></extAccess>
          </xsl:when>
          <xsl:when test=".//externalAccess='1' or .//externalAccess='Allow'">
            <extAccess value="On"></extAccess>
          </xsl:when>
        </xsl:choose>

      </SharingData>
    </Folder>
  </xsl:template>
  <xsl:template name="GetXFormedScope">
    <xsl:param name="scope"/>
    <xsl:attribute name="value">
      <xsl:choose>
        <xsl:when test="$scope='PreviousScope'">previous</xsl:when>
        <xsl:when test="$scope='personal' or $scope='Personal'">personal</xsl:when>
        <xsl:when test="$scope='everyone' or $scope='Everyone'">everyone</xsl:when>
        <xsl:when test="$scope='account' or $scope='Account'">account</xsl:when>
        <xsl:otherwise>personal</xsl:otherwise>
      </xsl:choose>
    </xsl:attribute>
  </xsl:template>
</xsl:stylesheet>
