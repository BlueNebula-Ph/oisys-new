import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreditMemoListComponent } from './credit-memo-list.component';

describe('CreditMemoListComponent', () => {
  let component: CreditMemoListComponent;
  let fixture: ComponentFixture<CreditMemoListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreditMemoListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreditMemoListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
