using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Covidien.CGRS.BinaryLogDecoder
{
    public class RenderDefinition
    {
        public String Name { get; set; }

        public string RenderTableAttribs { get; set; }
        public string RenderTableHeader { get; set; }
        public List<string> RenderTableRows { get; set; }
        public List<RenderFieldDefinition> RenderFields { get; set; }

        /// <summary>
        /// ctor()
        /// </summary>
        /// <param name="name">Name of the message</param>
        public RenderDefinition(string name)
        {
            Name = name;

            RenderTableRows = new List<string>() ;
            RenderFields = new List<RenderFieldDefinition>();
        }


        public bool Itor( string xPathPrefix, XmlElement union )
        {
            RenderFieldDefinition renderFieldDefn = null;
            string xPathPrefix1 = string.Format( "{0}[@name='{1}']/RenderField", xPathPrefix, Name ) ;
            XmlNodeList renderFields = union.OwnerDocument.SelectNodes( xPathPrefix1 ) ;
            foreach( XmlElement renderField in renderFields )
            {
                string name = renderField.GetAttribute("name");
                if  ( !String.IsNullOrEmpty( name ) )
                {
                    renderFieldDefn = new RenderFieldDefinition( name ) ;
                }
                else
                {
                    break ;   // cannot take a field without a name
                }

                if  ( renderFieldDefn.Itor( xPathPrefix1, renderField ) )
                {
                    RenderFields.Add( renderFieldDefn ) ;
                }
            }

            string xPathPrefix2 = string.Format("{0}[@name='{1}']/RenderTableAttribs", xPathPrefix, Name ) ;
            XmlNodeList renderTblAttrs = union.OwnerDocument.SelectNodes( xPathPrefix2 ) ;
            foreach( XmlElement tblAttrs in renderTblAttrs )
            {
                RenderTableAttribs = tblAttrs.InnerXml;
            }

            string xPathPrefix3 = string.Format("{0}[@name='{1}']/RenderTableHeader", xPathPrefix, Name ) ;
            XmlNodeList renderTblHdr = union.OwnerDocument.SelectNodes( xPathPrefix3 ) ;
            foreach( XmlElement tblHdr in renderTblHdr )
            {
                RenderTableHeader = tblHdr.InnerXml;
            }

            string xPathPrefix4 = string.Format("{0}[@name='{1}']/RenderTableRow", xPathPrefix, Name ) ;
            XmlNodeList renderTblRows = union.OwnerDocument.SelectNodes( xPathPrefix4 ) ;
            foreach( XmlElement tblRow in renderTblRows )
            {
                RenderTableRows.Add( tblRow.InnerXml ) ;
            }


            return( true ) ;
        }
    }
}
