/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as webpack from "webpack";
import * as path from "path";
import * as os from "os";

interface IEnvArguments {
    readonly configuration: "Debug" | "Release";
    readonly outputFile: string;
    readonly target: "web" | "electron-renderer" | "electron-main";
}

export default function (env: IEnvArguments): webpack.Configuration {
    const outputFile = path.parse(env.outputFile);
    const mode = getWebpackMode(env);

    return {
        entry: path.join(__dirname, "Entries", env.target + ".ts"),
        output: {
            path: outputFile.dir,
            filename: outputFile.base
        },
        target: env.target,
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
            extensions: [".ts", ".js"]
        },
        stats: "errors-only",
        mode: mode,
        node: {
            __dirname: false
        }
    };
}

function getWebpackMode(env: IEnvArguments): "development" | "production" {
    switch (env.configuration) {
        case "Debug": return "development";
        case "Release": return "production";
        default: throw new Error(`Configuration '${env.configuration}' is not supported`);
    }
}
