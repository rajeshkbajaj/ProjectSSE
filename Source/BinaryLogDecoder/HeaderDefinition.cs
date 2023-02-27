using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Covidien.CGRS.BinaryLogDecoder
{
    public class HeaderDefinition
    {
        public List<FieldDefinition> Fields { get; set; }
        public List<RenderDefinition> Renders { get; set; }

        public HeaderDefinition()
        {
            Fields = new List<FieldDefinition>();
            Renders = new List<RenderDefinition>();
        }

        public bool Itor( string xPathPrefix, XmlElement header )
        {
            FieldDefinition fieldDefn = null ;
            string xPathPrefix1 = string.Format( "{0}/Field", xPathPrefix ) ;
            XmlNodeList headerFields = header.OwnerDocument.SelectNodes( xPathPrefix1 );
            foreach( XmlElement field in headerFields )
            {
                fieldDefn = new FieldDefinition() ;
                if  ( fieldDefn.Itor( xPathPrefix, field ) )
                {
                    Fields.Add( fieldDefn ) ;
                }
            }

            RenderDefinition renderDefn = null ;
            string xPathPrefix2 = string.Format( "{0}/RenderDescription", xPathPrefix ) ;
            XmlNodeList renders = header.OwnerDocument.SelectNodes( xPathPrefix2 ) ;
            foreach( XmlElement render in renders )
            {
                string renderName = render.GetAttribute("name");
                if ( !string.IsNullOrEmpty( renderName ) )
                {
                    renderDefn = new RenderDefinition( renderName ) ;
                }
                else
                {
                    continue;  // if the message is not named, then ignore it
                }

                // Note: must pass the Itor() in order to be added to the list.
                if  ( renderDefn.Itor( xPathPrefix2, render ) )
                {
                    Renders.Add( renderDefn ) ;
                }
            }
            
            return( true ) ;
        }
    }
}
