/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

export type Component = {
    id?: string;
    name?: string | null;
    type?: string | null;
    path?: string | null;
    createdAt?: string;
    updatedAt?: string;
    labels?: Array<string> | null;
    translations?: Array<string> | null;
};