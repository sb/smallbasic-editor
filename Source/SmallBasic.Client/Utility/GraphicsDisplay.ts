/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { CSIntrop } from "../Interop/CSInterop.Generated";

setInterval(checkForDisplay, 100);

let lastUpdatedX = 0;
let lastUpdatedY = 0;

function checkForDisplay(): void {
    const displays = document.getElementsByTagName("graphics-display");
    if (displays.length === 1) {
        const rect = displays.item(0).getBoundingClientRect();
        if (rect.left !== lastUpdatedX || rect.top !== lastUpdatedY) {
            lastUpdatedX = rect.left;
            lastUpdatedY = rect.top;
            CSIntrop.GraphicsDisplay.updateDisplayLocation(lastUpdatedX, lastUpdatedY);
        }
    }
}
