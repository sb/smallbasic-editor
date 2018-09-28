
/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { IControlsInterop } from "./JSInteropTypes.Generated";
import { CSIntrop } from "./CSInteropTypes.Generated";

export class ControlsInterop implements IControlsInterop {
    private container: HTMLElement | null = null;

    private buttonCounter: number = 0;
    private buttons: { [key: string]: HTMLButtonElement } = {};

    private textBoxCounter: number = 0;
    private textBoxes: { [key: string]: HTMLInputElement | HTMLTextAreaElement } = {};

    public initialize(containerId: string): Promise<void> {
        this.container = document.getElementById(containerId);

        if (!this.container) {
            throw new Error(`Container element with id '${containerId} not found`);
        }

        this.buttonCounter = 0;
        this.buttons = {};

        this.textBoxCounter = 0;
        this.textBoxes = {};

        return Promise.resolve();
    }

    public dispose(): Promise<void> {
        this.clear();

        this.container = null;
        this.buttonCounter = 0;
        this.textBoxCounter = 0;

        return Promise.resolve();
    }

    public addButton(caption: string, left: number, top: number): Promise<string> {
        if (!this.container) {
            throw new Error("ControlsInterop not initialized.");
        }

        const name = "Button" + (++this.buttonCounter);
        const containerRect = this.container.getBoundingClientRect();

        const button = document.createElement("button");
        button.textContent = caption;

        button.style.position = "absolute";
        button.style.top = (top + containerRect.top) + "px";
        button.style.left = (left + containerRect.left) + "px";

        this.buttons[name] = button;
        this.container.appendChild(button);

        button.addEventListener("click", () => {
            CSIntrop.Graphics.notifyButtonClicked(name);
        });

        button.addEventListener("mouseenter", () => {
            if (!this.container) {
                throw new Error("ControlsInterop not initialized.");
            }

            this.container.style.cursor = "pointer";
        });

        button.addEventListener("mouseleave", () => {
            if (!this.container) {
                throw new Error("ControlsInterop not initialized.");
            }

            this.container.style.cursor = "default";
        });

        return Promise.resolve(name);
    }

    public addTextBox(isMultiLine: boolean, left: number, top: number): Promise<string> {
        if (!this.container) {
            throw new Error("ControlsInterop not initialized.");
        }

        const name = "TextBox" + (++this.textBoxCounter);
        const boundingRect = this.container.getBoundingClientRect();

        let inputElement: HTMLInputElement | HTMLTextAreaElement;
        if (isMultiLine) {
            inputElement = <any>document.createElement("textarea");
        } else {
            inputElement = document.createElement("input");
            inputElement.type = "text";
        }

        inputElement.value = "";

        inputElement.style.position = "absolute";
        inputElement.style.top = (top + boundingRect.top) + "px";
        inputElement.style.left = (left + boundingRect.left) + "px";
        inputElement.style.width = "160px";

        this.textBoxes[name] = inputElement;
        this.container.appendChild(inputElement);

        inputElement.addEventListener("keydown", () => {
            CSIntrop.Graphics.notifyTextBoxControlEntry(name);
        });

        return Promise.resolve(name);
    }

    public getButtonCaption(buttonName: string): Promise<string> {
        const button = this.buttons[buttonName];
        return Promise.resolve(button ? (button.textContent || "") : "");
    }

    public getTextBoxText(textBoxName: string): Promise<string> {
        const textBox = this.textBoxes[textBoxName];
        return Promise.resolve(textBox ? textBox.value : "");
    }

    public hideControl(controlName: string): Promise<void> {
        const control = this.buttons[controlName] || this.textBoxes[controlName];
        if (control) {
            control.style.visibility = "hidden";
        }

        return Promise.resolve();
    }

    public moveControl(controlName: string, x: number, y: number): Promise<void> {
        if (!this.container) {
            throw new Error("ControlsInterop not initialized.");
        }

        const control = this.buttons[controlName] || this.textBoxes[controlName];
        if (control) {
            const containerRect = this.container.getBoundingClientRect();

            control.style.left = (x + containerRect.left) + "px";
            control.style.top = (y + containerRect.top) + "px";
        }

        return Promise.resolve();
    }

    public removeControl(controlName: string): Promise<void> {
        if (!this.container) {
            throw new Error("ControlsInterop not initialized.");
        }

        const control = this.buttons[controlName] || this.textBoxes[controlName];
        if (control) {
            if (control.parentElement) {
                control.parentElement.removeChild(control);
            }

            delete this.buttons[controlName];
            delete this.textBoxes[controlName];
        }

        return Promise.resolve();
    }

    public setButtonCaption(buttonName: string, caption: string): Promise<void> {
        const button = this.buttons[buttonName];
        if (button) {
            button.textContent = caption;
        }

        return Promise.resolve();
    }

    public setTextBoxText(textBoxName: string, text: string): Promise<void> {
        const textBox = this.textBoxes[textBoxName];
        if (textBox) {
            textBox.value = text;
        }

        return Promise.resolve();
    }

    public setSize(controlName: string, width: number, height: number): Promise<void> {
        const control = this.buttons[controlName] || this.textBoxes[controlName];
        if (control) {
            control.style.height = height + "px";
            control.style.width = width + "px";
        }

        return Promise.resolve();
    }

    public showControl(controlName: string): Promise<void> {
        const control = this.buttons[controlName] || this.textBoxes[controlName];
        if (control) {
            control.style.visibility = "visible";
        }

        return Promise.resolve();
    }

    public getHeight(): Promise<number> {
        if (!this.container) {
            throw new Error("GraphicsInterop not initialized");
        }

        return Promise.resolve(this.container.getBoundingClientRect().height);
    }

    public getWidth(): Promise<number> {
        if (!this.container) {
            throw new Error("GraphicsInterop not initialized");
        }

        return Promise.resolve(this.container.getBoundingClientRect().width);
    }

    public clear(): Promise<void> {
        Object.keys(this.buttons).forEach(this.removeControl);
        Object.keys(this.textBoxes).forEach(this.removeControl);
        return Promise.resolve();
    }
}
