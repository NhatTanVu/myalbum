import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
  selector: 'app-world-map',
  templateUrl: './world-map.component.html',
  styleUrls: ['./world-map.component.css']
})
export class WorldMapComponent implements OnInit {

  @ViewChild('gmap', { static: false }) gmapElement: any;
  map: google.maps.Map;

  constructor() { 
    this.initializeMap();
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.initializeMap();
  }

  initializeMap() {
    if (this.gmapElement != undefined) {
      // Create map
      var mapProp = {
        center: new google.maps.LatLng(3.140853, 101.693207), // KUL
        zoom: 12,
        mapTypeId: google.maps.MapTypeId.ROADMAP
      };
      this.map = new google.maps.Map(this.gmapElement.nativeElement, mapProp);
    }
  }

}
