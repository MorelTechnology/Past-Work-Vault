import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditLookupComponent } from './edit-lookup.component';

describe('EditLookupComponent', () => {
  let component: EditLookupComponent;
  let fixture: ComponentFixture<EditLookupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditLookupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditLookupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
