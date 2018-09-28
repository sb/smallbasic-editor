/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

import * as Konva from "konva";

// TODO: review all graphics, controls, and shapes, to have the right look and feel
export module StageConfig {
    export let fontName = "Calibri";
    export let fontSize = 16;
    export let fontItalic = false;
    export let fontBold = false;

    export let penWidth = 2;
    export let penColor = "#6A5ACD";

    export let brushColor = "#000000";
    export let backgroundColor = "#FFFFFF";

    export const buttonColor = "#DDDDDD";
    export const buttonHoverColor = "#BEE6FD";

    export const controlBorderSize = 1;
    export const controlBorderColor = "#000000";
    export const controlBorderPadding = 5;

    let stage: Konva.Stage | undefined;
    export function getStage(elementId: string): Konva.Stage {
        if (!stage) {
            const element = document.getElementById(elementId);

            if (!element) {
                throw new Error(`Container element with id '${elementId} not found`);
            }

            stage = new Konva.Stage({
                container: element.id,
                width: element.offsetWidth,
                height: element.offsetHeight,
                listening: true
            });
        }

        return stage;
    }
}
