import { Album, AlbumQuery } from './../../models/album';
import { AlbumService } from './../../services/album.service';
import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';

declare var Tessarray: any;

@Component({
  selector: 'app-explore-album',
  templateUrl: './explore-album.component.html',
  styleUrls: ['./explore-album.component.css']
})
export class ExploreAlbumComponent implements OnInit {
  albums: Album[];
  query: AlbumQuery;  
  @ViewChildren('albumBoxes') albumBoxes: QueryList<any>;

  constructor(private albumService: AlbumService) { 
    this.albumService.getAll(this.query)
    .subscribe(albums => {
      this.albums = albums;
    });    
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.albumBoxes.changes.subscribe(t => {
      this.ngForRendered();
    });
  }
  
  ngForRendered() {
    var tessarray = new Tessarray(".album-gallery", ".album-box", {
      selectorClass: false,
      imageClass: ".album-box .main-photo img",
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
        containerPadding: 0,
        forceAspectRatio: 1
      }
    });
  }  
}
