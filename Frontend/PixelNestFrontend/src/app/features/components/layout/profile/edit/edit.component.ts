import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';
import { ImageCompressorService } from 'src/app/uitility/image-compressor.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit{
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
    private _userService:UserService
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

    if(this.selectedFile){
      this._userService.changeProfilePicture(FORM_DATA).subscribe({
        next:response=>{
          console.log(response);
        }
      })
    }
  
    
  }
}
