/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

/// <reference path="../node_modules/monaco-editor/monaco.d.ts" />

import { IMonacoInterop } from "./Interop.Generated";
import * as elementResizeEvent from "element-resize-event";

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
