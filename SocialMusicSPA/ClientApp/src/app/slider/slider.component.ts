import { Component, OnInit } from '@angular/core';
import { timer } from '../../../node_modules/rxjs';

@Component({
  selector: 'app-slider',
  templateUrl: './slider.component.html',
  styleUrls: ['./slider.component.sass']
})
export class SliderComponent implements OnInit {

  url = '';
  private i = 0;

  private imageUrls: string[] =
  [
    'http://www.commongroundgroup.net/wp-content/uploads/2011/10/beOurGuest-472.jpg',
    'http://www.commongroundgroup.net/wp-content/uploads/2011/10/earth.jpg.gif',
    'http://www.commongroundgroup.net/wp-content/uploads/2011/10/DresdenAfterBombing.jpg'
  ];



  constructor() {
  }

  ngOnInit() {
    timer(1000, 4000).subscribe(() => {
      if (this.i < this.imageUrls.length) {
        this.url = this.imageUrls[this.i];
        this.i++;
      } else {
        this.i = 0;
      }
    });
  }

}
