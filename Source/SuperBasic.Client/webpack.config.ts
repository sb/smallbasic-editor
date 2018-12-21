/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as os from "os";
import * as path from "path";
import * as webpack from "webpack";
import * as HtmlWebpackPlugin from "html-webpack-plugin";
import * as MiniCssExtractPlugin from "mini-css-extract-plugin";
import * as CopyWebpackPlugin from "copy-webpack-plugin";

interface IEnvArguments {
    readonly watch: "True" | null;
    readonly isBuildingForDesktop: "True" | null;
    readonly configuration: "Debug" | "Release";
    readonly outputPath: string;
}

function createCommonConfig(params: {
    env: IEnvArguments,
    entry: webpack.Entry,
    target: "web" | "electron-main" | "electron-renderer",
    plugins?: webpack.Plugin[]
}): webpack.Configuration {
    return {
        entry: params.entry,
        output: {
            path: params.env.outputPath
        },
        watch: params.env.watch === "True",
        target: params.target,
        devtool: "source-map",
        watchOptions: {
            ignored: /node_modules/
        },
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
                },
                {
                    test: /\.(png|svg)$/,
                    use: "base64-inline-loader"
                },
                {
                    test: /\.(ttf|eot|woff(2)?)(\?[a-z0-9=&.]+)?$/,
                    use: {
                        loader: "file-loader",
                        options: {
                            name: "Fonts/[name].[ext]"
                        }
                    }
                },
                {
                    test: /\.(s?css)$/,
                    use: [
                        MiniCssExtractPlugin.loader, // instead of style-loader to inject it inline
                        { loader: "css-loader" },
                        {
                            loader: "postcss-loader",
                            options: {
                                plugins: () => {
                                    return [
                                        require("precss"),
                                        require("autoprefixer")
                                    ];
                                }
                            }
                        },
                        { loader: "sass-loader" },
                        { loader: "import-glob-loader" }
                    ]
                }
            ]
        },
        resolve: {
            extensions: [".ts", ".scss", ".js"]
        },
        stats: "errors-only",
        plugins: params.plugins,
        mode: params.env.configuration === "Release" ? "production" : "development",
        node: {
            __dirname: false,
            __filename: false
        }
    };
}

export default function (env: IEnvArguments): webpack.Configuration[] {
    const getEntry = (name: string) => path.join(__dirname, "Entries", name);

    const configs = [
        createCommonConfig({
            env: env,
            entry: {
                "monaco.editor": "@timkendrick/monaco-editor/dist/standalone/index.js",
                "SmallBasic.Interop": getEntry(env.isBuildingForDesktop === "True" ? "Renderer.ts" : "Web.ts")
            },
            target: env.isBuildingForDesktop === "True" ? "electron-renderer" : "web",
            plugins: [
                new MiniCssExtractPlugin({
                    filename: "SmallBasic.Interop.css"
                }),
                new HtmlWebpackPlugin({
                    template: path.join(__dirname, "Entries", "Template.html"),
                    favicon: path.join(__dirname, "Images", "favicon.ico"),
                    showErrors: true,
                    inject: false
                }),
                new CopyWebpackPlugin([
                    { from: "Images/Turtle.svg" }
                ])
            ]
        })
    ];

    if (env.isBuildingForDesktop === "True") {
        configs.push(createCommonConfig({
            env: env,
            entry: {
                "SmallBasic.Electron": getEntry("Electron.ts")
            },
            target: "electron-main"
        }));
    }

    return configs;
}
