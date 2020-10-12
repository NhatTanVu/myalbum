import { PhotoService } from './../../services/photo.service';
import { Photo } from './../../models/photo';
import { Component, OnInit, ViewChild } from '@angular/core';
import MarkerClusterer from '@googlemaps/markerclustererplus';

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

  constructor(
    private photoService: PhotoService
  ) { 
    this.initializeMap();
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.initializeMap();
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
              google.maps.event.addListener(marker, 'click', function() {
                var bounds = new google.maps.LatLngBounds(marker.getPosition());
                this.map.fitBounds(bounds);
                this.map.setZoom(((this.map.getZoom() - 10) > 0) ? (this.map.getZoom() - 10) : 1);
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
          // Create the search box and link it to the UI element.
          this.searchBox = new google.maps.places.SearchBox(this.gmapSearchBox.nativeElement);
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
          });
        });
    }
  }

  panTo(photo: Photo) {
    var location = new google.maps.LatLng({ lat: photo.locLat, lng: photo.locLng });
    this.map.setCenter(location);
    this.map.setZoom(this.map.getZoom() + 2);
  }
}
