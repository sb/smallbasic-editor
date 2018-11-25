/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

/// <reference path="../node_modules/monaco-editor/monaco.d.ts" />

import { IMonacoInterop } from "./JSInterop.Generated";
import * as elementResizeEvent from "element-resize-event";
import { CSIntrop } from "./CSInterop.Generated";

let idCounter = 1;
const activeEditors: { [id: string]: monaco.editor.IStandaloneCodeEditor } = {};

export class MonacoInterop implements IMonacoInterop {
    public initialize(editorElement: HTMLElement, initialValue: string, isReadOnly: boolean): string {
        Object.keys(activeEditors).forEach(id => {
            if (!document.getElementById(id)) {
                activeEditors[id].dispose();
                delete activeEditors[id];
            }
        });

        const outerContainer = editorElement.parentElement && editorElement.parentElement.parentElement;
        if (!outerContainer) {
            throw new Error("Editor outer container not found");
        }

        const editorInstance = monaco.editor.create(editorElement, {
            value: initialValue,
            language: "sb",
            scrollBeyondLastLine: false,
            readOnly: isReadOnly,
            fontFamily: "Consolas, monospace, Hack",
            fontSize: 18,
            glyphMargin: true,
            minimap: {
                enabled: false
            }
        });

        this.setLayout(editorInstance, outerContainer);
        elementResizeEvent(outerContainer, this.setLayout.bind(this, editorInstance, outerContainer));

        editorElement.id = "monaco_" + (idCounter++);
        let decorations: string[] = [];

        editorInstance.onDidChangeModelContent(() => {
            CSIntrop.Monaco.onChange(editorElement.id, editorInstance.getModel()!.getValue()).then(ranges => {
                decorations = editorInstance.deltaDecorations(decorations, ranges.map(range => {
                    return {
                        range: range,
                        options: {
                            className: "wavy-line",
                            glyphMarginClassName: "error-line-glyph"
                        }
                    };
                }));
            });
        });

        activeEditors[editorElement.id] = editorInstance;
        return editorElement.id;
    }

    public selectRange(id: string, range: monaco.IRange): void {
        activeEditors[id].setSelection(range);
    }

    private setLayout(editorInstance: monaco.editor.IStandaloneCodeEditor, outerContainer: HTMLElement): void {
        const rect = outerContainer.getBoundingClientRect();
        editorInstance.layout({
            // To account for container padding
            height: rect.height - 60,
            width: rect.width
        });
    }
}

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
    provideCompletionItems: (model: monaco.editor.IReadOnlyModel, position: monaco.IPosition): monaco.languages.ProviderResult<monaco.languages.CompletionList> => {
        // TODO: Issue with monaco typing. This actually expects a CompletionItem[] not a CompletionList. Cast to <any> for now.
        return <any>CSIntrop.Monaco.provideCompletionItems(model.getValue(), position);
    }
});

monaco.languages.registerHoverProvider("sb", {
    provideHover: (model: monaco.editor.IReadOnlyModel, position: monaco.IPosition): monaco.languages.ProviderResult<monaco.languages.Hover> => {
        // TODO: Issue with monaco typing. It accepts string[], but types specify MarkdownString[] only. Cast to <any> for now.
        return CSIntrop.Monaco.provideHover(model.getValue(), position).then(lines => {
            return {
                contents: <any>lines
            };
        });
    }
});
