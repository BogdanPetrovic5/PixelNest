import { Injectable } from '@angular/core';
import imageCompression from 'browser-image-compression';
import { max } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ImageCompressorService {

  constructor() { }

  async compressImage(file:any):Promise<File>{
      const compressOptions ={
        maxSizeMb:0.5,
        maxWidthOrHeight:1080,
        useWebWorker:true,
        fileType: 'image/jpeg',
        initialQuality: 0.7
      }

      try{
          const compressedBlob = await imageCompression(file, compressOptions); 

          const fileName = file.name;
          const compressedFile = new File([compressedBlob], fileName, {
            type: 'image/jpeg'
          });
          const originalSizeMB = (file.size / (1024 * 1024)).toFixed(2); // Convert to MB and round to 2 decimal places
          const compressedSizeMB = (compressedFile.size / (1024 * 1024)).toFixed(2); // Convert to MB and round to 2 decimal places
      
          console.log(`Original File Size: ${originalSizeMB} MB`);
          console.log(`Compressed File Size: ${compressedSizeMB} MB`);
          return compressedFile;
      }catch(error){
        console.error('Error compressing file:', error);
        throw error;
      }
  }
}
