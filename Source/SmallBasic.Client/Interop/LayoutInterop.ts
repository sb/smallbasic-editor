/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { ILayoutInterop } from "./JSInterop.Generated";

export class LayoutInterop implements ILayoutInterop {
    public async initializeWebView(locale: string, title: string): Promise<void> {
        document.documentElement.setAttribute("lang", locale);
        document.title = title;
    }

    public async openExternalLink(url: string): Promise<void> {
        window.open(url, "_blank");
    }

    public async getElementHeight(element: HTMLElement | null): Promise<number> {
        if (element) {
            return element.getBoundingClientRect().height;
        }
        return 0;
    }

    public async getElementWidth(element: HTMLElement | null): Promise<number> {
        if (element) {
            return element.getBoundingClientRect().width;
        }
        return 0;
    }

    public async scrollIntoView(element: HTMLElement | null): Promise<void> {
        if (element) {
            element.scrollIntoView();
        }
    }

    public async focus(element: HTMLElement | null): Promise<void> {
        if (element) {
            element.focus();
        }
    }

    public async showMessage(text: string, title: string): Promise<void> {
        // second parameter will do nothing in web, but will display the title in electron
        (<any>alert)(text, title);
    }
}
