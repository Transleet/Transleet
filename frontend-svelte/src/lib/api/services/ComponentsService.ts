/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Component } from '../models/Component';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class ComponentsService {

    /**
     * @param id 
     * @returns Component Success
     * @throws ApiError
     */
    public static getApiComponents(
id: string,
): CancelablePromise<Component> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/components/{id}',
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
    public static putApiComponents(
id: string,
requestBody?: Component,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/components/{id}',
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
    public static deleteApiComponents(
id: string,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/components/{id}',
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
    public static postApiComponents(
requestBody?: Component,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/components',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}