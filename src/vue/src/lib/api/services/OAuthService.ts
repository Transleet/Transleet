/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class OAuthService {

    /**
     * @param returnUrl 
     * @returns any Success
     * @throws ApiError
     */
    public static getApiOauthGithubLogin(
returnUrl?: string,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/oauth/github_login',
            query: {
                'returnUrl': returnUrl,
            },
        });
    }

    /**
     * @param code 
     * @param state 
     * @returns any Success
     * @throws ApiError
     */
    public static getApiOauthGithubCallback(
code?: string,
state?: string,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/oauth/github_callback',
            query: {
                'code': code,
                'state': state,
            },
        });
    }

    /**
     * @param state 
     * @returns any Success
     * @throws ApiError
     */
    public static getApiOauthGithubCallback1(
state?: string,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/oauth/github_callback1',
            query: {
                'state': state,
            },
        });
    }

}
