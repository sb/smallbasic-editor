/*!
 * Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
 */

export module ScrollAreaContentsUtils {
    const storeName = "_scrollValue";

    function getOffset(element: HTMLElement): number {
        const attribute = element.getAttribute(storeName);
        if (attribute) {
            return parseInt(attribute);
        }

        setOffset(element, 0);
        return 0;
    }

    function setOffset(element: HTMLElement, value: number): void {
        element.setAttribute(storeName, value.toString());
        element.style.marginTop = value + "px";
    }

    export function scrollUp(element: HTMLElement, scrollAmount: number): void {
        if (!element.parentElement) {
            throw new Error(`${element.nodeName} has no parent.`);
        }

        const currentOffset = getOffset(element);
        if (currentOffset === 0) {
            return;
        }

        const nextOffset = Math.min(0, currentOffset + scrollAmount);
        setOffset(element, nextOffset);
    }

    export function scrollDown(element: HTMLElement, scrollAmount: number): void {
        if (!element.parentElement) {
            throw new Error(`${element.nodeName} has no parent.`);
        }

        const currentOffset = getOffset(element);
        const elementRect = element.getBoundingClientRect();
        const parentRect = element.parentElement.getBoundingClientRect();

        const hiddenOffset = Math.max(0, elementRect.height + currentOffset - parentRect.height);
        if (hiddenOffset === 0) {
            return;
        }

        const toScroll = Math.min(hiddenOffset, scrollAmount);
        setOffset(element, currentOffset - toScroll);
    }
}
