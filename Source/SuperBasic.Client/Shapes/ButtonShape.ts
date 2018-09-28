// <copyright file="ButtonShape.ts" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

import * as Konva from "konva";
import { BaseShape, ShapeStyles } from "./BaseShape";
import { CSIntrop } from "../Interop/CSInteropTypes.Generated";

export class ButtonShape extends BaseShape<Konva.Label> {
    public constructor(caption: string, left: number, top: number) {
        const label = new Konva.Label({
            x: left,
            y: top,
            listening: true
        });

        label.add(new Konva.Tag({
            fill: ShapeStyles.defaultButtonColor,
            stroke: ShapeStyles.defaultBorderColor,
            strokeWidth: ShapeStyles.defaultBorderSize
        }));

        label.add(new Konva.Text({
            text: caption,
            fontFamily: ShapeStyles.defaultFontName,
            fontSize: ShapeStyles.defaultFontSize,
            padding: ShapeStyles.defaultPadding,
            fill: ShapeStyles.defaultFontColor
        }));

        label.on("click", () => {
            CSIntrop.Shapes.notifyButtonClicked(this.name);
        });

        label.on("mouseenter", () => {
            this.instance.getStage().container().style.cursor = "pointer";
            label.getTag().fill(ShapeStyles.hoverButtonColor);
        });

        label.on("mouseleave", () => {
            this.instance.getStage().container().style.cursor = "default";
            label.getTag().fill(ShapeStyles.defaultButtonColor);
        });

        super(label, "Button");
    }

    public get text(): string {
        return this.instance.getText().text();
    }

    public set text(value: string) {
        this.instance.getText().text(value);
    }
}
