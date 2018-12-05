/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { IControlsInterop } from "./JSInterop.Generated";
import { CSIntrop } from "./CSInterop.Generated";

export class ControlsInterop implements IControlsInterop {
    private graphicsDisplayElement: HTMLElement | null = null;

    public async initialize(graphicsDisplayElement: HTMLElement): Promise<void> {
        this.graphicsDisplayElement = graphicsDisplayElement;
    }

    public async addButton(controlName: string, caption: string, left: number, top: number): Promise<void> {
        const button = this.addElement(document.createElement("button"), controlName, left, top, 30, 80);
        button.textContent = caption;

        button.addEventListener("click", async () => {
            await CSIntrop.Controls.buttonClicked(controlName);
        });
    }

    public async addMultiLineTextBox(controlName: string, left: number, top: number): Promise<void> {
        const textarea = this.addElement(document.createElement("textarea"), controlName, left, top, 50, 200);

        textarea.addEventListener("input", async () => {
            await CSIntrop.Controls.textBoxTyped(controlName, textarea.value);
        });
    }

    public async addTextBox(controlName: string, left: number, top: number): Promise<void> {
        const input = this.addElement(document.createElement("input"), controlName, left, top, 20, 200);
        input.type = "text";

        input.addEventListener("input", async () => {
            await CSIntrop.Controls.textBoxTyped(controlName, input.value);
        });
    }

    public async hideControl(controlName: string): Promise<void> {
        document.getElementById(controlName)!.style.visibility = "hidden";
    }

    public async move(controlName: string, x: number, y: number): Promise<void> {
        const element = document.getElementById(controlName)!;
        element.style.top = y + "px";
        element.style.left = x + "px";
    }

    public async remove(controlName: string): Promise<void> {
        this.graphicsDisplayElement!.removeChild(document.getElementById(controlName)!);
    }

    public async setButtonCaption(controlName: string, caption: string): Promise<void> {
        document.getElementById(controlName)!.textContent = caption;
    }

    public async setSize(controlName: string, width: number, height: number): Promise<void> {
        const element = document.getElementById(controlName)!;
        element.style.width = width + "px";
        element.style.height = height + "px";
    }

    public async setTextBoxText(controlName: string, text: string): Promise<void> {
        document.getElementById(controlName)!.textContent = text;
    }

    public async showControl(controlName: string): Promise<void> {
        document.getElementById(controlName)!.style.visibility = "visible";
    }

    private addElement<TElement extends HTMLElement>(element: TElement, controlName: string, left: number, top: number, height: number, width: number): TElement {
        element.style.left = left + "px";
        element.style.top = top + "px";
        element.style.height = height + "px";
        element.style.width = width + "px";
        element.id = controlName;

        this.graphicsDisplayElement!.appendChild(element);
        return element;
    }
}
