import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewFormGeneralComponent } from './new-form-general.component';

describe('NewFormGeneralComponent', () => {
  let component: NewFormGeneralComponent;
  let fixture: ComponentFixture<NewFormGeneralComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewFormGeneralComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewFormGeneralComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
