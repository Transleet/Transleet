/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { RegisterResource } from '../models/RegisterResource';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class AccountService {

    /**
     * Register a new account
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static register(
requestBody?: RegisterResource,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/account',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}