/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Component } from './Component';
import type { Entry } from './Entry';

export type Translation = {
    id?: string;
    component?: Component;
    entries?: Array<Entry> | null;
};
