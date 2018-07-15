import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageLookupsComponent } from './manage-lookups.component';

describe('ManageLookupsComponent', () => {
  let component: ManageLookupsComponent;
  let fixture: ComponentFixture<ManageLookupsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageLookupsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageLookupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
