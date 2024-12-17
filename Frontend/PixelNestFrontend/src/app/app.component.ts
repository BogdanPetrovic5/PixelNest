import { Component, HostListener, OnInit } from '@angular/core';
import { Meta } from '@angular/platform-browser';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit{
  constructor(private meta: Meta){

  }
  ngOnInit(): void {
    
  }
  
  title = 'PixelNestFrontend';

  
}
