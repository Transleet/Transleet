/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Component } from '../models/Component';
import type { ObjectId } from '../models/ObjectId';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class ComponentsService {

    /**
     * @param id 
     * @param requestBody 
     * @returns Component Success
     * @throws ApiError
     */
    public static getComponentById(
id: string,
requestBody?: ObjectId,
): CancelablePromise<Component> {
        return __request(OpenAPI, {
            method: 'GET',
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
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static deleteComponentById(
id: string,
requestBody?: ObjectId,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/components/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
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