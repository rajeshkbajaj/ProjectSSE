
function fMakeHtmlTable( source, lineStruct, header )
{
  var tmpArray = [];
  var re;

  tmpArray[0] = header + '\t<tbody id="data-area">';
  for( var i = 0, k = 1, j = source.length ;   i < j ;   i++, k++ )
  {
    tmpArray[k] = lineStruct.replace(/%(\d+)%/g, cvrt ) ;
  }
  tmpArray[k] = "\t</tbody>\n</table>" ;

  return( tmpArray.join( "\n" ) ) ;


  function cvrt()
  {
    return( source[ i ][ arguments[ 1 ] ] ) ;
  }
}


function fMakeHtmlPivotTable( source, lineStructs, header )
{
    var tmpArray = [];
    var re;

    tmpArray[0] = header + '\t<tbody id="data-area">' ;
    for( var i = 0, k = 1, j = lineStructs.length ;   i < j ;   i++, k++ )
    {
        tmpArray[k] = lineStructs[i].replace(/%(\d+)%/g, cvrt ) ;
    }
    tmpArray[k] = "\t</tbody>\n</table>";

    return( tmpArray.join( "\n" ) ) ;


    function cvrt()
    {
        return( source[arguments[1]] ) ;
    }
}


