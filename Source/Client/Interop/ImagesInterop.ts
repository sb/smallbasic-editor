/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { IImagesInterop } from "./JSInteropTypes.Generated";

export class ImagesInterop implements IImagesInterop {
    public getImageUrl(_: string): Promise<string> {
        throw new Error("Method not implemented.");
    }
}
