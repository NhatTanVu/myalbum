import { NameClaimType } from './../../../api-authorization/api-authorization.constants';
import { AuthorizeService } from './../../../api-authorization/authorize.service';
import { GlobalData, DisplayMode } from 'src/app/models/globalData';
import { GlobalDataService } from 'src/app/services/globalData.service';
import { AlbumService } from './../../services/album.service';
import { Album } from './../../models/album';
import { Photo } from './../../models/photo';
import { PhotoService } from './../../services/photo.service';
import { Component, OnInit, QueryList, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { map } from 'rxjs/operators';

declare var Tessarray: any;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  photos: Photo[];
  query: any = {};
  album: Album = new Album();
  globalData: GlobalData = {
    displayMode: DisplayMode.Photo,
    enableDisplayMode: true
  };
  userName: string;
  @ViewChildren('imageBoxes') imageBoxes: QueryList<any>;

  constructor(
    private photoService: PhotoService,
    private albumService: AlbumService,
    private globalDataService: GlobalDataService,
    private route: ActivatedRoute,
    private router: Router,
    private authorizeService: AuthorizeService
  ) { }

  ngOnInit() {
    this.globalDataService.changeDisplayMode(this.globalData);
    this.authorizeService.getUser().pipe(map(u => u && u[NameClaimType])).subscribe(userName => {
      this.userName = userName;
    });    
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

  isEditable() {
    return this.album.author && this.userName == this.album.author.userName;
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
        targetRowHeightTolerance: 0.25,
        boxSpacing: {
          horizontal: 20,
          vertical: 20
        },
        containerPadding: 0
      }
    });
  }
}
