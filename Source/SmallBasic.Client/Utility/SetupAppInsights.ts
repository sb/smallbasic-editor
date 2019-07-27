/*!
 * Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { AppInsights } from "applicationinsights-js";

AppInsights.downloadAndSetup!({
    instrumentationKey: "4558db03-9e28-4a5a-9212-8a76bd8f6d1a"
});

AppInsights.trackPageView("PageLoad", window.location.toString());
