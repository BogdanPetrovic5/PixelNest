import { Component } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import { StoryService } from 'src/app/core/services/story/story.service';
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
  objectFit:string = 'cover';

  imageDisplay:boolean = false;
  img:boolean = true;
  newStory:boolean = false;

  constructor(
    private _imageCompressorService:ImageCompressorService,
    private _dashboardState:DashboardStateService,
    private _cookieService:CookieService,
    private _storyService:StoryService,
    private _lottieState:LottieStateService
  ){

  }
  closeTab(){
    this._dashboardState.setIsNewStoryTabOpen(false);
  }


  discardPhoto(){
    this.selectedFiles = []
    this.imageSrc = ""
    this.img = true
  }

  shareStory(){
    const formData = this._appendToForm()

    this._storyService.publishStory(formData).subscribe({
      next:response=>{
        
        this._lottieState.setIsSuccess(true);
        this._dashboardState.setIsNewStoryTabOpen(false);
        setTimeout(()=>{
          this._lottieState.setIsSuccess(false)
        },1400)
      },
      error:error=>{
        console.error(error);
      }
    })
  }

  async onFileSelected(event:any){
    const inputElement = event.target as HTMLInputElement;
    const files = inputElement.files;

    if (files && files.length > 0) {
      this.selectedFiles = [];
      

      for (let i = 0; i < files.length; i++) {
        const file = files[i];
       
        try {
 
          const compressedFile = await this._imageCompressorService.compressImage(file);
          
          
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

  private _appendToForm(){
    let username = this._cookieService.get("username");
    const FORM_DATA = new FormData();

    FORM_DATA.append("Username", username);
    FORM_DATA.append("PhotoDisplay", this.objectFit)

    for(let i = 0; i< this.selectedFiles.length;i++){
      FORM_DATA.append("StoryImage", this.selectedFiles[i]);
    }
    return FORM_DATA;
  }
}
