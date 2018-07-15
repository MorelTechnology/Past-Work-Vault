import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewLookupComponent } from './new-lookup.component';

describe('NewLookupComponent', () => {
  let component: NewLookupComponent;
  let fixture: ComponentFixture<NewLookupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewLookupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewLookupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
