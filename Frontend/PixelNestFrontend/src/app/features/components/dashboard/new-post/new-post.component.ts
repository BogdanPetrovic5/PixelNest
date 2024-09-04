import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-new-post',
  templateUrl: './new-post.component.html',
  styleUrls: ['./new-post.component.scss']
})
export class NewPostComponent implements OnInit{
  charCount:number = 0;
  maxChar:number = 1000;

  img:boolean = true;
  isCaption:boolean = true;

  imageSrc: string | ArrayBuffer | null = null;
  ngOnInit(): void {
   
  }
  hideCaption(){
    this.isCaption = !this.isCaption;
  }
  showCaption(){
    this.isCaption = !this.isCaption
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


  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      const file = input.files[0];
      const reader = new FileReader();
      this.img = false;
      reader.onload = () => {
        this.imageSrc = reader.result; 
      };

      reader.readAsDataURL(file);
    }
  }

}
