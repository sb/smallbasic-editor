/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as fs from "fs";
import * as path from "path";
import * as os from "os";
import * as Interfaces from "./NativeApis.Interfaces";
import { CSIntrop } from "../Interop/CSInteropTypes.Generated";

const success = "SUCCESS";
const failed = "FAILED";
const linesRegex = /(\r\n|\r|\n)/;

export class FileApi implements Interfaces.IFileApi {
    public appendContents(filePath: string, contents: string): Promise<string> {
        return new Promise<string>(resolve => {
            fs.appendFile(filePath, contents, null, error => this.handleError(resolve, error));
        });
    }

    public copyFile(sourceFilePath: string, destinationFilePath: string): Promise<string> {
        return new Promise<string>(resolve => {
            fs.copyFile(sourceFilePath, destinationFilePath, error => this.handleError(resolve, error));
        });
    }

    public createDirectory(directoryPath: string): Promise<string> {
        return new Promise<string>(resolve => {
            fs.mkdir(directoryPath, error => this.handleError(resolve, error));
        });
    }

    public deleteDirectory(directoryPath: string): Promise<string> {
        return new Promise<string>(resolve => {
            fs.rmdir(directoryPath, error => this.handleError(resolve, error));
        });
    }

    public deleteFile(filePath: string): Promise<string> {
        return new Promise<string>(resolve => {
            fs.unlink(filePath, error => this.handleError(resolve, error));
        });
    }

    public getDirectories(directoryPath: string): Promise<string[]> {
        return this.listDirectory(directoryPath, "directories");
    }

    public getFiles(directoryPath: string): Promise<string[]> {
        return this.listDirectory(directoryPath, "files");
    }

    public getTemporaryFilePath(): Promise<string> {
        return Promise.resolve(path.join(os.tmpdir(), Math.random().toString()));
    }

    public insertLine(filePath: string, lineNumber: number, contents: string): Promise<string> {
        return this.writeLineToFile(filePath, lineNumber, contents, false);
    }

    public readContents(filePath: string): Promise<string> {
        return new Promise<string>(resolve => {
            fs.readFile(filePath, null, (error, data) => {
                if (error) {
                    this.handleError(resolve, error);
                } else {
                    resolve(data.toString());
                }
            });
        });
    }

    public readLine(filePath: string, lineNumber: number): Promise<string> {
        return new Promise<string>(resolve => {
            fs.readFile(filePath, (error, data) => {
                if (error) {
                    this.handleError(resolve, error);
                    return;
                }

                const lines = data.toString().split(linesRegex);
                resolve(lineNumber > 0 && lineNumber <= lines.length ? lines[lineNumber - 1] : "");
            });
        });
    }

    public writeContents(filePath: string, contents: string): Promise<string> {
        return new Promise<string>(resolve => {
            fs.writeFile(filePath, contents, error => this.handleError(resolve, error));
        });
    }

    public writeLine(filePath: string, lineNumber: number, contents: string): Promise<string> {
        return this.writeLineToFile(filePath, lineNumber, contents, true);
    }

    private writeLineToFile(filePath: string, lineNumber: number, contents: string, replace: boolean): Promise<string> {
        lineNumber = Math.max(lineNumber, 1);

        if (!fs.existsSync(filePath)) {
            const lines = new Array(lineNumber);
            lines.fill("", 0, lineNumber - 1);
            lines[lineNumber - 1] = contents;

            return this.writeContents(filePath, lines.join("\n"));
        }

        return new Promise<string>(resolve => {
            fs.readFile(filePath, (error, data) => {
                if (error) {
                    this.handleError(resolve, error);
                    return;
                }

                const lines = data.toString().split(linesRegex);
                lines.splice(lineNumber <= lines.length ? lineNumber - 1 : lines.length, replace ? 1 : 0, contents);
                fs.writeFile(filePath, lines.join("\n"), error => this.handleError(resolve, error));
            });
        });
    }

    private listDirectory(directoryPath: string, filter: "directories" | "files"): Promise<string[]> {
        return new Promise<string[]>(resolve => {
            fs.readdir(directoryPath, null, (error, files) => {
                if (error) {
                    CSIntrop.File.reportFileError(error.message);
                    resolve([]);
                } else {
                    resolve(files.map(name => path.join(directoryPath, name)).filter(file => {
                        const stats = fs.lstatSync(file);
                        switch (filter) {
                            case "directories": return stats.isDirectory();
                            case "files": return stats.isFile();
                        }
                    }));
                }
            });
        });
    }

    private handleError(resolve: (result: string) => void, error: NodeJS.ErrnoException): void {
        if (error) {
            CSIntrop.File.reportFileError(error.message);
            resolve(failed);
        } else {
            resolve(success);
        }
    }
}
