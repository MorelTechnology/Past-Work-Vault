import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewWorkRequestComponent } from './new-work-request.component';

describe('NewWorkRequestComponent', () => {
  let component: NewWorkRequestComponent;
  let fixture: ComponentFixture<NewWorkRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewWorkRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewWorkRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
