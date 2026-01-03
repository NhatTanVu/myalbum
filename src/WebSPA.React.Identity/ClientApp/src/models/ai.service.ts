export interface DescribeImageRequest {
    image_url: string;
}

export interface DescribeImageResponse {
    title: string;
    description: string;
    tags: string[];
}

export interface SuggestCommentRequest {
    image_url: string;
    hints: string[];
}

export interface SuggestCommentResponse {
    comments: string[];
}