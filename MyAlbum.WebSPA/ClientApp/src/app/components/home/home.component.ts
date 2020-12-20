import { Photo } from './../../models/photo';
import { PhotoService } from './../../services/photo.service';
import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

declare var Tessarray: any;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  photos: Photo[];
  query: any = {};
  @ViewChildren('imageBoxes') imageBoxes: QueryList<any>;

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
        .subscribe(photos => {
          this.photos = photos;
        });
    });
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.imageBoxes.changes.subscribe(t => {
      this.ngForRendered();
    });
  }

  ngForRendered() {
    var tessarray = new Tessarray(".explore-gallery", ".image-box", {
      selectorClass: false,
      boxTransition: false,
      boxTransformOutTransition: false,
      flickr: {
        // http://flickr.github.io/justified-layout/
        targetRowHeight: 300,
        targetRowHeightTolerance: 0.05,
        boxSpacing: {
          horizontal: 20,
          vertical: 20
        },
        containerPadding: 0
      }
    });
  }
}
