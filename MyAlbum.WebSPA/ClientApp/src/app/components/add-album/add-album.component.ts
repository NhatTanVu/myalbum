import { Router } from '@angular/router';
import { ToastData, ToastyService } from 'ng2-toasty';
import { AlbumService } from './../../services/album.service';
import { Album } from './../../models/album';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-album',
  templateUrl: './add-album.component.html',
  styleUrls: ['./add-album.component.css']
})
export class AddAlbumComponent implements OnInit {
  album: Album = {
    id: 0,
    name: null,
    createdDate: null,
    modifiedDate: null,
    author: null
  };

  constructor(private albumService: AlbumService,
    private toasty: ToastyService,
    private router: Router) { }

  ngOnInit() {
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