import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExploreAlbumComponent } from './explore-album.component';

describe('ExploreAlbumComponent', () => {
  let component: ExploreAlbumComponent;
  let fixture: ComponentFixture<ExploreAlbumComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExploreAlbumComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExploreAlbumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
