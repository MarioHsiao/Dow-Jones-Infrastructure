<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- TYPE TEMPLATE -->
  <xsl:template match="type">
    <section class="type">
      <div class="name"><xsl:value-of select="@fullName"/></div>
      <div class="description"><xsl:value-of select="summary"/></div>
      
      <h3>Constructors</h3>
      <xsl:apply-templates select="constructors"/>

      <h3>Properties</h3>
      <xsl:apply-templates select="properties"/>

      <h3>Methods</h3>
      <xsl:apply-templates select="methods"/>

      <h3>Events</h3>
      <xsl:apply-templates select="events"/>
      
    </section>
  </xsl:template>

  <!-- CONSTRUCTORS TEMPLATE -->
  <xsl:template match="constructors">
    <section class="constructors">
      <xsl:call-template name="parameterized-methods" />
    </section>
  </xsl:template>
  
  <!-- PROPERTY TEMPLATES -->
  <xsl:template match="properties">
    <section class="properties">
      <table>
        <thead>
          <th class="name">Name</th>
          <th>Description</th>
        </thead>
        <tbody>
          <xsl:apply-templates select=".//property"/>
        </tbody>
      </table>
    </section>
  </xsl:template>
  <xsl:template match="property">
    <tr class="property">
      <td class="name">
        <xsl:value-of select="@name"/>
      </td>
      <td class="description">
        <xsl:value-of select="summary"/>
      </td>
    </tr>
  </xsl:template>
  
  <!-- METHOD TEMPLATES -->
  <xsl:template match="methods">
    <section class="methods">
      <xsl:call-template name="parameterized-methods" />
    </section>
  </xsl:template>

  <!-- EVENT TEMPLATES -->
  <xsl:template match="events">
    <section class="events">
      <table>
        <thead>
          <th class="name">Name</th>
          <th>Description</th>
        </thead>
        <tbody>
          <xsl:apply-templates select=".//event"/>
        </tbody>
      </table>
    </section>
  </xsl:template>
  <xsl:template match="event">
    <tr class="event">
      <td class="name">
        <xsl:value-of select="@name"/>
      </td>
      <td class="description">
        <xsl:value-of select="summary"/>
      </td>
    </tr>
  </xsl:template>


  <!-- PARAMETERIZED METHOD TEMPLATE -->
  <xsl:template name="parameterized-methods">
    <table>
      <thead>
        <th class="name">Name</th>
        <th>Description</th>
      </thead>
      <tbody>
        <xsl:for-each select="./*">
          <tr class="parameterized-method">
            <td class="name">
              <xsl:value-of select="@name"/>
            </td>
            <td class="description">
              <xsl:value-of select="summary"/>
              <xsl:value-of select="remarks"/>

              <!-- Parameters table -->
              <xsl:if test="count(.//parameter)">
                <table class="parameters">
                  <tbody>
                    <xsl:for-each select=".//parameter">
                      <tr class="parameter">
                        <td class="name">
                          <xsl:value-of select="@name"/>
                        </td>
                        <td class="description">
                          <xsl:value-of select="remarks"/>
                        </td>
                      </tr>
                    </xsl:for-each>
                  </tbody>
                </table>
              </xsl:if>
              <!-- End Parameters table -->
              
            </td>
          </tr>
        </xsl:for-each>
      </tbody>
    </table>
  </xsl:template>
  
</xsl:stylesheet>
