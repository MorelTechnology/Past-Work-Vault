import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RestoreDeniedRequestsComponent } from './restore-denied-requests.component';

describe('RestoreDeniedRequestsComponent', () => {
  let component: RestoreDeniedRequestsComponent;
  let fixture: ComponentFixture<RestoreDeniedRequestsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RestoreDeniedRequestsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RestoreDeniedRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
