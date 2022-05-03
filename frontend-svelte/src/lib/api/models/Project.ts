/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Component } from './Component';
import type { ObjectId } from './ObjectId';
import type { Term } from './Term';

export type Project = {
    id?: ObjectId;
    version?: number;
    displayName?: string | null;
    name?: string | null;
    avatar?: string | null;
    description?: string | null;
    createdAt?: string;
    updatedAt?: string;
    terms?: Array<Term> | null;
    components?: Array<Component> | null;
    status?: number | null;
    accessLevel?: number | null;
    hide?: boolean | null;
};