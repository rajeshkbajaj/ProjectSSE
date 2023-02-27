<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:template match="/">
		<h1>Log</h1>
        <table border="1">
            <xsl:apply-templates select="log/header" />
        </table>
		<h2>Log Entries</h2>
        <table border="1">
            <xsl:apply-templates select="log/record[1]" />
        </table>
      <hr></hr>
    </xsl:template>
	
<!-- HEADER -->	
	<xsl:template match="header">
        <tr>
            <xsl:apply-templates mode="th" />
        </tr>
        <xsl:apply-templates select="../header" mode="td" />
    </xsl:template>
	
    <xsl:template match="header/*" mode="th">
        <th>
            <xsl:value-of select="name()" />
        </th>
    </xsl:template>
    <xsl:template match="header" mode="td">
        <tr>
            <xsl:apply-templates />
        </tr>
    </xsl:template>
    <xsl:template match="header/*">
        <td>
            <xsl:apply-templates />
        </td>
    </xsl:template>

<!-- LOG ENTRIES -->	
    <xsl:template match="record">
        <tr>
            <xsl:apply-templates mode="th" />
        </tr>
        <xsl:apply-templates select="../record" mode="td" />
    </xsl:template>
	
    <xsl:template match="record/*" mode="th">
        <th>
            <xsl:value-of select="@name" />
        </th>
    </xsl:template>
    <xsl:template match="record" mode="td">
        <tr>
            <xsl:apply-templates />
        </tr>
    </xsl:template>
    <xsl:template match="record/*">
        <td>
            <xsl:apply-templates />
        </td>
    </xsl:template>
</xsl:stylesheet>