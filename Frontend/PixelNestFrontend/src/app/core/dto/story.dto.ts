import { ImagePathDto } from "./post.dto";

export interface StoryDto{
    ownerUsername:string;
    seenByUser:boolean;
    imagePaths:ImagePathDto[];
    storyID:number;
}