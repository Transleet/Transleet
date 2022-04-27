/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Project } from '../models/Project';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class ProjectsService {

    /**
     * Get a project by its id.
     * @param id 
     * @returns Project Success
     * @throws ApiError
     */
    public static getProject(
id: string,
): CancelablePromise<Project> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/projects/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * Delete a project by its id.
     * @param id 
     * @returns any Success
     * @throws ApiError
     */
    public static deleteProject(
id: string,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/projects/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * Get all projects.
     * @returns Project Success
     * @throws ApiError
     */
    public static getProjects(): CancelablePromise<Array<Project>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/projects',
        });
    }

    /**
     * Create a new project.
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
     * Update a project.
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