/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Project } from './Project';
import type { User } from './User';

export type Organization = {
    id?: string;
    name?: string | null;
    projects?: Array<Project> | null;
    users?: Array<User> | null;
};