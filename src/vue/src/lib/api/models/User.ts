/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

import type { Organization } from './Organization';

export type User = {
    id?: string | null;
    userName?: string | null;
    normalizedUserName?: string | null;
    email?: string | null;
    normalizedEmail?: string | null;
    emailConfirmed?: boolean;
    passwordHash?: string | null;
    securityStamp?: string | null;
    concurrencyStamp?: string | null;
    phoneNumber?: string | null;
    phoneNumberConfirmed?: boolean;
    twoFactorEnabled?: boolean;
    lockoutEnd?: string | null;
    lockoutEnabled?: boolean;
    accessFailedCount?: number;
    organizations?: Array<Organization> | null;
    avatarUrl?: string | null;
};
