/**
 * $File: JCS_XML_Loader.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System.Xml;

namespace JCSUnity
{
    public class JCS_XML_Loader
    {
        private XmlDocument mXmlFile = null;

        public XmlDocument GetXmlFile() { return this.mXmlFile; }

        public JCS_XML_Loader(TextAsset file)
        {
            mXmlFile = new XmlDocument();
            mXmlFile.LoadXml(file.text);
        }

    }
}
