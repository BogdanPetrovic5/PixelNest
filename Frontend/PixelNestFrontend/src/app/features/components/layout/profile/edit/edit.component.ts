import { ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
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
    // this._userService.getProfilePicture(this.username).subscribe({
    //   next:response=>{
     
    //     this.imageSrc = "http://localhost:7157/Photos/" + response.path
    //     console.log(response)
    //   }
    // })
  }
  ngOnDestroy(): void {
    
  }
  async onFileSelected(event:any){
    const inputElement = event.target as HTMLInputElement;
    const file = inputElement.files;

    if(file){
        try{
          const compressedFile = await this._imageCompression.compressImage(file[0])
          const reader = new FileReader();
          reader.onload = (e: ProgressEvent<FileReader>) => {
            if (e.target?.result) {
              this.imageSrc = e.target.result as string;
            
            }
          }
          reader.readAsDataURL(compressedFile);
          this.selectedFile?.push(compressedFile);
      }catch(error) {
        console.error('Error processing file:', error);
      }
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
}