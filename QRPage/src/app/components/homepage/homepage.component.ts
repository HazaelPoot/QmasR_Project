import { Component, OnInit } from '@angular/core';

import * as Aos from 'aos';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.scss']

})



export class HomepageComponent {

  public slides = [
    { src: "https://image1.com" },
    { src: "https://image2.com" },
    { src: "https://image3.com" },
    { src: "https://image4.com" }
  ];

    constructor() {}
}


