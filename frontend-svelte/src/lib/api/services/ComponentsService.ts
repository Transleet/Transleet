/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Component } from '../models/Component';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class ComponentsService {

    /**
     * Get a component by its id.
     * @param id 
     * @returns Component Success
     * @throws ApiError
     */
    public static getComponent(
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
     * Delete a component by its id.
     * @param id 
     * @returns any Success
     * @throws ApiError
     */
    public static deleteComponent(
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
     * Create a new component.
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static createComponent(
requestBody?: Component,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/components',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * Update a component.
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static updateComponent(
requestBody?: Component,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/components',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}