using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Covidien.CGRS.BinaryLogDecoder
{
    public class UnionSetDefinition
    {
        public String Name { get; set; }

        public List<UnionChoiceDefinition> UnionChoices { get; set; }

        /// <summary>
        /// ctor()
        /// </summary>
        /// <param name="name">Name of the message</param>
        public UnionSetDefinition(string name)
        {
            Name = name;

            UnionChoices = new List<UnionChoiceDefinition>();
        }


        public bool Itor( string xPathPrefix, XmlElement union )
        {
            //int tmpInt;

            UnionChoiceDefinition unionChoiceDefn = null;
            xPathPrefix = string.Format( "{0}[@name='{1}']/unionChoice", xPathPrefix, Name ) ;
            XmlNodeList unionChoices = union.OwnerDocument.SelectNodes( xPathPrefix ) ;
            foreach( XmlElement unionChoice in unionChoices )
            {
                string attrStr = unionChoice.GetAttribute("name");
                if  ( !String.IsNullOrEmpty( attrStr ) )
                {
                    unionChoiceDefn = new UnionChoiceDefinition( attrStr );
                }
                else
                {
                    return( false ) ;   // cannot take a field without a name
                }

                if  ( unionChoiceDefn.Itor( xPathPrefix, unionChoice ) )
                {
                    UnionChoices.Add( unionChoiceDefn ) ;
                }
            }

            return( true ) ;
        }
    }
}
