// <copyright file="webpack.config.ts" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

import * as webpack from "webpack";
import * as path from "path";
import * as os from "os";

interface IEnvArguments {
    readonly configuration: "Debug" | "Release";
    readonly inputFile: string;
    readonly outputFile: string;
}

export default function (env: IEnvArguments): webpack.Configuration {
    const outputFile = path.parse(env.outputFile);
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
            extensions: [".ts", ".js"]
        },
        stats: "errors-only",
        mode: configurationToMode(env.configuration)
    };
}

function configurationToMode(configuration: string): "development" | "production" {
    switch (configuration) {
        case "Debug": return "development";
        case "Release": return "production";
        default: throw `Configuration '${configuration}' is not supported`;
    }
}
