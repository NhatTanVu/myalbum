import { PhotoService } from './../../services/photo.service';
import { Photo } from './../../models/photo';
import { Component, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import MarkerClusterer from '@googlemaps/markerclustererplus';

declare var Tessarray: any;

@Component({
  selector: 'app-world-map',
  templateUrl: './world-map.component.html',
  styleUrls: ['./world-map.component.css']
})
export class WorldMapComponent implements OnInit {

  @ViewChild('gmap', { static: false }) gmapElement: any;
  map: google.maps.Map;
  @ViewChild('gmapSearchBox', { static: false }) gmapSearchBox: any;
  searchBox: google.maps.places.SearchBox;
  markers: google.maps.Marker[];
  markerCluster: MarkerClusterer;

  allPhotos: Photo[];
  viewportPhotos: Photo[];
  query: any = {};
  @ViewChildren('imageBoxes') imageBoxes: QueryList<any>;

  constructor(
    private photoService: PhotoService
  ) {
    this.initializeMap();
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.initializeMap();
    this.imageBoxes.changes.subscribe(t => {
      this.ngForRendered();
    });    
  }

  initializeMap() {
    if (this.gmapElement != undefined && this.gmapSearchBox != undefined) {
      // Get photos with location
      this.query.hasLocation = true;
      this.photoService.getAll(this.query)
        .subscribe(photos => {
          // Create map
          var mapProp = {
            center: new google.maps.LatLng(3.140853, 101.693207), // KUL
            zoom: 12,
            mapTypeId: google.maps.MapTypeId.ROADMAP
          };
          this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
          this.allPhotos = photos;
          if (this.allPhotos.length > 0) {
            var allBounds = new google.maps.LatLngBounds();
            this.markers = this.allPhotos.map((photo, i) => {
              var markerPosition = { lat: photo.locLat, lng: photo.locLng };
              var marker = new google.maps.Marker({
                position: markerPosition,
                label: "",
              });
              google.maps.event.addListener(marker, 'click', function () {
                this.map.setCenter(marker.getPosition());
                this.map.setZoom(this.map.getZoom() + 1);
              });
              google.maps.event.addListener(marker, 'rightclick', function () {
                this.map.setCenter(marker.getPosition());
                this.map.setZoom((this.map.getZoom() - 1) > 1 ? (this.map.getZoom() - 1) : 1);
              });
              allBounds.extend(markerPosition);
              return marker;
            });
            this.map.fitBounds(allBounds);
            this.markerCluster = new MarkerClusterer(this.map, this.markers, {
              imagePath: "/lib/@googlemaps/markerclustererplus/m"
            });
          }
          google.maps.event.addListener(this.map, 'bounds_changed', (e) => {
            var bound = this.map.getBounds();
            this.viewportPhotos = this.allPhotos.filter((photo) => {
              return bound.contains({ lat: photo.locLat, lng: photo.locLng });
            });
          });
          google.maps.event.addListener(this.map, 'click', (e) => {
            this.map.setCenter(e.latLng);
            this.map.setZoom(this.map.getZoom() + 2);
          });
          google.maps.event.addListener(this.map, 'rightclick', (e) => {
            this.map.setCenter(e.latLng);
            this.map.setZoom((this.map.getZoom() - 2) > 2 ? (this.map.getZoom() - 2) : 2);
          });
          // Create the search box and link it to the UI element.
          this.searchBox = new google.maps.places.SearchBox(this.gmapSearchBox.nativeElement);
          this.searchBox.addListener('places_changed', (e) => {
            var places = this.searchBox.getPlaces();
            if (places.length > 0) {
              var bounds = new google.maps.LatLngBounds();
              // For each place, get the icon, name and location.

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

              this.viewportPhotos = this.allPhotos.filter((photo) => {
                return bounds.contains({ lat: photo.locLat, lng: photo.locLng });
              });
            }
          });
        });
    }
  }

  panTo(photo: Photo) {
    var location = new google.maps.LatLng({ lat: photo.locLat, lng: photo.locLng });
    this.map.setCenter(location);
  }

  ngForRendered() {
    var tessarray = new Tessarray(".explore-gallery", ".image-box", {
      selectorClass: false,
      boxTransition: false,
      boxTransformOutTransition: false,
      flickr: {
        // http://flickr.github.io/justified-layout/
        targetRowHeight: 150,
        targetRowHeightTolerance: 0.05,
        boxSpacing: {
          horizontal: 5,
          vertical: 5
        },
        containerPadding: 0
      }
    });
  }  
}
