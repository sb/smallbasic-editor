/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as webpack from "webpack";
import * as path from "path";
import * as os from "os";

interface IEnvArguments {
    readonly configuration: string;
    readonly inputFile: string;
    readonly outputFile: string;
    readonly isBuildingForDesktop: string;
}

export default function (env: IEnvArguments): webpack.Configuration {
    const outputFile = path.parse(env.outputFile);
    const mode = getWebpackMode(env.configuration);
    const nativeApisFile = getNativeApisFile(env.isBuildingForDesktop);

    return {
        entry: env.inputFile,
        output: {
            path: outputFile.dir,
            filename: outputFile.base
        },
        target: "web",
        devtool: "source-map",
        module: {
            rules: [
                {
                    test: /\.ts$/,
                    loader: "tslint-loader",
                    enforce: "pre",
                    options: {
                        emitErrors: true
                    }
                },
                {
                    test: /\.ts?$/,
                    loader: "awesome-typescript-loader",
                    options: {
                        silent: true,
                        useCache: true,
                        cacheDirectory: path.join(os.tmpdir(), "atloader-cache")
                    }
                },
                {
                    test: /\.js?$/,
                    loader: "source-map-loader",
                    enforce: "pre"
                }
            ]
        },
        resolve: {
            extensions: [".ts", ".js"],
            alias: {
                NativeApis$: path.join(__dirname, "Native", nativeApisFile)
            }
        },
        stats: "errors-only",
        mode: mode
    };
}

function getWebpackMode(configuration: string): "development" | "production" {
    switch (configuration) {
        case "Debug": return "development";
        case "Release": return "production";
        default: throw new Error(`Configuration '${configuration}' is not supported`);
    }
}

function getNativeApisFile(isBuildingForDesktop: string): string {
    switch (isBuildingForDesktop) {
        case "true": return "NativeApis.Electron.ts";
        case "false": return "NativeApis.Web.ts";
        default: throw new Error(`isBuildingForDesktop value '${isBuildingForDesktop} is not supported;`);
    }
}
