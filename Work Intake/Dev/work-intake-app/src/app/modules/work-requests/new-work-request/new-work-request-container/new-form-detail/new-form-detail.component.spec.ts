import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewFormDetailComponent } from './new-form-detail.component';

describe('NewFormDetailComponent', () => {
  let component: NewFormDetailComponent;
  let fixture: ComponentFixture<NewFormDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewFormDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewFormDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
