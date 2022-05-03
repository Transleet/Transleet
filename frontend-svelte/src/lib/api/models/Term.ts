/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Entry } from './Entry';

export type Term = {
    key?: string | null;
    type?: string | null;
    from?: Entry;
    to?: Entry;
    labels?: Array<string> | null;
    description?: string | null;
    variants?: Array<string> | null;
};