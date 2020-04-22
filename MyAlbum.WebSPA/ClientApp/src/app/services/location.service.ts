import { WindowRef } from '../models/WindowRef';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  constructor(private window: WindowRef) { }

  // https://stackoverflow.com/questions/42495636/angular-2-subscribe-when-coordinates-are-received
  getLocation(): Observable<Position> {
    return Observable.create(observer => {
        if(this.window.nativeWindow.navigator && this.window.nativeWindow.navigator.geolocation) {
          this.window.nativeWindow.navigator.geolocation.getCurrentPosition(
                (position) => {
                    observer.next(position);
                    observer.complete();
                },
                (error) => observer.error(error)
            );
        } else {
            observer.error('Unsupported Browser');
        }
    });
  }
}
