import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { LoadingBarModule } from '@ngx-loading-bar/core';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { PhotoFormComponent } from './components/photo-form/photo-form.component';
import { PhotoService } from './services/photo.service';
import { ToastyModule } from 'ng2-toasty';
import { ViewPhotoComponent } from './components/view-photo/view-photo.component';
import { LocationService } from './services/location.service';
import { WindowRef } from './models/WindowRef';
import { CommentService } from './services/comment.service';
import { ReplyFormComponent } from './components/_partials/reply-form/reply-form.component';
import { ReplyListComponent } from './components/_partials/reply-list/reply-list.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PhotoFormComponent,
    ViewPhotoComponent,
    ReplyFormComponent,
    ReplyListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'photos/new', component: PhotoFormComponent, canActivate: [AuthorizeGuard] },
      { path: 'photos/:id', component: ViewPhotoComponent }
    ]),
    ToastyModule.forRoot(),
    LoadingBarHttpClientModule, // for HttpClient use
    LoadingBarModule
  ],
  providers: [
    PhotoService,
    CommentService,
    WindowRef,
    LocationService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
