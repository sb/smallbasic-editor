/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as Interfaces from "./NativeApis.Interfaces";

class NativeApis implements Interfaces.INativeApis {
    public readonly desktop: Interfaces.IDesktopApi = <any>null;
    public readonly file: Interfaces.IFileApi = <any>null;
}

export default NativeApis;
