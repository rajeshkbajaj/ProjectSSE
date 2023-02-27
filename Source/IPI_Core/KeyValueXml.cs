//*
//*  File:            KeyValueXml.cs
//*  Product:         Integrated Patient Intelligence
//*  Module:          InformaticsCore
//*
//*  Description:     Provides helper functions for getting and setting data
//*                   from an XML document.  Serves as base class.
//*
//*  $Author: bill.jordan2 $
//*  $Revision: #3 $
//*  $Date: 2012/06/26 $
//*
//*  Copyright:       (c) 2011 - 2012 Nellcor Puritan Bennett LLC.
//*                   This document contains proprietary information to Nellcor Puritan Bennett LLC.
//*                   Transmittal, receipt or possession of this document does not express, license,
//*                   or imply any rights to use, design or manufacture from this information.
//*                   No reproduction, publication, or disclosure of this information, in whole or in part,
//*                   shall be made without prior written authorization from Nellcor Puritan Bennett LLC.


using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace Covidien.Ipi.InformaticsCore
{
    public class KeyValueXml
    {
        /// <summary>
        /// syncronization object for thread safety.
        /// </summary>
        protected static object MSyncRoot = new Object();

        /// <summary>
        /// Parent node xpath string representation
        /// </summary>
        protected const string XPATH_PARENT_NODE = "configuration";

        /// <summary>
        /// Parent node xpath string format
        /// </summary>
        protected const string XPATH_SECTION_NAME_FORMAT = "//configuration//{0}";

        /// <summary>
        /// node xpath string format for setting
        /// </summary>
        protected const string XPATH_SETTING_NAME_FORMAT = "//configuration//{0}//{1}";

        /// <summary>
        /// node xpath string format for setting
        /// </summary>
        protected const string XPATH_SETTING_NAME_FULL_FORMAT = "//configuration//{0}";

        /// <summary>
        /// Child Section node name
        /// </summary>
        protected const string SECTION = "section";

        /// <summary>
        /// Section Setting XPath Name
        /// </summary>
        protected const string SECTION_SETTING_XPATH_NAME = "{0}//{1}";

        /// <summary>
        /// section name attribute
        /// </summary>
        protected const string SECTION_NAME_ATTRIBUTE = "name";

        /// <summary>
        /// Attribute name for key
        /// </summary>
        protected const string KEY_ATTRIBUTE = "key";

        /// <summary>
        /// Attribute name for value
        /// </summary>
        protected const string VALUE_ATTRIBUTE = "value";

        /// <summary>
        /// Alternate section delimitor
        /// </summary>
        protected const string ALT_DELIMITER = ".";

        /// <summary>
        /// XPath element delimitor
        /// </summary>
        protected const string XPATH_ELEMENT_DELIMITER = "//";

        /// <summary>
        /// path of the xml configuration file
        /// </summary>
        protected string mFilePath;

        /// <summary>
        /// Represents an Xml document
        /// </summary>
        protected XmlDocument mXmlDoc;

        /// <summary>
        /// We do not want to expose the constructor
        /// as we want to prevent anyone from creating
        /// the object without requesting the singleton.
        /// </summary>
        protected KeyValueXml(XmlDocument documentIn)
        {
            mXmlDoc = documentIn;
        }

        /// <summary>
        /// We do not want to expose the constructor
        /// as we want to prevent anyone from creating
        /// the object without requesting the singleton.
        /// </summary>
        protected KeyValueXml()
        {
        }
       
        /// <summary>
        /// This determines if a particular key exists.
        /// </summary>
        /// <param name="sectionName">Section Name that contains the key</param>
        /// <param name="key">Key to be found</param>
        /// <returns>A bool indicating if the key exists or not</returns>
        public bool DoesKeyExist(string sectionName, string key)
        {
            // get setting node
            string xpath = String.Format(XPATH_SETTING_NAME_FORMAT, sectionName, key);
            // look for the node
            XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);
            if (null == node)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// This determines if a particular section exists.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <returns>A bool indicating if the key exists or not</returns>
        public bool DoesSectionExist(string sectionName)
        {
            // get setting node
            string xpath = String.Format(XPATH_SECTION_NAME_FORMAT, sectionName);
            bool returnVal = true;
            try
            {
                // look for the node
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);
                if (null == node)
                {
                    returnVal = false;
                }
            }
            catch (XPathException)
            {
                string mLogMessage = string.Format("KeyValueXml::DoesSectionExist() - section: {0} does NOT exist\n", xpath ) ;
                ErrorTrace.Trace(IpiEventCode.InvalidProtocolType, mLogMessage, EventStatusType.Warning);

                returnVal = false;
            }
            return returnVal;
        }

        /// <summary>
        /// This method gets the values as an int.
        /// </summary>
        /// <param name="fullKeyName">Full key name for which the value is to be found</param>
        /// <returns>An int representation of the value that was queried</returns>
        public int GetValueAsInt(string fullKeyName)
        {
            string strValue = GetValue(fullKeyName);
            // No need to Assert for a null. It is valid to return a Null string.
            // If the key does not exist.
            try
            {
                return Convert.ToInt32(strValue);
            }
            catch(Exception exp)
            {
                IpiConfigurationEvaluationException exception = new IpiConfigurationEvaluationException("KeyValueXml - GetValueAsInt - Invalid Int Value", exp);
                throw exception;
            }
        }

        /// <summary>
        /// This method gets the values as a bool.
        /// </summary>
        /// <param name="fullKeyName">Full key name for which the value is to be found</param>
        /// <returns>A bool representation of the value that was queried</returns>
        public bool GetValueAsBool(string fullKeyName)
        {
            string strValue = GetValue(fullKeyName);

            try
            {
                return Convert.ToBoolean(strValue);
            }
            catch (Exception exp)
            {
                IpiConfigurationEvaluationException exception = new IpiConfigurationEvaluationException("KeyValueXml - GetValueAsBool - Invalid Bool Value", exp);
                throw exception;
            }
        }

        /// <summary>
        /// This method gets the values as a string.
        /// </summary>
        /// <param name="fullKeyName">Full name of the key</param>
        /// <returns>A String representation of the value that was queried</returns>
        public string GetValueAsString(string fullKeyName)
        {
            string strValue = GetValue(fullKeyName);
            return strValue;
        }

        /// <summary>
        /// This method gets the values as a list of strings.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <returns>A list of strings representing the values in the section</returns>
        public List<string> GetValuesAsStringList(string sectionName)
        {
            return GetAllValues(sectionName);
        }

        /// <summary>
        /// This method creates a section.
        /// </summary>
        /// <param name="fullSettingName">Full name of the setting for which the value has to be found</param>
        /// <returns>Returns a string representation of the value.</returns>
        public virtual string GetValue(string fullSettingName)
        {
            string valueRet = String.Empty;

            // translate . to // to support both formats
// RTB - have a . embedded tag (node name), commenting out to see if doing so eliminates my problem
// RTB - temp            string xpathSettingName = fullSettingName.Replace(ALT_DELIMITER, XPATH_ELEMENT_DELIMITER);
            string xpathSettingName = fullSettingName;

            // get setting node
            string xpath = String.Format(XPATH_SETTING_NAME_FULL_FORMAT, xpathSettingName);
            try
            {
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);

                // display value
                if (null != node)
                {
                    XmlAttribute xmlAttr = node.Attributes[VALUE_ATTRIBUTE];
                    if (null != xmlAttr)
                    {
                        return xmlAttr.Value;
                    }
                }
            }
            catch (XPathException)
            {
                // do noting node not found is a valid state - actually - let's document it.

                string mLogMessage = string.Format("KeyValueXml::GetValue(): string: {0} in path: {1}\n", fullSettingName, xpath ) ;
                ErrorTrace.Trace(IpiEventCode.InvalidProtocolType, mLogMessage, EventStatusType.Warning);
            }
            return null;
        }

        /// <summary>
        /// This section will return a list of child elements for a parent element.
        /// </summary>
        /// <param name="fullKeyName">Full Key name</param>
        /// <returns>Returns a string representation of the value.</returns>
        public Dictionary<string, string> GetAsList(string fullKeyName)
        {
            Dictionary<string, string> valueRet = new Dictionary<string,string>();

            // get setting node
            string xpath = String.Format(XPATH_SETTING_NAME_FULL_FORMAT, fullKeyName);
            try
            {
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);

                // display value
                if (null != node && node.HasChildNodes)
                {
                    XmlNode child = node.FirstChild;
                    while (null != child)
                    {
                        string nodeName = child.Name;
                        XmlAttribute xmlAttr = child.Attributes[VALUE_ATTRIBUTE];
                        if (null != xmlAttr)
                        {
                            valueRet.Add(nodeName, xmlAttr.Value);
                        }
                        child = child.NextSibling;
                    }
                }
            }
            catch (XPathException)
            {
                // Do nothing as section doesn't exist in XML - again, let's document it.

                string mLogMessage = string.Format("KeyValueXml::GetAsList(): string: {0} in path: {1}\n", fullKeyName, xpath);
                ErrorTrace.Trace(IpiEventCode.InvalidProtocolType, mLogMessage, EventStatusType.Warning);
            }
            return valueRet;
        }

        /// <summary>
        /// This section will return a list of child elements for a parent element.
        /// </summary>
        /// <param name="fullKeyName">Full Key name</param>
        /// <returns>Returns a string representation of the value.</returns>
        public Dictionary<string, string> GetAttribsAsList(string fullKeyName)
        {
            Dictionary<string, string> valueRet = new Dictionary<string, string>();

            // get setting node
            string xpath = String.Format(XPATH_SETTING_NAME_FULL_FORMAT, fullKeyName);
            try
            {
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);

                // display value
                if (null != node)
                {
                    XmlAttributeCollection attrs = node.Attributes;
                    foreach (XmlAttribute attr in attrs)
                    {
                        valueRet.Add(attr.Name, attr.Value);
                    }
                }

            }
            catch (XPathException)
            {
                // Do nothing as node doesn't exist in xml document

                string mLogMessage = string.Format("KeyValueXml::GetAttribsAsList(): string: {0} in path: {1}\n", fullKeyName, xpath);
                ErrorTrace.Trace(IpiEventCode.InvalidProtocolType, mLogMessage, EventStatusType.Warning);
            }
            return valueRet;
        }

        /// <summary>
        /// This method gets the child node names as a list of strings.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <returns>A list of strings representing the child node names in the section</returns>
        public List<string> GetAllChildNodeNames(string sectionName)
        {
            List<string> listOfNames;
            if (true == DoesSectionExist(sectionName))
            {
                // get setting node
                string xpath = String.Format(XPATH_SECTION_NAME_FORMAT, sectionName);
                // look for the node
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);
                XmlNodeList keysAndValues = node.ChildNodes;
                listOfNames = new List<string>( keysAndValues.Count + 1 );
                for (int i = 0; i < keysAndValues.Count; i++)
                {
                    XmlNode childNode = keysAndValues.Item(i);
                    if  ( null != childNode )
                    {
                        listOfNames.Add(childNode.Name);
                    }
                }
            }
            else
            {
                // IpiAssert.Fail(false, "KeyValueXml.cs - Failure in GetAllChildNodeNames");
                string logMessage = string.Format("KeyValueXml::GetAllChildNodeNames() - Section {0} does not exist", sectionName);
                ErrorTrace.Trace(IpiEventCode.InvalidSectionName, logMessage, EventStatusType.Warning);
                listOfNames = new List<string>();
            }
            return listOfNames;
        }

        /// <summary>
        /// This method gets the values as a list of strings.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <returns>A list of strings representing the values in the section</returns>
        public List<string> GetAllValues(string sectionName)
        {
            List<string> listOfValues ;
            if (true == DoesSectionExist(sectionName))
            {
                // get setting node
                string xpath = String.Format(XPATH_SECTION_NAME_FORMAT, sectionName);
                // look for the node
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);
                XmlNodeList keysAndValues = node.ChildNodes;
                listOfValues = new List<string>( keysAndValues.Count + 1 ) ;
                for (int i = 0; i < keysAndValues.Count; i++)
                {
                    XmlNode childNode = keysAndValues.Item(i);
                    if  ( null != childNode )
                    {
                        string fullKeyPath = string.Format( "{0}//{1}", sectionName, childNode.Name ) ;
                        string value = GetValueAsString(fullKeyPath);
                        listOfValues.Add(value);
                    }
                }
            }
            else
            {
               // IpiAssert.Fail(false, "KeyValueXml.cs - Failure in GetAllValues");
                string logMessage = string.Format("KeyValueXml::GetAllValues() - Section {0} does not exist", sectionName);
                ErrorTrace.Trace(IpiEventCode.InvalidSectionName, logMessage, EventStatusType.Warning);
                listOfValues = new List<string>();
            }
            return listOfValues;
        }

        /// <summary>
        /// This method gets the keys and values as a list of pairs
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <returns>A list of KeyValuePairs representing the keys and values in the section</returns>
        public List<KeyValuePair<string,string>> GetAllChildKeyValuePairs(string sectionName)
        {
            List<KeyValuePair<string, string>> listOfPairs ;
            if (true == DoesSectionExist(sectionName))
            {
                // get setting node
                string xpath = String.Format(XPATH_SECTION_NAME_FORMAT, sectionName);
                // look for the node
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);
                XmlNodeList keysAndValues = node.ChildNodes;
                listOfPairs = new List<KeyValuePair<string, string>>( keysAndValues.Count + 1 ) ;
                for (int i = 0; i < keysAndValues.Count; i++)
                {
                    XmlNode childNode = keysAndValues.Item(i);
                    if (null != childNode)
                    {
                        string fullKeyPath = string.Format("{0}//{1}", sectionName, childNode.Name);
                        KeyValuePair<string, string> newPair = new KeyValuePair<string, string>(childNode.Name, GetValueAsString(fullKeyPath));
                        listOfPairs.Add(newPair);
                    }
                }
            }
            else
            {
               // IpiAssert.Fail(false, "KeyValueXml.cs - Failure in GetAllKeyValuePairs");
                string logMessage = string.Format("KeyValueXml::GetAllChildKeyValuePairs() - Section {0} does not exist", sectionName);
                ErrorTrace.Trace(IpiEventCode.InvalidSectionName, logMessage, EventStatusType.Warning);
                listOfPairs = new List<KeyValuePair<string, string>>();
            }
            return listOfPairs;
        }

        /// <summary>
        /// This method creates a section.
        /// </summary>
        /// <param name="sectionName">Name of the section to be added</param>
        public void AddSection(string sectionName)
        {
            string resXPathName = sectionName.Replace(ALT_DELIMITER, XPATH_ELEMENT_DELIMITER);
            if (false == DoesSectionExist(resXPathName))
            {
                // get setting node
                string[] nodes = resXPathName.Split(XPATH_ELEMENT_DELIMITER.ToCharArray()[0]);
                XmlElement rootNode = mXmlDoc.DocumentElement;
                XmlNodeList currChildren = rootNode.ChildNodes;
                XmlNode currNode = null;
                foreach (string elementName in nodes)
                {
                    XmlNode foundNode = null;
                    if (!String.IsNullOrEmpty(elementName))
                    {
                        foreach (XmlNode child in currChildren)
                        {
                            if (child.Name.Equals(elementName))
                            {
                                foundNode = child;
                                break;
                            }
                        }
                        if (foundNode == null)
                        {
                            // Create child nodes for each section in the XPath
                            XmlNode newSub = mXmlDoc.CreateNode(XmlNodeType.Element, elementName, null);
                            if (null != newSub)
                            {
                                if (null == currNode)
                                {
                                    // Add new node to root node
                                    rootNode.AppendChild(newSub);
                                }
                                else
                                {
                                    currNode.AppendChild(newSub);
                                }
                                currNode = newSub;
                            }
                            else
                            {
                                IpiAssert.Fail(false, "KeyValueXml.cs - Failure in CreateSection");
                            }
                        }
                        else
                        {
                            currNode = foundNode;
                        }
                        currChildren = currNode.ChildNodes;
                    }
                }
            }
        }

        /// <summary>
        /// This method removes a section.
        /// </summary>
        /// <param name="sectionName">Name of the section to be deleted</param>
        public void DeleteSection(string sectionName)
        {
            if (true == DoesSectionExist(sectionName))
            {
                // get setting node
                string xpath = String.Format(XPATH_SECTION_NAME_FORMAT, sectionName);
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);
                lock (MSyncRoot)
                {
                    node.ParentNode.RemoveChild(node);
                }
            }
            else
            {
                //IpiAssert.Fail(false, "KeyValueXml.cs - Failure in RemoveSection");
                string logMessage = string.Format("KeyValueXml::DeleteSection() - Section {0} does not exist", sectionName);
                ErrorTrace.Trace(IpiEventCode.InvalidSectionName, logMessage, EventStatusType.Warning);

            }
        }

        /// <summary>
        /// This method Adds a key.
        /// </summary>
        /// <param name="sectionName">Section name where the key is supposed to be added</param>
        /// <param name="key">Key to be added</param>
        /// <param name="value">Value of the key to be added</param>
        public void AddKey(string sectionName, string key, string value)
        {
            if (false == DoesKeyExist(sectionName, key))
            {
                // get setting node
                string xpath = String.Format(XPATH_SECTION_NAME_FORMAT, sectionName);
                XmlNode node = null;
                try
                {
                    node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);
                }
                catch (XPathException)
                {
                    // Do nothing as section doesn't exist
                }
                if (null != node)
                {
                    XmlNode newSub = mXmlDoc.CreateNode(XmlNodeType.Element, key, null);
                    if (null != newSub)
                    {
                        XmlAttribute attributeValue = mXmlDoc.CreateAttribute(VALUE_ATTRIBUTE);
                        lock (MSyncRoot)
                        {
                            attributeValue.Value = value;
                            newSub.Attributes.Append(attributeValue);
                            node.AppendChild(newSub);
                        }
                    }
                    else
                    {
                        IpiAssert.Fail(false, "KeyValueXml.cs - Failure in CreateKey");
                     
                    }
                }
                else
                {
                    string logMessage = string.Format("KeyValueXml::AddKey() - Failed to create a key {0}. Section {1} not found", key, sectionName);
                    ErrorTrace.Trace(IpiEventCode.InvalidSectionName, logMessage, EventStatusType.Warning);
                    //IpiAssert.Fail(false, "KeyValueXml.cs - CreateKey Failed to create a key. Section not found");
                   
                }
            }
            else
            {
                //IpiAssert.Fail(false, "KeyValueXml.cs - Failure in CreateKey");
                string logMessage = string.Format("KeyValueXml::AddKey() - Key {0} to be added already exists - Section {1}", key, sectionName);
                ErrorTrace.Trace(IpiEventCode.InvalidKeyName, logMessage, EventStatusType.Warning);
            }
        }

        /// <summary>
        /// This method Updates a key.
        /// </summary>
        /// <param name="sectionName">Name of the section that contains key to be updated</param>
        /// <param name="key">Key to be updated</param>
        /// <param name="newValue">New value of the key</param>
        public void UpdateKey(string sectionName, string key, string newValue)
        {
            if (true == DoesKeyExist(sectionName, key))
            {
                // get setting node
                string xpath = String.Format(XPATH_SETTING_NAME_FORMAT, sectionName, key);
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);

                // display value
                if (null == node)
                {
                    //IpiAssert.Fail(false, "KeyValueXml.cs - UpdateValue - Failed to select the section name");
                    string logMessage = string.Format("KeyValueXml::UpdateKey() Failed to update a key {0}. Section {1} not found in path: {2}", key, sectionName, xpath);
                    ErrorTrace.Trace(IpiEventCode.InvalidSectionName, logMessage, EventStatusType.Warning);
                }
                else
                {
                    XmlAttribute xmlAttr = node.Attributes[VALUE_ATTRIBUTE];
                    if (null == xmlAttr)
                    {
                       // IpiAssert.Fail(false, "KeyValueXml.cs - Failure in UpdateValue");
                        string logMessage = string.Format("KeyValueXml::UpdateKey() Failed to update a key {0}. Section {1} not found in path: {2}, Attribute {3} not found", key, sectionName, xpath, VALUE_ATTRIBUTE);
                        ErrorTrace.Trace(IpiEventCode.InvalidAttributeName, logMessage, EventStatusType.Warning);
                    }
                    else
                    {
                        lock (MSyncRoot)
                        {
                            xmlAttr.Value = newValue;
                        }
                    }
                }
            }
            else
            {
                AddKey(sectionName, key, newValue);
            }
        }

        /// <summary>
        /// This method Deletes a key.
        /// </summary>
        /// <param name="sectionName">Name of the section that contains key to be deleted</param>
        /// <param name="key">Key to be deleted</param>
        public void DeleteKey(string sectionName, string key)
        {
            if (true == DoesKeyExist(sectionName, key))
            {
                // get setting node
                string xpath = String.Format(XPATH_SETTING_NAME_FORMAT, sectionName, key);
                XmlNode node = mXmlDoc.DocumentElement.SelectSingleNode(xpath);

                // display value
                if (null != node)
                {
                    // get setting node
                    string xpathSection = String.Format(XPATH_SECTION_NAME_FORMAT, sectionName);
                    lock (MSyncRoot)
                    {
                        mXmlDoc.SelectSingleNode(xpathSection).RemoveChild(node);
                    }
                }
                else
                {
                    IpiAssert.Fail(false, "KeyValueXml.cs - Failure in RemoveKey");
                }
            }
            else
            {
                //IpiAssert.Fail(false, "KeyValueXml.cs - Failure in RemoveKey");
                string logMessage = string.Format("KeyValueXml::DeleteKey() - Failed to delete a key. Key {0}//{1} not found", sectionName, key );
                ErrorTrace.Trace(IpiEventCode.InvalidKeyName, logMessage, EventStatusType.Warning);
            }
        }

        /// <summary>
        /// This method gets the values as a string. If the key and value do not exists
        /// it inserts the default value being passed in.
        /// </summary>
        /// <param name="fullKeyName">Full path of the key</param>
        /// <param name="defaultValue">The default value in case value is not found</param>
        /// <returns>A String representation of the value that was queried</returns>
        public string GetValueAsString(string fullKeyName, string defaultValue)
        {
            string strValue = GetValue(fullKeyName);
            if (null == strValue)
            {
                strValue = defaultValue;
                //IpiConfigurationEvaluationException exception = new IpiConfigurationEvaluationException("KeyValueXml - GetValueAsString - Invalid String Value");
                //throw exception;
                string logMessage = string.Format("KeyValueXml::GetValueAsString() - Invalid Value: {0}, {1}", fullKeyName, defaultValue);
                ErrorTrace.Trace(IpiEventCode.InvalidValue, logMessage, EventStatusType.Warning);
            }
            return strValue;
        }

        /// <summary>
        /// This method gets the values as an int.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="key">Name of the key</param>
        /// <returns>An int representation of the value that was queried</returns>
        public int GetAsInt(string sectionName, string key)
        {
            return GetValueAsInt(
                String.Format(SECTION_SETTING_XPATH_NAME, sectionName, key));
        }

        /// <summary>
        /// This method gets the values as an int. If the key and value do not exists
        /// it inserts the default value being passed in.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="key">Name of the key</param>
        /// <param name="defaultValue">Default value if the value for the key not found</param>
        /// <returns>An int representation of the value that was queried</returns>
        public int GetAsInt(string sectionName, string key, int defaultValue)
        {
            return GetValueAsInt(
                String.Format(SECTION_SETTING_XPATH_NAME, sectionName, key), defaultValue);
        }

        /// <summary>
        /// This method gets the values as a bool.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="key">Name of the key</param>
        /// <returns>A bool representation of the value that was queried</returns>
        public bool GetAsBool(string sectionName, string key)
        {
            return GetValueAsBool(
                String.Format(SECTION_SETTING_XPATH_NAME, sectionName, key));
        }

        /// <summary>
        /// This method gets the values as a bool.If the key and value do not exists
        /// it inserts the default value being passed in.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="key">Name of the key</param>
        /// <param name="defaultValue">Default value if the value for the key not found</param>
        /// <returns>A bool representation of the value that was queried</returns>
        public bool GetAsBool(string sectionName, string key, bool defaultValue)
        {
            return GetValueAsBool(
                String.Format(SECTION_SETTING_XPATH_NAME, sectionName, key),
                defaultValue);
        }

        /// <summary>
        /// This method gets the values as a string.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="key">Name of the key</param>
        /// <returns>A String representation of the value that was queried</returns>
        public string GetAsString(string sectionName, string key)
        {
            return GetValueAsString(
                String.Format(SECTION_SETTING_XPATH_NAME, sectionName, key));
        }

        /// <summary>
        /// This method gets the values as a string. If the key and value do not exists
        /// it inserts the default value being passed in.
        /// </summary>
        /// <param name="sectionName">Name of the section</param>
        /// <param name="key">Name of the key</param>
        /// <param name="defaultValue">Default value if the value for the key not found</param>
        /// <returns>A String representation of the value that was queried</returns>
        public string GetAsStringDefault(string sectionName, string key, string defaultValue)
        {
            return GetValueAsString(
                String.Format(SECTION_SETTING_XPATH_NAME, sectionName, key), defaultValue);
        }

        /// <summary>
        /// This method gets the values as an int.
        /// </summary>
        /// <param name="fullKeyName">Full name of the key</param>
        /// <returns>An int representation of the value that was queried</returns>
        public int GetAsInt(string fullKeyName)
        {
            return GetValueAsInt(fullKeyName);
        }

        /// <summary>
        /// This method gets the values as an int. If the key and value do not exists
        /// it inserts the default value being passed in.
        /// </summary>
        /// <param name="fullKeyName">Full name of the key</param>
        /// <param name="defaultValue">Default value if the value for the key not found</param>
        /// <returns>An int representation of the value that was queried</returns>
        public int GetAsInt(string fullKeyName, int defaultValue)
        {
            return GetValueAsInt(fullKeyName, defaultValue);
        }

        /// <summary>
        /// This method gets the values as a bool.
        /// </summary>
        /// <param name="fullKeyName">Full name of the key</param>
        /// <returns>A bool representation of the value that was queried</returns>
        public bool GetAsBool(string fullKeyName)
        {
            return GetValueAsBool(fullKeyName);
        }

        /// <summary>
        /// This method gets the values as a bool.If the key and value do not exists
        /// it inserts the default value being passed in.
        /// </summary>
        /// <param name="fullKeyName">Full name of the key</param>
        /// <param name="defaultValue">Default value if the value for the key not found</param>
        /// <returns>A bool representation of the value that was queried</returns>
        public bool GetAsBool(string fullKeyName, bool defaultValue)
        {
            return GetValueAsBool(fullKeyName, defaultValue);
        }

        /// <summary>
        /// This method gets the values as a string.
        /// </summary>
        /// <param name="fullKeyName">Full name of the key</param>
        /// <returns>A String representation of the value that was queried</returns>
        public string GetAsString(string fullKeyName)
        {
            return GetValueAsString(fullKeyName);
        }

        /// <summary>
        /// This method gets the values as a string. If the key and value do not exists
        /// it inserts the default value being passed in.
        /// </summary>
        /// <param name="fullKeyName">Full name of the key</param>
        /// <param name="defaultValue">Default value if the value for the key not found</param>
        /// <returns>A String representation of the value that was queried</returns>
        public string GetAsStringDefault(string fullKeyName, string defaultValue)
        {
            return GetValueAsString(fullKeyName, defaultValue);
        }

        /// <summary>
        /// This method gets the values as a list of strings.
        /// </summary>
        /// <param name="sectionName">Section name</param>
        /// <returns>A list of strings representing the values in the section</returns>
        public List<string> GetAsStringList(string sectionName)
        {
            return GetValuesAsStringList(sectionName);
        }

        /// <summary>
        /// This method gets the values as an int. If the key and value do not exists
        /// it inserts the default value being passed in.
        /// </summary>
        /// <param name="fullKeyName">Full name of the key</param>
        /// <param name="defaultValue">Default value if the value for the key not found</param>
        /// <returns>An int representation of the value that was queried</returns>
        public int GetValueAsInt(string fullKeyName, int defaultValue)
        {
            int returnValue = 0;
            string strValue = GetValue(fullKeyName);
            if (null == strValue)
            {
                returnValue = defaultValue;
                //IpiConfigurationEvaluationException exception = new IpiConfigurationEvaluationException("KeyValueXml - GetValueAsInt - Invalid Value");
                //throw exception;
                string logMessage = string.Format("KeyValueXml::GetValueAsInt() - Invalid Value {0}, {1}", fullKeyName, defaultValue);
                ErrorTrace.Trace(IpiEventCode.InvalidValue, logMessage, EventStatusType.Warning);
            }
            else
            {
                try
                {
                    returnValue = Convert.ToInt32(strValue);
                }
                catch (Exception exp)
                {
                    IpiConfigurationEvaluationException exception = new IpiConfigurationEvaluationException("KeyValueXml - GetValueAsInt - Invalid Int Value",exp);
                    throw exception;
                }
            }
            return returnValue;
        }

        /// <summary>
        /// This method gets the values as a bool.If the key and value do not exists
        /// it inserts the default value being passed in.
        /// </summary>
        /// <param name="fullKeyName">Full name of the key</param>
        /// <param name="defaultValue">Default value if value not found for the given key</param>
        /// <returns>A bool representation of the value that was queried</returns>
        public bool GetValueAsBool(string fullKeyName, bool defaultValue)
        {
            bool returnVal = false;
            string strValue = GetValue(fullKeyName);
            if (null == strValue)
            {
                returnVal = defaultValue;
                //IpiConfigurationEvaluationException exception = new IpiConfigurationEvaluationException("KeyValueXml - GetValueAsBool - Invalid Value");
                //throw exception;
                string logMessage = string.Format("KeyValueXml::GetValueAsBool() - Invalid Value: {0}, {1}", fullKeyName, defaultValue);
                ErrorTrace.Trace(IpiEventCode.InvalidValue, logMessage, EventStatusType.Warning);
            }
            else
            {
                try
                {
                    returnVal = Convert.ToBoolean(strValue);
                }
                catch (Exception exp)
                {
                    IpiConfigurationEvaluationException exception = new IpiConfigurationEvaluationException("KeyValueXml - GetValueAsBool - Invalid Bool Value", exp);
                    throw exception;
                }
            }
            return returnVal;
        }

        /// <summary>
        /// This method saves the data
        /// </summary>
        public virtual void Save()
        {
            mXmlDoc.Save(mFilePath);
        }
    }
}


