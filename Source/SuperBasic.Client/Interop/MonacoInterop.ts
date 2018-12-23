/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import { IMonacoInterop } from "./JSInterop.Generated";
import * as elementResizeEvent from "element-resize-event";
import * as FileSaver from "file-saver";
import { CSIntrop } from "./CSInterop.Generated";

export class MonacoInterop implements IMonacoInterop {
    private clipboard: string = "";
    private decorations: string[] = [];
    private outerContainer: HTMLElement | null = null;
    private activeEditor: monaco.editor.IStandaloneCodeEditor | null = null;

    public async initialize(editorElement: HTMLElement, initialValue: string, isReadOnly: boolean): Promise<void> {
        this.outerContainer = editorElement.parentElement && editorElement.parentElement.parentElement;
        if (!this.outerContainer) {
            throw new Error("Editor outer container not found");
        }

        this.activeEditor = monaco.editor.create(editorElement, {
            value: initialValue,
            language: "sb",
            scrollBeyondLastLine: false,
            autoIndent: true,
            readOnly: isReadOnly,
            fontFamily: "Consolas, monospace, Hack",
            fontSize: 18,
            glyphMargin: true,
            contextmenu: false,
            theme: "super-basic",
            minimap: {
                enabled: false
            }
        });

        this.setLayout();
        elementResizeEvent(this.outerContainer, this.setLayout.bind(this));

        this.activeEditor.onDidChangeModelContent(async () => {
            const code = this.activeEditor!.getModel()!.getValue();
            const ranges = await CSIntrop.Monaco.updateDiagnostics(code);
            this.decorations = this.activeEditor!.deltaDecorations(this.decorations, ranges.map(range => {
                return {
                    range: range,
                    options: {
                        className: "wavy-line",
                        glyphMarginClassName: "error-line-glyph"
                    }
                };
            }));
        });
    }

    public async dispose(): Promise<void> {
        if (this.activeEditor) {
            this.activeEditor.dispose();
        }
    }

    public async selectRange(range: monaco.IRange): Promise<void> {
        this.activeEditor!.setSelection(range);
        this.activeEditor!.revealLineInCenter(range.startLineNumber);
    }

    public async highlightLine(line: number): Promise<void> {
        this.decorations = this.activeEditor!.deltaDecorations(this.decorations, [{
            range: {
                startLineNumber: line,
                startColumn: 0,
                endLineNumber: line,
                endColumn: Number.MAX_SAFE_INTEGER
            },
            options: {
                className: "debugger-line-highlight"
            }
        }]);

        this.activeEditor!.revealLineInCenter(line);
    }

    public async removeDecorations(): Promise<void> {
        this.decorations = this.activeEditor!.deltaDecorations(this.decorations, []);
    }

    public async saveToFile(): Promise<void> {
        const code = this.activeEditor!.getModel().getValue();
        const blob = new Blob([code]);
        FileSaver.saveAs(blob, "Program.txt");
    }

    public async openFile(confirmationMessage: string): Promise<void> {
        if (!confirm(confirmationMessage)) {
            return;
        }

        const filePicker = document.createElement("input");
        filePicker.type = "file";
        filePicker.style.display = "none";

        filePicker.addEventListener("change", () => {
            if (!filePicker.files) {
                return;
            }

            for (let i = 0; i < filePicker.files.length; i++) {
                const file = filePicker.files.item(i);
                if (!file) {
                    continue;
                }

                const reader = new FileReader();
                reader.onloadend = () => {
                    this.activeEditor!.getModel().setValue(reader.result as string);
                };

                reader.readAsBinaryString(file);
            }

            filePicker.remove();
        });

        filePicker.click();
    }

    public async clearEditor(confirmationMessage: string): Promise<void> {
        if (!confirm(confirmationMessage)) {
            return;
        }

        this.activeEditor!.getModel().setValue("");
    }

    public async cut(): Promise<void> {
        const selection = this.activeEditor!.getSelection();
        this.clipboard = this.activeEditor!.getModel().getValueInRange(selection);

        this.activeEditor!.executeEdits("", [{
            identifier: { major: 1, minor: 1 },
            range: selection,
            text: "",
            forceMoveMarkers: true
        }]);
    }

    public async copy(): Promise<void> {
        const selection = this.activeEditor!.getSelection();
        this.clipboard = this.activeEditor!.getModel().getValueInRange(selection);
    }

    public async paste(): Promise<void> {
        this.activeEditor!.executeEdits("", [{
            identifier: { major: 1, minor: 1 },
            range: this.activeEditor!.getSelection(),
            text: this.clipboard,
            forceMoveMarkers: true
        }]);
    }

    public async undo(): Promise<void> {
        this.activeEditor!.trigger("", "undo", "");
    }

    public async redo(): Promise<void> {
        this.activeEditor!.trigger("", "redo", "");
    }

    private setLayout(): void {
        const rect = this.outerContainer!.getBoundingClientRect();
        this.activeEditor!.layout({
            // To account for container padding
            height: rect.height - 60,
            width: rect.width
        });
    }
}
