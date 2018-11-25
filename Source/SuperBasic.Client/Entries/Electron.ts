/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { app, BrowserWindow } from "electron";

let window: BrowserWindow | null = null;

function createWindow(): void {
    window = new BrowserWindow({
        width: 800,
        height: 600
    });

    window.setMenu(null);
    window.loadFile("index.html");

    window.on("closed", () => {
        window = null;
    });
}

app.on("ready", createWindow);

app.on("window-all-closed", () => {
    if (process.platform !== "darwin") {
        app.quit();
    }
});

app.on("activate", () => {
    if (window === null) {
        createWindow();
    }
});
