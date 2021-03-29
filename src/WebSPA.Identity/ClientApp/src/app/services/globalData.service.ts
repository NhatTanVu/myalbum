import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/internal/operators/map';
import { DisplayMode, GlobalConfiguration, GlobalData } from '../models/globalData';

@Injectable({
  providedIn: 'root'
})
export class GlobalDataService {
  private globalDataSource = new BehaviorSubject<GlobalData>({
    displayMode: DisplayMode.Photo,
    enableDisplayMode: true
  });
  currentGlobalData$ = this.globalDataSource.asObservable();

  private globalConfigurationSource = new BehaviorSubject<GlobalConfiguration>(new GlobalConfiguration());
  currentGlobalConfiguration$ = this.globalConfigurationSource.asObservable();

  private readonly getConfigurationEndpoint = "/GetConfiguration";
  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    })
  };

  constructor(private http: HttpClient) { }

  getConfiguration() {
    return this.http.get(this.getConfigurationEndpoint, this.httpOptions)
      .pipe(map(res => {
        var configuration = <GlobalConfiguration>res;
        this.globalConfigurationSource.next(configuration);
        return configuration;
      }));
  }

  changeDisplayMode(value: GlobalData) {
    this.globalDataSource.next(value)
  }
}
