/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

export interface IDesktopApi {
    getHeight(): Promise<number>;
    getWidth(): Promise<number>;
    setWallPaperFromFile(filePath: string): Promise<void>;
    setWallPaperFromUrl(url: string): Promise<void>;
}

export interface IFileApi {
    appendContents(filePath: string, contents: string): Promise<string>;
    copyFile(sourceFilePath: string, destinationFilePath: string): Promise<string>;
    createDirectory(directoryPath: string): Promise<string>;
    deleteDirectory(directoryPath: string): Promise<string>;
    deleteFile(filePath: string): Promise<string>;
    getDirectories(directoryPath: string): Promise<string[]>;
    getFiles(directoryPath: string): Promise<string[]>;
    getTemporaryFilePath(): Promise<string>;
    insertLine(filePath: string, lineNumber: number, contents: string): Promise<string>;
    readContents(filePath: string): Promise<string>;
    readLine(filePath: string, lineNumber: number): Promise<string>;
    writeContents(filePath: string, contents: string): Promise<string>;
    writeLine(filePath: string, lineNumber: number, contents: string): Promise<string>;
}

export interface INativeApis {
    readonly desktop: IDesktopApi;
    readonly file: IFileApi;
}
