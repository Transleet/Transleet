/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Entry } from '../models/Entry';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class EntriesService {

    /**
     * @param id 
     * @returns Entry Success
     * @throws ApiError
     */
    public static getApiEntries(
id: string,
): CancelablePromise<Entry> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/entries/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * @param id 
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiEntries(
id: string,
requestBody?: Entry,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/entries/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * @param id 
     * @returns any Success
     * @throws ApiError
     */
    public static deleteApiEntries(
id: string,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/entries/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static postApiEntries(
requestBody?: Entry,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/entries',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}