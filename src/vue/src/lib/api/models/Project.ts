/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

export type Project = {
    id?: string;
    displayName?: string | null;
    name?: string | null;
    avatar?: string | null;
    description?: string | null;
    createdAt?: string;
    updatedAt?: string;
    terms?: Array<string> | null;
    components?: Array<string> | null;
    status?: number | null;
    accessLevel?: number | null;
    hide?: boolean | null;
};