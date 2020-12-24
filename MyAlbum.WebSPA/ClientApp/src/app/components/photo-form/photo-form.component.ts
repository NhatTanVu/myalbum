/// <reference types="@types/googlemaps" />

import { SavePhoto, PositionModel } from '../../models/photo';
import { PhotoService } from './../../services/photo.service';
import { LocationService } from './../../services/location.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastyService, ToastData } from 'ng2-toasty';
import { AlbumService } from './../../services/album.service';
import { AlbumQuery, SaveAlbum } from './../../models/album';
import { AuthorizeService } from './../../../api-authorization/authorize.service';
import { map } from 'rxjs/operators';
import { NameClaimType } from './../../../api-authorization/api-authorization.constants';

@Component({
  selector: 'app-photo-form',
  templateUrl: './photo-form.component.html',
  styleUrls: ['./photo-form.component.css']
})
export class PhotoFormComponent implements OnInit {
  @ViewChild("photoFile", { static: false }) fileInput: ElementRef;
  photo: SavePhoto = {
    id: 0,
    name: null,
    locLat: null,
    locLng: null,
    centerLat: null,
    centerLng: null,
    mapZoom: null,
    photoCategories: [],
    album: {
      id: 0,
      name: null
    }
  };
  position: PositionModel = null;
  albums: SaveAlbum[];
  selectedAlbums: number[];
  albumQuery: AlbumQuery;
  userName: string;

  @ViewChild('gmap', { static: false }) gmapElement: any;
  map: google.maps.Map;
  @ViewChild('gmapSearchBox', { static: false }) gmapSearchBox: any;
  searchBox: google.maps.places.SearchBox;
  marker: google.maps.Marker;

  constructor(private photoService: PhotoService,
    private locationService: LocationService,
    private toasty: ToastyService,
    private router: Router,
    private albumService: AlbumService,
    private authorizeService: AuthorizeService) { }

  ngOnInit() {
    this.authorizeService.getUser().pipe(map(u => u && u[NameClaimType]))
      .subscribe(userName => {
        this.userName = userName;
        this.albumQuery = {
          categoryId: null,
          hasLocation: null,
          authorUserName: this.userName
        };
        this.albumService.getAll(this.albumQuery)
          .subscribe(albums => {
            this.albums = albums;
          });
      });
  }

  ngAfterViewInit() {
    // Create map
    var mapProp = {
      center: new google.maps.LatLng(3.140853, 101.693207), // KUL
      zoom: 12,
      mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);

    // Set map to current location
    this.locationService.getLocation().subscribe(loc => {
      var gPosition = new google.maps.LatLng(loc.coords.latitude, loc.coords.longitude);
      this.map.setCenter(gPosition);
    });

    // Listen to map's click event
    google.maps.event.addListener(this.map, 'click', (e) => {
      this.placeMarkerAndPanTo(e.latLng);
    });

    // Create the search box and link it to the UI element.
    this.searchBox = new google.maps.places.SearchBox(this.gmapSearchBox.nativeElement);

    // Bias the SearchBox results towards current map's viewport.
    google.maps.event.addListener(this.map, 'bounds_changed', (e) => {
      this.searchBox.setBounds(this.map.getBounds());
    });

    // Listen for the event fired when the user selects a prediction and retrieve
    // more details for that place.
    this.searchBox.addListener('places_changed', (e) => {
      var places = this.searchBox.getPlaces();

      if (places.length == 0) {
        return;
      }

      // For each place, get the icon, name and location.
      var bounds = new google.maps.LatLngBounds();
      places.forEach(function (place) {
        if (!place.geometry) {
          console.log("Returned place contains no geometry");
          return;
        }

        if (place.geometry.viewport) {
          // Only geocodes have viewport.
          bounds.union(place.geometry.viewport);
        } else {
          bounds.extend(place.geometry.location);
        }
      });

      this.map.fitBounds(bounds);
      this.placeMarkerAndPanTo(this.map.getCenter());
    });
  }

  placeMarkerAndPanTo(latLng: google.maps.LatLng) {
    this.position = {
      latitude: latLng.lat(),
      longitude: latLng.lng()
    };
    if (this.marker)
      this.marker.setMap(null);
    this.marker = new google.maps.Marker({
      position: latLng,
      map: this.map
    });
    this.map.panTo(latLng);

    this.marker.addListener('click', (e) => {
      this.marker.setMap(null);
      this.position = null;
    });
  }

  submit() {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    if (!nativeElement.files || nativeElement.files.length == 0) {
      this.toasty.error({
        title: "Error",
        msg: "Photo required.",
        theme: "bootstrap",
        showClose: true,
        timeout: 1500
      });
      return;
    }

    var photoFile = nativeElement.files[0];
    if (this.position) {
      this.photo.locLat = this.position.latitude;
      this.photo.locLng = this.position.longitude;
      this.photo.centerLat = this.map.getCenter().lat();
      this.photo.centerLng = this.map.getCenter().lng();
      this.photo.mapZoom = this.map.getZoom();
    }

    if (this.selectedAlbums) {
      this.photo.album = this.albums.filter(album => {
        return this.selectedAlbums.indexOf(album.id) >= 0;
      })[0];
    }

    var result$ = this.photoService.create(this.photo, photoFile);
    var router = this.router;
    result$.subscribe(
      photo => {
        this.toasty.success({
          title: "Success",
          msg: "Saved successfully.",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500,
          onRemove: function (toast: ToastData) {
            router.navigate(['/']);
          }
        });
      },
      err => {
        console.log(err);
      }
    );
  }

  cancel() {
    this.router.navigate(['/']);
  }
}