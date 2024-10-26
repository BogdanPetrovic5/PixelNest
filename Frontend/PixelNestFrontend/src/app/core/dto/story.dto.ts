import { ImagePathDto } from "./post.dto";

export interface StoryDto{
    ownerUsername:string,
    imagePaths:ImagePathDto[]
}