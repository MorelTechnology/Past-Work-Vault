import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyActionItemsComponent } from './my-action-items.component';

describe('MyActionItemsComponent', () => {
  let component: MyActionItemsComponent;
  let fixture: ComponentFixture<MyActionItemsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MyActionItemsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyActionItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
