import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LookupListComponent } from './lookup-list.component';

describe('LookupListComponent', () => {
  let component: LookupListComponent;
  let fixture: ComponentFixture<LookupListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LookupListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LookupListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
