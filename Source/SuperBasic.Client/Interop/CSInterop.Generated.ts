/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

/// <summary>
/// This file is auto-generated by a build task. It shouldn't be edited by hand.
/// </summary>

// TODO: Definition of this file is not published yet to somewhere we can consume. Declare for now:
// https://github.com/aspnet/Blazor/issues/1452
// https://github.com/aspnet/Blazor/blob/0.5.1/src/Microsoft.JSInterop/JavaScriptRuntime/src/Microsoft.JSInterop.ts
export declare module DotNet {
    function invokeMethodAsync<T>(assemblyName: string, methodIdentifier: string, ...args: any[]): Promise<T>;
}

export module CSIntrop {
    export module Monaco {
        export function updateDiagnostics(code: string): Promise<monaco.IRange[]> {
            return DotNet.invokeMethodAsync<monaco.IRange[]>("SuperBasic.Editor", "CSIntrop.Monaco.UpdateDiagnostics", code);
        }

        export function provideCompletionItems(code: string, position: monaco.IPosition): Promise<monaco.languages.CompletionItem[]> {
            return DotNet.invokeMethodAsync<monaco.languages.CompletionItem[]>("SuperBasic.Editor", "CSIntrop.Monaco.ProvideCompletionItems", code, position);
        }

        export function provideHover(code: string, position: monaco.IPosition): Promise<string[]> {
            return DotNet.invokeMethodAsync<string[]>("SuperBasic.Editor", "CSIntrop.Monaco.ProvideHover", code, position);
        }
    }
}
