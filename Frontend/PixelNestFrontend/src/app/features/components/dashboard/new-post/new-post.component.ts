import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { PostService } from 'src/app/core/services/post/post.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';

@Component({
  selector: 'app-new-post',
  templateUrl: './new-post.component.html',
  styleUrls: ['./new-post.component.scss']
})
export class NewPostComponent implements OnInit{

  @ViewChild('descriptionDiv') descriptionDiv: ElementRef<HTMLDivElement> | undefined
  charCount:number = 0;
  maxChar:number = 1000;
  
  img:boolean = true;
  isCaption:boolean = true;
  selectedFiles:File[] = []
  imageSrc: string | ArrayBuffer | null = null;
  imageUrls: string[] = [];
  description:string = ""
  constructor(
    private _dashboardStateMenagment:DashboardStateService,
    private _userSessionService:UserSessionService,
    private _postService:PostService
  ){

  }
  ngOnInit(): void {
    
  }
 
  sharePost(){
    const text = this.takeText()
    const username = this._userSessionService.getUsername();
    const formData = this.appendToForm(text, username)

    this._postService.createNewPost(formData).subscribe((response) =>{
      console.log(response.message)
    }, (error:HttpErrorResponse) =>{
      console.log(error);
    })

  }

  onFileSelected(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const files = inputElement.files;

    if (files && files.length > 0) {
      this.selectedFiles = [];
      

      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        this.selectedFiles.push(file);

  
        const reader = new FileReader();
        reader.onload = (e: ProgressEvent<FileReader>) => {
          if (e.target?.result) {
            this.imageSrc = e.target.result as string
            this.img = false
          }
        };
        reader.readAsDataURL(file);
      }
    }
  }

  closeCreateNewPost(){
    this._dashboardStateMenagment.setIsTabSelected(false)
  }

  hideCaption(){

    this.isCaption = false
  }

  showCaption(){
    const text = this.takeText()
    if(text?.length == 0) this.isCaption = true
  }

  updateCharCount(event:any){
    
    const element = event.target as HTMLElement;
    const content = element.innerText;

    if (content.length > this.maxChar) {
      element.innerText = content.substring(0, this.maxChar);
      this.setCaretAtEnd(element);
    
    } else {
      this.charCount = content.length;
    }
  
  }

 
  private setCaretAtEnd(element: HTMLElement): void {
    const range = document.createRange();
    const selection = window.getSelection();
    range.selectNodeContents(element);
    range.collapse(false);
    selection?.removeAllRanges();
    selection?.addRange(range);
  }
  private takeText(){
    const description = this.descriptionDiv?.nativeElement
    const text = description?.innerHTML
    return text;
  }
  private appendToForm(description:any, username:string):FormData{
    const formData = new FormData()
    formData.append("PostDescription", description)
    formData.append("OwnerUsername", username);
    for (let i = 0; i < this.selectedFiles.length; i++) {
      formData.append("Photos", this.selectedFiles[i]);
    }
    return formData
  }
 

}
