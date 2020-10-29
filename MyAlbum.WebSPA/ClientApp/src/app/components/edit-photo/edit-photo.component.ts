import { ActivatedRoute, Router } from '@angular/router';
import { ToastData, ToastyService } from 'ng2-toasty';
import { LocationService } from './../../services/location.service';
import { PhotoService } from './../../services/photo.service';
import { PositionModel, Photo, PhotoCategory } from './../../models/photo';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-edit-photo',
  templateUrl: './edit-photo.component.html',
  styleUrls: ['./edit-photo.component.css']
})
export class EditPhotoComponent implements OnInit {
  readonly tagSeparator: string = ", ";
  readonly availableCategories: PhotoCategory[] = [
    {
      id: 1,
      name: "aeroplane"
    },
    {
      id: 2,
      name: "bicycle"
    },
    {
      id: 3,
      name: "bird"
    },
    {
      id: 4,
      name: "boat"
    },
    {
      id: 5,
      name: "bottle"
    },
    {
      id: 6,
      name: "bus"
    },
    {
      id: 7,
      name: "car"
    },
    {
      id: 8,
      name: "cat"
    },{
      id: 9,
      name: "chair"
    },
    {
      id: 10,
      name: "cow"
    },
    {
      id: 11,
      name: "diningtable"
    },
    {
      id: 12,
      name: "dog"
    },
    {
      id: 13,
      name: "horse"
    },
    {
      id: 14,
      name: "motorbike"
    },
    {
      id: 15,
      name: "person"
    },
    {
      id: 16,
      name: "pottedplant"
    },
    {
      id: 17,
      name: "sheep"
    },
    {
      id: 18,
      name: "sofa"
    },
    {
      id: 19,
      name: "train"
    },
    {
      id: 20,
      name: "tvmonitor"
    }    
  ];
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
    author: null
  };
  photoTags: number[];
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
      this.photoService.get(+p['id'])
        .subscribe(photo => {
          if (photo) {
            this.photo = photo;
            this.photoTags = this.photo.photoCategories.map(c => c.id);
            this.hasMap = (photo.locLat != null) && (photo.locLng != null);
            this.initializeMap();
          }
          else {
            this.router.navigate(['/']);
          }
        },
        err => {
          this.router.navigate(['/']);
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
        mapProp.zoom = this.photo.mapZoom;
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

  submit() {
    var nativeElement: HTMLInputElement = this.fileInput.nativeElement;
    var photoFile = null;
    if (nativeElement.files && nativeElement.files.length > 0) {
      photoFile = nativeElement.files[0];
    }
    if (this.position) {
      this.photo.locLat = this.position.latitude;
      this.photo.locLng = this.position.longitude;
      this.photo.centerLat = this.map.getCenter().lat();
      this.photo.centerLng = this.map.getCenter().lng();
      this.photo.mapZoom = this.map.getZoom();
    }
    else {
      this.photo.locLat = null;
      this.photo.locLng = null;
      this.photo.centerLat = null;
      this.photo.centerLng = null;
      this.photo.mapZoom = null;
    }
    this.photo.photoCategories = this.availableCategories.filter(cat => {
      return this.photoTags.indexOf(cat.id) >= 0;
    });

    var result$ = this.photoService.save(this.photo, photoFile);
    result$.subscribe(
      photo => {
        this.toasty.success({
          title: "Success",
          msg: "Saved successfully.",
          theme: "bootstrap",
          showClose: true,
          timeout: 1500
        });
        this.photo = photo;
      },
      err => {
        console.log(err);
      }
    );
  }

  back() {
    this.router.navigate(['/photos/' + this.photo.id]);
  }

  delete() {
    var result$ = this.photoService.delete(this.photo.id);
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
