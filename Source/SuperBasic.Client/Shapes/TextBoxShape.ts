// <copyright file="TextBoxShape.ts" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

import * as Konva from "konva";
import { BaseShape, ShapeStyles } from "./BaseShape";
import { CSIntrop } from "../Interop/CSInteropTypes.Generated";

export class TextBoxShape extends BaseShape<Konva.Text> {
    public constructor(isMultiLine: boolean, left: number, top: number) {
        const text = new Konva.Text({
            text: "",
            x: left,
            y: top,
            fontFamily: ShapeStyles.defaultFontName,
            fontSize: ShapeStyles.defaultFontSize,
            fill: ShapeStyles.defaultFontColor
        });

        text.on("click", () => {
            const position = text.getAbsolutePosition();
            const boundingRect = text.getStage().container().getBoundingClientRect();

            let inputElement: HTMLInputElement;
            if (isMultiLine) {
                inputElement = <any>document.createElement("textarea");
            } else {
                inputElement = document.createElement("input");
                inputElement.type = "text";
            }

            document.body.appendChild(inputElement);
            inputElement.value = text.text();

            inputElement.style.position = "absolute";
            inputElement.style.top = (position.y + boundingRect.top) + "px";
            inputElement.style.left = (position.x + boundingRect.left) + "px";
            inputElement.style.width = text.width() + "px";
            inputElement.style.height = text.height() + "px";
            inputElement.style.font = text.fontFamily();
            inputElement.style.fontSize = text.fontSize() + "px";
            inputElement.style.fill = text.fill();

            inputElement.focus();

            inputElement.addEventListener("keydown", () => {
                CSIntrop.Shapes.notifyTextTyped(this.name);
            });

            inputElement.addEventListener("focusout", () => {
                text.text(inputElement.value);
                document.removeChild(inputElement);
            });
        });

        super(text, "TextBox");
    }

    public get text(): string {
        return this.instance.text();
    }

    public set text(value: string) {
        this.instance.text(value);
    }
}
