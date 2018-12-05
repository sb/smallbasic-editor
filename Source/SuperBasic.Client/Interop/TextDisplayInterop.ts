/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { ITextDisplayInterop } from "./JSInterop.Generated";
import { CSIntrop } from "./CSInterop.Generated";

export class TextDisplayInterop implements ITextDisplayInterop {
    private textDisplay: HTMLElement | null = null;

    public async initialize(textDisplayElement: HTMLElement): Promise<void> {
        this.textDisplay = textDisplayElement;

        document.addEventListener("keydown", this.inputReceived);
    }

    public async dispose(): Promise<void> {
        document.removeEventListener("keydown", this.inputReceived);
    }

    public async scrollTo(element: HTMLElement): Promise<void> {
        element.scrollIntoView();
    }

    public async setBackgroundColor(hexColor: string): Promise<void> {
        this.textDisplay!.style.backgroundColor = hexColor;
    }

    private async inputReceived(event: KeyboardEvent): Promise<void> {
        await CSIntrop.TextDisplay.acceptInput(event.key);
    }
}
