/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as Interfaces from "./NativeApis.Interfaces";
import { DesktopApi } from "./DesktopApi";
import { FileApi } from "./FileApi";

class NativeApis implements Interfaces.INativeApis {
    public readonly desktop: Interfaces.IDesktopApi = new DesktopApi();
    public readonly file: Interfaces.IFileApi = new FileApi();
}

export default NativeApis;
