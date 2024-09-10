import { Injectable } from '@angular/core';
import imageCompression from 'browser-image-compression';
import { max } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ImageCompressorService {

  constructor() { }

  async compressImage(file:File):Promise<File>{
      const compressOptions ={
        maxSizeMb:0.8,
        maxWidthOrHeight:1080,
        useWebWorker:true,
        fileType: 'image/jpeg'
      }

      try{
          const compressedBlob = await imageCompression(file, compressOptions); 

          const fileName = file.name;
          const compressedFile = new File([compressedBlob], fileName, {
            type: 'image/jpeg'
          });

          return compressedFile;
      }catch(error){
        console.error('Error compressing file:', error);
        throw error;
      }
  }
}
