import { GlobalData, DisplayMode } from 'src/app/models/globalData';
import { GlobalDataService } from 'src/app/services/globalData.service';
import { AlbumService } from './../../services/album.service';
import { SaveAlbum } from './../../models/album';
import { Photo } from './../../models/photo';
import { PhotoService } from './../../services/photo.service';
import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

declare var Tessarray: any;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  photos: Photo[];
  query: any = {};
  album: SaveAlbum = {
    id: 0,
    name: null
  };
  globalData: GlobalData = {
    displayMode: DisplayMode.Photo,
    enableDisplayMode: true
  };
  @ViewChildren('imageBoxes') imageBoxes: QueryList<any>;

  constructor(
    private photoService: PhotoService,
    private albumService: AlbumService,
    private globalDataService: GlobalDataService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.globalDataService.changeDisplayMode(this.globalData);
    this.route.queryParams.subscribe(p => {
      if (+p['catId']) {
        this.globalData.displayMode = DisplayMode.Photo;
        this.globalData.enableDisplayMode = false;
        this.query.categoryId = +p['catId'];
      }
      else
        delete this.query.categoryId;
      if (+p['albumId']) {
        this.globalData.displayMode = DisplayMode.Album;
        this.globalData.enableDisplayMode = false;
        this.query.albumId = +p['albumId'];
        this.albumService.get(this.query.albumId).subscribe(album => {
          if (album) {
            this.album = album;
            this.globalData.displayMode = DisplayMode.Album;
          }
          else
            this.globalData.displayMode = DisplayMode.Photo;
          this.globalDataService.changeDisplayMode(this.globalData);
        },
          err => {
            this.router.navigate(['/album']);
          });
      }
      else {
        delete this.query.albumId;
        this.globalData.displayMode = DisplayMode.Photo;
        this.globalDataService.changeDisplayMode(this.globalData);
      }
      this.photoService.getAll(this.query)
        .subscribe(photos => {
          this.photos = photos;
        });
    });
  }

  ngAfterViewInit() {
    this.imageBoxes.changes.subscribe(t => {
      this.ngForRendered(document.querySelector(".explore-gallery").clientWidth);
    });
  }

  containerWidth: number;
  ngForRendered(containerWidth) {
    this.containerWidth = containerWidth;
    var tessarray = new Tessarray(".explore-gallery", ".image-box", {
      selectorClass: false,
      boxTransition: false,
      boxTransformOutTransition: false,
      resize: true,
      flickr: {
        // http://flickr.github.io/justified-layout/
        targetRowHeight: 300,
        targetRowHeightTolerance: 0.25,
        boxSpacing: {
          horizontal: 20,
          vertical: 20
        },
        containerPadding: 0,
        containerWidth: containerWidth
      },
      onRender: () => {
        this.checkScrollbar(this.containerWidth);
      }
    });
  }

  checkScrollbar(containerWidth) {
    if (document.querySelector(".explore-gallery")) {
      let clientWidth = document.querySelector(".explore-gallery").clientWidth;
      let scrollWidth = document.querySelector(".explore-gallery").scrollWidth;
      if (scrollWidth > clientWidth) {
        console.log("containerWidth = " + containerWidth);
        document.querySelector(".explore-gallery").classList.remove("container-is-loaded");
        document.querySelector(".explore-gallery").removeAttribute("style");
        document.querySelector(".image-box").classList.remove("container-is-loaded");
        document.querySelector(".image-box").removeAttribute("style");
        this.ngForRendered((containerWidth > clientWidth) ? clientWidth : containerWidth - 10);
      }
      else {
        window.setTimeout(() => {
          this.checkScrollbar(containerWidth);
        }, 1000);
      }
    }
  }
}
