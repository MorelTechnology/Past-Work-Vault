import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewWorkRequestSuccessComponent } from './new-work-request-success.component';

describe('NewWorkRequestSuccessComponent', () => {
  let component: NewWorkRequestSuccessComponent;
  let fixture: ComponentFixture<NewWorkRequestSuccessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewWorkRequestSuccessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewWorkRequestSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
