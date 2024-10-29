import { Component } from '@angular/core';
import { ImageCompressorService } from 'src/app/uitility/image-compressor.service';

@Component({
  selector: 'app-new-story',
  templateUrl: './new-story.component.html',
  styleUrls: ['./new-story.component.scss']
})
export class NewStoryComponent {
  selectedFiles:File[] = []

  imageSrc: string | ArrayBuffer | null = null;
  imageUrls: string[] = [];

  imageDisplay:boolean = false;
  img:boolean = true;
  constructor(private _imageCompressorService:ImageCompressorService){

  }
  async onFileSelected(event:any){
    const inputElement = event.target as HTMLInputElement;
    const files = inputElement.files;

    if (files && files.length > 0) {
      this.selectedFiles = [];
      

      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        console.log(file.size / 1024)
        try {
 
          const compressedFile = await this._imageCompressorService.compressImage(file);
          console.log(compressedFile.size / 1024)
          
          const reader = new FileReader();
          reader.onload = (e: ProgressEvent<FileReader>) => {
            if (e.target?.result) {
              this.imageSrc = e.target.result as string;
              this.img = false; 
            }
          };

          reader.readAsDataURL(compressedFile);
          this.selectedFiles.push(compressedFile);
          this.imageDisplay = true
  
          
        
        }catch (error) {
          console.error('Error processing file:', error);
        }
      }
    }
  }
}
