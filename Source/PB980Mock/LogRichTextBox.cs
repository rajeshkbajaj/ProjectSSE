//----------------------------------------------------------------------------
//            Copyright (c) 2012 Covidien, Inc.
//
// This software is copyrighted by and is the sole property of Covidien. This
// is a proprietary work to which Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Covidien.
//----------------------------------------------------------------------------
using System;

namespace PB980Mock
{
    class LogRichTextBox:ILog
    {
        private readonly System.Windows.Forms.RichTextBox m_richtextbox;

        delegate void SetTextCallback(string text);

        private SetTextCallback mySetTextCallback;
        private SetTextCallback mySimpleSetTextCallback;

        public LogRichTextBox(System.Windows.Forms.RichTextBox richtextbox)
        {
            mySetTextCallback = new SetTextCallback(LogMessage2);
            mySimpleSetTextCallback = new SetTextCallback(LogMessage2Simple);

            m_richtextbox = richtextbox;
        }


        private void LogMessage2(string inStr)
        {
            DateTime today=DateTime.Now;
            string time = today.ToString();

            m_richtextbox.AppendText(time+":-"+inStr+"\n");
        }

        private void LogMessage2Simple(string inStr)
        {
            int length = DateTime.Now.ToString().Length;

            m_richtextbox.AppendText(new string(' ',length)+inStr + "\n");
        }


        public void LogMessage(string inStr)
        {
            try
            {
                m_richtextbox.Invoke(mySetTextCallback,inStr);
            }
            catch(Exception)
            {
                
            }
        }

        public void LogMessageSimple(string inStr)
        {
            try
            {
                m_richtextbox.Invoke(mySimpleSetTextCallback, inStr);
            }
            catch (Exception)
            {

            }
        }
    }
}