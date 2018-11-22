/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

/// <reference path="../node_modules/monaco-editor/monaco.d.ts" />

import { IMonacoInterop } from "./JSInterop.Generated";
import * as elementResizeEvent from "element-resize-event";
import { CSIntrop } from "./CSInterop.Generated";

export class MonacoInterop implements IMonacoInterop {
    private outerContainer: HTMLElement | null = null;
    private editorInstance: monaco.editor.IStandaloneCodeEditor | null = null;

    public initialize(editorElement: HTMLElement, initialValue: string, isReadOnly: boolean): void {
        this.outerContainer = editorElement.parentElement && editorElement.parentElement.parentElement;
        if (!this.outerContainer) {
            throw new Error("Editor outer container not found");
        }

        this.editorInstance = monaco.editor.create(editorElement, {
            value: initialValue,
            language: "sb",
            scrollBeyondLastLine: false,
            readOnly: isReadOnly,
            fontFamily: "Consolas, monospace, Hack",
            fontSize: 18,
            minimap: {
                enabled: false
            }
        });

        this.setLayout();
        elementResizeEvent(this.outerContainer, this.setLayout.bind(this));
    }

    private setLayout(): void {
        if (!this.editorInstance) {
            throw new Error("Resizing non-existent editor.");
        } else if (!this.outerContainer) {
            throw new Error("Editor container not found.");
        }

        const rect = this.outerContainer.getBoundingClientRect();
        this.editorInstance.layout({
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
    provideCompletionItems: (model: monaco.editor.IReadOnlyModel, position: monaco.Position): monaco.languages.ProviderResult<monaco.languages.CompletionList> => {
        // TODO: Issue with monaco typing. This actually expects a CompletionItem[] not a CompletionList. Cast to <any> for now.
        return <any>CSIntrop.Monaco.provideCompletionItems(model.getValue(), position.lineNumber, position.column);
    }
});

monaco.languages.registerHoverProvider("sb", {
    provideHover: (model: monaco.editor.IReadOnlyModel, position: monaco.Position): monaco.languages.ProviderResult<monaco.languages.Hover> => {
        // TODO: Issue with monaco typing. It accepts string[], but types specify MarkdownString[] only. Cast to <any> for now.
        return CSIntrop.Monaco.provideHover(model.getValue(), position.lineNumber, position.column).then(lines => {
            return {
                contents: <any>lines
            };
        });
    }
});
