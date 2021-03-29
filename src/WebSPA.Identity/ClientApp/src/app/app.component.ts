import { Component } from '@angular/core';
import { GlobalConfiguration } from './models/globalData';
import { GlobalDataService } from './services/globalData.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';

  globalConfiguration: GlobalConfiguration =
    {
      AlbumUrl: "",
      PhotoUrl: "",
      CommentUrl: ""
    };

  constructor(private globalDataService: GlobalDataService) {
    this.globalDataService.getConfiguration().subscribe(globalConfiguration => {
      this.globalConfiguration = globalConfiguration;
    });
  }
}
