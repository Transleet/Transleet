/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Translation } from '../models/Translation';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class TranslationsService {

    /**
     * @param id 
     * @returns Translation Success
     * @throws ApiError
     */
    public static getApiTranslations(
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
     * @param id 
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiTranslations(
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
     * @param id 
     * @returns any Success
     * @throws ApiError
     */
    public static deleteApiTranslations(
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
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static postApiTranslations(
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