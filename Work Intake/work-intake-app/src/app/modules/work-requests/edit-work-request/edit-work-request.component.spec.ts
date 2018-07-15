import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditWorkRequestComponent } from './edit-work-request.component';

describe('EditWorkRequestComponent', () => {
  let component: EditWorkRequestComponent;
  let fixture: ComponentFixture<EditWorkRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditWorkRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditWorkRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
