import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
  providers:[DatePipe]
})
export class PostComponent implements OnInit{
  @Input() post:any
  formattedDate:string = ""
  constructor(private _datePipe:DatePipe){
   
  }

  ngOnInit():void{
    const date = this.post.publishDate;
    const dateObject = new Date(date)

    if (isNaN(dateObject.getTime())) {
      this.formattedDate = 'Invalid Date';
    } else {
      this.formattedDate = this._datePipe.transform(dateObject, 'd MMM \'at\' HH:mm') || 'Invalid Format';
    }
  }
}
