import { ImagePathDto } from "./imagePath.dto";


export interface StoryDto{
    ownerUsername:string;
    seenByUser:boolean;
    imagePaths:ImagePathDto[];
    storyID:string;
}