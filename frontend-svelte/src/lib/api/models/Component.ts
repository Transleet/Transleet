/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Label } from './Label';
import type { Translation } from './Translation';

export type Component = {
    id?: string;
    version?: number;
    name?: string | null;
    createdAt?: string;
    updatedAt?: string;
    labels?: Array<Label> | null;
    translations?: Array<Translation> | null;
};