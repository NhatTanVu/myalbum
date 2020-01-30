import { Photo } from './../../models/photo';
import { PhotoService } from './../../services/photo.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  photos: Photo[];

  constructor(private photoService: PhotoService) { 
    this.photoService.getAll()
      .subscribe(photos => this.photos = photos);
  }

  ngOnInit() {
  }
}
