/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Label } from './Label';

export type Term = {
    id?: string;
    type?: string | null;
    labels?: Array<Label> | null;
    description?: string | null;
};
