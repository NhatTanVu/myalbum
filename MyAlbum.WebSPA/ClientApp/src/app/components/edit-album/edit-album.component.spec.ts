import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditAlbumComponent } from './edit-album.component';

describe('EditAlbumComponent', () => {
  let component: EditAlbumComponent;
  let fixture: ComponentFixture<EditAlbumComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditAlbumComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAlbumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
