import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewWorkRequestContainerComponent } from './new-work-request-container.component';

describe('NewWorkRequestContainerComponent', () => {
  let component: NewWorkRequestContainerComponent;
  let fixture: ComponentFixture<NewWorkRequestContainerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewWorkRequestContainerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewWorkRequestContainerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
