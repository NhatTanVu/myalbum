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
        return result.PhotoUrl; // https://localhost:5002/api/photos
    }

    async getAlbumApiEndpoint() {
        let result = await this.getConfiguration();
        return result.AlbumUrl; // https://localhost:5003/api/albums
    }

    async getCommentApiEndpoint() {
        let result = await this.getConfiguration();
        return result.CommentUrl; // https://localhost:5004/api/comments
    }
}