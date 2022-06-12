/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { LoginResource } from '../models/LoginResource';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class AuthorizationService {

    /**
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static getToken(
requestBody?: LoginResource,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/authorize/token',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}
