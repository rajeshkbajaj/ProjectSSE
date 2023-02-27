using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LogViewer
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

        public bool Itor( string xPathPrefix, XmlElement field )
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
