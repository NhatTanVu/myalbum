import { GlobalDataService } from 'src/app/services/globalData.service';
import { DisplayMode, GlobalData } from 'src/app/models/globalData';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
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
    private toastr: ToastrService,
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
        if (album) {
          this.toastr.success("Saved successfully.", "Success", {
            timeOut: 1500,
            closeButton: true
          }).onHidden.subscribe(() => {
            router.navigate(['/album']);
          });
        }
        else {
          this.toastr.error("Error occurred. Please try again!", "Error", {
            timeOut: 1500,
            closeButton: true
          });
        }
      },
      err => {
        this.toastr.error("Error occurred. Please try again!", "Error", {
          timeOut: 1500,
          closeButton: true
        });        
        console.log(err);
      }
    );
  }

  cancel() {
    this.router.navigate(['/album']);
  }
}
