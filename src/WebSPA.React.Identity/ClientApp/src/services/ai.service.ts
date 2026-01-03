import authService from '../components/api-authorization/AuthorizeService';
import { SuggestCommentRequest, SuggestCommentResponse, DescribeImageRequest, DescribeImageResponse } from '../models/ai.service';
import { GlobalDataService } from './globalData.service';

export class AIService {
    //private aiServiceApiEndpoint: string = "http://localhost:8000";
    private globalDataService = new GlobalDataService();

    async describeImage(payload: DescribeImageRequest) {
        if (!payload) return null;
        const token = await authService.getAccessToken();

        let aiServiceEndpoint = await this.globalDataService.getAI_ServiceApiEndpoint();
        if (!aiServiceEndpoint) return null;

        let headers: any = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };
        if (token) {
            headers = {
                ...headers,
                'Authorization': `Bearer ${token}`
            }
        }

        return fetch(aiServiceEndpoint + "/image/describe", {
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            headers: headers,
            body: JSON.stringify(payload)
        })
            .then(response => {
                if (response.ok) {
                    return response.json()
                } else {
                    return null;
                }
            })
            .then(data => {
                let response = data as DescribeImageResponse;
                return response;
            });
    }

    async suggestComment(payload: SuggestCommentRequest) {
        if (!payload) return null;
        const token = await authService.getAccessToken();

        let aiServiceEndpoint = await this.globalDataService.getAI_ServiceApiEndpoint();
        if (!aiServiceEndpoint) return null;

        let headers: any = {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        };
        if (token) {
            headers = {
                ...headers,
                'Authorization': `Bearer ${token}`
            }
        }

        return fetch(aiServiceEndpoint + "/comment/suggest", {
            method: 'POST', // *GET, POST, PUT, DELETE, etc.
            headers: headers,
            body: JSON.stringify(payload)
        })
            .then(response => {
                if (response.ok) {
                    return response.json()
                } else {
                    return null;
                }
            })
            .then(data => {
                let response = data as SuggestCommentResponse;
                return response;
            });
    }
}