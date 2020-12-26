import { DisplayMode, GlobalData } from './../../models/globalData';
import { Component } from '@angular/core';
import { GlobalDataService } from 'src/app/services/globalData.service';
import { Location } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  globalData: GlobalData;
  displayMode: DisplayMode;

  constructor(
    private globalDataService: GlobalDataService,
    private location: Location,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.globalDataService.currentGlobalData$.subscribe(globalData => {
      this.globalData = globalData;
      this.displayMode = this.globalData.displayMode;
    });
    this.route.queryParams.subscribe(params => {
      if (params['displayMode']) {
        this.changeDisplayMode(params['displayMode']);
      }
      else if (this.location.path().indexOf(DisplayMode.Album) >= 0) {
        this.changeDisplayMode(DisplayMode.Album);
      }
      else {
        this.changeDisplayMode(DisplayMode.Photo);
      }
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  changeDisplayMode(displayMode: string) {
    let currentPath = this.location.path();

    if (currentPath == "/worldmap") {
      this.displayMode = DisplayMode.Photo;
    }
    else {
      this.globalData.displayMode = displayMode as DisplayMode;
      this.globalDataService.changeDisplayMode(this.globalData);
      this.displayMode = displayMode as DisplayMode;

      switch (currentPath) {
        case "":
        case "?displayMode=album":
        case "?displayMode=photo":
          if (displayMode == DisplayMode.Album) {
            this.router.navigate(["album/"]);
          }
          else {
            this.router.navigate(["/"]);
          }
          break;
        case "/photo/new":
          if (displayMode == DisplayMode.Album) {
            this.router.navigate(["album/new"]);
          }
          break;
        case "/album":
          if (displayMode == DisplayMode.Photo) {
            this.router.navigate(["/"]);
          }
          break;
        case "/album/new":
          if (displayMode == DisplayMode.Photo) {
            this.router.navigate(["photo/new"]);
          }
          break;
      }
    }

    console.log("currentPath = " + currentPath);
    console.log("displayMode = " + this.displayMode);
  }
}
