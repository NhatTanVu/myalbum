import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { DisplayMode, GlobalData } from '../models/globalData';

@Injectable({
  providedIn: 'root'
})
export class GlobalDataService {
  private globalDataSource = new BehaviorSubject<GlobalData>({
    displayMode: DisplayMode.Photo
  });
  currentGlobalData$ = this.globalDataSource.asObservable();

  constructor() { }

  changeDisplayMode(value: GlobalData) {
    this.globalDataSource.next(value)
  }
}
