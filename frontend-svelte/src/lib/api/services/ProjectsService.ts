/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ObjectId } from '../models/ObjectId';
import type { Project } from '../models/Project';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class ProjectsService {

    /**
     * @param id 
     * @param requestBody 
     * @returns Project Success
     * @throws ApiError
     */
    public static getProjectById(
id: string,
requestBody?: ObjectId,
): CancelablePromise<Project> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/projects/{id}',
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
    public static deleteProjectById(
id: string,
requestBody?: ObjectId,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/projects/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * @returns Project Success
     * @throws ApiError
     */
    public static getAllProjects(): CancelablePromise<Array<Project>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/projects',
        });
    }

    /**
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static createProject(
requestBody?: Project,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/projects',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static updateProject(
requestBody?: Project,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/projects',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}