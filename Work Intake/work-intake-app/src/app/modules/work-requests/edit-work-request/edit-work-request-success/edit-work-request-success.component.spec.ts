import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditWorkRequestSuccessComponent } from './edit-work-request-success.component';

describe('EditWorkRequestSuccessComponent', () => {
  let component: EditWorkRequestSuccessComponent;
  let fixture: ComponentFixture<EditWorkRequestSuccessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditWorkRequestSuccessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditWorkRequestSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
