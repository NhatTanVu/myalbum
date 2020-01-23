import { PhotoService } from './../../services/photo.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  photos: any[];

  constructor(private photoService: PhotoService) { }

  ngOnInit() {
    this.photoService.getAll()
      .subscribe(photos => this.photos = photos);
  }
}
