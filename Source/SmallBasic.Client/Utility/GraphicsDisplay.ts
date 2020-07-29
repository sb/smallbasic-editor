/*!
 * Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as $ from "jquery";
import { CSIntrop } from "../Interop/CSInterop.Generated";

let lastUpdatedX = 0;
let lastUpdatedY = 0;

$(window).resize(() => {
    const display = getGraphicsDisplay();
    if (display === null) {
        return;
    }

    const rect = display.getBoundingClientRect();
    if (rect.left !== lastUpdatedX || rect.top !== lastUpdatedY) {
        lastUpdatedX = rect.left;
        lastUpdatedY = rect.top;
        CSIntrop.GraphicsDisplay.updateDisplayLocation(lastUpdatedX, lastUpdatedY);
    }
});

const CHECK_INTERVAL = 250;

setInterval(() => {
    const display = getGraphicsDisplay();
    if (display === null) {
        return;
    }

    const stamp = "x-display-key-events-done";
    if (display.hasAttribute(stamp)) {
        return;
    }

    $(display).keyup(e => {
        CSIntrop.GraphicsDisplay.onKeyUp(e.key);
    }).keydown(e => {
        CSIntrop.GraphicsDisplay.onKeyDown(e.key);
    });

    display.setAttribute(stamp, "true");
}, CHECK_INTERVAL);

function getGraphicsDisplay(): Element | null {
    return document.getElementsByTagName("graphics-display").item(0);
}
