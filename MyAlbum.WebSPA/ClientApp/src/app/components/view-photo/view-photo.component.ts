import { Photo } from './../../models/photo';
import { PhotoService } from './../../services/photo.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-view-photo',
  templateUrl: './view-photo.component.html',
  styleUrls: ['./view-photo.component.css']
})
export class ViewPhotoComponent implements OnInit {
  photo: Photo = {
    id: 0,
    name: "",
    filePath: "",
    boundingBoxFilePath: "",
    width: 0,
    height: 0,
    photoCategories: []
  };
  photoId: number;
  isShownBoundingBox: boolean = false;

  constructor(
    private photoService: PhotoService,
    private route: ActivatedRoute
  ) { 
    route.params.subscribe(p => {
      this.photoId = +p['id'];
      this.photoService.getPhoto(this.photoId)
        .subscribe(photo => this.photo = photo);
    });
  }

  ngOnInit() {
  }
}
