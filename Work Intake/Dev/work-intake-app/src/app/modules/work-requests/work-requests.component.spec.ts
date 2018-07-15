import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkRequestsComponent } from './work-requests.component';

describe('WorkRequestsComponent', () => {
  let component: WorkRequestsComponent;
  let fixture: ComponentFixture<WorkRequestsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkRequestsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
