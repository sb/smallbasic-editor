/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { ILayoutInterop } from "./JSInterop.Generated";

export class LayoutInterop implements ILayoutInterop {
    public async initializeWebView(locale: string, title: string): Promise<void> {
        document.documentElement.setAttribute("lang", locale);
        document.title = title;

        document.onkeydown = () => {
            // Focus on the text display if it exists
            const displays = document.getElementsByTagName("text-display");
            if (displays.length === 1) {
                const display = displays.item(0) as HTMLElement;
                display.focus();
            }
        };
    }

    public async openExternalLink(url: string): Promise<void> {
        window.open(url, "_blank");
    }

    public async getElementHeight(element: HTMLElement): Promise<number> {
        return element.getBoundingClientRect().height;
    }

    public async getElementWidth(element: HTMLElement): Promise<number> {
        return element.getBoundingClientRect().width;
    }

    public async scrollIntoView(element: HTMLElement): Promise<void> {
        element.scrollIntoView();
    }

    public async showMessage(text: string, title: string): Promise<void> {
        // second parameter will do nothing in web, but will display the title in electron
        (<any>alert)(text, title);
    }
}
