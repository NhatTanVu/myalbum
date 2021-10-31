import { GlobalConfiguration } from '../models/globalData';

export class GlobalDataService {
    private readonly getConfigurationEndpoint = "/GetConfiguration";

    getConfiguration() {
        return fetch(this.getConfigurationEndpoint)
            .then(response => response.json())
            .then(data => {
                return data as GlobalConfiguration;
            });
    }

    async getPhotoApiEndpoint() {
        let result = await this.getConfiguration();
        return result.PhotoApiUrl; // https://localhost:5002/api/photos
    }

    async getAlbumApiEndpoint() {
        let result = await this.getConfiguration();
        return result.AlbumApiUrl; // https://localhost:5003/api/albums
    }

    async getCommentApiEndpoint() {
        let result = await this.getConfiguration();
        return result.CommentApiUrl; // https://localhost:5004/api/comments
    }

    async getGoogleApiKeyEndpoint() {
        let result = await this.getConfiguration();
        return result.GoogleApiKey;
    }
}