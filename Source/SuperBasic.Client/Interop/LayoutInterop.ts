/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { ILayoutInterop } from "./JSInterop.Generated";
import { ScrollAreaContentsUtils } from "../Utility/ScrollAreaContentsUtils";

export class LayoutInterop implements ILayoutInterop {
    public async initializeWebView(locale: string, title: string): Promise<void> {
        document.documentElement.setAttribute("lang", locale);
        document.title = title;
    }

    public async openExternalLink(url: string): Promise<void> {
        window.open(url, "_blank");
    }

    public async attachSideBarEvents(upButton: HTMLElement, scrollContentsArea: HTMLElement, downButton: HTMLElement): Promise<void> {
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
            event.preventDefault();
        });
    }
}
