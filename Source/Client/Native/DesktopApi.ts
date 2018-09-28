/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as electron from "electron";
import * as Interfaces from "./NativeApis.Interfaces";
import * as wallpaper from "wallpaper";
import * as fs from "fs";
import * as request from "request";
import * as path from "path";
import * as os from "os";

export class DesktopApi implements Interfaces.IDesktopApi {
    public getHeight(): Promise<number> {
        return Promise.resolve(electron.screen.getPrimaryDisplay().workAreaSize.height);
    }

    public getWidth(): Promise<number> {
        return Promise.resolve(electron.screen.getPrimaryDisplay().workAreaSize.width);
    }

    public setWallPaperFromFile(filePath: string): Promise<void> {
        if (fs.existsSync(filePath)) {
            return wallpaper.set(filePath);
        } else {
            return Promise.resolve();
        }
    }

    public setWallPaperFromUrl(url: string): Promise<void> {
        return new Promise<void>(resolve => {
            const destination = path.join(os.tmpdir(), "smallbasic.wallpaper");
            request(url).pipe(fs.createWriteStream(destination)).on("end", () => {
                this.setWallPaperFromFile(destination).then(resolve);
            });
        });
    }
}
