/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Translation } from '../models/Translation';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class TranslationsService {

    /**
     * Get translation.
     * @param id 
     * @returns Translation Success
     * @throws ApiError
     */
    public static getTranslation(
id: string,
): CancelablePromise<Translation> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/translations/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * Update translation.
     * @param id 
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static updateTranslation(
id: string,
requestBody?: Translation,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/translations/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * Delete translation.
     * @param id 
     * @returns any Success
     * @throws ApiError
     */
    public static deleteTranslation(
id: string,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/translations/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * Create translation.
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static createTranslation(
requestBody?: Translation,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/translations',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}