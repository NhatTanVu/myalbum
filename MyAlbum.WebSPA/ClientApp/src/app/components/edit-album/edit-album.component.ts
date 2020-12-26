import { NameClaimType } from './../../../api-authorization/api-authorization.constants';
import { AuthorizeService } from './../../../api-authorization/authorize.service';
import { GlobalDataService } from 'src/app/services/globalData.service';
import { GlobalData, DisplayMode } from 'src/app/models/globalData';
import { Router, ActivatedRoute } from '@angular/router';
import { AlbumService } from './../../services/album.service';
import { Album } from './../../models/album';
import { Component, OnInit } from '@angular/core';
import { ToastData, ToastyService } from 'ng2-toasty';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-edit-album',
  templateUrl: './edit-album.component.html',
  styleUrls: ['./edit-album.component.css']
})
export class EditAlbumComponent implements OnInit {
  album: Album = new Album();
  globalData: GlobalData = {
    displayMode: DisplayMode.Album,
    enableDisplayMode: false
  };
  userName: string;

  constructor(private albumService: AlbumService,
    private toasty: ToastyService,
    private route: ActivatedRoute,
    private router: Router,
    private globalDataService: GlobalDataService,
    private authorizeService: AuthorizeService) { }

  ngOnInit() {
    this.globalDataService.changeDisplayMode(this.globalData);
    this.route.params.subscribe(p => {
      this.authorizeService.getUser().pipe(map(u => u && u[NameClaimType])).subscribe(userName => {
        this.userName = userName;
        this.albumService.get(+p['id']).subscribe(album => {
          if (album) {
            if (album.author.userName == this.userName)
            {
              this.album = album;
            }
            else
            {
              this.router.navigate(["/"], {
                queryParams: {
                  ["albumId"]: album.id
                }
              });
            }
          }
          else {
            this.router.navigate(['/album']);
          }
        },
          err => {
            this.router.navigate(['/album']);
          });
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
