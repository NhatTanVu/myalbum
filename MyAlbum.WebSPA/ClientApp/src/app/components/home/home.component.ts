import { Photo } from './../../models/photo';
import { PhotoService } from './../../services/photo.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  photos: Photo[];
  query: any = {};

  constructor(
    private photoService: PhotoService,
    private route: ActivatedRoute
  ) {
    route.queryParams.subscribe(p => {
      if (+p['catId'])
        this.query.categoryId = +p['catId'];
      else
        delete this.query.categoryId;
      this.photoService.getAll(this.query)
        .subscribe(photos => this.photos = photos);
    });
  }

  ngOnInit() {
  }
}
