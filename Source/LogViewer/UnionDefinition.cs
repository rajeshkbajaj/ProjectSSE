using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LogViewer
{
    public class UnionDefinition
    {
        public String Name { get; set; }

        public int NumBytes { get; set; }

        public List<FieldDefinition> Fields { get; set; }
        public List<RenderFieldDefinition> RenderFields { get; set; }
        public List<RenderExtraDefinition> RenderExtraFields { get; set; }

        /// <summary>
        /// ctor()
        /// </summary>
        /// <param name="name">Name of the message</param>
        public UnionDefinition( string name )
        {
            Name = name;

            Fields = new List<FieldDefinition>();
            RenderFields = new List<RenderFieldDefinition>();
            RenderExtraFields = new List<RenderExtraDefinition>();
        }


        public bool Itor( string xPathPrefix, XmlElement union )
        {
            int tmpInt;
            string attrStr;

            attrStr = union.GetAttribute("numBytes");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                if  ( Int32.TryParse( attrStr, out tmpInt ) )
                {
                    NumBytes = tmpInt;
                }
                else
                {
                    return( false ) ;
                }
            }
            else
            {
                return( false ) ;   // cannot take a field without a name
            }


            FieldDefinition fieldDefn = null;
            string xPathPrefix1 = string.Format( "{0}[@name='{1}']/field", xPathPrefix, Name ) ;
            XmlNodeList fields = union.OwnerDocument.SelectNodes( xPathPrefix1 ) ;
            foreach( XmlElement field in fields )
            {
                fieldDefn = new FieldDefinition();
                if  ( fieldDefn.Itor( xPathPrefix1, field ) )
                {
                    Fields.Add( fieldDefn ) ;
                }
            }

            RenderFieldDefinition renderFieldDefn = null;
            string xPathPrefix2 = string.Format("{0}[@name='{1}']/RenderField", xPathPrefix, Name ) ;
            XmlNodeList renderFields = union.OwnerDocument.SelectNodes( xPathPrefix2 ) ;
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

                if  ( renderFieldDefn.Itor( xPathPrefix2, renderField ) )
                {
                    RenderFields.Add( renderFieldDefn ) ;
                }
            }

            RenderExtraDefinition renderExtraFieldDefn = null;
            string xPathPrefix3 = string.Format("{0}[@name='{1}']/RenderExtra", xPathPrefix, Name ) ;
            XmlNodeList renderExtraFields = union.OwnerDocument.SelectNodes( xPathPrefix3 ) ;
            foreach( XmlElement renderExtraField in renderExtraFields )
            {
                string name = renderExtraField.GetAttribute("name");
                if  ( !String.IsNullOrEmpty( name ) )
                {
                    renderExtraFieldDefn = new RenderExtraDefinition( name ) ;
                }
                else
                {
                    break ;   // cannot take a field without a name
                }

                if  ( renderExtraFieldDefn.Itor( xPathPrefix3, renderExtraField ) )
                {
                    RenderExtraFields.Add( renderExtraFieldDefn ) ;
                }
            }


            return( true ) ;
        }
    }
}
