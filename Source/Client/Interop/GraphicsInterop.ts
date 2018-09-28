/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as Konva from "konva";
import { IGraphicsInterop } from "./JSInteropTypes.Generated";
import { CSIntrop } from "./CSInteropTypes.Generated";
import { StageConfig } from "./StageConfig";

export class GraphicsInterop implements IGraphicsInterop {
    private layer: Konva.Layer | null = null;

    public initialize(containerId: string): Promise<void> {
        const stage = StageConfig.getStage(containerId);
        this.layer = new Konva.Layer({
            id: "graphics-layer-id"
        });

        stage.add(this.layer);

        const layerElement = document.getElementById(this.layer.id());
        if (!layerElement) {
            throw new Error("Cannot get layer element to set events on.");
        }

        layerElement.addEventListener("keydown", e => {
            CSIntrop.Graphics.notifyKeyDown(e.keyCode);
        });

        layerElement.addEventListener("keyup", e => {
            CSIntrop.Graphics.notifyKeyUp(e.keyCode);
        });

        layerElement.addEventListener("keypress", e => {
            CSIntrop.Graphics.notifyGraphicsWindowTextEntry(e.keyCode);
        });

        layerElement.addEventListener("mousedown", e => {
            const rect = layerElement.getBoundingClientRect();
            CSIntrop.Graphics.notifyMouseDown(e.clientX - rect.left, e.clientY - rect.top);
        });

        layerElement.addEventListener("mouseup", e => {
            const rect = layerElement.getBoundingClientRect();
            CSIntrop.Graphics.notifyMouseUp(e.clientX - rect.left, e.clientY - rect.top);
        });

        layerElement.addEventListener("mousemove", e => {
            const rect = layerElement.getBoundingClientRect();
            CSIntrop.Graphics.notifyMouseMove(e.clientX - rect.left, e.clientY - rect.top);
        });

        return Promise.resolve();
    }

    public dispose(): Promise<void> {
        if (this.layer) {
            this.clear();

            const layerElement = document.getElementById(this.layer.id());
            if (!layerElement) {
                throw new Error("Cannot get layer element to unbind events from.");
            }

            const cloned = layerElement.cloneNode(false);
            if (!layerElement.parentNode) {
                throw new Error(`Floating container for the graphics window. Cannot unbind events.`);
            }

            layerElement.parentNode.replaceChild(cloned, layerElement);
            this.layer.destroy();
        }

        return Promise.resolve();
    }

    public clear(): Promise<void> {
        if (!this.layer) {
            throw new Error("GraphicsInterop not initialized.");
        }

        this.layer.clear();
        return Promise.resolve();
    }

    public getBackgroundColor(): Promise<string> {
        return Promise.resolve(StageConfig.backgroundColor);
    }

    public setBackgroundColor(backgroundColor: string): Promise<void> {
        StageConfig.backgroundColor = backgroundColor;
        return Promise.resolve();
    }

    public getBrushColor(): Promise<string> {
        return Promise.resolve(StageConfig.brushColor);
    }

    public setBrushColor(brushColor: string): Promise<void> {
        StageConfig.brushColor = brushColor;
        return Promise.resolve();
    }

    public getPenColor(): Promise<string> {
        return Promise.resolve(StageConfig.penColor);
    }

    public setPenColor(penColor: string): Promise<void> {
        StageConfig.penColor = penColor;
        return Promise.resolve();
    }

    public getPenWidth(): Promise<number> {
        return Promise.resolve(StageConfig.penWidth);
    }

    public setPenWidth(penWidth: number): Promise<void> {
        StageConfig.penWidth = penWidth;
        return Promise.resolve();
    }

    public getFontBold(): Promise<boolean> {
        return Promise.resolve(StageConfig.fontBold);
    }

    public setFontBold(fontBold: boolean): Promise<void> {
        StageConfig.fontBold = fontBold;
        return Promise.resolve();
    }

    public getFontItalic(): Promise<boolean> {
        return Promise.resolve(StageConfig.fontItalic);
    }

    public setFontItalic(fontItalic: boolean): Promise<void> {
        StageConfig.fontItalic = fontItalic;
        return Promise.resolve();
    }

    public getFontName(): Promise<string> {
        return Promise.resolve(StageConfig.fontName);
    }

    public setFontName(fontName: string): Promise<void> {
        StageConfig.fontName = fontName;
        return Promise.resolve();
    }

    public getFontSize(): Promise<number> {
        return Promise.resolve(StageConfig.fontSize);
    }

    public setFontSize(fontSize: number): Promise<void> {
        StageConfig.fontSize = fontSize;
        return Promise.resolve();
    }

    public drawBoundText(x: number, y: number, width: number, text: string): Promise<void> {
        return this.addNode(new Konva.Text({
            x: x,
            y: y,
            width: width,
            text: text
        }));
    }

    public drawEllipse(x: number, y: number, width: number, height: number): Promise<void> {
        return this.addNode(new Konva.Ellipse({
            x: x,
            y: y,
            radius: {
                x: width,
                y: height
            }
        }));
    }

    public drawImage(imageName: string, x: number, y: number): Promise<void> {
        return this.tryCeateImage(imageName, x, y).then(imageOpt => {
            if (imageOpt) {
                return this.addNode(imageOpt);
            } else {
                return Promise.resolve();
            }
        });
    }

    public drawLine(x1: number, y1: number, x2: number, y2: number): Promise<void> {
        return this.addNode(new Konva.Line({
            points: [x1, y1, x2, y2]
        }));
    }

    public drawRectangle(x: number, y: number, width: number, height: number): Promise<void> {
        return this.addNode(new Konva.Rect({
            x: x,
            y: y,
            width: width,
            height: height
        }));
    }

    public drawResizedImage(imageName: string, x: number, y: number, width: number, height: number): Promise<void> {
        return this.tryCeateImage(imageName, x, y).then(imageOpt => {
            if (imageOpt) {
                imageOpt.width(width);
                imageOpt.height(height);
                return this.addNode(imageOpt);
            } else {
                return Promise.resolve();
            }
        });
    }

    public drawText(x: number, y: number, text: string): Promise<void> {
        return this.addNode(new Konva.Text({
            x: x,
            y: y,
            text: text
        }));
    }

    public drawTriangle(x1: number, y1: number, x2: number, y2: number, x3: number, y3: number): Promise<void> {
        return this.addNode(new Konva.Line({
            points: [x1, y1, x2, y2, x3, y3]
        }));
    }

    public fillEllipse(x: number, y: number, width: number, height: number): Promise<void> {
        return this.addNode(new Konva.Ellipse({
            x: x,
            y: y,
            radius: {
                x: width,
                y: height
            }
        }), true);
    }

    public fillRectangle(x: number, y: number, width: number, height: number): Promise<void> {
        return this.addNode(new Konva.Rect({
            x: x,
            y: y,
            width: width,
            height: height
        }), true);
    }

    public fillTriangle(x1: number, y1: number, x2: number, y2: number, x3: number, y3: number): Promise<void> {
        return this.addNode(new Konva.Line({
            points: [x1, y1, x2, y2, x3, y3]
        }), true);
    }

    public getPixel(x: number, y: number): Promise<string> {
        if (!this.layer) {
            throw new Error("GraphicsInterop not initialized.");
        }

        const color = this.layer.getCanvas().getContext().getImageData(x, y, 1, 1).data;
        return Promise.resolve(("#" + color[0].toString(16) + color[1].toString(16) + color[2].toString(16)).toUpperCase());
    }

    public setPixel(x: number, y: number, color: string): Promise<void> {
        if (!this.layer) {
            throw new Error("GraphicsInterop not initialized.");
        }

        const groups = /^#([0-9a-zA-Z]{2})([0-9a-zA-Z]{2})([0-9a-zA-Z]{2})$/.exec(color);
        if (!groups || groups.length !== 4) {
            throw new Error(`Trying to use an invalid color: '${color}'`);
        }

        const colorArray = new Uint8ClampedArray([parseInt(groups[1], 16), parseInt(groups[2], 16), parseInt(groups[3], 16)]);
        this.layer.getCanvas().getContext().putImageData(new ImageData(colorArray, 1, 1), x, y);
        return Promise.resolve();
    }

    public showMessage(text: string, _: string): Promise<void> {
        alert(text);
        // TODO: implement modal to show title (Graphics.ShowMessage());
        return Promise.resolve();
    }

    private tryCeateImage(imageName: string, x: number, y: number): Promise<Konva.Image | undefined> {
        return JSInterop.Images.getImageUrl(imageName).then(imageUrl => {
            if (!imageUrl) {
                return Promise.resolve(undefined);
            }

            return new Promise<Konva.Image>(resolve => {
                Konva.Image.fromURL(imageUrl, image => {
                    image.x(x);
                    image.y(y);
                    resolve(image);
                });
            });
        });
    }

    private addNode(node: Konva.Node, isFilling: boolean = false): Promise<void> {
        if (!this.layer) {
            throw new Error("GraphicsInterop not initialized.");
        }

        console.log("// TODO: should be filling: " + isFilling);

        this.layer.add(node);
        return Promise.resolve();
    }
}
