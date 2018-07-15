import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewFormGoalAlignmentComponent } from './new-form-goal-alignment.component';

describe('NewFormGoalAlignmentComponent', () => {
  let component: NewFormGoalAlignmentComponent;
  let fixture: ComponentFixture<NewFormGoalAlignmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewFormGoalAlignmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewFormGoalAlignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
