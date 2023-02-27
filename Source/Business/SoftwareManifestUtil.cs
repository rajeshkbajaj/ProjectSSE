// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Covidien.CGRS.PcAgentInterfaceBusiness
{
    using Serilog;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    internal class SoftwareManifestUtil
    {
        private class XMLSAXParserState
        {
            public XMLSAXParserState()
            {
                element_name_ = null;
                attributes_ = new Dictionary<string, string>();
            }

            public string element_name_;

            public Dictionary<string, string> attributes_;
        };



        public static void MakeDirPP(string name)
        {
            Directory.CreateDirectory(name);
        }


        public static string Test()
        {

            //string folderName = "C:\\VikingTFTPRoot";

            //string sb = Archive(folderName);

            // RTB - byte [] ou ;//= Gzip(sb.ToString());

            //File.WriteAllBytes("C:\\Output.pre", Encoding.UTF8.GetBytes(sb));
            // File.WriteAllBytes("C:\\Output.gz", ou);

            byte[] ou = File.ReadAllBytes("C:\\Output.pre.gz");

            UnarchiveToFolder("C:\\VikingTFTPRoot2", Gunzip(ou));

            return ("test");
        }

        static public string Gunzip(byte[] rslt)
        {
            MemoryStream inStream = new MemoryStream(rslt);
            MemoryStream outStream = new MemoryStream();

            System.IO.Compression.GZipStream gz = new System.IO.Compression.GZipStream(inStream, System.IO.Compression.CompressionMode.Decompress, false);

            gz.CopyTo(outStream);

            gz.Close();

            return Encoding.UTF8.GetString(outStream.ToArray());
        }



        public static byte[] Gzip(string rslt)
        {
            MemoryStream inStream = new MemoryStream(Encoding.UTF8.GetBytes(rslt));
            MemoryStream outStream = new MemoryStream();


            System.IO.Compression.GZipStream gz = new System.IO.Compression.GZipStream(outStream, System.IO.Compression.CompressionMode.Compress, false);

            gz.Write(inStream.ToArray(), 0, (int)inStream.Length);

            gz.Close();

            return outStream.GetBuffer();
        }


        public static string Archive(string folderName)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<files>");
            foreach (string file in GetFiles(folderName))
            {
                byte[] bytes = File.ReadAllBytes(file);


                string[] strings = file.Split(new string[] { folderName }, StringSplitOptions.None);
                string o = Convert.ToBase64String(bytes);

                if (strings.Length == 2)
                {
                    sb.Append("<file name='" + strings[1] + "'>");
                    sb.Append(o);
                    sb.Append("</file>");
                }
            }
            sb.Append("</files>");



            return sb.ToString();
        }

        public static void UnarchiveToFolder(string root_folder, string archived_string)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(archived_string)))
            {
                Stack<XMLSAXParserState> state_stack = new Stack<XMLSAXParserState>();

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                XMLSAXParserState xs = new XMLSAXParserState
                                {
                                    element_name_ = reader.Name
                                };

                                state_stack.Push(xs);

                                if (reader.HasAttributes)
                                {
                                    reader.MoveToFirstAttribute();

                                    do
                                    {
                                        xs.attributes_.Add(reader.Name, reader.Value);
                                    } while (reader.MoveToNextAttribute());

                                    reader.MoveToElement();
                                }
                            }
                            break;

                        case XmlNodeType.Text:
                            {
                                XMLSAXParserState xs = state_stack.Peek();

                                if (xs.element_name_.Equals("file"))
                                {
                                    byte[] output;
                                    //will allow file names with ~ and spaces. they are valid file name characters
                                    string ret = Regex.Replace(xs.attributes_["name"], "[\\][\\][a-zA-Z\\._0-9\\-~ ]*$", string.Empty);
                                    MakeDirPP(root_folder + ret);

                                    output = Convert.FromBase64String(reader.Value);

                                    string fname = root_folder + xs.attributes_["name"];
                                    try
                                    {
                                        File.WriteAllBytes(fname, output);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error($"XMLSAXParserState::UnarchiveToFolder Ignoring Exception:{ex.Message}");
                                        // if we failed, ignore it.  or can we???
                                    }
                                }
                            }
                            break;

                        case XmlNodeType.EndElement:
                            {
                                state_stack.Pop();
                            }
                            break;

                    }
                }
            }
        }
        private static IEnumerable<string> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);

            while (queue.Count > 0)
            {
                path = queue.Dequeue();

                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"XMLSAXParserState::GetFiles Ignoring Exception1:{ex.Message}");

                    //Ignore
                }

                string[] files = null;

                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception ex)
                {
                    Log.Error($"XMLSAXParserState::GetFiles Ignoring Exception2:{ex.Message}");
                    //Ignore
                }

                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++) { yield return files[i]; }
                }
            }
        }
    }
}