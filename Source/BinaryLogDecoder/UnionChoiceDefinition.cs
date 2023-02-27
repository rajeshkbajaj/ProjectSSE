using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Covidien.CGRS.BinaryLogDecoder
{
    public class UnionChoiceDefinition
    {
        public String Name { get; set; }
        public String CtrlField { get; set; }
        public int LowValue { get; set; }
        public int HighValue { get; set; }


        /// <summary>
        /// ctor()
        /// </summary>
        /// <param name="name">Name of the message</param>
        public UnionChoiceDefinition( string name )
        {
            Name = name;
        }


        public bool Itor( string xPathPrefix, XmlElement union )
        {
            int tmpInt;
            string attrStr;

            attrStr = union.GetAttribute("ctrlField");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                CtrlField = attrStr;
            }
            else
            {
                return (false);   // cannot take a field without a name
            }

            attrStr = union.GetAttribute("inRangeLow");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                if  ( Int32.TryParse( attrStr, out tmpInt ) )
                {
                    LowValue = tmpInt;
                }
                else
                {
                    return( false ) ;   // cannot take a field without a data order.
                }
            }

            attrStr = union.GetAttribute("inRangeHigh");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                if  ( Int32.TryParse( attrStr, out tmpInt ) )
                {
                    HighValue = tmpInt;
                }
                else
                {
                    return( false ) ;   // cannot take a field without a data order.
                }
            }

            return( true ) ;
        }
    }
}
