import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { CounterComponent } from './components/counter/counter.component';
import { FetchDataComponent } from './components/fetch-data/fetch-data.component';
import { PhotoFormComponent } from './components/photo-form/photo-form.component';
import { PhotoService } from './services/photo.service';
import { ToastyModule } from 'ng2-toasty';
import { ViewPhotoComponent } from './components/view-photo/view-photo.component';
import { LocationService } from './services/location.service';
import { WindowRef } from './models/WindowRef';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    PhotoFormComponent,
    ViewPhotoComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'photos/new', component: PhotoFormComponent },
      { path: 'photos/:id', component: ViewPhotoComponent }
    ]),
    ToastyModule.forRoot()
  ],
  providers: [
    PhotoService,
    WindowRef,
    LocationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
