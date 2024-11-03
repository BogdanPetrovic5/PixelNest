import { StoryDto } from "./story.dto"

export interface StoriesDto {
    ownerUsername:string;
    stories:StoryDto[];
}