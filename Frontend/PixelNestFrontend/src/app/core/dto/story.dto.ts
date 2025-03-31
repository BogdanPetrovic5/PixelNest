import { ImagePathDto } from "./imagePath.dto";


export interface StoryDto{
    ownerUsername:string;
    clientGuid:string;
    seenByUser:boolean;
    imagePaths:ImagePathDto[];
    storyID:string;
}