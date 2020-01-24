import { SavePhoto } from './../../models/SavePhoto';
import { PhotoService } from './../../services/photo.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService, ToastData } from 'ng2-toasty';

@Component({
  selector: 'app-photo-form',
  templateUrl: './photo-form.component.html',
  styleUrls: ['./photo-form.component.css']
})
export class PhotoFormComponent implements OnInit {
  @ViewChild("photoFile", {static: false}) fileInput: ElementRef;
  photo: SavePhoto = {
    id: 0,
    name: ""
  }

  constructor(private photoService: PhotoService,
    private toasty: ToastyService,
    private router: Router) { }

  ngOnInit() {
  }

  submit() {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    var photoFile = nativeElement.files[0];
    nativeElement.value = "";

    var result$ =  this.photoService.create(this.photo, photoFile);
    var router = this.router;
    result$.subscribe(
      photo => {
        this.toasty.success({
          title: "Success",
          msg: "Data was successfully saved.",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500,
          onRemove: function(toast: ToastData) {
            router.navigate(['/']);
          }
        });
      },
      err => {
        console.log(err);
      }
    );
  }
}