import { query } from '@angular/animations';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import axios from 'axios';
import { PostService } from 'src/app/core/services/post/post.service';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { ImageCompressorService } from 'src/app/uitility/image-compressor.service';
import { Map, Marker, geocoding, config } from '@maptiler/sdk'; 
import { LottieStateService } from 'src/app/core/services/states/lottie-state.service';
import { PostStateService } from 'src/app/core/services/states/post-state.service';

@Component({
  selector: 'app-new-post',
  templateUrl: './new-post.component.html',
  styleUrls: ['./new-post.component.scss']
})
export class NewPostComponent implements OnInit{

  @ViewChildren('descriptionDiv') descriptionDiv: QueryList<ElementRef> | undefined; 

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
  next:boolean = false;

  locationValue:string = ""
  locationCenter:string = ""
  objectFit:string = 'cover';
  selectedFiles:File[] = []
  imageSrc: string | ArrayBuffer | null = null;
  imageUrls: string[] = [];
  // description:string = ""

  
  constructor(
    private _dashboardStateMenagment:DashboardStateService,
    private _userSessionService:UserSessionService,
    private _postService:PostService,
    private _postState:PostStateService,
    private _imageCompressorService:ImageCompressorService,
    private _lottieState:LottieStateService
  ){
    config.apiKey = 'aqR39NWYQyZAdFc6KtYh'
  }
  ngOnInit(): void {
    
  }
  
  async onLocationInput(event:any){
    const query = event.target.value;
    if(query.length > 3){
      const response = await geocoding.forward(query)
    
      this.suggestions = response.features.map((feature:any)=>({
        
          
          place_name: feature?.context?.[0]?.text || feature?.place_name
        
      }))
    }else this.suggestions = [];
    
  }
  setLocation(location:any){
    this.locationCenter = location.place_name;
    
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
  navigateThroughNewPost(){
    this.next = !this.next;
  }
  sharePost(){
    const text = this._takeText()
    const username = this._userSessionService.getFromCookie("username");
    const formData = this._appendToForm(text, username)

    this._postService.createNewPost(formData).subscribe((response) =>{
      this.newPostForm = false
      this._lottieState.setIsSuccess(true);
      this._dashboardStateMenagment.setIsTabSelected(false);
      setTimeout(()=>{
        
        this._lottieState.setIsSuccess(false);
        this._postState.setPosts([]);
        this._postState.loadPosts(1);
      }, 1600)
    }, (error:HttpErrorResponse) =>{
      this.newPostForm = false
     
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
    const description = this.descriptionDiv?.find(desc => {
      const text = desc.nativeElement?.innerHTML.trim();
      return text && text.length > 0; 
    });
  
    return description ? description.nativeElement.innerHTML : null;
  }
  private _appendToForm(description:any, username:string):FormData{
    const FORM_DATA = new FormData()
    FORM_DATA.append("PostDescription", description)
    FORM_DATA.append("OwnerUsername", username);
    FORM_DATA.append("PhotoDisplay", this.objectFit);
    FORM_DATA.append("Location", this.locationCenter);
    for (let i = 0; i < this.selectedFiles.length; i++) {
      FORM_DATA.append("Photos", this.selectedFiles[i]);
    }
    return FORM_DATA;
  }
 

}
