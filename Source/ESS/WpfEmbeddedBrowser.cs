// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Covidien.CGRS.ESS
{
    using IdentityModel.OidcClient.Browser;
    using Serilog;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    public class WpfEmbeddedBrowser : IBrowser
    {
        private BrowserOptions _options = null;

        public WpfEmbeddedBrowser()
        {
        }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            _options = options;
            Log.Information($"WpfEmbeddedBrowser:InvokeAsync  Entry");
            var window = new Window()
            {
                Width =600,
                Height=590,
                Title = "ESS Authentication on Server",
                WindowStartupLocation=WindowStartupLocation.CenterScreen
            };

            // Note: Unfortunately, WebBrowser is very limited and does not give sufficient information for 
            //   robust error handling. The alternative is to use a system browser or third party embedded
            //   library (which tend to balloon the size of your application and are complicated).
            var webBrowser = new WebBrowser();

            var signal = new SemaphoreSlim(0, 1);

            var result = new BrowserResult()
            {
                ResultType = BrowserResultType.UserCancel
            };

            webBrowser.Navigating += (s, e) =>
            {
                if (BrowserIsNavigatingToRedirectUri(e.Uri))
                {
                    e.Cancel = true;

                    result = new BrowserResult()
                    {
                        ResultType = BrowserResultType.Success,
                        Response = e.Uri.AbsoluteUri
                    };

                    signal.Release();

                    window.Close();
                }
            };

            window.Closing += (s, e) =>
            {
                signal.Release();
            };

            window.Content = webBrowser;
            window.Show();
            webBrowser.Source = new Uri(_options.StartUrl);

            await signal.WaitAsync();
            Log.Information($"WpfEmbeddedBrowser:InvokeAsync  Exit");

            return result;
        }
        
        private bool BrowserIsNavigatingToRedirectUri(Uri uri)
        {
            Log.Information($"WpfEmbeddedBrowser:BrowserIsNavigatingToRedirectUri uri:{uri}");
            return uri.AbsoluteUri.StartsWith(_options.EndUrl);
        }
    }
}
