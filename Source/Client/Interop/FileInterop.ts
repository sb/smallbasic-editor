/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { IFileInterop } from "./JSInteropTypes.Generated";
import { INativeApis } from "../Native/NativeApis.Interfaces";

const NativeApis: INativeApis = require("NativeApis");

export class FileInterop implements IFileInterop {
    public appendContents(filePath: string, contents: string): Promise<string> {
        return NativeApis.file.appendContents(filePath, contents);
    }

    public copyFile(sourceFilePath: string, destinationFilePath: string): Promise<string> {
        return NativeApis.file.copyFile(sourceFilePath, destinationFilePath);
    }

    public createDirectory(directoryPath: string): Promise<string> {
        return NativeApis.file.createDirectory(directoryPath);
    }

    public deleteDirectory(directoryPath: string): Promise<string> {
        return NativeApis.file.deleteDirectory(directoryPath);
    }

    public deleteFile(filePath: string): Promise<string> {
        return NativeApis.file.deleteFile(filePath);
    }

    public getDirectories(directoryPath: string): Promise<string[]> {
        return NativeApis.file.getDirectories(directoryPath);
    }

    public getFiles(directoryPath: string): Promise<string[]> {
        return NativeApis.file.getFiles(directoryPath);
    }

    public getTemporaryFilePath(): Promise<string> {
        return NativeApis.file.getTemporaryFilePath();
    }

    public insertLine(filePath: string, lineNumber: number, contents: string): Promise<string> {
        return NativeApis.file.insertLine(filePath, lineNumber, contents);
    }

    public readContents(filePath: string): Promise<string> {
        return NativeApis.file.readContents(filePath);
    }

    public readLine(filePath: string, lineNumber: number): Promise<string> {
        return NativeApis.file.readLine(filePath, lineNumber);
    }

    public writeContents(filePath: string, contents: string): Promise<string> {
        return NativeApis.file.writeContents(filePath, contents);
    }

    public writeLine(filePath: string, lineNumber: number, contents: string): Promise<string> {
        return NativeApis.file.writeLine(filePath, lineNumber, contents);
    }
}
