import { NameClaimType } from './../../../api-authorization/api-authorization.constants';
import { AuthorizeService } from './../../../api-authorization/authorize.service';
import { GlobalDataService } from 'src/app/services/globalData.service';
import { GlobalData, DisplayMode } from 'src/app/models/globalData';
import { Photo } from './../../models/photo';
import { PhotoService } from './../../services/photo.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Comment, findReplyInComment, mergeNewComment } from './../../models/comment';
import { CommentService } from 'src/app/services/comment.service';
import { ToastyService } from 'ng2-toasty';
import { CommentAdded } from 'src/app/models/commentAdded';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-view-photo',
  templateUrl: './view-photo.component.html',
  styleUrls: ['./view-photo.component.css'],
  host: {
    '(window:resize)': 'onResize($event)'
  }  
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
    comments: [],
    totalComments: 0,
    createdDate: null,
    modifiedDate: null,
    author: null,
    album: null
  };
  newComment: Comment = {
    id: 0,
    photoId: 0,
    parentId: null,
    content: "",
    author: {
      userName: "",
      firstName: "",
      lastName: "",
      displayName: "",
      createdDate: null,
      modifiedDate: null
    },
    connectionId: "",
    isNew: true,
    isReplying: false,
    isViewing: false,
    isEditing: false,
    areRepliesLoaded: false,
    numOfReplies: 0,
    replies: [],
    createdDate: null,
    modifiedDate: null
  };
  photoId: number = 0;
  isShownBoundingBox: boolean = false;
  hasMap: boolean = null;
  globalData: GlobalData = {
    displayMode: DisplayMode.Photo,
    enableDisplayMode: false
  };
  userName: string;
  maxWidth: number = 0;
  maxHeight: number = 0;
  flipContainerWidth: number = 0;
  gmapHeight: number = 0;

  @ViewChild('gmap', { static: false }) gmapElement: any;
  map: google.maps.Map;

  @ViewChild('photoContainer', { static: false }) photoContainer : ElementRef;
  @ViewChild('imagePhoto', { static: false }) imageElement : ElementRef;
  
  constructor(
    private photoService: PhotoService,
    private commentService: CommentService,
    private route: ActivatedRoute,
    private router: Router,
    private toasty: ToastyService,
    private globalDataService: GlobalDataService,
    private authorizeService: AuthorizeService
  ) { }

  onResize(event){
    this.calcPhotoSize();
  }

  ngOnInit() {
    this.globalDataService.changeDisplayMode(this.globalData);
    this.authorizeService.getUser().pipe(map(u => u && u[NameClaimType])).subscribe(userName => {
      this.userName = userName;
    });
    this.route.params.subscribe(p => {
      this.photoId = +p['id'];
      this.newComment.photoId = this.photoId;
      this.photoService.get(this.photoId).subscribe(photo => {
        if (photo) {
          this.photo = photo;
          this.hasMap = (photo.locLat != null) && (photo.locLng != null);
          this.initializeMap();
          this.calcPhotoSize();
        }
        else {
          this.router.navigate(['/']);
        }
      },
        err => {
          this.router.navigate(['/']);
        });
    });

    this.commentService.addedComment$.subscribe(newComment => this.addedCommentSubscriber(newComment));
  }

  calcPhotoSize() {
    window.setTimeout(() => {
      let element = this.photoContainer.nativeElement;
      let computedStyle = window.getComputedStyle(element);
      let elementHeight = element.clientHeight;  // height with padding
      let elementWidth = element.clientWidth;   // width with padding
      elementHeight -= parseFloat(computedStyle.paddingTop) + parseFloat(computedStyle.paddingBottom);
      elementWidth -= parseFloat(computedStyle.paddingLeft) + parseFloat(computedStyle.paddingRight);
      let maxWidth = elementWidth;
      let maxHeight = elementHeight;
      if (maxHeight > 10) {
        if (this.imageElement.nativeElement.clientWidth > 0 && this.maxWidth == maxWidth && this.maxHeight == maxHeight) {
          this.flipContainerWidth = parseFloat(this.imageElement.nativeElement.clientWidth);
        }
        else {
          this.calcPhotoSize();  
        }
        this.maxHeight = maxHeight;
        this.maxWidth = maxWidth;        
      }
      else {
        this.calcPhotoSize();
      }
    }, 200);
  }  

  isEditable() {
    return this.photo.author && this.userName == this.photo.author.userName;
  }

  addedCommentSubscriber(newComment: CommentAdded) {
    if (newComment.ancestorOrSelf.photoId == this.photoId) {
      this.toasty.info({
        title: "Info",
        msg: "New comment added.",
        theme: "bootstrap",
        showClose: true,
        timeout: 10000
      });

      let index = this.photo.comments.findIndex((element, index, array) => {
        return (element.id == newComment.ancestorOrSelf.id);
      });
      if (index >= 0) // Add reply and expand ancestor
        this.photo.comments[index] = mergeNewComment(this.photo.comments[index], newComment.ancestorOrSelf);
      else // Add comment
        this.photo.comments.push(newComment.ancestorOrSelf);

      let commentId: number = newComment.id;
      window.setTimeout(() => {
        let comment: Comment = this.photo.comments.find(c => c.id == commentId);
        if (comment)
          comment.isNew = false;
        else {
          let reply: Comment;
          for (var i = 0; i < this.photo.comments.length; i++) {
            reply = findReplyInComment(commentId, this.photo.comments[i]);
            if (reply != null) {
              reply.isNew = false;
              break;
            }
          }
        }
      }, 30000);
    }
  }

  initializeMap() {
    if (this.hasMap === true && this.gmapElement != undefined) {
      let windowWidth = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;
      this.gmapHeight = (windowWidth > 575.98) ? 400 : 300;
      // Create map
      var mapProp = {
        center: new google.maps.LatLng(this.photo.centerLat, this.photo.centerLng),
        zoom: this.photo.mapZoom ? this.photo.mapZoom : 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
      };
      this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
      var marker = new google.maps.Marker({
        position: new google.maps.LatLng(this.photo.locLat, this.photo.locLng),
        map: this.map
      });
      this.map.panTo(marker.getPosition());
    }
  }

  resetNewComment() {
    this.newComment = {
      id: 0,
      photoId: this.photoId,
      parentId: null,
      content: "",
      author: {
        userName: "",
        firstName: "",
        lastName: "",
        displayName: "",
        createdDate: null,
        modifiedDate: null
      },
      connectionId: "",
      isNew: true,
      isReplying: false,
      isViewing: false,
      isEditing: false,
      areRepliesLoaded: false,
      numOfReplies: 0,
      replies: [],
      createdDate: null,
      modifiedDate: null
    };
  }

  submitComment() {
    var result$ = this.commentService.create(this.newComment);
    result$.subscribe(
      comment => {
        this.toasty.success({
          title: "Success",
          msg: "Posted successfully.",
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