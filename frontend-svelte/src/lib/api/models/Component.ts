/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Label } from './Label';
import type { ObjectId } from './ObjectId';
import type { Translation } from './Translation';

export type Component = {
    id?: ObjectId;
    version?: number;
    projectId?: ObjectId;
    name?: string | null;
    createdAt?: string;
    updatedAt?: string;
    labels?: Array<Label> | null;
    translations?: Array<Translation> | null;
};