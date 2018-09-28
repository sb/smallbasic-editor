// <copyright file="ShapesInterop.ts" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

import * as Konva from "konva";
import { IShapesInterop } from "../Generated/JSInteropTypes.Generated";
import { BaseShape } from "./Shapes/BaseShape";
import { ButtonShape } from "./Shapes/ButtonShape";
import { TextBoxShape } from "./Shapes/TextBoxShape";

export class ShapesInterop implements IShapesInterop {
    private stage: Konva.Stage | undefined;
    private layer: Konva.Layer | undefined;

    private shapes: { [key: string]: BaseShape<Konva.Node> } = {};

    public initialize(elementId: string): Promise<boolean> {
        const element = document.getElementById(elementId);

        if (!element) {
            throw `Container element with id '${elementId} not found`;
        }

        this.stage = new Konva.Stage({
            container: element.id,
            width: element.offsetWidth,
            height: element.offsetHeight
        });

        this.layer = new Konva.Layer();
        this.stage.add(this.layer);

        this.shapes = {};

        return Promise.resolve(true);
    }

    public addButton(caption: string, left: number, top: number): Promise<string> {
        return this.addShape(new ButtonShape(caption, left, top));
    }

    public addTextBox(isMultiLine: boolean, left: number, top: number): Promise<string> {
        return this.addShape(new TextBoxShape(isMultiLine, left, top));
    }

    public getText(controlName: string): Promise<string> {
        const shape = this.shapes[controlName];
        return Promise.resolve(shape ? shape.text : "");
    }

    public hideControl(controlName: string): Promise<boolean> {
        const shape = this.shapes[controlName];
        if (shape) {
            shape.instance.hide();
        }

        return Promise.resolve(true);
    }

    public moveControl(controlName: string, x: number, y: number): Promise<boolean> {
        const shape = this.shapes[controlName];
        if (shape) {
            shape.instance.move(({
                x: x,
                y: y
            }));
        }

        return Promise.resolve(true);
    }

    public removeControl(controlName: string): Promise<boolean> {
        const shape = this.shapes[controlName];
        if (shape) {
            shape.instance.remove();
        }

        return Promise.resolve(true);
    }

    public setControlText(controlName: string, text: string): Promise<boolean> {
        const shape = this.shapes[controlName];
        if (shape) {
            shape.text = text;
        }

        return Promise.resolve(true);
    }

    public setControlSize(controlName: string, width: number, height: number): Promise<boolean> {
        const shape = this.shapes[controlName];
        if (shape) {
            shape.instance.setSize({
                width: width,
                height: height
            });
        }

        return Promise.resolve(true);
    }

    public showControl(controlName: string): Promise<boolean> {
        const shape = this.shapes[controlName];
        if (shape) {
            shape.instance.show();
        }

        return Promise.resolve(true);
    }

    private addShape(shape: BaseShape<Konva.Node>): Promise<string> {
        this.shapes[shape.name] = shape;

        if (!this.stage || !this.layer) {
            throw `ShapesInterop not initialized`;
        }

        this.layer.add(shape.instance);
        this.stage.draw();

        return Promise.resolve(shape.name);
    }
}
