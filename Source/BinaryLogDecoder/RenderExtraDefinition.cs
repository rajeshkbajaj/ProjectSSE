using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Covidien.CGRS.BinaryLogDecoder
{
    public class RenderExtraDefinition : RenderFieldDefinition
    {
        /// <summary>
        /// Display pre-formatting
        /// </summary>
        public string PreFormat { get; set; }

        /// <summary>
        /// Comma Separated Strings identifying element indexes to use to fulfill the format
        /// </summary>
        public string Elements { get; set; }

        
        public RenderExtraDefinition( string name )
            : base(name)
        {
        }

#pragma warning disable 108,114
        public bool Itor( string xPathPrefix, XmlElement field )
#pragma warning restore 108,114
        {
            if  ( base.Itor( xPathPrefix, field ) )
            {
                string attrStr = field.GetAttribute("PreFormat");
                if  ( !String.IsNullOrEmpty( attrStr ) )
                {
                    PreFormat = attrStr;
                }
                else
                {
                    return( false ) ;
                }

                attrStr = field.GetAttribute("Elements");
                if  ( !String.IsNullOrEmpty( attrStr ) )
                {
                    Elements = attrStr;
                }

                return( true ) ;
            }
            else
            {
                return( false ) ;
            }
        }
    }
}
