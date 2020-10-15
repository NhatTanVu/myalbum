import { ActivatedRoute, Router } from '@angular/router';
import { ToastyService } from 'ng2-toasty';
import { LocationService } from './../../services/location.service';
import { PhotoService } from './../../services/photo.service';
import { PositionModel, Photo } from './../../models/photo';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-edit-photo',
  templateUrl: './edit-photo.component.html',
  styleUrls: ['./edit-photo.component.css']
})
export class EditPhotoComponent implements OnInit {
  photoId: number = 0;
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
    createdDate: null,
    modifiedDate: null
  };
  position: PositionModel = null;

  @ViewChild("photoFile", { static: false }) fileInput: ElementRef;
  @ViewChild('gmap', { static: false }) gmapElement: any;
  map: google.maps.Map;
  @ViewChild('gmapSearchBox', { static: false }) gmapSearchBox: any;
  searchBox: google.maps.places.SearchBox;
  marker: google.maps.Marker;
  hasMap: boolean = null;

  constructor(private photoService: PhotoService,
    private locationService: LocationService,
    private toasty: ToastyService,
    private route: ActivatedRoute,
    private router: Router) {
    this.route.params.subscribe(p => {
      this.photoId = +p['id'];
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
    if (this.gmapElement != undefined) {
      // Create map
      var mapProp = {
        center: new google.maps.LatLng(3.140853, 101.693207), // KUL
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
      };
      if (this.hasMap) {
        mapProp.center = new google.maps.LatLng(this.photo.centerLat, this.photo.centerLng);
      }
      this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);

      if (this.hasMap) {
        this.position = {
          latitude: this.photo.locLat,
          longitude: this.photo.locLng
        };

        this.placeMarkerAndPanTo(new google.maps.LatLng(this.photo.locLat, this.photo.locLng));
      }
      else {
        // Set map to current location
        this.locationService.getLocation().subscribe(loc => {
          var gPosition = new google.maps.LatLng(loc.coords.latitude, loc.coords.longitude);
          this.map.setCenter(gPosition);
        });
      }

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
}
