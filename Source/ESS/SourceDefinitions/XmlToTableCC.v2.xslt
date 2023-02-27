<xsl:stylesheet
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  version="1.0">

  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/">
    <html>
      <head></head>
      <body>
        <center>
          <h1>
            ESS Log
            <!-- <br/>
            <xsl:value-of select="current-date()"/>
            -->
          </h1>
          <xsl:apply-templates/>
        </center>
      </body>
    </html>
  </xsl:template>



  <xsl:template match="/log/header">
    <table border="1" >
      <xsl:apply-templates/>
    </table>
    <br/>
  </xsl:template>

  <xsl:template match="/log/header/*">
    <tr>
      <th style="white-space:nowrap;">
        <xsl:call-template name="CamelCase">
          <xsl:with-param name="pText" select="name()" />
        </xsl:call-template>
      </th>
      <td>
        <xsl:value-of select="."/>
      </td>
    </tr>
  </xsl:template>

  
  <xsl:template match="/log/records">
    <table border="1" >
      <thead>
        <tr>
          <xsl:apply-templates select="*[1]/*" mode="th"/>
        </tr>
      </thead>
      <tbody>
        <xsl:apply-templates select="*"/>
      </tbody>
    </table>
  </xsl:template>


  <xsl:template match="/log/records/*/*" mode="th">
    <th style="white-space:nowrap;">
      <xsl:value-of select="@display_name"/>
    </th>
  </xsl:template>


  <xsl:template match="/log/records/*">
    <tr>
      <xsl:apply-templates select="*"/>
    </tr>
  </xsl:template>

  <xsl:template match="/log/records/*/*">
    <td>
      <xsl:value-of select="."/>
    </td>
  </xsl:template>


  <xsl:template match="@display_name">
    <xsl:value-of select="."/>
  </xsl:template>


  <xsl:template name="CamelCase">
    <xsl:param name="pText" />
    <xsl:variable name="vLower">abcdefghijklmnopqrstuvwxyz</xsl:variable>
    <xsl:variable name="vUpper">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
    <xsl:value-of select="translate(substring($pText,1,1), $vLower, $vUpper)" />
    <xsl:call-template name="CamelCase_recurse">
      <xsl:with-param name="pText" select="substring($pText,2)" />
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="CamelCase_recurse">
    <xsl:param name="pText" />
    <xsl:variable name="vLower">abcdefghijklmnopqrstuvwxyz</xsl:variable>
    <xsl:variable name="vUpper">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
    <xsl:if test="string-length($pText) &gt; 1">
      <xsl:choose>
        <xsl:when test="not(substring($pText,1,1) = '_')">
          <xsl:if test="not(substring($pText,1,1) = '_')">
            <xsl:value-of select="translate(substring($pText,1,1), $vUpper, $vLower)" />
          </xsl:if>
          <xsl:call-template name="CamelCase_recurse">
            <xsl:with-param name="pText" select="substring($pText,2)" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:text> </xsl:text>
          <xsl:value-of select="translate(substring($pText,2,1), $vLower, $vUpper)" />
          <xsl:if test="string-length($pText) &gt; 2">
            <xsl:call-template name="CamelCase_recurse">
              <xsl:with-param name="pText" select="substring($pText,3)" />
            </xsl:call-template>
          </xsl:if>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    <xsl:if test="string-length($pText) = 1">
      <xsl:value-of select="$pText" />
    </xsl:if>
  </xsl:template>

</xsl:stylesheet>