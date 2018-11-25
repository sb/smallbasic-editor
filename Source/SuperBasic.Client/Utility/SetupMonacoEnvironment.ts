/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

/// Do not actually import in TypeScript. This is bundled by webpack separately:
/// <reference path="../node_modules/monaco-editor/monaco.d.ts" />

import { CSIntrop } from "../Interop/CSInterop.Generated";

(<any>window).MonacoEnvironment = {
    getWorkerUrl: function (): string {
        return "./monaco/editor.worker.js";
    }
};

function createRange(start: string, stop: string): string[] {
    const result: string[] = [];
    for (let idx = start.charCodeAt(0), end = stop.charCodeAt(0); idx <= end; ++idx) {
        result.push(String.fromCharCode(idx));
    }
    return result;
}

monaco.languages.registerCompletionItemProvider("sb", {
    triggerCharacters: [
        ".",
        ...createRange("a", "z"),
        ...createRange("A", "Z")
    ],
    provideCompletionItems: (model: monaco.editor.IReadOnlyModel, position: monaco.IPosition): monaco.Thenable<monaco.languages.CompletionItem[]> => {
        return CSIntrop.Monaco.provideCompletionItems(model.getValue(), position);
    }
});

monaco.languages.registerHoverProvider("sb", {
    provideHover: (model: monaco.editor.IReadOnlyModel, position: monaco.IPosition): monaco.Thenable<monaco.languages.Hover> => {
        return CSIntrop.Monaco.provideHover(model.getValue(), position).then(lines => {
            return {
                range: <any>null,
                contents: lines.map(line => {
                    return {
                        language: <any>null,
                        value: line
                    };
                })
            };
        });
    }
});
