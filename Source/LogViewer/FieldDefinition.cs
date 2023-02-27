using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace LogViewer
{
    public class FieldDefinition
    {
        /// <summary>
        /// Identifier for the field, so can be referenced in a couple of places
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Order of field for proper sequencing as decoding a data (e.g., byte) stream.  This is needed because sometimes various xml viewers reorder the xml, usually just attrs though.
        /// </summary>
        public int DataOrder { get; set; }

        /// <summary>
        /// (Internal) Data Type
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// Raw/Original Data Type
        /// </summary>
        public string ActualDataType { get; set; }

        /// <summary>
        /// Bit fields may start partway into a byte - at a given offset.  ALWAYS count offset from left-most bit.
        /// NEED to revisit once get a couple more permutation examples for bit & byte ordering
        /// Intended plan would be to get all the data ordered sequentially, then offset should be clean
        /// VALID values 0..7 because, for now, will only treat input sequence as bytes
        /// </summary>
        public int BitOffset { get; set; }

        /// <summary>
        /// Bit field length - how many count
        /// VALID values 1..63.  MUST use appropriate BitManager read function
        /// </summary>
        public int BitLength { get; set; }

        /// <summary>
        /// Bit field length - how many count
        /// VALID values 1..63.  MUST use appropriate BitManager read function
        /// </summary>
        public int NumBytes { get; set; }

        /// <summary>
        /// Special decoding, such as a enum to string lookup, might be necessary given a value or even byte sequence
        /// </summary>
        public string DecodingInfo { get; set; }

        /// <summary>
        /// Indicates that field value should be considered as if it is the named other field and a new "row" of data be created, cloning other values
        /// The reason for this is to flatten messages (for search) where multiple codes (e.g. error codes) are packed into the same line.
        /// </summary>
        public string NewRowAsIfField { get; set; }

        /// <summary>
        /// When non-null indicates which union set to apply to this field
// Note to self - thinking of holding on to the actual memory "here", whatever that means.
        /// </summary>
        public string UnionSetName { get; set; }



        public FieldDefinition()
        {
            DataOrder = 0;
            DataType = null;
            ActualDataType = null;
            BitOffset = 0;
            BitLength = 0;
            NumBytes = 0;
            DecodingInfo = null;
            NewRowAsIfField = null;
            UnionSetName = null;
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

            attrStr = field.GetAttribute("dataOrder");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                if  ( Int32.TryParse( attrStr, out tmpInt ) )
                {
                    DataOrder = tmpInt ;
                }
                else
                {
                    return( false ) ;   // cannot take a field without a data order.
                }
            }

            attrStr = field.GetAttribute("bitOffset");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                if  ( Int32.TryParse( attrStr, out tmpInt ) )
                {
                    BitOffset = tmpInt ;
                }
            }

            attrStr = field.GetAttribute("bitLength");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                if  ( Int32.TryParse( attrStr, out tmpInt ) )
                {
                    BitLength = tmpInt ;
                }
            }

            attrStr = field.GetAttribute("numBytes");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                if  ( Int32.TryParse( attrStr, out tmpInt ) )
                {
                    NumBytes = tmpInt ;
                }
            }


            attrStr = field.GetAttribute("dataType");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                DataType = Type.GetType( attrStr ) ;
                if  ( null == DataType )
                {
                    return( false ) ;
                }
            }

            attrStr = field.GetAttribute("actualDataType");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                ActualDataType = attrStr ;
            }

            attrStr = field.GetAttribute("decodingInfo");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                DecodingInfo = attrStr ;
            }

            attrStr = field.GetAttribute("newRowAsIf");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                NewRowAsIfField = attrStr ;
            }

            attrStr = field.GetAttribute("isUnionSet");
            if  ( !String.IsNullOrEmpty( attrStr ) )
            {
                UnionSetName = attrStr ;
            }
            

            return( true ) ;
        }
    }
}
