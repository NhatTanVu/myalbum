import { GlobalDataService } from 'src/app/services/globalData.service';
import { DisplayMode, GlobalData } from 'src/app/models/globalData';
import { Router } from '@angular/router';
import { ToastData, ToastyService } from 'ng2-toasty';
import { AlbumService } from './../../services/album.service';
import { SaveAlbum } from './../../models/album';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-album',
  templateUrl: './add-album.component.html',
  styleUrls: ['./add-album.component.css']
})
export class AddAlbumComponent implements OnInit {
  album: SaveAlbum = {
    id: 0,
    name: null
  };
  globalData: GlobalData = {
    displayMode: DisplayMode.Album,
    enableDisplayMode: true
  };

  constructor(private albumService: AlbumService,
    private toasty: ToastyService,
    private router: Router,
    private globalDataService: GlobalDataService) { }

  ngOnInit() {
    this.globalDataService.changeDisplayMode(this.globalData);
  }

  submit() {
    var result$ = this.albumService.create(this.album);
    var router = this.router;
    result$.subscribe(
      album => {
        this.toasty.success({
          title: "Success",
          msg: "Saved successfully.",
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

  cancel() {
    this.router.navigate(['/album']);
  }
}