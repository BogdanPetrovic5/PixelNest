import { query } from '@angular/animations';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import axios from 'axios';
import { PostService } from 'src/app/core/services/post/post.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { ImageCompressorService } from 'src/app/uitility/image-compressor.service';
import { Map, Marker, geocoding, config } from '@maptiler/sdk'; 
@Component({
  selector: 'app-new-post',
  templateUrl: './new-post.component.html',
  styleUrls: ['./new-post.component.scss']
})
export class NewPostComponent implements OnInit{

  @ViewChild('descriptionDiv') descriptionDiv: ElementRef<HTMLDivElement> | undefined
  apiKey: string = 'aqR39NWYQyZAdFc6KtYh'; // Replace with your MapTiler API Key
  geocodeUrl: string = 'https://api.maptiler.com/geocoding/';
  suggestions: any[] = [];

  charCount:number = 0;
  maxChar:number = 1000;
  
  img:boolean = true;
  isCaption:boolean = true;
  isShared:boolean = false;  
  isError:boolean = false;
  newPostForm:boolean = true
  imageDisplay:boolean = false;
  anim:boolean = false;
  isFocus:boolean = false;

  locationValue:string = ""
  locationCenter:string = ""
  objectFit:string = 'cover';
  selectedFiles:File[] = []
  imageSrc: string | ArrayBuffer | null = null;
  imageUrls: string[] = [];
  description:string = ""

  
  constructor(
    private _dashboardStateMenagment:DashboardStateService,
    private _userSessionService:UserSessionService,
    private _postService:PostService,
    private _imageCompressorService:ImageCompressorService
  ){
    config.apiKey = 'aqR39NWYQyZAdFc6KtYh'
  }
  ngOnInit(): void {
    
  }
  
  async onLocationInput(event:any){
    const query = event.target.value;
    if(query.length > 3){
      const response = await geocoding.forward(query)
      console.log(response)
      this.suggestions = response.features.map((feature:any)=>({
        
          
          place_name: feature?.context?.[0]?.text || feature?.place_name
        
      }))
    }else this.suggestions = [];
    
  }
  setLocation(location:any){
    this.locationCenter = location.place_name;
    console.log(this.locationCenter)
    this.suggestions = [];
    this.removeFocus();
  }

  setFocus(){
    this.isFocus = true;
  }
  removeFocus(){
    this.isFocus = false;
  }
  inFocus(){
    return this.locationValue.length >= 3
  }
  sharePost(){
    const text = this._takeText()
    const username = this._userSessionService.getFromCookie("username");
    const formData = this._appendToForm(text, username)

    this._postService.createNewPost(formData).subscribe((response) =>{
      this.newPostForm = false
      this.isShared = true;
      setTimeout(()=>{
        this._dashboardStateMenagment.setIsTabSelected(false);
      }, 1000)
    }, (error:HttpErrorResponse) =>{
      this.newPostForm = false
      console.log(error)
      this.isError = true;
      setTimeout(()=>{
        this._dashboardStateMenagment.setIsTabSelected(false);
      }, 1500)
    })

  }
  toggleObjectFit(): void {
    this.objectFit = this.objectFit === 'cover' ? 'contain' : 'cover';
  }
  async onFileSelected(event: Event){
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

  closeCreateNewPost(){

    this.anim = true;
    setTimeout(()=>{
      this._dashboardStateMenagment.setIsTabSelected(false)
    },500)
  }

  hideCaption(){
    this.isCaption = false
  }

  showCaption(){
    const text = this._takeText()
    if(text?.length == 0) this.isCaption = true
  }

  updateCharCount(event:any){
    
    const element = event.target as HTMLElement;
    const content = element.innerText;

    if (content.length > this.maxChar) {
      element.innerText = content.substring(0, this.maxChar);
      this._setCaretAtEnd(element);
    
    } else {
      this.charCount = content.length;
    }
  
  }

 
  private _setCaretAtEnd(element: HTMLElement): void {
    const range = document.createRange();
    const selection = window.getSelection();
    range.selectNodeContents(element);
    range.collapse(false);
    selection?.removeAllRanges();
    selection?.addRange(range);
  }
  private _takeText(){
    const description = this.descriptionDiv?.nativeElement
    const text = description?.innerHTML
    return text;
  }
  private _appendToForm(description:any, username:string):FormData{
    const formData = new FormData()
    formData.append("PostDescription", description)
    formData.append("OwnerUsername", username);
    formData.append("PhotoDisplay", this.objectFit);
    formData.append("Location", this.locationCenter);
    for (let i = 0; i < this.selectedFiles.length; i++) {
      formData.append("Photos", this.selectedFiles[i]);
    }
    return formData
  }
 

}
