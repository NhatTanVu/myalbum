import { Photo } from './../../models/photo';
import { PhotoService } from './../../services/photo.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Comment, User } from './../../models/comment';
import { CommentService } from 'src/app/services/comment.service';
import { ToastyService } from 'ng2-toasty';

@Component({
  selector: 'app-view-photo',
  templateUrl: './view-photo.component.html',
  styleUrls: ['./view-photo.component.css']
})
export class ViewPhotoComponent implements OnInit {
  photo: Photo = {
    id: 0,
    name: "",
    filePath: "",
    boundingBoxFilePath: "",
    width: 0,
    height: 0,
    photoCategories: [],
    locLat: null,
    locLng: null,
    centerLat: null,
    centerLng: null,
    mapZoom: null,
    comments: []
  };
  newComment: Comment = {
    id: 0,
    photoId: 0,
    content: "",
    author: {
      userName: "",
      firstName: "",
      lastName: "",
      displayName: ""
    }
  };
  photoId: number;
  isShownBoundingBox: boolean = false;
  hasMap: boolean = null;

  @ViewChild('gmap', {static: false}) gmapElement: any;
  map: google.maps.Map;

  constructor(
    private photoService: PhotoService,
    private commentService: CommentService,
    private route: ActivatedRoute,
    private toasty: ToastyService
  ) { 
    this.route.params.subscribe(p => {
      this.photoId = +p['id'];
      this.newComment.photoId = this.photoId;
      this.photoService.getPhoto(this.photoId)
        .subscribe(photo => {
          this.photo = photo;
          this.hasMap = (photo.locLat != null) && (photo.locLng != null);
          this.initializeMap();
        });
    });
  }

  ngOnInit() {
  }

  initializeMap() {
    if (this.hasMap === true && this.gmapElement != undefined) {
      // Create map
      var mapProp = {
        center: new google.maps.LatLng(this.photo.centerLat, this.photo.centerLng),
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
      };
      this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
      var marker = new google.maps.Marker({
        position: new google.maps.LatLng(this.photo.locLat, this.photo.locLng),
        map: this.map
      });
    }
  }

  resetNewComment() {
    this.newComment = {
      id: 0,
      photoId: 0,
      content: "",
      author: {
        userName: "",
        firstName: "",
        lastName: "",
        displayName: ""
      }
    };
  }

  submitComment() {
    var result$ =  this.commentService.create(this.newComment);
    result$.subscribe(
      comment => {
        this.toasty.success({
          title: "Success",
          msg: "Comment was successfully posted.",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500
        });
        this.photo.comments.push(comment);
        this.resetNewComment();
      },
      err => {
        console.log(err);
      }
    );
  }
}
