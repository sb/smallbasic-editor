// <copyright file="BaseShape.ts" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

import * as Konva from "konva";

export module ShapeStyles {
    // TODO: review all styles to match the existing one, and make sure textbox transition is seamless  

    export const defaultFontName = "Calibri";
    export const defaultFontSize = 16;
    export const defaultFontColor = "slateblue";

    export const defaultButtonColor = "rgb(221, 221, 221)";
    export const hoverButtonColor = "rgb(190, 230, 253)";

    export const defaultBorderSize = 1;
    export const defaultBorderColor = "black";
    export const defaultPadding = 5;
}

export abstract class BaseShape<TKonva extends Konva.Node> {
    public readonly name: string;
    public readonly instance: TKonva;

    private static readonly shapeIdCounters: { [key: string]: number } = {};

    public constructor(instance: TKonva, shapeKey: string) {
        const counter = (BaseShape.shapeIdCounters[shapeKey] || 0) + 1;
        BaseShape.shapeIdCounters[shapeKey] = counter;

        this.name = shapeKey + counter;
        this.instance = instance;
    }

    public abstract get text(): string;
    public abstract set text(value: string);
}
