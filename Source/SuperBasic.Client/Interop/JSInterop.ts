/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { IJSInterop } from "./Interop.Generated";
import { ScrollAreaContentsUtils } from "../Utility/ScrollAreaContentsUtils";

export class JSInterop implements IJSInterop {
    public initializeWebView(locale: string, title: string): void {
        document.documentElement.setAttribute("lang", locale);
        document.title = title;
    }

    public openExternalLink(url: string): void {
        window.open(url, "_blank");
    }

    public attachSideBarEvents(upButton: HTMLElement, scrollContentsArea: HTMLElement, downButton: HTMLElement): void {
        upButton.addEventListener("click", () => {
            ScrollAreaContentsUtils.scrollUp(scrollContentsArea, 200);
        });

        downButton.addEventListener("click", () => {
            ScrollAreaContentsUtils.scrollDown(scrollContentsArea, 200);
        });

        scrollContentsArea.addEventListener("wheel", event => {
            if (event.wheelDeltaY < 0) {
                ScrollAreaContentsUtils.scrollDown(scrollContentsArea, -event.wheelDeltaY);
            } else {
                ScrollAreaContentsUtils.scrollUp(scrollContentsArea, event.wheelDeltaY);
            }
        });
    }
}
