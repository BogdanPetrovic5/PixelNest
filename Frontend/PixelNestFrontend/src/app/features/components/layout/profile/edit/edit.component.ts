import { ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { ImageCroppedEvent,base64ToFile  } from 'ngx-image-cropper';
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import { ProfileStateService } from 'src/app/core/services/states/profile-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';
import { ImageCompressorService } from 'src/app/uitility/image-compressor.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit, OnDestroy{
  @Input() name!:string
  username:string = "";
  newUsername:string = ""
  description:string = "";
  imageSrc:string = "/assets/images/user.png"
  selectedFile:File[] = [];
  imageChangedEvent: any = '';
  croppedImage:any;
  compressedFile:any;
  isOpened:boolean = false;
  @Output() closeEditTab:EventEmitter<void> = new EventEmitter<void>();
  constructor(
    private _userSession:UserSessionService, 
    private _imageCompression:ImageCompressorService,
    private _userService:UserService,
    private _lottieState:LottieStateService,
    private _cdr:ChangeDetectorRef,
    private _profileState:ProfileStateService
  ){}  
  ngOnInit(): void {
    this.username = this._userSession.getFromCookie("username")
    this.newUsername = this.username;

  }
  ngOnDestroy(): void {
    
  }
  async onFileSelected(event:any){
    const inputElement = event.target as HTMLInputElement;
    const file = inputElement.files;
    this.isOpened = true;
    this.imageChangedEvent = event;
    // if(file){
    //     this._applyCompression(file)
    // }
  }
  private async _applyCompression(file:any){
    try{
      this.compressedFile = await this._imageCompression.compressImage(file[0])
      const reader = new FileReader();
      reader.onload = (e: ProgressEvent<FileReader>) => {
        if (e.target?.result) {
          this.imageSrc = e.target.result as string;
         
        }
      }
      reader.readAsDataURL(this.compressedFile);
      
    
      this.selectedFile?.push(this.croppedImage);

  }catch(error) {
    console.error('Error processing file:', error);
  }
  }
  close(){
    this.closeEditTab.emit();
  }

  saveChanges(){
    const FORM_DATA = new FormData();
    
    FORM_DATA.append("ProfilePicture", this.selectedFile[0])
    this._lottieState.setIsInitialized(true);
    if(this.selectedFile){
      this._userService.changeProfilePicture(FORM_DATA).subscribe({
        next:response=>{
          this._lottieState.setIsInitialized(false);
          this._lottieState.setIsSuccess(true);
          setTimeout(() => {
            this._lottieState.setIsSuccess(false)
            this._profileState.setCurrentUrl(this.imageSrc)
          }, 1600);
          this._cdr.detectChanges()
        },
        error:error=>{
          this._lottieState.setIsInitialized(false);
          alert(error.message)
        }
      })
    }
  
    
  }


  
  fileChangeEvent(event: any): void {
    this.isOpened = true;
    this.imageChangedEvent = event;
  }


  async onImageCropped(event: ImageCroppedEvent) {
    if(event.blob){
    
      const fileName = `cropped-image-${Date.now()}.jpeg`; 
      const croppedFile = new File([event.blob], fileName, { type: event.blob.type });
  
      this.croppedImage = await this._imageCompression.compressImage(croppedFile);

      this.imageSrc = URL.createObjectURL(this.croppedImage);
  
  
      this.selectedFile = [this.croppedImage];
    }
    
  }

  applyCrop(){
    this.isOpened = false;
  }
}
