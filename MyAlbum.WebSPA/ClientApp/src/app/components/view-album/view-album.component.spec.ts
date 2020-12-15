import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAlbumComponent } from './view-album.component';

describe('ViewAlbumComponent', () => {
  let component: ViewAlbumComponent;
  let fixture: ComponentFixture<ViewAlbumComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewAlbumComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAlbumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
