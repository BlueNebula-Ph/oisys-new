import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreditMemoDetailComponent } from './credit-memo-detail.component';

describe('CreditMemoDetailComponent', () => {
  let component: CreditMemoDetailComponent;
  let fixture: ComponentFixture<CreditMemoDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreditMemoDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreditMemoDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
