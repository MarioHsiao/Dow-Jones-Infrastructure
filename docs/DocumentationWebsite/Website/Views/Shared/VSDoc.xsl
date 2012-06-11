<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <!-- DOCUMENT TEMPLATE -->
  <!-- Format the whole document as a valid HTML document -->
  <xsl:template match="/">
    <xsl:apply-templates select="//type"/>
  </xsl:template>

  <!-- TYPE TEMPLATE -->
  <xsl:template match="type">
    <section class="type">
      <div class="name"><xsl:value-of select="@fullName"/></div>
      <div class="description"><xsl:value-of select="summary"/></div>
      <xsl:apply-templates select="constructors"/>
      <xsl:apply-templates select="properties"/>
      <xsl:apply-templates select="methods"/>
      <xsl:apply-templates select="events"/>
    </section>
  </xsl:template>

  <!-- CONSTRUCTORS TEMPLATE -->
  <xsl:template match="constructors">
    <h3>Constructors</h3>
    <section class="constructors">
        <xsl:apply-templates select="constructor"/>
    </section>
  </xsl:template>
  <xsl:template match="constructor">
    <section class="constructor parameterized-method">
      <xsl:call-template name="parameterized-method" />
    </section>
  </xsl:template>
  
  <!-- PROPERTY TEMPLATES -->
  <xsl:template match="properties">
    <h3>Properties</h3>
    <section class="properties">
      <table>
        <thead>
          <th>Name</th>
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
    <h3>Methods</h3>
    <section class="methods">
        <xsl:apply-templates select=".//method"/>
    </section>
  </xsl:template>
  <xsl:template match="method">
    <div class="method parameterized-method">
      <xsl:call-template name="parameterized-method" />
    </div>
  </xsl:template>

  <!-- EVENT TEMPLATES -->
  <xsl:template match="events">
    <h3>Events</h3>
    <section class="events">
      <table>
        <thead>
          <th>Name</th>
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
  <xsl:template name="parameterized-method">
      <div class="name"><xsl:value-of select="@name"/></div>
      <div class="description">
        <xsl:value-of select="summary"/>
        <xsl:value-of select="remarks"/>
      </div>
      <div class="parameters">
        <table>
          <thead>
            <th>Name</th>
            <th>Type</th>
            <th>Description</th>
          </thead>
          <tbody>
            <xsl:apply-templates select=".//parameter"/>
          </tbody>
        </table>
      </div>
  </xsl:template>
  
  <!-- PARAMETER TEMPLATE -->
  <xsl:template match="parameter">
    <tr class="parameter">
      <td class="name">
        <xsl:value-of select="@name"/>
      </td>
      <td class="type">
        <xsl:value-of select="@type"/>
      </td>
      <td class="description">
        <xsl:value-of select="remarks"/>
      </td>
    </tr>
  </xsl:template>

</xsl:stylesheet>
