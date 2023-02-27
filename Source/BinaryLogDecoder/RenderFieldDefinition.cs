using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Covidien.CGRS.BinaryLogDecoder
{
    public class RenderFieldDefinition
    {
        /// <summary>
        /// Identifier for the field, so can be referenced in a couple of places
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Order of field for proper sequencing as decoding a data (e.g., byte) stream.  This is needed because sometimes various xml viewers reorder the xml, usually just attrs though.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Report column header
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Display formatting
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Display formatting to wrap an array of values - where each element was first formatted using Format
        /// </summary>
        public string OuterFormat { get; set; }




        public RenderFieldDefinition( string name )
        {
            Name = name;
            Order = 0;
        }


        public bool Itor( string xPathPrefix, XmlElement field )
        {
            int tmpInt;
            string attrStr;

            attrStr = field.GetAttribute("name");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                Name = attrStr ;
            }
            else
            {
                return( false ) ;   // cannot take a field without a name
            }

            attrStr = field.GetAttribute("order");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                if  ( Int32.TryParse( attrStr, out tmpInt ) )
                {
                    Order = tmpInt ;
                }
                else
                {
                    return( false ) ;   // cannot take a field without a data order.
                }
            }

            attrStr = field.GetAttribute("header");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                Header = attrStr;
            }

            attrStr = field.GetAttribute("format");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                Format = attrStr;
            }

            attrStr = field.GetAttribute("outerFormat");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                OuterFormat = attrStr;
            }

            return (true);
        }
    }
}
