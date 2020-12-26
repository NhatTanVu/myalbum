import { GlobalDataService } from 'src/app/services/globalData.service';
import { GlobalData, DisplayMode } from 'src/app/models/globalData';
import { Router, ActivatedRoute } from '@angular/router';
import { AlbumService } from './../../services/album.service';
import { SaveAlbum } from './../../models/album';
import { Component, OnInit } from '@angular/core';
import { ToastData, ToastyService } from 'ng2-toasty';

@Component({
  selector: 'app-edit-album',
  templateUrl: './edit-album.component.html',
  styleUrls: ['./edit-album.component.css']
})
export class EditAlbumComponent implements OnInit {
  album: SaveAlbum = {
    id: 0,
    name: null
  };
  globalData: GlobalData = {
    displayMode: DisplayMode.Album,
    enableDisplayMode: false
  };   

  constructor(private albumService: AlbumService,
    private toasty: ToastyService,
    private route: ActivatedRoute,
    private router: Router,
    private globalDataService: GlobalDataService) { }

  ngOnInit() {
    this.globalDataService.changeDisplayMode(this.globalData);
    this.route.params.subscribe(p => {
      this.albumService.get(+p['id']).subscribe(album => {
        if (album) {
          this.album = album;
        }
        else {
          this.router.navigate(['/album']);
        }
      },
        err => {
          this.router.navigate(['/album']);
        });
    });
  }

  submit() {
    var result$ = this.albumService.save(this.album);
    result$.subscribe(
      album => {
        this.toasty.success({
          title: "Success",
          msg: "Saved successfully.",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500,
          onRemove: function (toast: ToastData) {
            this.album = album;
          }
        });
      },
      err => {
        console.log(err);
      }
    );
  }

  back() {
    this.router.navigate(["/"], {
      queryParams: {
        ["albumId"]: this.album.id
      }
    });
  }

  delete() {
    var result$ = this.albumService.delete(this.album.id);
    var router = this.router;
    result$.subscribe(
      res => {
        this.toasty.success({
          title: "Success",
          msg: "Deleted successfully.",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500,
          onRemove: function (toast: ToastData) {
            router.navigate(['/album']);
          }
        });
      },
      err => {
        console.log(err);
      }
    );
  }
}
