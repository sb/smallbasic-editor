/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { IDesktopInterop } from "./JSInteropTypes.Generated";
import { INativeApis } from "../Native/NativeApis.Interfaces";

const NativeApis: INativeApis = require("NativeApis");

export class DesktopInterop implements IDesktopInterop {
    public getHeight(): Promise<number> {
        return NativeApis.desktop.getHeight();
    }

    public getWidth(): Promise<number> {
        return NativeApis.desktop.getWidth();
    }

    public setWallPaperFromFile(filePath: string): Promise<void> {
        return NativeApis.desktop.setWallPaperFromFile(filePath);
    }

    public setWallPaperFromUrl(url: string): Promise<void> {
        return NativeApis.desktop.setWallPaperFromUrl(url);
    }
}
