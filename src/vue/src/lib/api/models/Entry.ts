/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Locale } from './Locale';
import type { Translation } from './Translation';

export type Entry = {
    id?: string;
    text?: string | null;
    locale?: Locale;
    isSource?: boolean;
    isSuggestion?: boolean;
    translation?: Translation;
};
