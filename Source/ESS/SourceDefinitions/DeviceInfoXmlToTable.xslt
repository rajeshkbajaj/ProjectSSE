<xsl:stylesheet
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="1.0">

  <xsl:template match="components">
    <html>
      <body>
        <h2>Device Information</h2>
        <h3>Device Configuration</h3>
        <xsl:apply-templates select="component[name='MasterVent']"/>
        <h3>Device Components</h3>
        <xsl:apply-templates select="component[name!='MasterVent'][1]"/>
        <hr></hr>
      </body>
    </html>
  </xsl:template>

   <xsl:template match="component[name='MasterVent']">
     <table border="1">
       <xsl:for-each select="descendant::*">
         <tr>
           <xsl:call-template name="masterventtable"/>
         </tr>
       </xsl:for-each>
     </table>
   </xsl:template>

   <xsl:template name="masterventtable">
     <td>
       <xsl:value-of select="name()"/>
     </td>
     <td>
       <xsl:value-of select="."/>
     </td>
   </xsl:template>

  <xsl:template match="component[name!='MasterVent']">
    <table border="1">
    <tr>
      <xsl:apply-templates mode="th" />
    </tr>
    <xsl:apply-templates select="../component[name!='MasterVent']" mode="td" />
    </table>
  </xsl:template>

  <xsl:template match="component[name!='MasterVent']/*" mode="th">
    <th>
      <xsl:value-of select="name()" />
    </th>
  </xsl:template>
  
  <xsl:template match="component[name!='MasterVent']" mode="td">
    <tr>
      <xsl:apply-templates />
    </tr>
  </xsl:template>
  <xsl:template match="component[name!='MasterVent']/*">
    <td>
      <xsl:apply-templates />
    </td>
  </xsl:template>


</xsl:stylesheet>